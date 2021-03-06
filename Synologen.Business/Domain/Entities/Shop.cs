using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales;

namespace Spinit.Wpc.Synologen.Business.Domain.Entities{
	[DataContract]
	public class Shop : IShop
	{
		[DataMember] public int ShopId { get; set; }
		[DataMember] public string Name { get; set; }
		[DataMember] public string Number { get; set; }
		[DataMember] public string Description { get; set; }
		[DataMember] public bool Active { get; set; }
		[DataMember] public string Address { get; set; }
		[DataMember] public string Address2 { get; set; }
		[DataMember] public string Zip { get; set; }
		[DataMember] public string City { get; set; }
        [DataMember] public decimal Latitude { get; set; }
        [DataMember] public decimal Longitude { get; set; }
		[DataMember] public string Phone { get; set; }
		[DataMember] public string Phone2 { get; set; }
		[DataMember] public string Fax { get; set; }
		[DataMember] public string Email { get; set; }
		[DataMember] public string ContactFirstName { get; set; }
		[DataMember] public string ContactLastName { get; set; }
		[DataMember] public int CategoryId { get; set; }
		public int? ShopGroupId { get; set; }
		public IList<IShop> GetAllShopsInConcern(ISqlProvider sqlProvider) {
			if(Concern == null || Concern.Id <= 0) return new List<IShop>();
			return sqlProvider.GetShopRows(null, null, null, null, null, null, Concern.Id, null).ToList();
		}
		[DataMember] public IEnumerable<ShopEquipment> Equipment { get; set; }
		[DataMember] public string Url { get; set; }
		[DataMember] public string MapUrl { get; set; }
		[DataMember] public int GiroId { get; set; }
		[DataMember] public string GiroNumber { get; set; }
		[DataMember] public string GiroSupplier { get; set; }
		[DataMember] public Concern Concern { get; set; }
		public string ContactCombinedName {
			get { return String.Concat( ContactFirstName ?? String.Empty,  " ",  ContactLastName ?? String.Empty ).Trim(); }
		}
		public string ExternalAccessUsername { get; set; }
		public string ExternalAccessHashedPassword { get; set; }
		public string OrganizationNumber { get; set; }

	    public string Format(string format)
	    {
	        return format.ReplaceWith(new
	        {
	            Environment.NewLine,
	            OrganizationNumber,
	            ShopId,
	            Name,
	            Number,
	            Description,
	            Active,
	            Address,
	            Address2,
	            Zip,
	            City,
	            Latitude,
	            Longitude,
	            Phone,
	            Phone2,
	            Fax,
	            Email,
	            ContactFirstName,
	            ContactLastName,
	            CategoryId,
	            Url,
	            MapUrl,
	            GiroId,
	            GiroNumber,
	            GiroSupplier,
	            ContactCombinedName,
	            AddressLine = GetShopAddressLine(this),
	            ZipAndCity = GetShopZipAndCity(this),
	        });
	    }

        protected string GetShopAddressLine(IShop shop)
        {
            if (string.IsNullOrEmpty(shop.Address2))
            {
                return shop.Address ?? string.Empty;
            }

            return shop.Address2;
        }

        protected string GetShopZipAndCity(IShop shop)
        {
            return "{Zip} {City}".ReplaceWith(new { shop.Zip, shop.City });
        }

	    public bool HasConcern { get{ return Concern != null;} }
		public ShopAccess Access { get; set; }
	}
}