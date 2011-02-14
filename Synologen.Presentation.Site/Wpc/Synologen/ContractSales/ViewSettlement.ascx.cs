using System;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Presenters.ContractSales;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Views.ContractSales;
using Spinit.Wpc.Synologen.Presentation.Site.Models.ContractSales;
using WebFormsMvp;
using WebFormsMvp.Web;

namespace Spinit.Wpc.Synologen.Presentation.Site.Wpc.Synologen.ContractSales
{
	[PresenterBinding(typeof(ViewSettlementPresenter))]
	public partial class ViewSettlement : MvpUserControl<ViewSettlementModel>, IViewSettlementView
	{
		public event EventHandler SwitchView;
		public event EventHandler MarkAllSaleItemsAsPayed;

		public int SubscriptionPageId { get; set; }

		protected void btnSwitchView_Click(object sender, EventArgs e) 
		{ 
			if(SwitchView == null) return;
			SwitchView(sender, e);
		}

		protected void btnMarkAsPayed_Click(object sender, EventArgs e) 
		{ 
			if(SwitchView == null) return;
			MarkAllSaleItemsAsPayed(sender, e);
		}
	}
}