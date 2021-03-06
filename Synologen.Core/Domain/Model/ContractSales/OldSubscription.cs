namespace Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales
{
	public class OldSubscription : Entity
	{
		public virtual OldCustomer Customer { get; set; }
	}

	public class NewSubscription : Entity
	{
		public virtual Shop Shop { get; set; }
		public virtual NewCustomer Customer { get; set; }
	}
}