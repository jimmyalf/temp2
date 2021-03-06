using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spinit.Wpc.Base.Data;
using Spinit.Wpc.Member.Data;
using Spinit.Wpc.Synologen.Business;
using Spinit.Wpc.Synologen.Business.Domain.Entities;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Utility.Core;
using SqlProvider = Spinit.Wpc.Synologen.Data.SqlProvider;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Code
{
	public class SynologenUserControl : MemberControlPage
	{
		protected SqlProvider _provider;
		private UrlFriendlyRenamingService _urlFriendlyRenamingService;

		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);
			_provider = GetSqlprovider ();
			_urlFriendlyRenamingService = new UrlFriendlyRenamingService();
		}

		protected virtual IUrlFriendlyRenamingService UrlFriendlyRenamingService
		{
			get { return _urlFriendlyRenamingService; }
		}

		protected new SqlProvider Provider
		{
			get { return _provider; }
		}

		public int? MemberShopId
		{
			get {

				//if (SynologenSessionContext.MemberShopId > 0){
				//    return SynologenSessionContext.MemberShopId;
				//}
				//if (MemberId <= 0){ 
				//    SynologenSessionContext.MemberShopId = 0;
				//    return 0;
				//}
				List<int> shops = Provider.GetAllShopIdsPerMember (MemberId);
				//if (shops == null || shops.Count == 0) return 0;
				return shops.FirstOrDefault ();
				//SynologenSessionContext.MemberShopId = shops[0];
				//return shops[0];
			}
		}

		public string MemberShopNumber
		{
			//get {
			//    if (!String.IsNullOrEmpty(SynologenSessionContext.MemberShopNumber)) 
			//    {
			//        return SynologenSessionContext.MemberShopNumber;
			//    }
			//    if (MemberShopId <= 0) return string.Empty;
			//SynologenSessionContext.MemberShopNumber = 
			get
			{
				return Provider.GetShop ((int) MemberShopId).Number;
				//return SynologenSessionContext.MemberShopNumber;
			}
		}

		public int? MemberShopGroupId
		{
			get {
				Shop shop = Provider.GetShop ((int) MemberShopId);
				return shop != null ? shop.ShopGroupId : null;
			}
		}

		//public override int MemberId {
		//	get {

		//if (SynologenSessionContext.MemberId > 0) 
		//{
		//    return SynologenSessionContext.MemberId;
		//}
		//else
		//{
		//    _
		//}
		//int memberId = 0;
		//try {
		//    PublicUser context = PublicUser.Current;
		//    if (context != null) {
		//        int userId = context.User.Id;
		//        memberId = Provider.GetMemberId(userId);
		//        SynologenSessionContext.MemberId = memberId;
		//        return memberId;
		//    }
		//}
		//catch {
		//    if (Request.Params["memberId"] != null) {
		//        memberId = Convert.ToInt32(Request.Params["memberId"]);
		//    }
		//    if (memberId>0) {
		//        SynologenSessionContext.MemberId = memberId;
		//        return memberId;
		//    }
		//}
		//return 0;

		//}
		//}

		public string CurrentUser
		{
			get
			{
				//try {
				//    PublicUser context = PublicUser.Current;
				//    if (context != null) {return context.User.UserName;}
				//    if (Request.Params["memberId"] != null) {
				//        int memberId = Convert.ToInt32(Request.Params["memberId"]);
				//        if (memberId <= 0) return string.Empty;
				//        return Provider.GetUserRow(memberId).UserName;
				//    }
				//}
				//catch {return String.Empty;}
				return Provider.GetUserRow (MemberId).UserName;
			}
		}

		protected static bool IsInSynologenRole (SynologenRoles.Roles role)
		{
			var sRole = role.ToString ();
			var synologenComponentName = Globals.ComponentName;
			try {
				return PublicUser.Current.IsInRole (synologenComponentName, sRole);
			}
			catch {
				return false;
			}
		}

		protected new SqlProvider GetSqlprovider ()
		{
			return new SqlProvider (ConnectionString);
		}
	}
}