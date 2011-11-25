using System;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Presentation.Intranet.AcceptanceTest.TestHelpers;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.Orders;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.AcceptanceTest.Orders
{
    [TestFixture, Category("Pick_Customer")]
	public class When_picking_a_customer : SpecTestbase<PickCustomerPresenter,IPickCustomerView>
    {
        private PickCustomerPresenter _pickCustomerPresenter;

        public When_picking_a_customer()
		{
            
			Context = () =>
			{
				_pickCustomerPresenter = GetPresenter();
			};
			Story = () =>
			{
                return new Berättelse("Välja kund")
                    .FörAtt("välja en kund att knyta till nytt abonnemang")
                    .Som("inloggad användare på intranätet")
                    .VillJag("kunna spara nödvändig information om användaren");
			};
            
		}
        [Test]
        public void InformationenÄrKorrektIfylld()
        {
            SetupScenario(scenario => scenario
                .Givet(AttFormuläretÄrKorrektIfyllt)
                .När(AnvändarenFörsökerFortsättaTillStegTvå)
                .Så(FörflyttasAnvändarenTillVynFörStegTvå)
            );
        }

        [Test]
        public void AnvändarenVillHämtaKunduppgifterViaPersonnummer()
        {
            SetupScenario(scenario => scenario
                .Givet(AttEttPersonnummerÄrIfyllt)
                .När(AnvändarenKlickarHämta)
                .Så(FyllsFormuläretMedKunduppgifter)
            );
        }

        private void FyllsFormuläretMedKunduppgifter()
        {
            throw new NotImplementedException();
        }

        private void AnvändarenKlickarHämta()
        {
            throw new NotImplementedException();
        }

        private void AttEttPersonnummerÄrIfyllt()
        {
            throw new NotImplementedException();
        }

        private void FörflyttasAnvändarenTillVynFörStegTvå()
        {
            throw new NotImplementedException();
        }

        private void AnvändarenFörsökerFortsättaTillStegTvå()
        {
            throw new NotImplementedException();
        }

        private void AttFormuläretÄrKorrektIfyllt()
        {
            throw new NotImplementedException();
        }
    }   
}