﻿using System;
using System.Collections.Generic;
using Spinit.Wpc.Synologen.Business.Interfaces;
using Spinit.Wpc.Synologen.Data.Types;
using Spinit.Wpc.Synologen.EDI;
using Spinit.Wpc.Synologen.EDI.Common.Types;
using Spinit.Wpc.Synologen.EDI.Types;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.CommonAggregateComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.Documents.BasicInvoice;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.Codelist;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes;
using Spinit.Wpc.Synologen.Utility.Types;
using NameType=Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents.NameType;
using PercentType=Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents.PercentType;
using QuantityType=Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents.QuantityType;

namespace Spinit.Wpc.Synologen.Utility {
	public static class Convert {

		public static Invoice ToEDIInvoice(EDIConversionSettings EDISettings, OrderRow order, List<IOrderItem> orderItems, CompanyRow company, IShop shop) {
			var invoiceValueIncludingVAT = System.Convert.ToSingle(order.InvoiceSumIncludingVAT);
			var invoiceValueExcludingVAT = System.Convert.ToSingle(order.InvoiceSumExcludingVAT);
			var interchangeHeader = new InterchangeHeader {RecipientId = company.EDIRecipientId, SenderId = EDISettings.SenderId};
			var invoiceExpieryDate = interchangeHeader.DateOfPreparation.AddDays(company.PaymentDuePeriod);
			var invoice = new Invoice(EDISettings.VATAmount, EDISettings.NumberOfDecimalsUsedAtRounding, invoiceValueIncludingVAT, invoiceValueExcludingVAT) {
				Articles = ToEDIArticles(orderItems, order, company),
				Buyer = GetBuyerInformation(company.EDIRecipientId, company),
             	BuyerOrderNumber = String.Empty,
             	BuyerRSTNumber = order.RstText,
             	DocumentNumber = order.InvoiceNumber.ToString(),
				InterchangeHeader = interchangeHeader,
             	InvoiceCreatedDate = order.CreatedDate,
				InvoiceSetting = new InvoiceSetting { InvoiceCurrency = EDISettings.InvoiceCurrencyCode, InvoiceExpiryDate = invoiceExpieryDate },
             	VendorOrderNumber = order.Id.ToString(),
				Supplier = GetSupplierInformation(EDISettings.SenderId, EDISettings.BankGiro,EDISettings.Postgiro, shop)
             };
			return invoice;
		}

		public static SFTIInvoiceType ToSvefakturaInvoice(SvefakturaConversionSettings settings, OrderRow order, List<IOrderItem> orderItems, CompanyRow company, ShopRow shop) {
			var invoice = new SFTIInvoiceType();
			TryAddSettingsInformation(invoice, settings, company);
			TryAddBuyerPartyInformation(invoice, company, order);
			//TryAddSellerPartyInformation(invoice, shop);
			TryAddOrderInformation(invoice, order);
			TryAddOrderItems(invoice, orderItems);
			TryAddPaymentTermsInformation(invoice, settings, company);
			TryAddPaymentDueDate(invoice, company);
			return invoice;
		}

		#region Svefaktura Helper Methods
		private static void TryAddTaxInformation(SFTIInvoiceType invoice, decimal VATAmount) {
			if (VATAmount == 0) return;
			if(invoice.TaxTotal == null) invoice.TaxTotal = new List<SFTITaxTotalType>();
			invoice.TaxTotal.Add( 
				new SFTITaxTotalType {
					TaxSubTotal = new List<SFTITaxSubTotalType> {
						new SFTITaxSubTotalType {
							TaxCategory = new SFTITaxCategoryType {
								ID = new IdentifierType { Value = "S" }, 
								Percent = new PercentType { Value = VATAmount * 100 }
							}
						}
					}
				}				
			);
		}

		private static void TryAddPaymentMeans(SFTIInvoiceType invoice, string giroNumber, string giroBIC, CompanyRow company) {
			if (String.IsNullOrEmpty(giroNumber)) return;
			if (invoice.PaymentMeans == null) invoice.PaymentMeans = new List<SFTIPaymentMeansType>();
				invoice.PaymentMeans.Add(
					new SFTIPaymentMeansType {
						PaymentMeansTypeCode = new PaymentMeansCodeType {
							Value = PaymentMeansCodeContentType.Item1
						},
						PayeeFinancialAccount = new SFTIFinancialAccountType {
							ID = new IdentifierType { Value = giroNumber },
							FinancialInstitutionBranch = (String.IsNullOrEmpty(giroBIC)) ? null :
							new SFTIBranchType { 
								FinancialInstitution = new SFTIFinancialInstitutionType {
									ID = new IdentifierType { Value = giroBIC }
								}
							}
						}
					}
				);
		}

		private static void TryAddSettingsInformation(SFTIInvoiceType invoice, SvefakturaConversionSettings settings, CompanyRow company) {
			if (invoice.SellerParty == null) invoice.SellerParty = new SFTISellerPartyType();
			invoice.SellerParty.Party = new SFTIPartyType {
      			PartyName =  String.IsNullOrEmpty(settings.SellingOrganizationName)? null : new List<NameType>{new NameType{Value=settings.SellingOrganizationName}},
				Address = new SFTIAddressType {
					StreetName = String.IsNullOrEmpty(settings.SellingOrganizationAddress) ? null : new StreetNameType { Value = settings.SellingOrganizationAddress },
					PostalZone = String.IsNullOrEmpty(settings.SellingOrganizationPostalCode) ? null : new ZoneType{Value=settings.SellingOrganizationPostalCode},
                    CityName = String.IsNullOrEmpty(settings.SellingOrganizationCity) ? null : new CityNameType{Value=settings.SellingOrganizationCity}
				},
				PartyTaxScheme =  new List<SFTIPartyTaxSchemeType> {
               		new SFTIPartyTaxSchemeType {
                   		RegistrationAddress = (!settings.SellingOrganizationCountryCode.HasValue)? null : new SFTIAddressType{Country= new SFTICountryType{IdentificationCode = new CountryIdentificationCodeType{Value = settings.SellingOrganizationCountryCode.Value}}},
						CompanyID = (String.IsNullOrEmpty(settings.SellingOrganizationNumber))? null : new IdentifierType{Value = settings.SellingOrganizationNumber},
						TaxScheme = (String.IsNullOrEmpty(settings.SellingOrganizationNumber))? null : new SFTITaxSchemeType{ID=new IdentifierType{Value="VAT"}}
					}
				}
			};
			if(settings.InvoiceIssueDate != DateTime.MinValue){
				invoice.IssueDate = new IssueDateType {Value = settings.InvoiceIssueDate};
			}
			if(!String.IsNullOrEmpty(settings.InvoiceTypeCode)){
				invoice.InvoiceTypeCode = new CodeType {Value = settings.InvoiceTypeCode};
			}
			TryAddPaymentMeans(invoice, settings.BankGiro, settings.BankgiroBankIdentificationCode, company);
			TryAddPaymentMeans(invoice, settings.Postgiro, settings.PostgiroBankIdentificationCode, company);
			TryAddTaxInformation(invoice, settings.VATAmount);
		}

		private static string TryParseInvoicePaymentTermsFormat(string format, CompanyRow company) {
			if(format == null || company == null) return null;
			format = format.Replace("{InvoiceNumberOfDueDays}", company.PaymentDuePeriod.ToString());
			return format;
		}

		private static void TryAddBuyerPartyInformation(SFTIInvoiceType invoice, CompanyRow company, OrderRow orderRow) {
			if (invoice.BuyerParty == null) invoice.BuyerParty = new SFTIBuyerPartyType();
			invoice.BuyerParty.Party = new SFTIPartyType();

			TrySetInvoiceBuyerAddressInformation(invoice, company, orderRow);

			if (!String.IsNullOrEmpty(orderRow.CustomerCombinedName)) {
				if (invoice.BuyerParty.Party.Contact == null) invoice.BuyerParty.Party.Contact = new SFTIContactType();
				invoice.BuyerParty.Party.Contact.Name = new NameType {Value = orderRow.CustomerCombinedName};
			}
			if (!String.IsNullOrEmpty(orderRow.Email)) {
				if (invoice.BuyerParty.Party.Contact == null) invoice.BuyerParty.Party.Contact = new SFTIContactType();
				invoice.BuyerParty.Party.Contact.ElectronicMail = new MailType {Value = orderRow.Email};
			}
			if (!String.IsNullOrEmpty(orderRow.Phone)) {
				if (invoice.BuyerParty.Party.Contact == null) invoice.BuyerParty.Party.Contact = new SFTIContactType();
				invoice.BuyerParty.Party.Contact.Telephone = new TelephoneType {Value = orderRow.Phone};
			}
			
			if(!String.IsNullOrEmpty(company.TaxAccountingCode)) {
				invoice.BuyerParty.Party.PartyTaxScheme =
					new List<SFTIPartyTaxSchemeType> {
						new SFTIPartyTaxSchemeType {
							CompanyID = new IdentifierType{Value=company.TaxAccountingCode},
							TaxScheme = new SFTITaxSchemeType{ID=new IdentifierType{Value="VAT"}}
						                           	
						}
					};
			}
			if (!String.IsNullOrEmpty(company.InvoiceCompanyName)) {
				invoice.BuyerParty.Party.PartyName = 
					new List<NameType> { new NameType {Value = company.InvoiceCompanyName} };
			}
			if (!String.IsNullOrEmpty(company.OrganizationNumber)) {
				invoice.BuyerParty.Party.PartyIdentification = 
					new List<SFTIPartyIdentificationType> {
						new SFTIPartyIdentificationType {
							ID = new IdentifierType {
								Value = company.OrganizationNumber
							}
						}
					};
			}
		}

		private static void TrySetInvoiceBuyerAddressInformation(SFTIInvoiceType invoice, ICompany company, IOrder orderRow) {
			if(!String.IsNullOrEmpty(company.Address1)) {
				if (invoice.BuyerParty.Party.Address == null) invoice.BuyerParty.Party.Address = new SFTIAddressType();
				invoice.BuyerParty.Party.Address.Postbox = new PostboxType { Value = company.Address1 };
			}
			if (!String.IsNullOrEmpty(company.Address2)) {
				if (invoice.BuyerParty.Party.Address == null) invoice.BuyerParty.Party.Address = new SFTIAddressType();
				invoice.BuyerParty.Party.Address.StreetName = new StreetNameType { Value = company.Address2 };
			}
			if (!String.IsNullOrEmpty(company.Zip)) {
				if (invoice.BuyerParty.Party.Address == null) invoice.BuyerParty.Party.Address = new SFTIAddressType();
				invoice.BuyerParty.Party.Address.PostalZone = new ZoneType {Value = company.Zip};
			}
			if (!String.IsNullOrEmpty(company.City)) {
				if (invoice.BuyerParty.Party.Address == null) invoice.BuyerParty.Party.Address = new SFTIAddressType();
				invoice.BuyerParty.Party.Address.CityName = new CityNameType {Value = company.City};
			}
			if (!String.IsNullOrEmpty(orderRow.CompanyUnit)) {
				if (invoice.BuyerParty.Party.Address == null) invoice.BuyerParty.Party.Address = new SFTIAddressType();
				invoice.BuyerParty.Party.Address.Department = new DepartmentType {Value = orderRow.CompanyUnit};
			}
		}

		private static void TryAddOrderInformation(SFTIInvoiceType invoice, OrderRow order ) {
			if(order.InvoiceNumber > 0){
				invoice.ID = new SFTISimpleIdentifierType {Value = order.InvoiceNumber.ToString()};
			}
			if(order.InvoiceSumIncludingVAT > 0){
				if (invoice.LegalTotal == null) invoice.LegalTotal = new SFTILegalTotalType();
				invoice.LegalTotal.TaxInclusiveTotalAmount = new TotalAmountType { Value = (decimal)order.InvoiceSumIncludingVAT, amountCurrencyID = "SEK" };
			}
			if(order.InvoiceSumExcludingVAT > 0){
				if (invoice.LegalTotal == null) invoice.LegalTotal = new SFTILegalTotalType();
				invoice.LegalTotal.TaxExclusiveTotalAmount = new TotalAmountType { Value = (decimal)order.InvoiceSumExcludingVAT, amountCurrencyID = "SEK" };
			}
			if(!String.IsNullOrEmpty(order.CustomerOrderNumber)){
				if(invoice.RequisitionistDocumentReference == null){
					invoice.RequisitionistDocumentReference = new List<SFTIDocumentReferenceType> {new SFTIDocumentReferenceType()};
				}
				foreach (var requisitionistDocumentReference in invoice.RequisitionistDocumentReference){
					requisitionistDocumentReference.ID = new IdentifierType {Value = order.CustomerOrderNumber};
				}
			}
			if(order.InvoiceSumIncludingVAT > 0 && order.InvoiceSumExcludingVAT > 0){
				var totalTaxAmount = order.InvoiceSumIncludingVAT - order.InvoiceSumExcludingVAT;
				if(invoice.TaxTotal == null) invoice.TaxTotal = new List<SFTITaxTotalType>{new SFTITaxTotalType()};
				foreach (var taxTotal in invoice.TaxTotal){
					taxTotal.TotalTaxAmount = new TaxAmountType {Value = (decimal) totalTaxAmount, amountCurrencyID = "SEK"};
				}
			}
			if(!String.IsNullOrEmpty(order.CustomerOrderNumber)){
				invoice.RequisitionistDocumentReference = new List<SFTIDocumentReferenceType> {
					new SFTIDocumentReferenceType {
						ID = new IdentifierType {
							Value = order.CustomerOrderNumber
						}
					}
				};
			}
			
		}

		private static void TryAddOrderItems(SFTIInvoiceType invoice, IEnumerable<IOrderItem> orderItems ) {
			if(invoice.InvoiceLine == null) invoice.InvoiceLine = new List<SFTIInvoiceLineType>();
			foreach (var orderItem in orderItems){
				invoice.InvoiceLine.Add(
					new SFTIInvoiceLineType {
						Item = new SFTIItemType {
							Description = new DescriptionType {Value = orderItem.ArticleDisplayName},
							StandardItemIdentification = new SFTIItemIdentificationType {ID = new IdentifierType {Value = orderItem.ArticleDisplayNumber}},
							BasePrice = new SFTIBasePriceType {
								PriceAmount = new PriceAmountType { Value = (decimal) orderItem.SinglePrice, amountCurrencyID="SEK" }
							}
						},
						InvoicedQuantity = new QuantityType{Value = orderItem.NumberOfItems, quantityUnitCode = "styck"},
                        LineExtensionAmount = new ExtensionAmountType { Value = (decimal) orderItem.DisplayTotalPrice, amountCurrencyID="SEK" }
					}
				);
			}
			
		}

		private static void TryAddPaymentTermsInformation(SFTIInvoiceType invoice, SvefakturaConversionSettings settings, CompanyRow company) {
			if (String.IsNullOrEmpty(settings.InvoicePaymentTermsTextFormat) && !settings.InvoiceExpieryPenaltySurchargePercent.HasValue) return;
			var text = TryParseInvoicePaymentTermsFormat(settings.InvoicePaymentTermsTextFormat, company);
			invoice.PaymentTerms = new SFTIPaymentTermsType {
				Note = (String.IsNullOrEmpty(text)) ? null: new NoteType {Value = text},
				PenaltySurchargePercent = (settings.InvoiceExpieryPenaltySurchargePercent.HasValue) ? new SurchargePercentType {Value = settings.InvoiceExpieryPenaltySurchargePercent.Value} : null
			};
	}

		private static void TryAddPaymentDueDate(SFTIInvoiceType invoice, CompanyRow company) {
		    if (company.PaymentDuePeriod <= 0) return;
		    if(invoice.IssueDate == null) return;
		    if(invoice.PaymentMeans == null) invoice.PaymentMeans = new List<SFTIPaymentMeansType>{new SFTIPaymentMeansType()};
		    foreach (var paymentMean in invoice.PaymentMeans){
		        paymentMean.DuePaymentDate = new PaymentDateType {
		            Value = invoice.IssueDate.Value.AddDays(company.PaymentDuePeriod)
		        };
		    }
		}

		#endregion

		#region EDI Helper Methods
		private static Supplier GetSupplierInformation(string supplierId, string bankGiro, string postGiro, IShop shop) {
			var supplier = new Supplier {
				BankGiroNumber = bankGiro,
				PostGiroNumber = postGiro,
				Contact = new Contact {
					ContactInfo = shop.Name,
					Email = shop.Email,
					Fax = shop.Fax,
					Telephone = shop.Phone
				},
				SupplierIdentity = supplierId
			};
			return supplier;
		}

		private static Buyer GetBuyerInformation(string buyerId, ICompany company) {
			var buyer = new Buyer {                	
				BuyerIdentity = buyerId,
                InvoiceIdentity = company.BankCode,
				DeliveryAddress = new Address {
      				Address1 = company.Address1, 
					Address2 = company.Address2, 
					City = company.City, 
					Zip = company.Zip
				  }
			};
			return buyer;
		}

		public static InvoiceRow ToEDIArticle(IOrderItem orderItem, int invoiceRowNumber) {
			var EDIitem = new InvoiceRow {
              	ArticleName = orderItem.ArticleDisplayName,
              	ArticleNumber = orderItem.ArticleDisplayNumber,
              	Quantity = orderItem.NumberOfItems,
              	RowNumber = invoiceRowNumber,
              	SinglePriceExcludingVAT = orderItem.SinglePrice,
              	ArticleDescription = orderItem.Notes,
				NoVAT = orderItem.NoVAT
			  };

			return EDIitem;
		}

		public static InvoiceRow ToEDIFreeTextInformationRow(IList<string> listOfFreeTextRows) {
			var eDIitem = new InvoiceRow {FreeTextRows = new List<string>(listOfFreeTextRows), UseInvoiceRowAsFreeTextRow = true, RowNumber = 1};
			return eDIitem;
		}

		public static List<string> GetOrderBuyerInformation(OrderRow order) {
			var listOfStrings = new List<string>();
			if (!String.IsNullOrEmpty(order.CustomerFirstName) && !String.IsNullOrEmpty(order.CustomerLastName)){
				listOfStrings.Add(String.Format("Beställare Namn, {0} {1}", order.CustomerFirstName, order.CustomerLastName));
			}
			if (!String.IsNullOrEmpty(order.PersonalIdNumber)){
				listOfStrings.Add(String.Format("Beställare Personnummer, {0}", order.PersonalIdNumber));
			}
			if(!String.IsNullOrEmpty(order.CompanyUnit)){
				listOfStrings.Add(String.Format("Beställare Enhet, {0}", order.CompanyUnit));
			}
			return listOfStrings;
		}

		//public static List<InvoiceRow> ToEDIArticles(List<IOrderItem> orderItems) {
		//    var EDIArticles = new List<InvoiceRow>();
		//    var articleCounter = 1;
		//    foreach(var item in orderItems) {
		//        EDIArticles.Add(ToEDIArticle(item, articleCounter));
		//        articleCounter++;
		//    }
		//    return EDIArticles;
		//}

		public static List<InvoiceRow> ToEDIArticles(List<IOrderItem> orderItems, OrderRow order, CompanyRow company) {
			var EDIArticles = new List<InvoiceRow>();
			var articleCounter = 1;
			//Add one freetextRow if any information is available
			//var freeTextRows = GetOrderBuyerInformation(order);
			var freeTextRows = GetFreeTextRows(company, order);
			if(freeTextRows!=null && freeTextRows.Count>0){
				var freeTextBuyerInvoiceRow = ToEDIFreeTextInformationRow(freeTextRows);
				EDIArticles.Add(freeTextBuyerInvoiceRow);
				articleCounter = 2;
			}
			foreach (var item in orderItems) {
				var ediArticle = ToEDIArticle(item, articleCounter);
				EDIArticles.Add(ediArticle);
				articleCounter++;
			}
			return EDIArticles;
		}

		public static IList<string> GetFreeTextRows(CompanyRow company, OrderRow order) {
			if (String.IsNullOrEmpty(company.InvoiceFreeTextFormat)) return new List<string>();
			var parsedInvoiceFreeText = ParseInvoiceFreeTeext(company.InvoiceFreeTextFormat, order);
			return parsedInvoiceFreeText.Trim().Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
		}

		public static string ParseInvoiceFreeTeext(string invoiceFreeTextFormat, OrderRow order) {
			invoiceFreeTextFormat = invoiceFreeTextFormat.Replace("{CustomerName}", order.CustomerCombinedName ?? String.Empty);
			invoiceFreeTextFormat = invoiceFreeTextFormat.Replace("{CustomerPersonalIdNumber}", order.PersonalIdNumber ?? String.Empty);
			invoiceFreeTextFormat = invoiceFreeTextFormat.Replace("{CompanyUnit}", order.CompanyUnit ?? String.Empty);
			invoiceFreeTextFormat = invoiceFreeTextFormat.Replace("{CustomerPersonalBirthDateString}", order.PersonalBirthDateString ?? String.Empty);
			invoiceFreeTextFormat = invoiceFreeTextFormat.Replace("{CustomerFirstName}", order.CustomerFirstName ?? String.Empty);
			invoiceFreeTextFormat = invoiceFreeTextFormat.Replace("{CustomerLastName}", order.CustomerLastName ?? String.Empty);
			return invoiceFreeTextFormat;
		}

		#endregion
	}
}