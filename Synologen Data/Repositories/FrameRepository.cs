using NHibernate;
using Spinit.Wpc.Synologen.Core.Domain.Model;
using Spinit.Wpc.Synologen.Core.Domain.Persistence;
using Spinit.Wpc.Synologen.Data.Repositories.NHibernate;

namespace Spinit.Wpc.Synologen.Data.Repositories
{
	public class FrameRepository : NHibernateRepository<Frame>, IFrameRepository
	{
		public FrameRepository(ISession session) : base(session) {}
	}
}