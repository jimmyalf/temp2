using System;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTypes;

namespace Spinit.Wpc.Synologen.Data.Test.Orders
{
	public static class Factory
	{
		public static OrderCustomer GetCustomer(Shop shop, string personalIdNumber = "197001013239")
		{
			return new OrderCustomer
			{
				AddressLineOne = "Box 1234",
				AddressLineTwo = "Datav�gen 23",
				City = "M�lndal",
				Email = "adam.b@testbolaget.se",
				FirstName = "Bertil",
				LastName = "Adamsson",
				MobilePhone = "0701-987654",
				Notes = "Anteckningar ABC DEF",
				PersonalIdNumber = personalIdNumber,
				Phone = "031123456",
				PostalCode = "41300",
				Shop = shop
			};
		}

		public static Subscription GetSubscription(Shop shop, OrderCustomer customer, int seed = 0, bool? active = null, DateTime? consentedDate = null, SubscriptionConsentStatus? consentStatus = null)
		{
			var isActive = active ?? (seed % 3 == 0);
			var usedConsentStatus = consentStatus ?? SubscriptionConsentStatus.Accepted.SkipItems(seed);
			return new Subscription
			{
				BankAccountNumber = "123456789",
				ClearingNumber = "1234",
				ConsentedDate = consentedDate,
				Active = isActive,
				ConsentStatus = usedConsentStatus,
				Customer = customer,
				Shop = shop,
			};
		}

		public static SubscriptionItem GetSubscriptionItem(Subscription subscription, bool useOngoingSubscription = false)
		{
			var item = new SubscriptionItem {PerformedWithdrawals = 0, Subscription = subscription};
			return useOngoingSubscription ? item.Setup(250, 50, 1250, 125) : item.Setup(3, 1000, 500);
		}

		public static SubscriptionTransaction GetTransaction(Subscription subscription, decimal amount, TransactionReason reason = TransactionReason.Payment, TransactionType type = TransactionType.Deposit)
		{
			var transaction = new CustomSubscriptionTransaction
			{
				Reason = reason, 
				Subscription = subscription, 
				Type = type
			};
			transaction.SetupOldAmount(amount);
			return transaction;
		}

		public static SubscriptionTransaction GetTransaction(Subscription subscription, decimal taxedAmount, decimal taxFreeAmount, TransactionReason reason = TransactionReason.Payment, TransactionType type = TransactionType.Deposit)
		{
			var transaction = new SubscriptionTransaction
			{
				Reason = reason, 
				Subscription = subscription, 
				Type = type
			};
			transaction.SetAmount(new SubscriptionAmount(taxedAmount,taxFreeAmount));
			return transaction;
		}

		private class CustomSubscriptionTransaction : SubscriptionTransaction
		{
			public void SetupOldAmount(decimal amount)
			{
				OldAmount = amount;
			}
		}
	}
}