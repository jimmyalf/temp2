using System;
using System.Collections.Generic;
using System.Linq;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Send;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Extensions;
using Account=Spinit.Wpc.Synologen.Core.Domain.Model.BGServer.Account;

namespace Synologen.LensSubscription.BGServiceCoordinator.Task.Test.Factories
{
	public static class ConsentsFactory 
	{
		public static IList<BGConsentToSend> GetList(AutogiroPayer payer) 
		{
			Func<int, BGConsentToSend> generateItem = seed => Get(seed, payer);
			return generateItem.GenerateRange(1, 15).ToList();
		}

		public static BGConsentToSend Get(int seed, AutogiroPayer payer)
		{
			return new BGConsentToSend
			{
				Account = new Account
				{
					AccountNumber = "1212121212",
					ClearingNumber = "8901"
				},
				Payer = payer,
				OrgNumber = null,
				PersonalIdNumber = "194608170000",
				SendDate = null,
				Type = ConsentType.New.SkipItems(seed),
			};
		}

		public static string GetTestConsentFileData() 
		{
			return "ABCDefg\r\nIJKlmn\r\nOPQrst\r\nuvxYZ���";
		}
	}
}