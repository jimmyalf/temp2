using NHibernate;
using NHibernate.Criterion;
using Spinit.Wpc.Synologen.Core.Domain.Exceptions;
using Spinit.Wpc.Synologen.Core.Domain.Model.FrameOrder;
using Spinit.Wpc.Synologen.Core.Domain.Persistence;
using Spinit.Wpc.Synologen.Data.Repositories.NHibernate;

namespace Spinit.Wpc.Synologen.Data.Repositories.FrameOrder
{
	public class FrameRepository : NHibernateRepository<Frame>, IFrameRepository
	{
		public FrameRepository(ISession session) : base(session) {}

		public override void Delete(Frame entity)
		{
			if(FrameHasConnectedOrders(entity))
			{
				throw new SynologenDeleteItemHasConnectionsException("Frame cannot be deleted because it has connected Orders");
			}
			base.Delete(entity);
			return;
		}

		private bool FrameHasConnectedOrders(Frame entity)
		{
			//TODO: Replace with a check of "connected number of orders" property on entity
			var ordersWithGivenFrame = Session.CreateCriteria<Core.Domain.Model.FrameOrder.FrameOrder>()
				.Add(Restrictions.Eq("Frame.Id", entity.Id))
				.GetCount().UniqueResult<long>();
			return (ordersWithGivenFrame > 0);
		}
	}
}