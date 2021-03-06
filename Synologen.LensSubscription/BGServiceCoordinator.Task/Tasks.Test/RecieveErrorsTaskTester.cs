using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Recieve;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Criterias.BGServer;
using Spinit.Wpc.Synologen.Core.Extensions;
using Synologen.LensSubscription.BGServiceCoordinator.Task.Test.Factories;
using Synologen.LensSubscription.BGServiceCoordinator.Task.Test.Helpers;

namespace Synologen.LensSubscription.BGServiceCoordinator.Task.Test
{
	[TestFixture, Category("RecieveErrorsTaskTests")]
	public class When_receiveing_errors : ReceiveErrorsTaskTestBase
	{
        private IEnumerable<ReceivedFileSection> _receivedSections;
        private Error _savedError;
		private AutogiroPayer payer;

		public When_receiveing_errors()
		{
			Context = () =>
            {
            	payer = PayerFactory.Get();
            	_receivedSections = RecievedFileSectionFactory.GetList(SectionType.ReceivedErrors);
                var errorsFileSection = ReceivedErrorsFactory.GetReceivedErrorFileSection(payer.Id);
                _savedError = ReceivedErrorsFactory.GetError(payer.Id);

                A.CallTo(() => ReceivedFileRepository.FindBy(A<AllUnhandledReceivedErrorFileSectionsCriteria>.Ignored)).Returns(_receivedSections);
                A.CallTo(() => ErrorFileReader.Read(A<string>.Ignored)).Returns(errorsFileSection); 
				A.CallTo(() => AutogiroPayerRepository.Get(payer.Id)).Returns(payer);
            };
		
			Because = task => task.Execute(ExecutingTaskContext);
		}

        [Test]
        public void Task_has_receive_task_ordering()
        {
            Task.TaskOrder.ShouldBe(BGTaskSequenceOrder.ReadTask.ToInteger());
        }

        [Test]
        public void Task_loggs_start_and_stop_messages()
        {
            LoggingService.AssertInfo("Started");
            LoggingService.AssertInfo("Finished");
        }

        [Test]
        public void Task_logs_number_of_received_sections()
        {
            LoggingService.AssertDebug("Fetched 15 error file sections from repository");
        }

        [Test]
        public void Task_logs_after_each_handled_section()
        {
            LoggingService.AssertDebug("Saved 10 errors to repository");
        }

        [Test]
        public void Task_fetches_new_paymentsections_from_repository()
        {
            A.CallTo(() => ReceivedFileRepository.FindBy(
                               A<AllUnhandledReceivedErrorFileSectionsCriteria>.Ignored))
                               .MustHaveHappened();
        }

        [Test]
        public void Task_saves_fetched_fileposts_as_errors()
        {
            A.CallTo(() => BGReceivedErrorRepository.Save(A<BGReceivedError>.That.Matches(x => 
				x.Amount.Equals(_savedError.Amount)
                && x.CreatedDate.Date.Equals(DateTime.Now.Date)
                && x.Payer.Id.ToString().Equals(_savedError.Transmitter.CustomerNumber)
                && x.PaymentDate.Date.Equals(_savedError.PaymentDate.Date)
                && x.Reference.Equals(_savedError.Reference)
                && x.CommentCode.Equals(_savedError.CommentCode)
			))).MustHaveHappened();
        }

        [Test]
        public void Task_updates_errorsection_as_handled()
        {
            _receivedSections.Each(receivedSection => A.CallTo(() => ReceivedFileRepository.Save(A<ReceivedFileSection>.That.Matches(x => 
				Equals(x.HasBeenHandled, true) && x.HandledDate.Value.Date.Equals(DateTime.Now.Date))
			)));
        }
	}
}