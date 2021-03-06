using System.Collections.Generic;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;

namespace Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonAggregateComponents {
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCode("xsd", "2.0.50727.42")]
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace="urn:oasis:names:tc:ubl:CommonAggregateComponents:1:0")]
	[System.Xml.Serialization.XmlRoot("TaxTotal", Namespace="urn:oasis:names:tc:ubl:CommonAggregateComponents:1:0", IsNullable=false)]
	public class TaxTotalType {
    
		private TaxAmountType totalTaxAmountField;

		private List<TaxSubTotalType> taxSubTotalField;
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement(Namespace="urn:oasis:names:tc:ubl:CommonBasicComponents:1:0")]
		public TaxAmountType TotalTaxAmount {
			get {
				return totalTaxAmountField;
			}
			set {
				totalTaxAmountField = value;
			}
		}
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement("TaxSubTotal")]
		public List<TaxSubTotalType> TaxSubTotal {
			get {
				return taxSubTotalField;
			}
			set {
				taxSubTotalField = value;
			}
		}
	}
}