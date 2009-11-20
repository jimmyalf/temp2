﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml;
using Spinit.Wpc.Synologen.Business.Domain.Entities;
using Spinit.Wpc.Synologen.Business.Domain.Enumerations;
using Spinit.Wpc.Synologen.Business.Domain.Exceptions;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Data;
using Spinit.Wpc.Synologen.EDI;
using Spinit.Wpc.Synologen.Invoicing;
using Spinit.Wpc.Synologen.Invoicing.Types;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.CommonAggregateComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.Documents.BasicInvoice;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.Codelist;
using Convert=Spinit.Wpc.Synologen.Invoicing.Convert;

namespace Spinit.Wpc.Synologen.ServiceLibrary{
	public class SynologenService : ISynologenService{
		private readonly ISqlProvider provider;
		private const string FtpFileUploadNotAccepted = "ej accepterad";
		private const string FtpFileUploadContainsError = "felaktig";
		private const int NumberOfDecimalsUsedForRounding = 2;
		private const string EDIFileNameFormat = "Synologen-{0}-{1}-{2}.txt";
		private const string SvefakturaFileNameFormat = "Synologen-{0}-{1}.xml";
		private const string DateFormat = "yyyy-MM-dd";

		public SynologenService() {
			provider = new SqlProvider(ConfigurationSettings.WebService.ConnectionString);
		}

		public SynologenService(ISqlProvider sqlProvider){
			provider = sqlProvider;
		}

		public List<Order> GetOrdersForInvoicing(){
			try{
				var statusIdFilter = ConfigurationSettings.WebService.NewSaleStatusId;
				var invoicingMethodIdFilter = ConfigurationSettings.WebService.InvoicingMethodIdFilter;
				var orders = provider.GetOrdersForInvoicing(statusIdFilter, invoicingMethodIdFilter, null);
				var orderString = GetOrderIdsFromList(orders);
				LogMessage(LogType.Information, "Client fetched orders from WPC: " + orderString);
				return orders;
			}
			catch (Exception ex){
				throw LogAndCreateException("SynologenService.GetOrdersForInvoicing failed", ex);
			}
		}

		public void SetOrderInvoiceNumber(int orderId, long invoiceNumber, double invoiceSumIncludingVAT, double invoiceSumExcludingVAT){
			try{
				var importedStatusId = ConfigurationSettings.WebService.SaleStatusIdAfterSPCSImport;
				provider.SetOrderInvoiceNumber(orderId, invoiceNumber, importedStatusId, invoiceSumIncludingVAT, invoiceSumExcludingVAT);
				
				var orderHistoryMessage = GetVismaNewOrderAddedHistoryMessage(invoiceNumber, invoiceSumIncludingVAT, invoiceSumExcludingVAT);
				provider.AddOrderHistory(orderId, orderHistoryMessage);
				var orderString = "OrderId: " + orderId + ", InvoiceNumber: " + invoiceNumber;
				LogMessage(LogType.Information, "Client wrote back order invoice number [" + orderString + "]: ");
				var newStatusId = ConfigurationSettings.WebService.SaleStatusIdAfterSPCSImport;
				provider.UpdateOrderStatus(newStatusId, orderId, 0, 0, 0, 0, 0);
			}
			catch(Exception ex) {
				throw LogAndCreateException("SynologenService.SetOrderInvoiceNumber failed", ex);
			}
		}

		public int LogMessage (LogType logType, string message ) {
			int returnValue;
			try {
				switch (logType) {
					case LogType.Error:
						if(ConfigurationSettings.WebService.SendAdminEmailOnError) {
							TrySendErrorEmail(message);
						}
						break;
					case LogType.Information:
						if(!ConfigurationSettings.WebService.LogInformation)
							return 0;
						break;
					case LogType.Other:
						if(!ConfigurationSettings.WebService.LogOther)
							return 0;
						break;
					default:
						throw new ArgumentOutOfRangeException("logType");
				}
				returnValue = provider.AddLog(logType, message);
			}
			catch(Exception ex) {
				throw LogAndCreateException("SynologenService.LogMessage failed", ex);
			}
			return returnValue;
		}

		public List<long> GetOrdersToCheckForUpdates() {
			List<long> listOfOrders;
			try{
				var statusId = ConfigurationSettings.WebService.SaleStatusIdAfterInvoicing;
				listOfOrders = provider.GetOrderInvoiceNumbers(statusId, null);
				var orderString = GetOrderIdsFromList(listOfOrders);
				LogMessage(LogType.Information, "Client fetched order-invoice-id's from WPC : " + orderString);
			}
			catch (Exception ex) {
				throw LogAndCreateException("SynologenService.GetOrdersToCheckForUpdates failed", ex);
			}
			return listOfOrders;

		}

		public void UpdateOrderStatuses(IInvoiceStatus invoiceStatus) {
			if (invoiceStatus.InvoiceCanceled || invoiceStatus.InvoicePaymentCanceled) {
				var newStatusId = ConfigurationSettings.WebService.InvoiceCancelledStatusId;
				provider.UpdateOrderStatus(newStatusId, 0, 0, 0, 0, 0, invoiceStatus.InvoiceNumber);
				var orderHistoryMessage = GetVismaOrderStatusUpdateHistoryMessage(invoiceStatus.InvoiceNumber);
				provider.AddOrderHistory(invoiceStatus.InvoiceNumber, orderHistoryMessage);
				return;
			}
			if (invoiceStatus.InvoiceIsPayed) {
				var newStatusId = ConfigurationSettings.WebService.InvoicePayedToSynologenStatusId;
				provider.UpdateOrderStatus(newStatusId, 0, 0, 0, 0, 0, invoiceStatus.InvoiceNumber);
				var orderHistoryMessage = GetVismaOrderStatusUpdateHistoryMessage(invoiceStatus.InvoiceNumber);
				provider.AddOrderHistory(invoiceStatus.InvoiceNumber, orderHistoryMessage);
				return;
			}

		}

		/// <summary>
		/// Creates an invoice and uploads it as a text file to a configurable ftp-url.
		/// If successful it will change sale status id when transfer is completed
		/// </summary>
		/// <param name="orderId"></param>
		public void SendInvoice(int orderId) {
			try{
				var order = provider.GetOrder(orderId);
				string ftpStatusMessage;
				switch (order.ContractCompany.InvoicingMethodId){
					case 1: //EDI
						ftpStatusMessage = SendEDIInvoice(order);
						break;
					case 2: //Svefaktura
						ftpStatusMessage = SendSvefakturaInvoice(order);
						break;
					default:
						throw new ArgumentOutOfRangeException("orderId","Orders comany invoicing method cannot be identified.");
				}
				var newStatusId = ConfigurationSettings.WebService.SaleStatusIdAfterInvoicing;
				provider.UpdateOrderStatus(newStatusId, orderId, 0, 0, 0, 0, 0);

				var orderHistoryMessage = GetInvoiceSentHistoryMessage(order.InvoiceNumber, ftpStatusMessage);
				provider.AddOrderHistory(orderId, orderHistoryMessage);
				
			}
			catch(Exception ex) {
				throw LogAndCreateException("SynologenService.SendInvoice failed [OrderId: "+orderId+"]", ex);
			}
		}

		/// <summary>
		/// Sends a regular text-email
		/// </summary>
		public void SendEmail(string from, string to, string subject, string message) {
			try {
				var smtpServerAddress = ConfigurationSettings.WebService.SMTPServer;
				var smtpClient = new SmtpClient(smtpServerAddress);
				var mailMessage = new MailMessage(from, to, subject, message);
				smtpClient.Send(mailMessage);
			}
			catch(Exception ex) {
				throw new WebserviceException("Email could not be sent", ex);
			}
		}

		#region Helper methods

		private string SendEDIInvoice(IOrder order){
			var invoice = GenerateEDIInvoice(order);
			var invoiceFileName = GenerateInvoiceFileName(invoice);
			if(ConfigurationSettings.WebService.SaveEDIFileCopy) {
				TrySaveContentToDisk(invoiceFileName, invoice.Parse().ToString());
			}
			var invoiceString = invoice.Parse().ToString();
			return UploadTextFile(invoiceFileName, invoiceString);
		}

		private string SendSvefakturaInvoice(IOrder order){
			var invoice = GenerateSvefakturaInvoice(order);
			var ruleViolations = SvefakturaValidator.ValidateObject(invoice);
			if (ruleViolations.Any()){
				throw new WebserviceException("The invoice could not be validated: " + SvefakturaValidator.FormatRuleViolations(ruleViolations));
			}
			var encoding = ConfigurationSettings.WebService.FTPCustomEncodingCodePage;
			var postOfficeheader = GetPostOfficeheader();
			var invoiceStringContent = SvefakturaSerializer.Serialize(invoice, encoding, "\r\n", Formatting.Indented, postOfficeheader);
			var invoiceFileName = GenerateInvoiceFileName(invoice);
			if(ConfigurationSettings.WebService.SaveSvefakturaFileCopy) {
				TrySaveContentToDisk(invoiceFileName, invoiceStringContent);
			}
			return UploadTextFile(invoiceFileName, invoiceStringContent);
		}

		private static string GetPostOfficeheader(){
			const string headerFormat = "<?POSTNET SND=\"{0}\" REC=\"{1}\" MSGTYPE=\"{2}\"?>";
			var sender = ConfigurationSettings.WebService.PostnetSender;
			var recipient = ConfigurationSettings.WebService.PostnetRecipient;
			var messageType = ConfigurationSettings.WebService.PostnetMessageType;
			return String.Format(headerFormat, sender, recipient, messageType);
		}

		private static string GetVismaNewOrderAddedHistoryMessage(long vismaOrderId, double invoiceSumIncludingVAT, double invoiceSumExcludingVAT) {
			var message = ServiceResources.resx.ServiceResources.OrderAddedToVismaHistoryMessage;
			message = message.Replace("{0}", vismaOrderId.ToString());
			message = message.Replace("{1}", invoiceSumIncludingVAT.ToString());
			message = message.Replace("{2}", invoiceSumExcludingVAT.ToString());
			return message;
		}

		private static string GetVismaOrderStatusUpdateHistoryMessage(long vismaOrderId) {
			var message = ServiceResources.resx.ServiceResources.OrderStatusUpdatedHistoryMessage;
			message = message.Replace("{0}", vismaOrderId.ToString());
			return message;
		}

		private static string GetInvoiceSentHistoryMessage(long invoiceNumber, string ftpStatusMessage) {
			var message = ServiceResources.resx.ServiceResources.InvoiceSentHistoryMessage;
			message = message.Replace("{0}", invoiceNumber.ToString());
			message = message.Replace("{1}", ftpStatusMessage);
			return message;
		}

		private static string GetOrderIdsFromList(IEnumerable<Order> orders) {
			var returnString = String.Empty;
			foreach (var order in orders) {
				returnString += order.Id + ",";
			}
			return returnString.TrimEnd(',');
		}
		private static string GetOrderIdsFromList(IEnumerable<long> orderIds) {
			var returnString = String.Empty;
			foreach(var id in orderIds) {
				returnString += id + ",";
			}
			return returnString.TrimEnd(',');
		}

		private Exception LogAndCreateException(string message, Exception ex) {
			var exception = new WebserviceException(message, ex);
			LogMessage(LogType.Error, ex.ToString());
			if(ConfigurationSettings.WebService.SendAdminEmailOnError) {
				TrySendErrorEmail(exception.ToString());
			}
			return exception;
		}

		private static void TrySendErrorEmail(string message) {
			try {
				var smtpClient = new SmtpClient(ConfigurationSettings.WebService.SMTPServer);
				var mailMessage = new MailMessage();
				mailMessage.To.Add(ConfigurationSettings.WebService.AdminEmail);
				mailMessage.From = new MailAddress(ConfigurationSettings.WebService.EmailSender);
				mailMessage.Subject = ServiceResources.resx.ServiceResources.ErrorEmailSubject;
				mailMessage.Body = message;
				smtpClient.Send(mailMessage);
			}
			catch{return;}
		}

		private void TryLogErrorAndSendEmail(string message) {
			try {
				LogMessage(LogType.Error, message);
			}
			catch { return; }
		}

		private static EDIConversionSettings GetEDISetting() {
			return new EDIConversionSettings
			{
				BankGiro = ConfigurationSettings.WebService.BankGiro,
				Postgiro = ConfigurationSettings.WebService.Postgiro,
				SenderId = ConfigurationSettings.WebService.EDISenderId,
				VATAmount = ConfigurationSettings.WebService.VATAmount,
				InvoiceCurrencyCode = ConfigurationSettings.WebService.InvoiceCurrencyCode,
				NumberOfDecimalsUsedAtRounding = NumberOfDecimalsUsedForRounding,
			};
		}

		private static SvefakturaConversionSettings GetSvefakturaSettings() {
			return new SvefakturaConversionSettings
			{
				InvoiceIssueDate = DateTime.Now,
				BankGiro = ConfigurationSettings.WebService.BankGiro,
				Postgiro = ConfigurationSettings.WebService.Postgiro,
				VATAmount = (decimal) ConfigurationSettings.WebService.VATAmount,
				BankgiroBankIdentificationCode = ConfigurationSettings.WebService.BankGiroCode,
				PostgiroBankIdentificationCode = ConfigurationSettings.WebService.PostGiroCode,
				ExemptionReason = ConfigurationSettings.WebService.ExemptionReason,
				InvoiceCurrencyCode = (CurrencyCodeContentType) ConfigurationSettings.WebService.CurrencyCodeId,
				InvoiceExpieryPenaltySurchargePercent = ConfigurationSettings.WebService.InvoiceExpieryPenaltySurchargePercent,
				InvoicePaymentTermsTextFormat = ConfigurationSettings.WebService.InvoicePaymentTermsTextFormat,
				InvoiceTypeCode = ConfigurationSettings.WebService.SvefakturaInvoiceTypeCode,
				SellingOrganizationCity = ConfigurationSettings.WebService.SellingOrganizationCity,
				SellingOrganizationContactEmail = ConfigurationSettings.WebService.SellingOrganizationContactEmail,
				SellingOrganizationContactName = ConfigurationSettings.WebService.SellingOrganizationContactName,
				SellingOrganizationCountry = GetSellingOrganizationCountry(),
				SellingOrganizationFax = ConfigurationSettings.WebService.SellingOrganizationFax,
				SellingOrganizationName = ConfigurationSettings.WebService.SellingOrganizationName,
				SellingOrganizationNumber = ConfigurationSettings.WebService.SellingOrganizationNumber,
				SellingOrganizationPostalCode = ConfigurationSettings.WebService.SellingOrganizationPostalCode,
				SellingOrganizationPostBox = ConfigurationSettings.WebService.SellingOrganizationPostBox,
				SellingOrganizationStreetName = ConfigurationSettings.WebService.SellingOrganizationStreetName,
				SellingOrganizationTelephone = ConfigurationSettings.WebService.SellingOrganizationTelephone,
				TaxAccountingCode = ConfigurationSettings.WebService.TaxAccountingCode,
				VATFreeReasonMessage = ConfigurationSettings.WebService.VATFreeReasonMessage,
			};
		}

		private static SFTICountryType GetSellingOrganizationCountry(){
			return new SFTICountryType {
				IdentificationCode = new CountryIdentificationCodeType {
					Value = (CountryIdentificationCodeContentType) ConfigurationSettings.WebService.SellingOrganizationCountryCode,
					name = ConfigurationSettings.WebService.SellingOrganizationCountryName
				}
			};
		}

		private static string GenerateInvoiceFileName(Invoice invoice) {
			var date = invoice.InterchangeHeader.DateOfPreparation.ToString(DateFormat);
			var referenceNumber = invoice.InterchangeControlReference;
			var invoiceNumber = invoice.DocumentNumber;
			return String.Format(EDIFileNameFormat, date, invoiceNumber, referenceNumber);
		}
		private static string GenerateInvoiceFileName(SFTIInvoiceType invoice) {
			var date = invoice.IssueDate.Value.ToString(DateFormat);
			var invoiceNumber = invoice.ID.Value;
			return String.Format(SvefakturaFileNameFormat, date, invoiceNumber);
		}

		private string UploadTextFile(string fileName, string fileContent) {
			try {
				var ftp = GetFtpClientObject();
				var usePassiveFtp = ConfigurationSettings.WebService.UsePassiveFTP;
				var encoding = ConfigurationSettings.WebService.FTPCustomEncodingCodePage;
				var useBinaryTransfer = ConfigurationSettings.WebService.FTPUseBinaryTransfer;
				var response = ftp.UploadStringAsFile(fileName, fileContent, usePassiveFtp, encoding, useBinaryTransfer);
				var responseStatusDescription = response.StatusDescription;
				//response.Close();
				CheckFtpUploadStatusDescriptionForErrorMessages(responseStatusDescription);
				return responseStatusDescription;
			}
			catch (Exception ex){
				throw LogAndCreateException("SynologenService.UploadTextFile failed", ex);
			}
		}

		private void TrySaveContentToDisk(string fileName, string fileContent) {
			var encoding = ConfigurationSettings.WebService.FTPCustomEncodingCodePage;
			TrySaveContentToDisk(fileName, fileContent, encoding);
		}
		private void TrySaveContentToDisk(string fileName, string fileContent, Encoding encoding) {
			try {
				var path = ConfigurationSettings.WebService.EDIFilesFolderPath;
				if(!Directory.Exists(path)) {Directory.CreateDirectory(path); }
				var filePath = Path.Combine(path, fileName);
				File.AppendAllText(filePath, fileContent, encoding);
			}
			catch(Exception ex) {
				TryLogErrorAndSendEmail("SynologenService.SaveContentToDisk failed: " +ex.Message);
			}
		}

		private static Ftp GetFtpClientObject() {
			var ftpUrl = ConfigurationSettings.WebService.FTPServerUrl;
			var ftpUserName = ConfigurationSettings.WebService.FTPUserName;
			var ftpPassword = ConfigurationSettings.WebService.FTPPassword;
			var credentials = new NetworkCredential { UserName = ftpUserName, Password = ftpPassword };
			return new Ftp(ftpUrl, credentials);
		}

		private static void CheckFtpUploadStatusDescriptionForErrorMessages(string statusDescription) {
			statusDescription = statusDescription.ToLower().Trim();
			if (!statusDescription.StartsWith(Ftp.FileTransferCompleteResponseCode)) {
				throw new WebserviceException("Ftp transmission failed: " + statusDescription);
			}
			if (statusDescription.Contains(FtpFileUploadNotAccepted)) {
				throw new WebserviceException("Ftp transmission reported EDI file was not accepted: " + statusDescription);
			}
			if (statusDescription.Contains(FtpFileUploadContainsError)) {
				throw new WebserviceException("Ftp transmission reported EDI file contains errors: " + statusDescription);
			}
		}

		private static Invoice GenerateEDIInvoice(IOrder order) {
			var ediSettings = GetEDISetting();
			var invoice = Convert.ToEDIInvoice(ediSettings, order);
			return invoice;
		}

		private static SFTIInvoiceType GenerateSvefakturaInvoice(IOrder order) {
			var settings = GetSvefakturaSettings();
			var invoice = Convert.ToSvefakturaInvoice(settings, order);
			return invoice;
		}

		#endregion

	}
}