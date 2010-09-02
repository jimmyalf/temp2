using System;
using Spinit.Wpc.Synologen.Core.Domain.Model.FrameOrder;
using Spinit.Wpc.Synologen.Core.Extensions;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.EventArguments;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Presenters;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Views;
using Spinit.Wpc.Synologen.Presentation.Site.Models;
using WebFormsMvp;
using WebFormsMvp.Web;

namespace Spinit.Wpc.Synologen.Presentation.Site.Wpc.Synologen {
	[PresenterBinding(typeof(EditFrameOrderPresenter))] 
	public partial class FrameOrder : MvpUserControl<EditFrameOrderModel>, IEditFrameOrderView<EditFrameOrderModel> {
		public event EventHandler<EditFrameFormEventArgs> FrameSelected;
		public event EventHandler<EditFrameFormEventArgs> SubmitForm;
		public event EventHandler<EditFrameFormEventArgs> GlassTypeSelected;
		public int RedirectPageId { get; set; }


		protected void Page_Load(object sender, EventArgs e) {
			WireupEventProxy();
		}

		private void WireupEventProxy()
		{
			drpFrames.SelectedIndexChanged += (sender, e) => HandleEvent(FrameSelected);
			drpGlassTypes.SelectedIndexChanged += (sender, e) => HandleEvent(GlassTypeSelected);
			btnSave.Click += (sender, e) => HandleEvent(SubmitForm, true);
		}

		protected void HandleEvent(EventHandler<EditFrameFormEventArgs> eventhandler, bool validate)
		{
			if(eventhandler == null) return;
			var eventArgs = GetEventArgs();
			if(validate) {
				Page.Validate();
				eventArgs.PageIsValid = Page.IsValid;
			}
			eventhandler(this, eventArgs);
		}
		protected void HandleEvent(EventHandler<EditFrameFormEventArgs> eventhandler)
		{
			HandleEvent(eventhandler, false);
		}

		private EditFrameFormEventArgs GetEventArgs()
		{
			return new EditFrameFormEventArgs
			{
				SelectedFrameId = drpFrames.SelectedValue.ToIntOrDefault(0),
				SelectedGlassTypeId = drpGlassTypes.SelectedValue.ToIntOrDefault(0),
				SelectedPupillaryDistance = new EyeParameter
				{
					Left = drpPupillaryDistanceLeft.SelectedValue.ToDecimalOrDefault(int.MinValue),
					Right = drpPupillaryDistanceRight.SelectedValue.ToDecimalOrDefault(int.MinValue)
				},
				SelectedSphere = new EyeParameter
				{
					Left = drpSphereLeft.SelectedValue.ToDecimalOrDefault(int.MinValue),
					Right = drpSphereRight.SelectedValue.ToDecimalOrDefault(int.MinValue)
				},
				SelectedCylinder = new EyeParameter
				{
					Left = drpCylinderLeft.SelectedValue.ToDecimalOrDefault(int.MinValue),
					Right = drpCylinderRight.SelectedValue.ToDecimalOrDefault(int.MinValue)
				},
				SelectedAxis = new EyeParameter
				{
					Left = txtAxisLeft.Text.ToDecimalOrDefault(0),
					Right = txtAxisRight.Text.ToDecimalOrDefault(0)
				},
                SelectedAddition = new EyeParameter
                {
                	Left = drpAdditionLeft.SelectedValue.ToDecimalOrDefault(0),
					Right = drpAdditionRight.SelectedValue.ToDecimalOrDefault(0)
                },
                SelectedHeight = new EyeParameter
                {
                	Left = drpHeightLeft.SelectedValue.ToDecimalOrDefault(0),
					Right = drpHeightRight.SelectedValue.ToDecimalOrDefault(0)
                },
                Notes = txtNotes.Text
			};
		}

	}
}