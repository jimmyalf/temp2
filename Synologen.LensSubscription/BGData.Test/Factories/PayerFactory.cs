using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Extensions;

namespace Synologen.LensSubscription.BGData.Test.Factories
{
	public static class PayerFactory 
	{
		public static AutogiroPayer Get()
		{
			return new AutogiroPayer
			{
				Name = "Adam Bertil",
				ServiceType = AutogiroServiceType.LensSubscription
			};
		}

		public static void Edit(AutogiroPayer payer) 
		{
			payer.Name = payer.Name.Reverse();
			payer.ServiceType = payer.ServiceType.Next();
		}
	}
}