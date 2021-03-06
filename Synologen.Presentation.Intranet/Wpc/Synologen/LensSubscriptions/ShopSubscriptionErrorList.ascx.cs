﻿using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Intranet.Models.LensSubscription;
using WebFormsMvp;
using WebFormsMvp.Web;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Wpc.Synologen.LensSubscriptions
{
	[PresenterBinding(typeof(ShopSubscriptionErrorListPresenter))]
	public partial class ShopSubscriptionErrorList : MvpUserControl<ShopSubscriptionErrorListModel>, IShopSubscriptionErrorListView
	{
		public int SubscriptionPageId { get; set; }
	}
}