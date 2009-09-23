using System.Collections.Generic;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes;

namespace Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonAggregateComponents {
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCode("xsd", "2.0.50727.42")]
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace="urn:oasis:names:tc:ubl:CommonAggregateComponents:1:0")]
	[System.Xml.Serialization.XmlRoot("cac:TransportHandlingUnit", Namespace="urn:oasis:names:tc:ubl:CommonAggregateComponents:1:0", IsNullable=false)]
	public class TransportHandlingUnitType {
    
		private IdentifierType idField;
    
		private CodeType unitTypeCodeField;
    
		private List<DespatchLineType> despatchLineField;
    
		private List<PackageType> actualPackageField = new List<PackageType>();
    
		private List<ReceiptLineType> receivedReceiptLineField = new List<ReceiptLineType>();
    
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
		public CodeType UnitTypeCode {
			get {
				return unitTypeCodeField;
			}
			set {
				unitTypeCodeField = value;
			}
		}
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement("DespatchLine")]
		public List<DespatchLineType> DespatchLine {
			get {
				return despatchLineField;
			}
			set {
				despatchLineField = value;
			}
		}
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement("ActualPackage")]
		public List<PackageType> ActualPackage {
			get {
				return actualPackageField;
			}
			set {
				actualPackageField = value;
			}
		}
    
		/// <remarks/>
		[System.Xml.Serialization.XmlElement("ReceivedReceiptLine")]
		public List<ReceiptLineType> ReceivedReceiptLine {
			get {
				return receivedReceiptLineField;
			}
			set {
				receivedReceiptLineField = value;
			}
		}
	}
}