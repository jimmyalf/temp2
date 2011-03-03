﻿using System;
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
	[TestFixture, Explicit("Fake it easy test")]
	public class Test_Fake_it_easy
	{
	    [Test]
	    public void Test_for_problem()
	    {
	        var fakeRepo = A.Fake<IRepo>();
	        A.CallTo(() => fakeRepo.Get(A<int>.Ignored)).Returns(new object());
	        var repositoryResolver = A.Fake<IRepoResolver>();
	        A.CallTo(() => repositoryResolver.GetRepository<IRepo>()).Returns(fakeRepo);
	        try
	        {
	            var fetchedRepo = repositoryResolver.GetRepository<IRepo>();
	            fetchedRepo.ShouldBe(fakeRepo);

	        }
			catch(Exception ex)
			{
			    Console.WriteLine(ex.Message);
			    Assert.Inconclusive(ex.Message);
			}
	    }
	}

	public interface IConstraint{}
	public interface IRepo
	{
	    object Get(int id);
		object GetByConstraint<TConstraint>(TConstraint constraint) where TConstraint : IConstraint;
	}

	public interface IRepoResolver
	{
	    TRepo GetRepository<TRepo>();
	}

    [TestFixture, Category("ReceivePaymentsTaskTests")]
    public class When_receiveing_payments : ReceivePaymentsTaskTestBase
    {
        private IEnumerable<ReceivedFileSection> _receivedSections;
        private Payment _savedPayment;
    	private AutogiroPayer payer;

    	public When_receiveing_payments()
        {
            Context = () =>
            {
            	payer = PayerFactory.Get();
            	_receivedSections = RecievedFileSectionFactory.GetList(SectionType.ReceivedPayments);
                var paymentsFileSection = ReceivedPaymentsFactory.GetReceivedPaymentsFileSection(payer.Id);
                _savedPayment = ReceivedPaymentsFactory.GetPayment(payer.Id);

                A.CallTo(() => ReceivedFileRepository.FindBy(A<AllUnhandledReceivedPaymentFileSectionsCriteria>
                                                      .Ignored.Argument)).Returns(_receivedSections);
                A.CallTo(() => PaymentFileReader.Read(A<string>.Ignored)).Returns(paymentsFileSection);
				A.CallTo(() => AutogiroPayerRepository.Get(payer.Id)).Returns(payer);
            };
            Because = task => task.Execute();    
        }

        [Test]
        public void Task_has_receive_task_ordering()
        {
            Task.TaskOrder.ShouldBe(BGTaskSequenceOrder.ReadTask.ToInteger());
        }

        [Test]
        public void Task_loggs_start_and_stop_messages()
        {
            A.CallTo(() => Log.Info(A<string>.That.Contains("Started"))).MustHaveHappened();
            A.CallTo(() => Log.Info(A<string>.That.Contains("Finished"))).MustHaveHappened();
        }

        [Test]
        public void Task_logs_number_of_received_sections()
        {
            A.CallTo(() => Log.Debug(A<string>.That.Contains("Fetched 15 payment file sections from repository"))).MustHaveHappened();
        }

        [Test]
        public void Task_logs_after_each_handled_section()
        {
            A.CallTo(() => Log.Debug(A<string>.That.Contains("Saved 10 payments to repository"))).MustHaveHappened();
        }


        [Test]
        public void Task_fetches_new_paymentsections_from_repository()
        {
            A.CallTo(() => ReceivedFileRepository.FindBy(
                               A<AllUnhandledReceivedPaymentFileSectionsCriteria>.Ignored.Argument))
                               .MustHaveHappened();
        }

        [Test]
        public void Task_saves_fetched_fileposts_as_payments()
        {
            A.CallTo(() => BGReceivedPaymentRepository.Save(
                            A<BGReceivedPayment>
                            .That.Matches(x => x.Amount.Equals(_savedPayment.Amount))
                            .And.Matches(x => x.CreatedDate.Date.Equals(DateTime.Now.Date))
                            .And.Matches(x => x.Payer.Id.ToString().Equals(_savedPayment.Transmitter.CustomerNumber))
                            .And.Matches(x => x.PaymentDate.Date.Equals(_savedPayment.PaymentDate.Date))
                            .And.Matches(x => x.Reference.Equals(_savedPayment.Reference))
                            .And.Matches(x => x.ResultType.Equals(_savedPayment.Result))
                        )).MustHaveHappened();
        }

        [Test]
        public void Task_updates_paymentsection_as_handled()
        {
            _receivedSections.Each(receivedSection => A.CallTo(() => ReceivedFileRepository.Save(
                A<ReceivedFileSection>
                    .That.Matches(x => Equals(x.HasBeenHandled, true))
                    .And.Matches(x => x.HandledDate.Value.Date.Equals(DateTime.Now.Date))
                )));
        }
    }
}
