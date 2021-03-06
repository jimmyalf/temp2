using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription;
using Spinit.Wpc.Synologen.Core.Extensions;
using Spinit.Wpc.Synologen.Presentation.Application.AutomapperMappings;
using Spinit.Wpc.Synologen.Presentation.Models.ContractSales;
using Spinit.Wpc.Synologen.Presentation.Models.LensSubscription;
using Settlement=Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales.Settlement;
using Subscription=Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription.Subscription;

namespace Spinit.Wpc.Synologen.Presentation
{
	public class WpcSynologenAdminProfile : Profile
	{
		protected override void Configure()
		{
			//Converters
			ForSourceType<DateTime>().AddFormatter<StandardDateFormatter>();
			ForSourceType<DateTime?>().AddFormatter<NullableDateFormatter>();

			
			// Model to ViewModel
			CreateMap<Subscription, SubscriptionView>()
				.ForMember(to => to.CustomerName, m => m.ResolveUsing<CustomerNameValueResolver>().FromMember(x => x.Customer))
				.ForMember(cv => cv.ConsentDate, m => m.MapFrom(x => x.ActivatedDate))
				.ForMember(cv => cv.Created, m => m.MapFrom(x => x.CreatedDate))
				.ForMember(cv => cv.AddressLineOne, m => m.MapFrom(x => x.Customer.Address.AddressLineOne))
				.ForMember(cv => cv.AddressLineTwo, m => m.MapFrom(x => x.Customer.Address.AddressLineTwo))
				.ForMember(cv => cv.City, m => m.MapFrom(x => x.Customer.Address.City))
				.ForMember(cv => cv.PostalCode, m => m.MapFrom(x => x.Customer.Address.PostalCode))
				.ForMember(cv => cv.Country, m => m.MapFrom(x => x.Customer.Address.Country.Name))
				.ForMember(cv => cv.Email, m => m.MapFrom(x => x.Customer.Contact.Email))
				.ForMember(cv => cv.MobilePhone, m => m.MapFrom(x => x.Customer.Contact.MobilePhone))
				.ForMember(cv => cv.Phone, m => m.MapFrom(x => x.Customer.Contact.Phone))
				.ForMember(cv => cv.PersonalIdNumber, m => m.MapFrom(x => x.Customer.PersonalIdNumber.FormatPersonalIdNumber()))
				.ForMember(cv => cv.ShopName, m => m.MapFrom(x => x.Customer.Shop.Name))
				.ForMember(cv => cv.TransactionList, m => m.ResolveUsing<TransactionValueResolver>().FromMember(x => x.Transactions))
				.ForMember(cv => cv.AccountNumber, m => m.MapFrom(x => x.PaymentInfo.AccountNumber))
				.ForMember(cv => cv.ClearingNumber, m => m.MapFrom(x => x.PaymentInfo.ClearingNumber))
				.ForMember(cv => cv.MonthlyAmount, m => m.MapFrom(x => x.PaymentInfo.MonthlyAmount.ToString("F", new CultureInfo("sv-SE"))))
				.ForMember(cv => cv.Status, m => m.MapFrom(x => x.Active ? SubscriptionStatus.Started.GetEnumDisplayName() : SubscriptionStatus.Stopped.GetEnumDisplayName()))
				.ForMember(cv => cv.ErrorList, m => m.ResolveUsing<SubscriptionErrorValueResolver>().FromMember(x => x.Errors))
				.ForMember(cv => cv.CustomerNotes, m => m.MapFrom(x => x.Customer.Notes))
				.ForMember(cv => cv.SubscriptionNotes, m => m.MapFrom(x => x.Notes))
				.ForMember(cv => cv.FirstName, m => m.MapFrom(x => x.Customer.FirstName))
				.ForMember(cv => cv.LastName, m => m.MapFrom(x => x.Customer.LastName))
				.ForMember(cv => cv.CustomerId, m => m.MapFrom(x => x.Customer.Id))
				.ForMember(cv => cv.ConsentStatus, m => m.MapFrom(x => x.ConsentStatus.GetEnumDisplayName()));

			//CreateMap<Settlement, SettlementView>().ConvertUsing(new SettlementViewTypeConverter());

			//CreateMap<Settlement, SettlementListViewItem>()
			//    .ForMember(x => x.NumberOfContractSalesInSettlement, m => m.MapFrom(x => x.ContractSales.Count()))
			//    .ForMember(x => x.NumberOfLensSubscriptionTransactionsInSettlement, m => m.MapFrom(x => x.OldTransactions.Count()))
			//    .ForMember(x => x.CreatedDate, m => m.MapFrom(x => x.CreatedDate.ToString("yyyy-MM-dd HH:mm")));

			CreateMap<TransactionArticle, TransactionArticleListItem>()
				.ForMember(x => x.Active, m => m.MapFrom(x => x.Active))
				.ForMember(x => x.ArticleId, m => m.MapFrom(x => x.Id))
				.ForMember(x => x.Name, m => m.MapFrom(x => x.Name))
				.ForMember(x => x.NumberOfConnectedTransactions, m => m.MapFrom(x => x.NumberOfConnectedTransactions));

			CreateMap<TransactionArticle, TransactionArticleModel>()
				.ForMember(x => x.Id, m => m.MapFrom(x => x.Id))
				.ForMember(x => x.Name, m => m.MapFrom(x => x.Name))
				.ForMember(x => x.Active, m => m.MapFrom(x => x.Active));
		}
	}
}