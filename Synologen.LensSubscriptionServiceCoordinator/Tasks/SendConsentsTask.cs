using System.Linq;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGWebService;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Criterias.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;

namespace Spinit.Wpc.Synologen.LensSubscriptionServiceCoordinator.Tasks
{
	public class SendConsentsTask : TaskBase
	{
		private readonly IBGWebService _bgWebService;
		private readonly ISubscriptionRepository _subscriptionRepository;

		public SendConsentsTask(IBGWebService bgWebService, ISubscriptionRepository subscriptionRepository, ILoggingService loggingService)
			: base("SendConsentsTask", loggingService)
		{
			_bgWebService = bgWebService;
			_subscriptionRepository = subscriptionRepository;
		}

		public override void Execute()
		{
			RunLoggedTask(() =>
			{
				var subscriptions = _subscriptionRepository.FindBy(new AllSubscriptionsToSendConsentsForCriteria()) ?? Enumerable.Empty<Subscription>();
				LogDebug("Fetched {0} subscriptions to send consents for", subscriptions.Count());
				foreach (var subscription in subscriptions)
				{
					var consent = ConvertSubscription(subscription);
					_bgWebService.SendConsent(consent);
					subscription.ConsentStatus = SubscriptionConsentStatus.Sent;
					_subscriptionRepository.Save(subscription);
					LogDebug("Consent for subscription with id \"{0}\" has been sent.", subscription.Id);
				}
			});
		}

		public static ConsentToSend ConvertSubscription(Subscription subscription)
		{
			return new ConsentToSend
			{
				BankAccountNumber = subscription.PaymentInfo.AccountNumber,
				ClearingNumber = subscription.PaymentInfo.ClearingNumber,
				PersonalIdNumber = subscription.Customer.PersonalIdNumber,
                SubscriptionId = subscription.Id
			};
		}
	}
}