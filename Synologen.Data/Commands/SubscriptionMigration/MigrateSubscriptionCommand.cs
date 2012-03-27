﻿using System;
using System.Linq;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Data.Queries.SubscripitionMigration;
using Shop = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.Shop;
using Subscription = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.Subscription;
using SubscriptionError = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionError;
using SubscriptionErrorType = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTypes.SubscriptionErrorType;
using SubscriptionTransaction = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTransaction;
using TransactionReason = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTypes.TransactionReason;
using TransactionType = Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTypes.TransactionType;

namespace Spinit.Wpc.Synologen.Data.Commands.SubscriptionMigration
{
	public class MigrateSubscriptionCommand : Command<Subscription>
	{
		public int AdditionalNumberOfWithdrawals { get; set; }
		private readonly Core.Domain.Model.LensSubscription.Subscription _oldSubscription;

		public MigrateSubscriptionCommand(Core.Domain.Model.LensSubscription.Subscription oldSubscription, int additionalNumberOfWithdrawals)
		{
			AdditionalNumberOfWithdrawals = additionalNumberOfWithdrawals;
			_oldSubscription = oldSubscription;
		}

		private Subscription ParseSubscription(Core.Domain.Model.LensSubscription.Subscription oldSubscription)
		{
			return new Subscription
			{
				AutogiroPayerId = oldSubscription.BankgiroPayerNumber,
				Active = oldSubscription.Active,
				BankAccountNumber = oldSubscription.PaymentInfo.AccountNumber,
				ClearingNumber = oldSubscription.PaymentInfo.ClearingNumber,
				ConsentStatus = GetNewConsentStatus(oldSubscription.ConsentStatus),
				ConsentedDate = oldSubscription.ActivatedDate,
				Customer = GetCustomer(oldSubscription.Customer),
				Errors = null, // insert later
				LastPaymentSent = oldSubscription.PaymentInfo.PaymentSentDate,
				Shop = Session.Get<Shop>(_oldSubscription.Customer.Shop.Id),
				SubscriptionItems = null //insert later
			};
		}

		private SubscriptionItem ParseSubscriptionItem(Core.Domain.Model.LensSubscription.Subscription oldSubscription, Subscription newSubscription)
		{
			var performedWithdrawals = oldSubscription.Transactions.Count(x => 
				x.Reason == Core.Domain.Model.LensSubscription.TransactionReason.Withdrawal && 
				x.Type == Core.Domain.Model.LensSubscription.TransactionType.Withdrawal);
			var withdrawalLimit = performedWithdrawals + AdditionalNumberOfWithdrawals;
			return new SubscriptionItem
			{
				FeePrice = 0,
				ProductPrice = oldSubscription.PaymentInfo.MonthlyAmount * withdrawalLimit,
				PerformedWithdrawals = oldSubscription.Transactions.Count(),
				Subscription = newSubscription,
				WithdrawalsLimit = withdrawalLimit,
			};
		}

		private SubscriptionTransaction ParseTransaction(Core.Domain.Model.LensSubscription.SubscriptionTransaction oldTransaction, Subscription newSubscription)
		{
			return new SubscriptionTransaction
			{
				Amount = oldTransaction.Amount, 
				Reason = GetNewTransactionReason(oldTransaction.Reason), 
				SettlementId = (oldTransaction.Settlement == null) ? (int?) null : oldTransaction.Settlement.Id, 
				Subscription = newSubscription, 
				Type = GetNewTransactionType(oldTransaction.Type),
			};
		}

		private SubscriptionError ParseError(Core.Domain.Model.LensSubscription.SubscriptionError oldSubscriptionError, Subscription newSubscription)
		{
			return new SubscriptionError
			{
				BGConsentId = oldSubscriptionError.BGConsentId,
				BGErrorId = oldSubscriptionError.BGErrorId,
				BGPaymentId = oldSubscriptionError.BGPaymentId,
				Code = oldSubscriptionError.Code,
				CreatedDate = oldSubscriptionError.CreatedDate,
				HandledDate = oldSubscriptionError.HandledDate,
				Subscription = newSubscription,
				Type = GetNewErrorType(oldSubscriptionError.Type)
			};
		}

		private Core.Domain.Model.Orders.SubscriptionTypes.SubscriptionConsentStatus GetNewConsentStatus(SubscriptionConsentStatus oldEntity)
		{
			switch (oldEntity)
			{
				case SubscriptionConsentStatus.NotSent: return Core.Domain.Model.Orders.SubscriptionTypes.SubscriptionConsentStatus.NotSent;
				case SubscriptionConsentStatus.Sent: return Core.Domain.Model.Orders.SubscriptionTypes.SubscriptionConsentStatus.Sent;
				case SubscriptionConsentStatus.Accepted: return Core.Domain.Model.Orders.SubscriptionTypes.SubscriptionConsentStatus.Accepted;
				case SubscriptionConsentStatus.Denied: return Core.Domain.Model.Orders.SubscriptionTypes.SubscriptionConsentStatus.Denied;
				default: throw new ArgumentOutOfRangeException("oldEntity");
			}
		}

		private TransactionType GetNewTransactionType(Core.Domain.Model.LensSubscription.TransactionType oldEntity)
		{
			switch (oldEntity)
			{
				case Core.Domain.Model.LensSubscription.TransactionType.Deposit: return TransactionType.Deposit;
				case Core.Domain.Model.LensSubscription.TransactionType.Withdrawal: return TransactionType.Withdrawal;
				default: throw new ArgumentOutOfRangeException("oldEntity");
			}
		}

		private TransactionReason GetNewTransactionReason(Core.Domain.Model.LensSubscription.TransactionReason oldEntity)
		{
			switch (oldEntity)
			{
				case Core.Domain.Model.LensSubscription.TransactionReason.Payment: return TransactionReason.Payment;
				case Core.Domain.Model.LensSubscription.TransactionReason.Withdrawal: return TransactionReason.Withdrawal;
				case Core.Domain.Model.LensSubscription.TransactionReason.Correction: return TransactionReason.Correction;
				case Core.Domain.Model.LensSubscription.TransactionReason.PaymentFailed: return TransactionReason.PaymentFailed;
				default: throw new ArgumentOutOfRangeException("oldEntity");
			}
		}

		private SubscriptionErrorType GetNewErrorType(Core.Domain.Model.LensSubscription.SubscriptionErrorType oldEntity)
		{
			switch (oldEntity)
			{
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.NoCoverage: return SubscriptionErrorType.NoCoverage;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.NoAccount: return SubscriptionErrorType.NoAccount;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentMissing: return SubscriptionErrorType.ConsentMissing;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.NotApproved: return SubscriptionErrorType.NotApproved;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.CosentStopped: return SubscriptionErrorType.CosentStopped;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.NotDebitable: return SubscriptionErrorType.NotDebitable;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentTurnedDownByBank: return SubscriptionErrorType.ConsentTurnedDownByBank;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentTurnedDownByPayer: return SubscriptionErrorType.ConsentTurnedDownByPayer;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.AccountTypeNotApproved: return SubscriptionErrorType.AccountTypeNotApproved;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentMissingInBankgiroConsentRegister: return SubscriptionErrorType.ConsentMissingInBankgiroConsentRegister;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.IncorrectAccountOrPersonalIdNumber: return SubscriptionErrorType.IncorrectAccountOrPersonalIdNumber;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentCanceledByBankgiro: return SubscriptionErrorType.ConsentCanceledByBankgiro;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentCanceledByBankgiroBecauseOfMissingStatement: return SubscriptionErrorType.ConsentCanceledByBankgiroBecauseOfMissingStatement;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentIsAlreadyInBankgiroConsentRegisterOrUnderConsederation: return SubscriptionErrorType.ConsentIsAlreadyInBankgiroConsentRegisterOrUnderConsederation;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.ConsentTemporarilyStoppedByPayer: return SubscriptionErrorType.ConsentTemporarilyStoppedByPayer;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.TemporaryConsentStopRevoked: return SubscriptionErrorType.TemporaryConsentStopRevoked;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.IncorrectPersonalIdNumber: return SubscriptionErrorType.IncorrectPersonalIdNumber;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.IncorrectPayerNumber: return SubscriptionErrorType.IncorrectPayerNumber;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.IncorrectAccountNumber: return SubscriptionErrorType.IncorrectAccountNumber;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.MaxAmountNotAllowed: return SubscriptionErrorType.MaxAmountNotAllowed;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.IncorrectPaymentReceiverBankgiroNumber: return SubscriptionErrorType.IncorrectPaymentReceiverBankgiroNumber;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.PaymentReceiverBankgiroNumberMissing: return SubscriptionErrorType.PaymentReceiverBankgiroNumberMissing;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.Canceled: return SubscriptionErrorType.Canceled;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.PaymentRejectedInsufficientFunds: return SubscriptionErrorType.PaymentRejectedInsufficientFunds;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.PaymentRejectedAgConnectionMissing: return SubscriptionErrorType.PaymentRejectedAgConnectionMissing;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.PaymentFailureWillTryAgain: return SubscriptionErrorType.PaymentFailureWillTryAgain;
				case Core.Domain.Model.LensSubscription.SubscriptionErrorType.Unknown: return SubscriptionErrorType.Unknown;
				default: throw new ArgumentOutOfRangeException("oldEntity");
			}
		}

		private OrderCustomer GetCustomer(Customer oldCustomer)
		{
			var existingNewCustomer = Query(new MigratedNewCustomer(oldCustomer.PersonalIdNumber, oldCustomer.Shop.Id));;
			if(existingNewCustomer != null) return existingNewCustomer;
			var command = new MigrateCustomerCommand(oldCustomer) {Session = Session};
			command.Execute();
			return command.Result;
		}

		public override void Execute()
		{
			var newSubscription = CreateSubscription();
			CreateSubscriptionItem(newSubscription);
			CreateTransactions(newSubscription);
			CreateErrors(newSubscription);
			Result = Session.SessionFactory.OpenSession().Get<Subscription>(newSubscription.Id);
		}

		private Subscription CreateSubscription()
		{
			var newSubscription = ParseSubscription(_oldSubscription);
			Session.Save(newSubscription);
			ExecuteCustomCommand("UPDATE SynologenOrderSubscription SET CreatedDate = @CreatedDate WHERE Id = @Id", new {Id = newSubscription.Id, CreatedDate = _oldSubscription.CreatedDate});
			return newSubscription;
		}

		private void CreateSubscriptionItem(Subscription newSubscription)
		{
			var subscriptionItem = ParseSubscriptionItem(_oldSubscription, newSubscription);
			Session.Save(subscriptionItem);
			ExecuteCustomCommand("UPDATE SynologenOrderSubscriptionItem SET CreatedDate = @CreatedDate WHERE Id = @Id", new {Id = subscriptionItem.Id, CreatedDate = _oldSubscription.CreatedDate});
		}

		private void CreateTransactions(Subscription newSubscription)
		{
			foreach (var oldTransaction in _oldSubscription.Transactions)
			{
				var newTransaction = ParseTransaction(oldTransaction, newSubscription);
				Session.Save(newTransaction);
				ExecuteCustomCommand("UPDATE SynologenOrderTransaction SET CreatedDate = @CreatedDate WHERE Id = @Id", new {Id = newTransaction.Id, CreatedDate = oldTransaction.CreatedDate});
			}
		}

		private void CreateErrors(Subscription newSubscription)
		{
			foreach (var oldError in _oldSubscription.Errors)
			{
				var newError = ParseError(oldError, newSubscription);
				Session.Save(newError);
				ExecuteCustomCommand("UPDATE SynologenOrderSubscriptionError SET CreatedDate = @CreatedDate WHERE Id = @Id", new {Id = newError.Id, CreatedDate = oldError.CreatedDate});
			}
		}
	}
}