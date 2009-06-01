// ==========================================================================
//
//  PROGRAM
//	
//
//  FILENAME
//	$Workfile: ShopList.ascx.cs $
//
//  ARCHIVE
//  $Archive: /Develop/WPC/Version 4.1/CustomerSpecific/Synologen/Synologen Site/Wpc/Synologen/ShopList.ascx.cs $
//
//  VERSION
//	$Revision: 5 $
//
//  DATES
//	Last check in: $Date: 09-01-08 18:08 $
//	Last modified: $Modtime: 09-01-08 17:34 $
//
//  AUTHOR(S)
//	$Author: Cber $
// 	
//
//  COPYRIGHT
// 	Copyright (c) 2008 Spinit AB --- ALL RIGHTS RESERVED
// 	Spinit AB, Datav�gen 2, 436 32 Askim, SWEDEN
//
// ==========================================================================
// 
//  DESCRIPTION
//  
//
// ==========================================================================
//
//	History
//
//	$Log: /Develop/WPC/Version 4.1/CustomerSpecific/Synologen/Synologen Site/Wpc/Synologen/ShopList.ascx.cs $
//
//5     09-01-08 18:08 Cber
//
//4     09-01-07 17:35 Cber
//
//3     08-12-19 17:22 Cber
//
//2     08-12-16 17:05 Cber
// 
// ==========================================================================
using System;
using System.Data;
using System.Web.UI.WebControls;
using Spinit.Wpc.Synologen.Presentation.Site.Code;

namespace Spinit.Wpc.Synologen.Presentation.Site.Wpc.Synologen {
	public partial class ShopList : SynologenUserControl {
		private int _categoryId;
		private int _contractCustomer;
		private int _equipmentId;
		private bool _showEquipmentFiltration;

		protected void Page_Load(object sender, EventArgs e) {
			if (Request.Params["Category"] != null)
				_categoryId = Convert.ToInt32(Request.Params["Category"]);
			if (Request.Params["ContractCustomer"] != null)
				_contractCustomer = Convert.ToInt32(Request.Params["ContractCustomer"]);
			if (Request.Params["Equipment"] != null)
				_equipmentId = Convert.ToInt32(Request.Params["Equipment"]);

			if (IsPostBack) return;
			base.OnInit(e);
			PopulateEquipment();
			PopulateShops();
		}

		public void PopulateEquipment() {
			drpEquipment.DataValueField = "cId";
			drpEquipment.DataTextField = "cName";
			drpEquipment.DataSource = Provider.GetShopEquipment(0, 0, "tblSynologenShopEquipment.cName");
			drpEquipment.DataBind();
			drpEquipment.Items.Insert(0,new ListItem("Alla butiker","0"));
			plShopFilter.Visible = ShowEquipmentFiltration;
		}

		public void PopulateShops() {
			var equipmentId = GetEquipmentId();
			if (_contractCustomer>0) {
				rptShops.DataSource = Provider.GetShops(0, 0, ContractCustomer, 0, equipmentId, false, "cCity");
			}
			else{
				rptShops.DataSource = Provider.GetShops(0, Category, ContractCustomer, 0, equipmentId, false, "cCity");
			}
			rptShops.DataBind();
		}

		private int GetEquipmentId() {
			var selectedDropDownValue = Int32.Parse(drpEquipment.SelectedValue);
			var propertyValue = _equipmentId;
			return selectedDropDownValue > 0 ? selectedDropDownValue : propertyValue;
		}

		protected void rptShops_ItemDataBound(object sender, RepeaterItemEventArgs e) {
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
			PlaceHolder plLink = (PlaceHolder)e.Item.FindControl("plLink");
			PlaceHolder plNoLink = (PlaceHolder)e.Item.FindControl("plNoLink");
			PlaceHolder plEquipment = (PlaceHolder)e.Item.FindControl("plEquipment");
			if (plLink == null || plNoLink == null) return;
			try {
				string url = ((DataRowView) e.Item.DataItem).Row["cUrl"].ToString();
				plNoLink.Visible = String.IsNullOrEmpty(url);
				plLink.Visible = !plNoLink.Visible;
			}
			catch{return;}
			try {
				string equipment = ((DataRowView) e.Item.DataItem).Row["cEquipment"].ToString();
				plEquipment.Visible = equipment.Length > 0;
			}
			catch {return;}
		}

		protected void drpEquipmentSelectedIndexChanged(object sender, EventArgs e) {
			PopulateShops();
		}

		public int Category {
			get { return _categoryId; }
			set { _categoryId = value; }
		}
		
		public int ContractCustomer {
			get { return _contractCustomer; }
			set { _contractCustomer = value; }
		}

		public int Equipment {
			get { return _equipmentId; }
			set { _equipmentId = value; }
		}

		public bool ShowEquipmentFiltration {
			get { return _showEquipmentFiltration; }
			set { _showEquipmentFiltration = value; }
		}

		#region Hidden base properties

		private new int LanguageId {
			get { return base.LanguageId; }
			set { base.LanguageId = value; }
		}

		private new int LocationId {
			get { return base.LocationId; }
			set { base.LocationId = value; }
		}


		#endregion


	}
}