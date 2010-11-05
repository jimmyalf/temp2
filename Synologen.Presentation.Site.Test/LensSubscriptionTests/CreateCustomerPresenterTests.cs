﻿using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Shouldly;
using Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.EventArguments.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Presenters.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Views.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Models.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Site.Test.LensSubscriptionTests.Factories;
using Spinit.Wpc.Synologen.Presentation.Site.Test.MockHelpers;

namespace Spinit.Wpc.Synologen.Presentation.Site.Test.LensSubscriptionTests
{
	[TestFixture]
	[Category("CreateCustomerPresenterTester")]
	public class  When_loading_create_customer_view
	{
		protected CreateCustomerPresenter _presenter;
		private readonly ICreateCustomerView _view;
		private readonly Mock<ICountryRepository> _mockedCountryRepository;
		private readonly Mock<IShopRepository> _mockedShopRepository;
		private readonly Mock<ICustomerRepository> _mockedCustomerRepository;
		private readonly Mock<ISynologenMemberService> _mockedSynologenMemberService;
		private readonly Country[] _countryList;

		public When_loading_create_customer_view()
		{
			//Arrange

			_countryList = CountryFactory.GetList().ToArray();

			var mockedView = new Mock<ICreateCustomerView>();

			_mockedCountryRepository = new Mock<ICountryRepository>();
			_mockedCountryRepository.Setup(x => x.GetAll()).Returns(_countryList);

			_mockedShopRepository = new Mock<IShopRepository>();
			_mockedShopRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new Shop());

			_mockedSynologenMemberService = new Mock<ISynologenMemberService>();
			_mockedSynologenMemberService.Setup(x => x.ShopHasAccessTo(ShopAccess.LensSubscription)).Returns(true);

			_mockedCustomerRepository = new Mock<ICustomerRepository>();
			Func<Country, CountryListItemModel> converter = (country) => new CountryListItemModel { Value = country.Id.ToString(), Text = country.Name };
			mockedView.SetupGet(x => x.Model).Returns(new CreateCustomerModel { List = _countryList.Select(converter) });
			_view = mockedView.Object;

			_presenter = new CreateCustomerPresenter(_view, 
													_mockedCustomerRepository.Object, 
													_mockedShopRepository.Object, 
													_mockedCountryRepository.Object,
													_mockedSynologenMemberService.Object);

			//Act
			_presenter.View_Load(null, new EventArgs());
		}

		[Test]
		public void Model_should_have_expected_values()
		{
			_view.Model.ShopDoesNotHaveAccessToLensSubscriptions.ShouldBe(false);
			_view.Model.DisplayForm.ShouldBe(true);
			_view.Model.List.Count().ShouldBe(4);
		}
		
	}

	[TestFixture]
	[Category("CreateCustomerPresenterTester")]
	public class When_submitting_create_customer_view
	{

		protected CreateCustomerPresenter _presenter;
		private readonly ICreateCustomerView _view;
		private readonly Mock<IShopRepository> _mockedShopRepository;
		private readonly Mock<ICountryRepository> _mockedCountryRepository;
		private readonly Mock<ICustomerRepository> _mockedCustomerRepository;
		private readonly Mock<ISynologenMemberService> _mockedSynologenMemberService;
		private readonly SaveCustomerEventArgs _saveEventArgs;
		private readonly Country[] _countryList;
		private readonly Country _selectedCountry;
		private readonly int _shopId;
		private readonly HttpContextMock _mockedHttpContext;
		private readonly string _redirectUrl;
		private readonly int _redirectPageId;

		public When_submitting_create_customer_view()
		{
			// Arrange
			_redirectPageId = 55;
			_redirectUrl = "/test/redirect/";
			_mockedHttpContext = new HttpContextMock();

			_shopId = 5;
			const int selectedCountryId = 5;
			_countryList = CountryFactory.GetList().ToArray();
			_selectedCountry = CountryFactory.Get(selectedCountryId);
			var mockedShop = new Mock<Shop>();
			mockedShop.SetupGet(x => x.Id).Returns(_shopId);

			_mockedCountryRepository = new Mock<ICountryRepository>();
			_mockedCountryRepository.Setup(x => x.GetAll()).Returns(_countryList);
			_mockedCountryRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(_selectedCountry);

			var mockedView = new Mock<ICreateCustomerView>();
			mockedView.SetupGet(x => x.Model).Returns(new CreateCustomerModel());
			mockedView.SetupGet(x => x.RedirectOnSavePageId).Returns(_redirectPageId);
			_view = mockedView.Object;

			_mockedShopRepository = new Mock<IShopRepository>();
			_mockedShopRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(mockedShop.Object);

			_mockedCustomerRepository = new Mock<ICustomerRepository>();
			
			_mockedSynologenMemberService = new Mock<ISynologenMemberService>();
			_mockedSynologenMemberService.Setup(x => x.ShopHasAccessTo(ShopAccess.LensSubscription)).Returns(true);
			_mockedSynologenMemberService.Setup(x => x.GetCurrentShopId()).Returns(_shopId);
			_mockedSynologenMemberService.Setup(x => x.GetPageUrl(It.IsAny<int>())).Returns(_redirectUrl);

			_presenter = new CreateCustomerPresenter(
				_view, 
				_mockedCustomerRepository.Object, 
				_mockedShopRepository.Object,                                   
				_mockedCountryRepository.Object, 
				_mockedSynologenMemberService.Object
			)
			{
				HttpContext = _mockedHttpContext.Object
			};
			
			// Act
			_saveEventArgs = new SaveCustomerEventArgs
			{
				FirstName = "Carina",
				LastName = "Melander",
				AddressLineOne = "Vinkelslipsgatan 32",
				AddressLineTwo = "Uppgång 3H",
				City = "Storstad",
				CountryId = _selectedCountry.Id,
				Email = "carina.melander@gmail.com",
				MobilePhone = "0704-565675",
				PersonalIdNumber = "8106296729",
				Phone = "0783-45674537",
				PostalCode = "688 44"
			};

			_presenter.View_Load(null, new EventArgs());
			_presenter.View_Submit(null, _saveEventArgs);
		}

		[Test]
		public void Presenter_gets_expected_customer()
		{
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.FirstName.Equals(_saveEventArgs.FirstName))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.LastName.Equals(_saveEventArgs.LastName))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Address.AddressLineOne.Equals(_saveEventArgs.AddressLineOne))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Address.AddressLineTwo.Equals(_saveEventArgs.AddressLineTwo))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Address.City.Equals(_saveEventArgs.City))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Address.Country.Id.Equals(_saveEventArgs.CountryId))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Contact.Email.Equals(_saveEventArgs.Email))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Contact.MobilePhone.Equals(_saveEventArgs.MobilePhone))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.PersonalIdNumber.Equals(_saveEventArgs.PersonalIdNumber))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Contact.Phone.Equals(_saveEventArgs.Phone))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Address.PostalCode.Equals(_saveEventArgs.PostalCode))));
			_mockedCustomerRepository.Verify(x => x.Save(It.Is<Customer>(c => c.Shop.Id.Equals(_shopId))));
		}

		[Test]
		public void Presenter_fetches_expected_country_and_shop()
		{
			_mockedCountryRepository.Verify(x => x.Get(It.Is<int>(id => id.Equals(_selectedCountry.Id))));
			_mockedShopRepository.Verify(x => x.Get(It.Is<int>(id => id.Equals(_shopId))));
		}

		[Test]
		public void Presenter_get_expected_page_url_and_perfoms_redirect()
		{
			_mockedSynologenMemberService.Verify(x => x.GetPageUrl(It.Is<int>( pageId => pageId.Equals(_redirectPageId))));
			_mockedHttpContext.MockedHttpResponse.Verify(x => x.Redirect(It.Is<string>(url => url.Equals(_redirectUrl))));
		}
	}

	[TestFixture]
	[Category("CreateCustomerPresenterTester")]
	public class When_loading_create_customer_view_with_shop_not_having_lens_subscription_access
	{
		protected CreateCustomerPresenter _presenter;
		private readonly ICreateCustomerView _view;
		private readonly Mock<IShopRepository> _mockedShopRepository;
		private readonly Mock<ICountryRepository> _mockedCountryRepository;
		private readonly Mock<ICustomerRepository> _mockedCustomerRepository;
		private readonly Mock<ISynologenMemberService> _mockedSynologenMemberService;
		private readonly Country[] _countryList;

		public When_loading_create_customer_view_with_shop_not_having_lens_subscription_access()
		{
			// Arrange
			const int shopId = 5;
			_countryList = CountryFactory.GetList().ToArray();
			var mockedShop = new Mock<Shop>();
			mockedShop.SetupGet(x => x.Id).Returns(shopId);

			_mockedCountryRepository = new Mock<ICountryRepository>();
			_mockedCountryRepository.Setup(x => x.GetAll()).Returns(_countryList);

			var mockedView = new Mock<ICreateCustomerView>();
			mockedView.SetupGet(x => x.Model).Returns(new CreateCustomerModel());
			_view = mockedView.Object;

			_mockedShopRepository = new Mock<IShopRepository>();
			_mockedShopRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(mockedShop.Object);

			_mockedCustomerRepository = new Mock<ICustomerRepository>();

			_mockedSynologenMemberService = new Mock<ISynologenMemberService>();
			_mockedSynologenMemberService.Setup(x => x.ShopHasAccessTo(ShopAccess.LensSubscription)).Returns(false);
			_mockedSynologenMemberService.Setup(x => x.GetCurrentShopId()).Returns(shopId);

			_presenter = new CreateCustomerPresenter(_view,
													_mockedCustomerRepository.Object,
													_mockedShopRepository.Object,
													_mockedCountryRepository.Object,
													_mockedSynologenMemberService.Object);

			// Act
			_presenter.View_Load(null, new EventArgs());
		}

		[Test]
		public void Model_should_have_expected_values()
		{
			_view.Model.ShopDoesNotHaveAccessToLensSubscriptions.ShouldBe(true);
			_view.Model.DisplayForm.ShouldBe(false);
		}
	}

}
