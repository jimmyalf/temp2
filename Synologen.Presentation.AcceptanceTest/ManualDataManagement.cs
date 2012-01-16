﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NUnit.Framework;
using Spinit.Wpc.Synogen.Test.Data;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Presentation.AcceptanceTest.Helpers;
using StructureMap;

namespace Spinit.Wpc.Synologen.Presentation.AcceptanceTest
{
	[TestFixture]
	public class ManualDataManagement
	{
		private ISession _session;
		private Article _article;
		private OrderCustomer _customerOne;
		private OrderCustomer _customerTwo;
		private DataManager _dataManager;
		private Shop _shop;

		[SetUp]
		public void Context()
		{
			_dataManager = new DataManager();
			_dataManager.CleanTables();
			_session = ObjectFactory.GetInstance<ISession>();
			var category = OrderFactory.GetCategory().StoreWith(_session);
			var articleType = OrderFactory.GetArticleType(category).StoreWith(_session);
			var supplier = OrderFactory.GetSupplier().StoreWith(_session);
			_shop = CreateShop(_session);
			_article = OrderFactory.GetArticle(articleType, supplier).StoreWith(_session);
			_customerOne = OrderFactory.GetCustomer(_shop).StoreWith(_session);
			_customerTwo = OrderFactory.GetCustomer(_shop, firstName: "Börje", lastName: "Svensson").StoreWith(_session);
		}

		[Test]
		public void Add_orders()
		{
			OrderFactory.GetOrders(_shop, _article, _customerOne).StoreItemsWith(_session);
			OrderFactory.GetOrders(_shop, _article, _customerTwo).StoreItemsWith(_session);
		}

		protected Shop CreateShop(ISession session, string shopName = "Testbutik")
		{
			var shop = _dataManager.CreateShop(shopName: shopName);
			return session.Get<Shop>(shop.ShopId);
		}

	}

	public static class HelperExtensions
	{
		public static TType StoreWith<TType>(this TType item, ISession session)
		{
			session.Save(item);
			return item;
		}

		public static IEnumerable<TType> StoreItemsWith<TType>(this IEnumerable<TType> items, ISession session)
		{
			var list = items.ToList();
		    foreach (var item in list)
		    {
		        session.Save(item);	
		    }
		    return list;
		}
	}



	//public class Error
	//{
	//    public string GetErrorMessage()
	//    {
	//        return "test";
	//    }
	//}
}
