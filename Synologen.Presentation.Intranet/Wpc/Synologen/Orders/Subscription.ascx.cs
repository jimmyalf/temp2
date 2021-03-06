﻿using System;
using System.Web.UI.WebControls;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.EventArguments.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.Orders;
using Spinit.Wpc.Synologen.Presentation.Intranet.Models.Orders;
using WebFormsMvp;
using WebFormsMvp.Web;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Wpc.Synologen.Orders
{
	[PresenterBinding(typeof(SubscriptionPresenter))] 
	public partial class Subscription : MvpUserControl<SubscriptionModel>, ISubscriptionView
	{
		public event EventHandler<HandleErrorEventArgs> HandleError;
		public event EventHandler<EventArgs> StopSubscription;
		public event EventHandler<EventArgs> StartSubscription;
		public int ReturnPageId { get; set; }
		public int CorrectionPageId { get; set; }
		public int SubscriptionItemDetailPageId { get; set; }
		public int SubscriptionResetPageId { get; set; }

		protected void SetHandled_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if(HandleError == null) return;
			var errorId = e.CommandName.ToInt();
			HandleError(this, new HandleErrorEventArgs(errorId));
		}

		protected void Start_Subscription(object sender, EventArgs e)
		{
			if(StartSubscription == null) return;
			StartSubscription(this, new EventArgs());
		}

		protected void Stop_Subscription(object sender, EventArgs e)
		{
			if(StopSubscription == null) return;
			StopSubscription(this, new EventArgs());
		}
	}
}