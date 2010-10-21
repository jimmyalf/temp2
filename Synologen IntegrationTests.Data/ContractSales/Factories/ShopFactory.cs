using System;
using Spinit.Wpc.Synologen.Business.Domain.Entities;
using Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales;

namespace Spinit.Wpc.Synologen.Integration.Data.Test.ContractSales.Factories
{
	public static class ShopFactory
	{
		public static Shop GetShop(int id)
		{
			return new Shop
			{
				//Access = ShopAccess.None,
				Active = true,
				Address = "Spinit AB",
				Address2 = "Datav�gen 2",
				CategoryId = 1,
				City = "Askim",
				Concern = null,
				ContactFirstName = "Carl",
				ContactLastName = "Berg",
				Description = "Testbutik",
				Email = "info@spinit.se",
				Fax = "031-684 630",
				GiroId = -1,
				GiroNumber = String.Empty,
				GiroSupplier = String.Empty,
				MapUrl = String.Empty,
				Name = "Testbutik f�r b�gbest�llning",
				Number = "0000",
				Phone = "031-7483000",
				Phone2 = String.Empty,
				ShopId = id,
				Url = String.Empty,
				Zip = "43632",
			};
		}
	}
}