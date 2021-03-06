using System.Linq;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.SFTI.CommonAggregateComponents;
using Spinit.Wpc.Synologen.Svefaktura.Svefakt2.UBL.CommonBasicComponents;

namespace Spinit.Wpc.Synologen.Invoicing.Test.Svefaktura.Validation 
{
	[TestFixture]
	public class TestValidateBasePrice : AssertionHelper 
    {
		[Test]
		public void Test_Complete_BasePrice_Validates()
		{
		    var basePrice = new SFTIBasePriceType { PriceAmount = new PriceAmountType { Value = 123m } };
			var ruleViolations = SvefakturaValidator.ValidateObject(basePrice).ToList();
			Expect(ruleViolations.Count(), Is.EqualTo(0), SvefakturaValidator.FormatRuleViolations(ruleViolations));
		}

		[Test]
		public void Test_Invoice_Missing_ID_Fails_Validation()
		{
		    var basePrice = new SFTIBasePriceType { PriceAmount = null };
			var ruleViolations = SvefakturaValidator.ValidateObject(basePrice).ToList();
			Expect(ruleViolations.Count(x => x.PropertyName.Equals("SFTIBasePriceType.PriceAmount")), Is.EqualTo(1), SvefakturaValidator.FormatRuleViolations(ruleViolations));
		}		
	}
}