using NHibernate;
using NHibernate.Transform;
using Spinit.Data;
using Spinit.Data.NHibernate;
using Spinit.Wpc.Synologen.Core.Domain.Model.Orders;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.Criterias.Orders;

namespace Spinit.Wpc.Synologen.Data.Repositories.CriteriaConverters.Orders
{
	public class AllSubscriptionsForShopCriteriaConverter : NHibernateActionCriteriaConverter<AllSubscriptionsForShopCriteria, Subscription>, IActionCriteria
	{
		public AllSubscriptionsForShopCriteriaConverter(ISession session) : base(session) {}
		public override ICriteria Convert(AllSubscriptionsForShopCriteria source)
		{
			return Criteria
                .CreateAlias(x => x.Customer)
                .CreateAlias(x => x.Customer.Shop)
				.Sort(x => x.Customer.LastName, true)
                .Sort(x => x.Customer.FirstName, true)
				.FilterEqual(x => x.Customer.Shop.Id, source.ShopId)
				.SetFetchMode(Property(x => x.Transactions), FetchMode.Join)
				.SetFetchMode(Property(x => x.SubscriptionItems), FetchMode.Select)
				.SetResultTransformer(new DistinctRootEntityResultTransformer());
		}
	}
}