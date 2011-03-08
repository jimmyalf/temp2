using System;
using System.Collections.Generic;
using System.Linq;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Send;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Criterias.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Account=Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes.Account;
using ConsentType=Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Send.ConsentType;

namespace Synologen.LensSubscription.BGServiceCoordinator.Task.SendConsents
{
	public class Task : TaskBase
	{
		private readonly IAutogiroFileWriter<ConsentsFile, Consent> _fileWriter;
		private readonly IBGServiceCoordinatorSettingsService _bgServiceCoordinatorSettingsService;

		public Task(
			ILoggingService loggingService, 
			IAutogiroFileWriter<ConsentsFile,  Consent> fileWriter ,
			IBGServiceCoordinatorSettingsService bgServiceCoordinatorSettingsService,
			ITaskRepositoryResolver taskRepositoryResolver) 
			: base("SendConsents", loggingService, taskRepositoryResolver, BGTaskSequenceOrder.SendTask)
		{
			_fileWriter = fileWriter;
			_bgServiceCoordinatorSettingsService = bgServiceCoordinatorSettingsService;
		}

		public override void Execute()
		{
			RunLoggedTask(repositoryResolver =>
			{
				var fileSectionToSendRepository = repositoryResolver.GetRepository<IFileSectionToSendRepository>();
				var bgConsentToSendRepository = repositoryResolver.GetRepository<IBGConsentToSendRepository>();
				var consents = bgConsentToSendRepository.FindBy(new AllNewConsentsToSendCriteria());
				if(consents == null || consents.Count() == 0)
				{
					LogInfo("Found no new consents to send.");
					return;
				}
				LogDebug("Found {0} new consents to send", consents.Count());
				var consentsFile = ToConsentFile(consents);
				var consentFileContent = _fileWriter.Write(consentsFile);
				var fileSectionToSend = ToFileSectionToSend(consentFileContent);
				fileSectionToSendRepository.Save(fileSectionToSend);
				consents.Each(consent =>
				{
					consent.SendDate = DateTime.Now;
					bgConsentToSendRepository.Save(consent);
				});
			});
		}

		protected virtual FileSectionToSend ToFileSectionToSend(string contents)
		{
			return new FileSectionToSend
			{
				CreatedDate = DateTime.Now,
                SectionData = contents,
                SentDate = null,
                Type = SectionType.ConsentsToSend
			};
		}

		protected virtual ConsentsFile ToConsentFile(IEnumerable<BGConsentToSend> consentsToSend)
		{
			return new ConsentsFile
			{
				Posts = consentsToSend.Select(consent => ToConsent(consent)),
				Reciever = new PaymentReciever
				{
					BankgiroNumber = _bgServiceCoordinatorSettingsService.GetPaymentRecieverBankGiroNumber(),
					CustomerNumber = _bgServiceCoordinatorSettingsService.GetPaymentRevieverCustomerNumber()
				},
				WriteDate = DateTime.Now
			};
		}

		protected virtual Consent ToConsent(BGConsentToSend consentsToSend)
		{
			return new Consent
			{
				Account = new Account
				{
					AccountNumber = consentsToSend.Account.AccountNumber,
					ClearingNumber = consentsToSend.Account.ClearingNumber
				},
				OrgNumber = consentsToSend.OrgNumber,
				PersonalIdNumber = consentsToSend.PersonalIdNumber,
				RecieverBankgiroNumber = _bgServiceCoordinatorSettingsService.GetPaymentRecieverBankGiroNumber(),
				Transmitter = new Payer
				{
					CustomerNumber = consentsToSend.Payer.Id.ToString()
				},
				Type = ToConsentType(consentsToSend.Type)
			};
		}

		protected virtual ConsentType ToConsentType(Spinit.Wpc.Synologen.Core.Domain.Model.BGServer.ConsentType inputType)
		{
			switch (inputType)
			{
				case Spinit.Wpc.Synologen.Core.Domain.Model.BGServer.ConsentType.New:
					return ConsentType.New;
				case Spinit.Wpc.Synologen.Core.Domain.Model.BGServer.ConsentType.Cancellation:
					return ConsentType.Cancellation;
				default:
					throw new ArgumentOutOfRangeException("inputType");
			}
		}
	}
}