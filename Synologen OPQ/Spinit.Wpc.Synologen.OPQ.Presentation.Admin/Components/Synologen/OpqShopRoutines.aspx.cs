﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Spinit.Wpc.Synologen.OPQ.Admin.Components.Synologen.Controls;
using Spinit.Wpc.Synologen.OPQ.Business;
using Spinit.Wpc.Synologen.OPQ.Core.Entities;
using Spinit.Wpc.Synologen.OPQ.Presentation;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Presentation.Components.Synologen;
using Spinit.Wpc.Utility.Business.SmartMenu;
using FileCategories=Spinit.Wpc.Synologen.OPQ.Core.FileCategories;

namespace Spinit.Wpc.Synologen.OPQ.Admin.Components.Synologen
{
	public partial class OpqShopRoutines : OpqPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (_nodeId <= 0)
				{
					ShowNegativeFeedBack("NoNodeException");
				}
				else
				{
					PopulateShopRoutines(_nodeId);					
				}
			}
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			RenderMemberSubMenu(Page.Master.Master);
		}

		/// <summary>
		/// Renders the submenu.
		/// </summary>
		public void RenderMemberSubMenu(MasterPage master)
		{
			var m = (SynologenMain)master;
			var _phSynologenSubMenu = m.SubMenu;
			var subMenu = new SmartMenu.Menu { ID = "SubMenu", ControlType = "ul", ItemControlType = "li", ItemWrapperElement = "span" };

			var itemCollection = new SmartMenu.ItemCollection();
			itemCollection.AddItem("Improvments", null, "Lista förbättringsförslag", "Visar alla inkomna förbättringsåtgärder", null, "btnImprovments_OnClick", false, null);
			itemCollection.AddItem("CentralRoutine", null, "Visa central rutin", "Visar central rutin för vald nod", null, "btnShowCentralRoutine_OnClick", false, null);

			subMenu.MenuItems = itemCollection;

			m.SynologenSmartMenu.Render(subMenu, _phSynologenSubMenu);
		}


		private void PopulateShopRoutines(int nodeId)
		{
			if (nodeId <= 0) return;
			var bDocument = new BDocument(_context);
			var documents = bDocument.GetShopDocuments(nodeId, true, false, true);
			rptShops.DataSource = null;
			rptShops.DataSource = documents;
			rptShops.DataBind();
		}

		#region Page Events

		protected void btnShowCentralRoutine_OnClick(object sender, EventArgs e)
		{
			Response.Redirect(string.Format(ComponentPages.OpqStartQueryNode, _nodeId));
		}

		protected void btnImprovments_OnClick(object sender, EventArgs e)
		{
			Response.Redirect(ComponentPages.OpqImprovments);
		}


		protected void gvFiles_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var opqFile = (File) e.Row.DataItem;
				if (opqFile == null) return;
				//var opqFileId = (int)gvFiles.DataKeys[e.Row.RowIndex].Value;
				//if (opqFileId <= 0) return;
				//var bFile = new BFile(_context);
				//var opqFile = bFile.GetFile(opqFileId, true);
				var ltFile = (Literal)e.Row.FindControl("ltFile");
				var ltFileDate = (Literal)e.Row.FindControl("ltFileDate");
				if (ltFile != null)
				{
					if (opqFile.BaseFile != null)
					{
						const string tag = "<a href=\"{0}\">{1}</a>";
						string link = String.Concat(Utility.Business.Globals.FilesUrl, opqFile.BaseFile.Name);
					    string fileName = opqFile.BaseFile.Description.IsNotNullOrEmpty()
					                          ? opqFile.BaseFile.Description.Substring(opqFile.BaseFile.Description.LastIndexOf("/") + 1)
					                          : opqFile.BaseFile.Name.Substring(opqFile.BaseFile.Name.LastIndexOf("/") + 1);
						ltFile.Text = String.Format(tag, link, fileName);
					}
				}
				if (ltFileDate != null)
				{
					ltFileDate.Text = opqFile.CreatedDate.ToShortDateString();
				}
			}
		}

		protected void rptShops_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var document = (Document) e.Item.DataItem;
				int? shopId = document.ShpId;
				int? shopGroupId = document.ShopGroupId;
				var ltShopName = (Literal) e.Item.FindControl("ltShopName");
				var ltRoutine = (Literal) e.Item.FindControl("ltRoutine");
				var gvOwnFiles = (OpqFileGridView) e.Item.FindControl("gvOwnFiles");
                var gvFilledFiles = (OpqFileGridView)e.Item.FindControl("gvFilledFiles");
				var hlShopLink = (HyperLink) e.Item.FindControl("hlShopLink");
				if (hlShopLink != null)
				{
					hlShopLink.NavigateUrl = String.Format(ComponentPages.OpqStartQueryNodeAndShop, _nodeId, shopId, shopGroupId);
				}
				if (ltShopName != null)
				{
					ltShopName.Text = document.Shop != null ? document.Shop.ShopName : document.ShopGroup.Name;
				}
				if ((ltRoutine != null) && (document.DocumentContent.IsNotNullOrEmpty()))
				{
					ltRoutine.Text = document.DocumentContent;
				}
				var bFile = new BFile(_context);
				var shopFiles = bFile.GetFiles(_nodeId, shopGroupId != null ? null : shopId, null, shopGroupId, FileCategories.ShopRoutineDocuments, true, true, false);
				var filledFiles = bFile.GetFiles (_nodeId, shopGroupId != null ? null : shopId, null, shopGroupId, FileCategories.ShopDocuments, true, true, false);
				if ((gvOwnFiles != null) && (shopFiles != null) && (shopFiles.Count > 0))
				{
                    gvOwnFiles.SetDataSource(shopFiles);
				    gvOwnFiles.Visible = true;
				}
                if ((gvFilledFiles != null) && (filledFiles != null) && (filledFiles.Count > 0))
                {
                    gvFilledFiles.SetDataSource(filledFiles);
                    gvFilledFiles.Visible = true;
                }
			}
		}

		#endregion
	}
}
