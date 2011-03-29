using System;
using System.Collections.Generic;
using NHibernate;
using ServiceCoordinator.AcceptanceTest;
using Spinit.Wpc.Core.Dependencies.NHibernate;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.BgWebService;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Spinit.Wpc.Synologen.Data.Repositories.LensSubscriptionRepositories;
using StructureMap;
using Synologen.LensSubscription.BGData.Repositories;
using Synologen.LensSubscription.ServiceCoordinator.Core.TaskRunner;
using Synologen.Test.Core;

namespace Synologen.LensSubscription.ServiceCoordinator.AcceptanceTest.TestHelpers
{
	public abstract class TaskBase : BehaviorTestBase
	{
		protected ICountryRepository countryRepository;
		protected IShopRepository shopRepository;
		protected ICustomerRepository customerRepository;
		protected IBGConsentToSendRepository bgConsentRepository;
		protected ISubscriptionRepository subscriptionRepository;
		protected IBGPaymentToSendRepository bgPaymentRepository;
		protected IBGReceivedConsentRepository bgReceivedConsentRepository;
		protected IBGReceivedPaymentRepository bgReceivedPaymentRepository;
		protected IBGReceivedErrorRepository bgReceivedErrorRepository;
		protected IAutogiroPayerRepository autogiroPayerRepository;
		protected const int TestShopId = 158;
		protected const int SwedenCountryId = 1;
		protected static ISession intermediateSession;
		protected ISubscriptionErrorRepository subscriptionErrorRepository;


		protected override void SetUp()
		{
			base.SetUp();
			intermediateSession = GetWPCSession();
			countryRepository = new CountryRepository(GetWPCSession());
			shopRepository = new ShopRepository(GetWPCSession());
			customerRepository = new CustomerRepository(GetWPCSession());
			subscriptionRepository = ResolveRepository<ISubscriptionRepository>(GetWPCSession);
			bgConsentRepository = new BGConsentToSendRepository(GetBGSession());
			bgPaymentRepository = new BGPaymentToSendRepository(GetBGSession());
			bgReceivedConsentRepository = new BGReceivedConsentRepository(GetBGSession());
			bgReceivedPaymentRepository = new BGReceivedPaymentRepository(GetBGSession());
			autogiroPayerRepository = new AutogiroPayerRepository(GetBGSession());
			subscriptionErrorRepository = new SubscriptionErrorRepository(GetWPCSession());
			bgReceivedErrorRepository = new BGReceivedErrorRepository(GetBGSession());
		}

		protected Subscription StoreSubscription(Func<Customer,Subscription> getSubscription, int payerNumber)
		{
			var countryToUse = countryRepository.Get(SwedenCountryId);
			var shopToUse = shopRepository.Get(TestShopId);
			var customer = Factory.CreateCustomer(countryToUse, shopToUse);
			customerRepository.Save(customer);
			var subscription = getSubscription.Invoke(customer);
			subscriptionRepository.Save(subscription);
			return subscription;
		}

		protected int RegisterPayerWithWebService()
		{
			var payerNumber = 0;
			InvokeWebService(service =>
			{
				payerNumber = service.RegisterPayer("Test payer", AutogiroServiceType.LensSubscription);
			});
			return payerNumber;
		}

		protected TTask ResolveTask<TTask>() where TTask : ITask
		{
			return ObjectFactory.GetInstance<TTask>();
		}

		protected ITaskRunnerService GetTaskRunnerService(ITask task)
		{
			return ObjectFactory
				.With(typeof(IEnumerable<ITask>),new []{task})
				.With(typeof(ITaskRepositoryResolver), new TestTaskRepositoryResolver())
				.GetInstance<TaskRunnerService>();
		}


		private class TestTaskRepositoryResolver : ITaskRepositoryResolver
		{
			public TRepository GetRepository<TRepository>()
			{
				return ObjectFactory.GetInstance<TRepository>();
			}
		}

		protected TRepository ResolveRepository<TRepository>(Func<ISession> resolveSession)
		{
			return ObjectFactory
				.With(typeof(ISession), resolveSession.Invoke())
				.GetInstance<TRepository>();
		}

		protected TEntity ResolveEntity<TEntity>()
		{
			return ObjectFactory.GetInstance<TEntity>();
		}

		protected void InvokeWebService(Action<IBGWebService> function)
		{
			var client = ResolveEntity<IBGWebServiceClient>();
			client.Open();
			function.Invoke(client);
			client.Close();
		}

		protected static ISession GetBGSession()
		{
			return Synologen.LensSubscription.BGData.NHibernateFactory.Instance.GetSessionFactory().OpenSession();
		}

		protected static ISession GetWPCSession()
		{
			return NHibernateFactory.Instance.GetSessionFactory().OpenSession();
		}
	}
}