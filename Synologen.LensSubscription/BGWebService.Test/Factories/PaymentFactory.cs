using System;
using System.Collections.Generic;
using System.Linq;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGWebService;
using Spinit.Wpc.Synologen.Core.Extensions;
using PaymentResult=Spinit.Wpc.Synologen.Core.Domain.Model.BGServer.PaymentResult;
using PaymentType=Spinit.Wpc.Synologen.Core.Domain.Model.BGWebService.PaymentType;

namespace Synologen.LensSubscription.BGWebService.Test.Factories
{
	public static class PaymentFactory 
	{
		public static PaymentToSend GetPaymentToSend(PaymentType type, int payerNumber) 
		{
			return new PaymentToSend
			{
				Amount = 566.23M,
				PayerNumber = payerNumber,
				Reference = "Spinit �gonbutik",
				Type = type
			};
		}

		public static IList<BGReceivedPayment> GetReceivedPaymentList(AutogiroPayer payer) 
		{
			Func<int, BGReceivedPayment> getItem = seed =>  GetReceivedPayment(seed, payer);
			return getItem.GenerateRange(1, 15).ToList();
		}

		public static BGReceivedPayment GetReceivedPayment(int seed, AutogiroPayer payer)
		{
			return new BGReceivedPayment
			{
				Amount = 823 - seed,
				CreatedDate = new DateTime(2011, 03, 09),
				Payer = payer,
				PaymentDate = new DateTime(2011, 03, 25),
				Reference = "�gonbutiken ABC",
				ResultType = PaymentResult.Approved.SkipValues(seed)
			};
		}
	}
}