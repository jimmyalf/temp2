using System;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Services.BgWebService;
using WebService_AutogiroServiceType=Spinit.Wpc.Synologen.Core.Domain.Model.BGWebService.AutogiroServiceType;
using BGServer_AutogiroServiceType=Spinit.Wpc.Synologen.Core.Domain.Model.BGServer.AutogiroServiceType;

namespace Synologen.LensSubscription.BGWebService.App.Services
{
	public class BGWebServiceDTOParser : IBGWebServiceDTOParser
	{
		public AutogiroPayer GetAutogiroPayer(string name, WebService_AutogiroServiceType serviceType) 
		{
			return new AutogiroPayer
			{
				Name = name,
				ServiceType = MapServiceType(serviceType)
			};
		}

		private static BGServer_AutogiroServiceType MapServiceType(WebService_AutogiroServiceType serviceType)
		{
			switch (serviceType)
			{
				case WebService_AutogiroServiceType.LensSubscription: return BGServer_AutogiroServiceType.LensSubscription;
				default: throw new ArgumentOutOfRangeException("serviceType");
			}
		}
	}
}