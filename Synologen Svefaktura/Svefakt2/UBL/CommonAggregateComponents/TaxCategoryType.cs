using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes;
using PercentType=Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents.PercentType;

namespace Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonAggregateComponents {
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCode("xsd", "2.0.50727.42")]
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace="urn:oasis:names:tc:ubl:CommonAggregateComponents:1:0")]
	[System.Xml.Serialization.XmlRoot("cac:TaxCategory", Namespace="urn:oasis:names:tc:ubl:CommonAggregateComponents:1:0", IsNullable=false)]
	public class TaxCategoryType {
    
		private IdentifierType idField;
    
		private PercentType percentField;
    
		private ReasonType exemptionReasonField;
    
		private TaxSchemeType taxSchemeField;
    
		/// <remarks/>
		public IdentifierType ID {
			get {
				return idField;
			}
			set {
				idField = value;
			}
		}
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement(Namespace="urn:oasis:names:tc:ubl:CommonBasicComponents:1:0")]
		public PercentType Percent {
			get {
				return percentField;
			}
			set {
				percentField = value;
			}
		}
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement(Namespace="urn:oasis:names:tc:ubl:CommonBasicComponents:1:0")]
		public ReasonType ExemptionReason {
			get {
				return exemptionReasonField;
			}
			set {
				exemptionReasonField = value;
			}
		}
    
		/// <remarks/>
		public TaxSchemeType TaxScheme {
			get {
				return taxSchemeField;
			}
			set {
				taxSchemeField = value;
			}
		}
	}
}