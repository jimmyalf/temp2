﻿using System;
using System.Linq;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGWebService;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Criterias.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;

namespace Spinit.Wpc.Synologen.LensSubscriptionServiceCoordinator.Tasks
{
	public class SendPaymentsTask :TaskBase
	{
		private readonly IBGWebService _bgWebService;
		private readonly ISubscriptionRepository _subscriptionRepository;

		public SendPaymentsTask(IBGWebService bgWebService, ISubscriptionRepository subscriptionRepository, ILoggingService loggingService)
			: base("SendPaymentsTask", loggingService)
		{
			_bgWebService = bgWebService;
			_subscriptionRepository = subscriptionRepository;
		}

		public override void Execute()
		{
			RunLoggedTask(() =>
			{
				var subscriptions = _subscriptionRepository.FindBy(new AllSubscriptionsToSendPaymentsForCriteria()) ?? Enumerable.Empty<Subscription>();
				LogDebug("Fetched {0} subscriptions to send payments for", subscriptions.Count());
				foreach (var subscription in subscriptions)
				{
					SendPaymentAndUpdateSubscriptionPaymentDate(subscription);
				}
			});
		}

		private void SendPaymentAndUpdateSubscriptionPaymentDate(Subscription subscription)
		{
			PaymentToSend payment = ConvertSubscription(subscription);
			_bgWebService.SendPayment(payment);
			subscription.PaymentInfo.PaymentSentDate = DateTime.Now;
			_subscriptionRepository.Save(subscription);
			LogDebug("Payment for subscription with id \"{0}\" has been sent.", subscription.Id);
		}

		private static PaymentToSend ConvertSubscription(Subscription subscription)
		{
			var payment = new PaymentToSend
		              	{
		              		Amount = subscription.PaymentInfo.MonthlyAmount,
		              		Reference = subscription.Customer.PersonalIdNumber,
		                    Type = PaymentType.Debit,
		                    PayerId = subscription.Id
		                };
			return payment;
		}
	}
}
