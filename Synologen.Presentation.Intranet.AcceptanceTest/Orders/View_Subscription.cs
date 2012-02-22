﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTypes;
using Spinit.Wpc.Synologen.Core.Extensions;
using Spinit.Wpc.Synologen.Presentation.Intranet.AcceptanceTest.TestHelpers;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.EventArguments.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.Orders;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.AcceptanceTest.Orders
{

	[TestFixture, Category("View_Subscription")]
	public class View_Subscription : GeneralOrderSpecTestbase<SubscriptionPresenter,ISubscriptionView>
	{
		private Shop _shop;
		private Subscription _subscription;
		private SubscriptionError _unHandledError;
		private IEnumerable<SubscriptionItem> _subscriptionItems;
		private IEnumerable<SubscriptionTransaction> _transactions;
		private IEnumerable<SubscriptionError> _errors;
		private SubscriptionPresenter _presenter;
		private string _returnUrl;
		private string _subscriptionItemDetailUrl;

		public View_Subscription()
		{
			Context = () =>
			{
				_shop = CreateShop<Shop>();
				_presenter = GetPresenter();
				_errors = null;
				_returnUrl = "return/page/";
				_subscriptionItemDetailUrl = "/subscription-item/page/";
				View.ReturnPageId = 56;
				View.SubscriptionItemDetailPageId = 57;
				RoutingService.AddRoute(View.ReturnPageId, _returnUrl);
				RoutingService.AddRoute(View.SubscriptionItemDetailPageId, _subscriptionItemDetailUrl);
			};

			Story = () => new Berättelse("Visa abonnemang")
				.FörAtt("Visa abonnemangsinformation")
				.Som("inloggad butikspersonal")
				.VillJag("se all information om ett abonnemang");
		}

		[Test]
		public void VisaAbonnemang()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
					.Och(AbonnemangetÄrStartat)
					.Och(DelAbonnemangFinns)
					.Och(TransaktionerFinns)
					.Och(FelFinns)
				.När(SidanVisas)
				.Så(AbonnemangsInformationVisas)
					.Och(StoppaKnappVisas)
					.Och(TillbakaLänkVisas)
					.Och(DelAbonnemangVisas)
					.Och(TransaktionerVisas)
					.Och(FelListaVisas)
					.Och(FelVisas)
			);
		}

		[Test]
		public void VisaInaktivtAbonnemang()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
					.Och(AbonnemangetÄrStoppat)
				.När(SidanVisas)
				.Så(StartaKnappVisas)
			);
		}

		[Test]
		public void VisaMedgivetAbonnemang()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
					.Och(AbonnemangetÄrMedgivet)
				.När(SidanVisas)
				.Så(MedgivandeInformationVisas)
			);
		}

		[Test]
		public void VisaAbonnemangUtanFel()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
				.När(SidanVisas)
				.Så(FelListaVisasInte)
			);
		}

		[Test]
		public void HanteraFel()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
					.Och(AttAbonnemangetHarEttOhanteratFel)
				.När(EttOhanteratFelHanteras)
				.Så(SkallFeletHanteras)
			);
		}

		[Test]
		public void StartaAbonnemang()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
					.Och(AbonnemangetÄrStoppat)
				.När(AbonnemangetStartas)
				.Så(SåSkallAbonnemangetVaraAktivt)
			);
		}

		[Test]
		public void StoppaAbonnemang()
		{
			SetupScenario(scenario => scenario
				.Givet(AbonnemangFinns)
					.Och(AbonnemangetÄrStartat)
				.När(AbonnemangetStoppas)
				.Så(SåSkallAbonnemangetVaraInaktivt)
			);
		}

		#region Arrange
		private void AbonnemangFinns()
		{
			_subscription = CreateSubscription(_shop);
			HttpContext.SetupRequestParameter("subscription", _subscription.Id.ToString());
		}

		private void AbonnemangetÄrMedgivet()
		{
			_subscription.ConsentStatus = SubscriptionConsentStatus.Accepted;
			_subscription.ConsentedDate = new DateTime(2012, 02, 20);
			Save(_subscription);
		}

		private void DelAbonnemangFinns()
		{
			_subscriptionItems = new []{ CreateSubscriptionItem(_subscription)};
		}

		private void TransaktionerFinns()
		{
			_transactions = StoreItems(() => OrderFactory.GetTransactions(_subscription));
		}

		private void FelFinns()
		{
			_errors = StoreItems(() => OrderFactory.GetErrors(_subscription));
		}

		private void AttAbonnemangetHarEttOhanteratFel()
		{
			_unHandledError = StoreItem(() => OrderFactory.GetError(_subscription, null));

		}

		private void AbonnemangetÄrStartat()
		{
			_subscription.Active = true;
			Save(_subscription);
		}

		private void AbonnemangetÄrStoppat()
		{
			_subscription.Active = false;
			Save(_subscription);
		}

		#endregion

		#region Act
		private void SidanVisas()
		{
			_presenter.View_Load(null, new EventArgs());
		}

		private void EttOhanteratFelHanteras()
		{
			_presenter.Handle_Error(null, new HandleErrorEventArgs(_unHandledError.Id));
		}

		private void AbonnemangetStartas()
		{
			_presenter.Start_Subscription(null, new EventArgs());
		}

		private void AbonnemangetStoppas()
		{
			_presenter.Stop_Subscription(null, new EventArgs());
		}
		#endregion

		#region Assert
		private void AbonnemangsInformationVisas()
		{
			View.Model.BankAccountNumber.ShouldBe(_subscription.BankAccountNumber);
			View.Model.ClearingNumber.ShouldBe(_subscription.ClearingNumber);
			View.Model.CustomerName.ShouldBe(_subscription.Customer.FirstName + " " + _subscription.Customer.LastName);
			View.Model.Status.ShouldBe(_subscription.Active ? "Startat" : "Stoppat");
			View.Model.Consented.ShouldBe(_subscription.ConsentStatus.GetEnumDisplayName());
			View.Model.CreatedDate.ShouldBe(_subscription.CreatedDate.ToString("yyyy-MM-dd"));
			View.Model.CurrentBalance.ShouldBe(Subscription.GetCurrentAccountBalance(_transactions.ToList()).ToString("C2"));
		}

		private void MedgivandeInformationVisas()
		{
			View.Model.Consented.ShouldBe("Medgivet " + _subscription.ConsentedDate.Value.ToString("yyyy-MM-dd"));
		}

		private void FelListaVisas()
		{
			View.Model.HasErrors.ShouldBe(true);
		}

		private void FelListaVisasInte()
		{
			View.Model.HasErrors.ShouldBe(false);
		}

		private void DelAbonnemangVisas()
		{
			View.Model.SubscriptionItems.And(_subscriptionItems).Do((viewModel, item) =>
			{
				viewModel.Active.ShouldBe(item.IsActive ? "Ja" : "Nej");
				viewModel.MontlyAmount.ShouldBe(item.AmountForAutogiroWithdrawal.ToString("C2"));
				viewModel.SubscriptionItemDetailUrl.ShouldBe(_subscriptionItemDetailUrl + "?subscription-item=" + item.Id);
				viewModel.CreatedDate.ShouldBe(item.CreatedDate.ToString("yyyy-MM-dd"));
				if(item.WithdrawalsLimit.HasValue)
				{
					viewModel.PerformedWithdrawals.ShouldBe(item.PerformedWithdrawals + "/" + item.WithdrawalsLimit.Value);
				}
				else
				{
					viewModel.PerformedWithdrawals.ShouldBe(item.PerformedWithdrawals.ToString());
				}
			});
		}

		private void TransaktionerVisas()
		{
			var orderedTransactions = _transactions.OrderByDescending(x => x.CreatedDate);
			View.Model.Transactions.And(orderedTransactions).Do((viewModel, item) =>
			{
				viewModel.Amount.ShouldBe(GetFormattedAmount(item));
				viewModel.Date.ShouldBe(item.CreatedDate.ToString("yyyy-MM-dd"));
				viewModel.Description.ShouldBe(item.Reason.GetEnumDisplayName());
				viewModel.IsPayed.ShouldBe(item.SettlementId.HasValue ? "Ja" : "Nej");
			});
		}

		private string GetFormattedAmount(SubscriptionTransaction transaction)
		{
			var amount = transaction.Type == TransactionType.Deposit 
				? transaction.Amount.ToString("C2")
				: (transaction.Amount * -1).ToString("C2");
			switch (transaction.Reason)
			{
				case TransactionReason.Payment: return amount;
				case TransactionReason.Withdrawal: return amount;
				case TransactionReason.Correction: return amount;
				case TransactionReason.PaymentFailed: return "(" + amount + ")";
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void FelVisas()
		{
			View.Model.Errors.And(_errors).Do((viewModel, item) =>
			{
				viewModel.Created.ShouldBe(item.CreatedDate.ToString("yyyy-MM-dd"));
				viewModel.Handled.ShouldBe(item.IsHandled ? item.HandledDate.Value.ToString("yyyy-MM-dd") : null);
				viewModel.IsHandled.ShouldBe(item.IsHandled);
				viewModel.Type.ShouldBe(item.Type.GetEnumDisplayName());
				viewModel.Id.ShouldBe(item.Id);
			});
		}

		private void SkallFeletHanteras()
		{
			Get<SubscriptionError>(_unHandledError.Id).HandledDate.Value.Date.ShouldBe(DateTime.Now.Date);
		}

		private void TillbakaLänkVisas()
		{
			View.Model.ReturnUrl.ShouldBe(_returnUrl);
		}

		private void SåSkallAbonnemangetVaraAktivt()
		{
			Get<Subscription>(_subscription.Id).Active.ShouldBe(true);
		}

		private void SåSkallAbonnemangetVaraInaktivt()
		{
			Get<Subscription>(_subscription.Id).Active.ShouldBe(false);
		}

		private void StoppaKnappVisas()
		{
			View.Model.ShowStopButton.ShouldBe(true);
		}

		private void StartaKnappVisas()
		{
			View.Model.ShowStartButton.ShouldBe(true);
		}
		#endregion
	}
}