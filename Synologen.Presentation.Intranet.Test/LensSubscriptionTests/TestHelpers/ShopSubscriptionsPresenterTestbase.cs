﻿using FakeItEasy;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Presenters.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Intranet.Logic.Views.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Intranet.Models.LensSubscription;
using Spinit.Wpc.Synologen.Presentation.Intranet.Test.TestHelpers;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Test.LensSubscriptionTests.TestHelpers
{
	public abstract class ShopSubscriptionsPresenterTestbase : PresenterTestbase<ShopSubscriptionsPresenter, IShopSubscriptionsView, ShopSubscriptionsModel>
	{
		protected ISubscriptionRepository SubscriptionRepository;
		protected ISynologenMemberService SynologenMemberService;
		protected ShopSubscriptionsPresenterTestbase()
		{
			SetUp = () =>
			{
				SubscriptionRepository = A.Fake<ISubscriptionRepository>();
				SynologenMemberService = A.Fake<ISynologenMemberService>();
			};

			GetPresenter = () => 
			{
				return new ShopSubscriptionsPresenter(MockedView.Object, SubscriptionRepository, SynologenMemberService);
			};
		}

	}
}