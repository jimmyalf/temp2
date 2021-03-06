using System.Collections.Generic;
using System.Text;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Invoicing.Svefaktura.Formatters;
using Spinit.Wpc.Synologen.Invoicing.Types;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.CommonAggregateComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.Documents.BasicInvoice;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.Codelist;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes;

namespace Spinit.Wpc.Synologen.Invoicing.Svefaktura.PartBuilders
{
    public class InvoiceInformationBuilder : PartBuilderBase, ISvefakturaPartBuilder
    {
        public InvoiceInformationBuilder(ISvefakturaConversionSettings settings, ISvefakturaFormatter formatter)
            : base(settings, formatter) { }

        public virtual void Build(IOrder order, SFTIInvoiceType invoice)
        {
            invoice.Note = GetTextEntity<NoteType>(GetFreeText(order));
            invoice.IssueDate = new IssueDateType { Value = Settings.InvoiceIssueDate };
            invoice.InvoiceTypeCode = new CodeType { Value = Settings.InvoiceTypeCode };
            invoice.ID = new SFTISimpleIdentifierType { Value = order.InvoiceNumber.ToString() };
            invoice.AdditionalDocumentReference = GetAdditionalDocumentReference(order);
            invoice.RequisitionistDocumentReference = GetRequisitionDocumentReference(order);
            invoice.InvoiceCurrencyCode = new CurrencyCodeType { Value = Settings.InvoiceCurrencyCode.GetValueOrDefault() };
            invoice.TaxPointDate = new TaxPointDateType { Value = order.CreatedDate };
        }

        protected virtual string GetFreeText(IOrder order)
        {
            const string ShopFormat = "{Name} ({OrganizationNumber}){NewLine}{AddressLine}{NewLine}{ZipAndCity}";
            var output = new StringBuilder();
            output.AppendLine("Synl�sningen g�ller:");
            output.Append(order.ParseFreeText());
            output.AppendLine();
            output.AppendLine("S�ljare av vara/tj�nst:");
            output.AppendLine(order.SellingShop.Format(ShopFormat));
            return output.ToString();
        }

        protected virtual List<SFTIDocumentReferenceType> GetRequisitionDocumentReference(IOrder order)
        {
            if (string.IsNullOrEmpty(order.RstText))
            {
                return null;
            }

            return new List<SFTIDocumentReferenceType>
            {
                new SFTIDocumentReferenceType { ID = new IdentifierType { Value = order.RstText } }
            };
        }

        protected virtual List<SFTIDocumentReferenceType> GetAdditionalDocumentReference(IOrder order)
        {
            return null;
        }
    }
}