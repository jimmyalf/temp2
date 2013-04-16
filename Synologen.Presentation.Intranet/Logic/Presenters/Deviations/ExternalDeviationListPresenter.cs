﻿using System;
using NHibernate;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.Deviations;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.Deviations
{
	public class ExternalDeviationListPresenter : DeviationPresenter<IExternalDeviationListView>
	{
		public ExternalDeviationListPresenter(IExternalDeviationListView view, ISession session) : base(view, session)
		{
			View.Load += View_Load;
		}

		public void View_Load(object sender, EventArgs e)
		{
			// TODO: Implement
		}

		public override void ReleaseView()
		{
			View.Load -= View_Load;
		}
	}
}