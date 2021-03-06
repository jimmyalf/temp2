using System;
using System.Web.UI.WebControls;
using Spinit.Wpc.Member.Business;
using Spinit.Wpc.Synologen.Business.Domain.Entities;
using Spinit.Wpc.Synologen.Presentation.Code;
using Spinit.Wpc.Utility.Business;

namespace Spinit.Wpc.Synologen.Presentation.Components.Synologen {
	public partial class ShopEquipment : SynologenPage {
		private int _equipmentId = -1;

		protected void Page_Load(object sender, EventArgs e) {
			if (Request.Params["id"] != null)
				_equipmentId = Convert.ToInt32(Request.Params["id"]);
			if (Page.IsPostBack) return;
			PopulateEquipment();
			if (_equipmentId > 0)
				SetupForEdit();
		}

		private void SetupForEdit() {
			ltHeading.Text = "Redigera utrustning";
			btnSave.Text = "�ndra";
			Business.Domain.Entities.ShopEquipment equipment = Provider.GetShopEquipmentRow(_equipmentId);
			txtName.Text = equipment.Name;
			txtDescription.Text = equipment.Description;
		}

		private void PopulateEquipment() {
			gvEquipment.DataSource = Provider.GetShopEquipment(0,0,null);
			gvEquipment.DataBind();
			ltHeading.Text = "L�gg till utrustning";
			btnSave.Text = "Spara";
		}

		#region Category Events

		/// <summary>
		/// Add delete confirmation
		/// </summary>
		/// <param Name="sender">The sending object.</param>
		/// <param Name="e">The event arguments.</param>

		protected void btnDelete_AddConfirmDelete(object sender, EventArgs e) {
			ClientConfirmation cc = new ClientConfirmation();
			cc.AddConfirmation(ref sender, "Vill du verkligen ta bort utrustningen?");
		}

		protected void gvEquipment_Editing(object sender, GridViewEditEventArgs e) {
			int index = e.NewEditIndex;

			int id = (int)gvEquipment.DataKeys[index].Value;
			if (!IsInRole(MemberRoles.Roles.Edit)) {
				Response.Redirect(ComponentPages.NoAccess);
			}
			else {
				Response.Redirect(ComponentPages.ShopEquipment + "?id=" + id, true);
			}
		}

		protected void gvEquipment_Deleting(object sender, GridViewDeleteEventArgs e) {
			int index = e.RowIndex;
			int equipmentId = (int)gvEquipment.DataKeys[index].Value;
			if (!IsInRole(MemberRoles.Roles.Delete)) {
				Response.Redirect(ComponentPages.NoAccess);
			}
			else {
				if (Provider.EquipmentHasConnectedShops(equipmentId)) {
					DisplayMessage("Utrustningen kan inte raderas d� det finns kopplade butiker.", true);
					return;
				}
				Business.Domain.Entities.ShopEquipment category = new Business.Domain.Entities.ShopEquipment();
				category.Id = equipmentId;
				Provider.AddUpdateDeleteShopEquipment(Enumerations.Action.Delete, ref category);
				Response.Redirect(ComponentPages.ShopEquipment);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Business.Domain.Entities.ShopEquipment equipment = new Business.Domain.Entities.ShopEquipment();
			Enumerations.Action action = Enumerations.Action.Create;
			if (_equipmentId > 0) {
				equipment = Provider.GetShopEquipmentRow(_equipmentId);
				action = Enumerations.Action.Update;
			}
			equipment.Name = txtName.Text;
			equipment.Description = txtDescription.Text;

			if (!IsInRole(MemberRoles.Roles.Create)) {
				Response.Redirect(ComponentPages.NoAccess);
			}
			else {
				Provider.AddUpdateDeleteShopEquipment(action, ref equipment);
				Response.Redirect(ComponentPages.ShopEquipment);
			}
		}


		#endregion

	}
}
