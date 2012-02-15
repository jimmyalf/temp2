﻿using FluentNHibernate.Mapping;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders.SubscriptionTypes;

namespace Spinit.Wpc.Synologen.Data.Repositories.NHibernate.Mappings.Orders
{
	public class SubscriptionMap : ClassMap<Subscription>
	{
		public SubscriptionMap()
		{
			Table("SynologenOrderSubscription");
			Id(x => x.Id);
			Map(x => x.ActivatedDate).Nullable();
			Map(x => x.Active);
			Map(x => x.AutogiroPayerId).Nullable();
			Map(x => x.BankAccountNumber).Length(12);
			Map(x => x.ClearingNumber).Length(4);
			Map(x => x.ConsentStatus).CustomType<SubscriptionConsentStatus>();
			Map(x => x.CreatedDate);
			References(x => x.Customer).Column("CustomerId");
			References(x => x.Shop).Column("ShopId").Not.Nullable();
			Map(x => x.LastPaymentSent).Nullable();
			HasMany(x => x.SubscriptionItems).KeyColumn("SubscriptionId");
			HasMany(x => x.Transactions).KeyColumn("SubscriptionId");
		} 
	}
}