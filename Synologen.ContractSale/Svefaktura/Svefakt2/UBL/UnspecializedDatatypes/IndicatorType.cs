using Spinit.Wpc.Synologen.Svefaktura.CustomEnumerations;
using Spinit.Wpc.Synologen.Svefaktura.CustomTypes;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;

namespace Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes {
	/// <remarks/>
	[System.Xml.Serialization.XmlInclude(typeof(RefrigerationOnIndicatorType))]
	[System.Xml.Serialization.XmlInclude(typeof(MaterialIndicatorType))]
	[System.Xml.Serialization.XmlInclude(typeof(MarkCareIndicatorType))]
	[System.Xml.Serialization.XmlInclude(typeof(MarkAttentionIndicatorType))]
	[System.Xml.Serialization.XmlInclude(typeof(CommonBasicComponents.IndicatorType))]
	[System.Xml.Serialization.XmlInclude(typeof(CopyIndicatorType))]
	[System.Xml.Serialization.XmlInclude(typeof(ChargeIndicatorType))]
	[System.CodeDom.Compiler.GeneratedCode("xsd", "2.0.50727.42")]
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace="urn:oasis:names:tc:ubl:UnspecializedDatatypes:1:0")]
	public class IndicatorType {
    
		private bool valueField;
    
		/// <remarks/>
		[System.Xml.Serialization.XmlText]
		[PropertyValidationRule("Value Required",ValidationType.RequiredNotNull)]
		public bool Value {
			get {
				return valueField;
			}
			set {
				valueField = value;
			}
		}
	}
}