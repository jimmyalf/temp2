using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NUnit.Framework;
using Spinit.Extensions;
using Spinit.Wpc.Core.Dependencies.NHibernate;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Extensions;
using Spinit.Wpc.Synologen.Data.Repositories.LensSubscriptionRepositories;
using Spinit.Wpc.Synologen.Data.Test.CommonDataTestHelpers;
using Spinit.Wpc.Synologen.Data.Test.ContractSales.Factories;
using Spinit.Wpc.Synologen.Data.Test.LensSubscriptionData.Factories;
using Spinit.Wpc.Utility.Business;
using Shop = Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription.Shop;
using ShopFactory=Spinit.Wpc.Synologen.Data.Test.ContractSales.Factories.ShopFactory;

namespace Spinit.Wpc.Synologen.Data.Test
{
	[SetUpFixture]
	public class GlobalTestBase
	{
		protected int TestCountryId = 1;
		protected int TestShopId = 158;
		protected int TestShop2Id = 159;

		[SetUp]
		public void RunBeforeAnyTests()
		{
			if (NHibernateFactory.MappingAssemblies.Any()) return;
			var assembly = typeof(Repositories.NHibernate.Mappings.LensSubscriptions.SubscriptionMap).Assembly;
			NHibernateFactory.MappingAssemblies.Add(assembly);
		}

		[TearDown]
		public void RunAfterAnyTests()
		{
			var provider = GetSqlProvider();
			var session = NHibernateFactory.Instance.GetSessionFactory().OpenSession();
			ClearTables(session);
			SetupLensSubscriptionData();
			SetupContractSaleSettlementData(provider);
			ResetTestShop(provider);
		}

		private void ClearTables(ISession session)
		{
			DataHelper.DeleteAndResetIndexForTable(session.Connection, "tblSynologenArticle");
		}

		private void SetupContractSaleData(ISqlProvider provider) 
		{ 
			if(String.IsNullOrEmpty(DataHelper.ConnectionString)){
				throw new OperationCanceledException("Connectionstring could not be found in configuration");
			}
			if(!IsDevelopmentServer(DataHelper.ConnectionString))
			{
				throw new OperationCanceledException("Make sure you are running tests against a development database!");
			}
			const int settlementableOrderStatus = 6;
			const int nonSettlementableOrderStatus = 5;
			const int testableShopId = 158;
			const int TestableShopMemberId = 485;
			const int TestableCompanyId = 57;
			const int TestableContractId = 14;

			var article = ArticleFactory.Get();
			provider.AddUpdateDeleteArticle(Enumerations.Action.Create, ref article);
			var contractArticleConnection = ArticleFactory.GetContractArticleConnection(article, TestableContractId, 999.23F, false);
			provider.AddUpdateDeleteContractArticleConnection(Enumerations.Action.Create, ref contractArticleConnection);

			var orders = new[]
			{
				OrderFactory.Get(TestableCompanyId, settlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
				OrderFactory.Get(TestableCompanyId, nonSettlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
				OrderFactory.Get(TestableCompanyId, settlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
				OrderFactory.Get(TestableCompanyId, settlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
				OrderFactory.Get(TestableCompanyId, nonSettlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
				OrderFactory.Get(TestableCompanyId, nonSettlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
				OrderFactory.Get(TestableCompanyId, settlementableOrderStatus, testableShopId, TestableShopMemberId, article.Id),
			};
			orders.Each(order =>
			{
				provider.AddUpdateDeleteOrder(Enumerations.Action.Create, ref order);
				provider.AddUpdateDeleteOrder(Enumerations.Action.Update, ref order);
				order.OrderItems.Each(orderItem =>
				{
					IOrderItem tempOrder = orderItem;
					tempOrder.OrderId = order.Id;
					provider.AddUpdateDeleteOrderItem(Enumerations.Action.Create, ref tempOrder);
				});
			});				
		}

		private void SetupContractSaleSettlementData(ISqlProvider provider) 
		{ 
			const int settlementableOrderStatus = 6;
			const int orderStatusAfterSettlement = 8;
			Action action  = () => 
			{
				SetupContractSaleData(provider);
				provider.AddSettlement(settlementableOrderStatus, orderStatusAfterSettlement);	
			};
			action.Times(5);
			SetupContractSaleData(provider);
		}

		private void ResetTestShop(ISqlProvider provider)
		{
			var testShop = ShopFactory.GetShop(TestShopId, ShopAccess.LensSubscription | ShopAccess.SlimJim);
			provider.AddUpdateDeleteShop(Enumerations.Action.Update, ref testShop);
		}

		private static ISqlProvider GetSqlProvider()
		{
			return new SqlProvider(DataHelper.ConnectionString);
		}

		private void SetupLensSubscriptionData() {
			var session = GetSessionFactory().OpenSession();
			var shop1 = new ShopRepository(session).Get(TestShopId);
			var shop2 = new ShopRepository(session).Get(TestShop2Id);
			var country = new CountryRepository(session).Get(TestCountryId);
			var transactionArticleRepository = new TransactionArticleRepository(session);
			var transactionArticlesToSave = TransactionArticleFactory.GetList(55);
			transactionArticlesToSave.Each(transactionArticleRepository.Save);

			for (var i = 0; i < 15; i++)
			{
				AddTransactions(i, session, shop1, country, transactionArticlesToSave);
				AddTransactions(i, session, shop2, country, transactionArticlesToSave);
			}
		}

		private static void AddTransactions(int seed, ISession session, Shop shop, Country country, IList<TransactionArticle> transactionArticlesToSave)
		{
			if (transactionArticlesToSave == null) throw new ArgumentNullException("transactionArticlesToSave");
			var subscriptionRepository = new SubscriptionRepository(session);
			var transactionRepository = new TransactionRepository(session);
			var errorRepository = new SubscriptionErrorRepository(session);
			var reposititory = new CustomerRepository(session);
			var customerToSave = CustomerFactory.Get(country, shop, "Tore " + seed, "Alm " + seed, "19630610613" + (seed%9));
			reposititory.Save(customerToSave);
			var subscriptionToSave = SubscriptionFactory.Get(customerToSave, seed);
			subscriptionRepository.Save(subscriptionToSave);
			if(subscriptionToSave.ConsentStatus !=  SubscriptionConsentStatus.Accepted) return;
			TransactionFactory.GetList(subscriptionToSave, seed).Each(transaction =>
			{
				transactionRepository.Save(transaction);
				if(seed.IsDivisibleBy(4)) { transaction.Article = transactionArticlesToSave[seed]; }
			});
			if(seed.IsDivisibleBy(5))
			{
				SubscriptionErrorFactory.GetList(subscriptionToSave).Each(errorRepository.Save);
			}
		}

		protected ISessionFactory GetSessionFactory()
		{
			return NHibernateFactory.Instance.GetSessionFactory();
		}

		protected virtual bool IsDevelopmentServer(string connectionString)
		{
			if(connectionString.ToLower().Contains("black")) return true;
			if(connectionString.ToLower().Contains("dev")) return true;
			if(connectionString.ToLower().Contains("localhost")) return true;
			if(connectionString.ToLower().Contains(@".\")) return true;
			return false;
		}
	}
}