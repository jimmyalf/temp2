using System.Collections.Generic;
using FakeItEasy;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Send;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Spinit.Wpc.Synologen.Core.Extensions;

namespace Synologen.LensSubscription.BGServiceCoordinator.Task.Test.Helpers
{
	public abstract class SendConsentsTaskTestBase : CommonTaskTestBase
	{
		protected IBGConsentToSendRepository BGConsentToSendRepository;
		protected IAutogiroFileWriter<ConsentsFile, Consent> ConsentFileWriter;

		protected override void SetUp()
		{
			base.SetUp();

			BGConsentToSendRepository = A.Fake<IBGConsentToSendRepository>();
			ConsentFileWriter = A.Fake<IAutogiroFileWriter<ConsentsFile, Consent>>();
			TaskRepositoryResolver.AddRepository(BGConsentToSendRepository);
		}

		protected override ITask GetTask() 
		{
			return new SendConsents.Task(LoggingService, ConsentFileWriter, BgServiceCoordinatorSettingsService);
		}

		protected virtual bool MatchConsents(IEnumerable<Consent> parsedConsents, IList<BGConsentToSend> originalConsents, string recieverBankGiroNumber)
		{
			var allMatches = true;
			parsedConsents.And(originalConsents).Do((parsedConsent,originalConsent) =>
			{
				if(!ConsentIsMatch(parsedConsent, originalConsent, recieverBankGiroNumber))
				{
					allMatches =  false;
				}
			});
			return allMatches;
		}

		protected virtual bool ConsentIsMatch(Consent consent, BGConsentToSend bgConsentToSend, string recieverBankGiroNumber)
		{
			return
				Equals(consent.Account.AccountNumber, bgConsentToSend.Account.AccountNumber) &&
	            Equals(consent.Account.ClearingNumber, bgConsentToSend.Account.ClearingNumber) &&
	            Equals(consent.OrgNumber, bgConsentToSend.OrgNumber) &&
	            Equals(consent.PersonalIdNumber, bgConsentToSend.PersonalIdNumber) &&
	            Equals(consent.RecieverBankgiroNumber, recieverBankGiroNumber) &&
	            Equals(consent.Transmitter.CustomerNumber, bgConsentToSend.Payer.Id.ToString()) &&
	            Equals(consent.Type.ToInteger(), bgConsentToSend.Type.ToInteger());
		}
	}
}