using System.Collections.Generic;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Invoicing;
using Spinit.Wpc.Synologen.Invoicing.Types;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.CommonAggregateComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.UnspecializedDatatypes;

namespace Spinit.Wpc.Synologen.Unit.Test.Svefaktura.CustomValidation{
	[TestFixture]
	public class TestValidateSellerParty : AssertionHelper {
		[Test]
		public void Test_SellerParty_Party_PartyTaxScheme_SWT_Missing_ExemptionReason() {
			var invoice = new SFTISellerPartyType {Party = new SFTIPartyType {PartyTaxScheme = new List<SFTIPartyTaxSchemeType> {new SFTIPartyTaxSchemeType {ExemptionReason = null, TaxScheme = new SFTITaxSchemeType {ID = new IdentifierType {Value = "SWT"}}}}}};
			var ruleViolations = new List<RuleViolation>(SvefakturaValidator.ValidateObject(invoice));
			Expect(ruleViolations.Exists(x => x.PropertyName.Equals("SFTIPartyTaxSchemeType.ExemptionReason")), Is.True);
		}
	}
}