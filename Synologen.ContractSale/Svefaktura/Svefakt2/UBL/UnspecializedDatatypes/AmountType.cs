using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.SpecializedDatatypes;

namespace Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes {
	/// <remarks/>
	[System.Xml.Serialization.XmlInclude(typeof(UBLAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(TotalAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(TaxTotalAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(TaxAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(PriceAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(ExtensionTotalAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(ExtensionAmountType))]
	[System.Xml.Serialization.XmlInclude(typeof(CommonBasicComponents.AmountType))]
	[System.CodeDom.Compiler.GeneratedCode("xsd", "2.0.50727.42")]
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(TypeName="AmountType", Namespace="urn:oasis:names:tc:ubl:UnspecializedDatatypes:1:0")]
	public class AmountType : CoreComponentTypes.AmountType {
	}
}