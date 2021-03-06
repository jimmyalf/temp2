using Spinit.Data;
using Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales;

namespace Spinit.Wpc.Synologen.Core.Domain.Persistence.ContractSales
{
	public interface ISettlementRepository : IReadonlyRepository<Settlement>
	{
		ShopSettlement GetForShop(int id, int shopId);
	}
}