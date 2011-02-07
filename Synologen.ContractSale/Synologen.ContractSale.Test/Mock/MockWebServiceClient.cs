using System;
using System.Collections.Generic;
using Spinit.Wpc.Synologen.Business.Domain.Entities;
using Spinit.Wpc.Synologen.Business.Domain.Enumerations;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;

namespace Spinit.Wpc.Synologen.Unit.Test.Mock{
	public class MockWebServiceClient : ISynologenService {
		private int logMessageNumberCounter;
		private int orderIdCounter;

		#region Implementation of ISynologenService

		public List<Order> GetOrdersForInvoicing() {
			return new List<Order> { Utility.GetMockOrderData(orderIdCounter++) };
		}

		public void SetOrderInvoiceNumber(int orderId, long newInvoiceNumber, double invoiceSumIncludingVAT, double invoiceSumExcludingVAT) {}

		public int LogMessage(LogType logType, string message) {
			return logMessageNumberCounter++;
		}

		public List<long> GetOrdersToCheckForUpdates() {
			var list = new List<long>();
			for (var i=0;i<10;i++){list.Add(i);}
			return list;
		}

		public void UpdateOrderStatuses(long invoiceNumber, bool invoiceIsCanceled, bool invoiceIsPayed) {  }

		//public void UpdateOrderStatuses(PaymentInfo invoiceStatus) {}

		//public void UpdateOrderStatuses(List<IInvoiceStatus> listOfStatusUpdates) {}

		public void SendInvoice(int orderId) {}

		public void SendInvoices(List<int> orderIds, string email) { throw new NotImplementedException(); }

		public void SendEmail(string from, string to, string subject, string message) {}

		#endregion


	}
}