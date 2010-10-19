using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Integration.Test.CommonDataTestHelpers;

namespace Spinit.Wpc.Synologen.Integration.Test.LensSubscriptionData.Factories
{
	public static class CustomerFactory {
		
		public static Customer Get(Country country, Shop shop)
		{

			return new Customer
			       	{
			       		Address =
			       			new CustomerAddress
			       				{
			       					AddressLineOne = "Datav�gen 2",
			       					AddressLineTwo = "Box 416 57",
			       					City = "G�vle",
			       					Country = country,
			       					PostalCode = "436 32"
			       				}
			       		,
			       		Contact = new CustomerContact {Email = "paiv@home.se", MobilePhone = "0702624715", Phone = "0322-16660"},
			       		FirstName = "Sune",
			       		LastName = "Mangs",
			       		PersonalIdNumber = "7301146069",
			       		Shop = shop
			       	};
		
		}

		public static Customer Get()
		{
			return new Customer();
		}

		public static Customer Edit(Customer customer)
		{
			customer.Address.AddressLineOne = customer.Address.AddressLineOne.Reverse();
			customer.Address.AddressLineTwo = customer.Address.AddressLineTwo.Reverse();
			customer.Address.City = customer.Address.City.Reverse();
			customer.Address.PostalCode = customer.Address.PostalCode.Reverse();
			customer.Contact.Email = customer.Contact.Email.Reverse();
			customer.Contact.MobilePhone = customer.Contact.MobilePhone.Reverse();
			customer.Contact.Phone = customer.Contact.Phone.Reverse();
			customer.FirstName = customer.FirstName.Reverse();
			customer.LastName = customer.LastName.Reverse();
			customer.PersonalIdNumber = customer.PersonalIdNumber.Reverse();

			return customer;
		}
	}
}