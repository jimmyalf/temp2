﻿using System;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.EventArguments.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Presenters.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Views.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Models.LensSubscription;
using WebFormsMvp;
using WebFormsMvp.Web;

namespace Spinit.Wpc.Synologen.Presentation.Site.Wpc.Synologen
{
	[PresenterBinding(typeof(CreateCustomerPresenter))] 
	public partial class LensSubscriptionCreateCustomer : MvpUserControl<CreateCustomerModel>, ICreateCustomerView
	{
		public event EventHandler<SaveCustomerEventArgs> Submit;
		public int RedirectOnSavePageId { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			btnSave.Click += Save;
		}

		private void Save(object sender, EventArgs e)
		{
			if (Submit == null) return;
			Page.Validate();
			if (Page.IsValid == false) return;
			var args = new SaveCustomerEventArgs
			           	{
			           		AddressLineOne = txtAddressLineOne.Text,
			           		AddressLineTwo = txtAddressLineTwo.Text,
			           		City = txtCity.Text,
			           		CountryId = int.Parse(drpCountry.SelectedValue),
			           		Email = txtEmail.Text,
			           		FirstName = txtFirstName.Text,
			           		LastName = txtLastName.Text,
			           		MobilePhone = txtMobilePhone.Text,
			           		PersonalIdNumber = txtPersonalIdNumber.Text,
							PostalCode = txtPostalCode.Text,
			           		Phone = txtPhone.Text
			           	};
			Submit(this, args);
		}
	}
}