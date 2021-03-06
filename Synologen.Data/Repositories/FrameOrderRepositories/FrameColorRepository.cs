using NHibernate;
using NHibernate.Criterion;
using Spinit.Data.NHibernate;
using Spinit.Wpc.Synologen.Core.Domain.Exceptions;
using Spinit.Wpc.Synologen.Core.Domain.Model.FrameOrder;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.FrameOrder;

namespace Spinit.Wpc.Synologen.Data.Repositories.FrameOrderRepositories
{
	public class FrameColorRepository : NHibernateRepository<FrameColor>, IFrameColorRepository
	{
		public FrameColorRepository(ISession session) : base(session) {}

		public override void Delete(FrameColor entity)
		{
			if(FrameColorHasConnectedFrames(entity))
			{
				throw new SynologenDeleteItemHasConnectionsException("FrameColor cannot be deleted because it has connected Frames");
			}
			base.Delete(entity);
			return;
		}

		private bool FrameColorHasConnectedFrames(FrameColor entity)
		{
			var framesWithGivenColor = Session.CreateCriteria<Frame>()
				.Add(Restrictions.Eq("Color.Id", entity.Id))
				.ToCountCriteria().UniqueResult<long>();
			return (framesWithGivenColor > 0);
		}
	}
}