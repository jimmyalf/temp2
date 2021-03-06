using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes;

namespace Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.CommonAggregateComponents {
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCode("xsd", "2.0.50727.42")]
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace="urn:sfti:CommonAggregateComponents:1:0")]
	[System.Xml.Serialization.XmlRoot("OrderLineReference", Namespace="urn:sfti:CommonAggregateComponents:1:0", IsNullable=false)]
	public class SFTIOrderLineReferenceType {
    
		private IdentifierType buyersLineIDField;
    
		private SFTIOrderReferenceType orderReferenceField;
    
		/// <remarks/>
		public IdentifierType BuyersLineID {
			get {
				return buyersLineIDField;
			}
			set {
				buyersLineIDField = value;
			}
		}
    
		/// <remarks/>
		public SFTIOrderReferenceType OrderReference {
			get {
				return orderReferenceField;
			}
			set {
				orderReferenceField = value;
			}
		}
	}
}