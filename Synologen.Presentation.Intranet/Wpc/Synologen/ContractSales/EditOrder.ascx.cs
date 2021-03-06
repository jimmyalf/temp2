using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Spinit.Wpc.Synologen.Business.Domain.Entities;
using Spinit.Wpc.Synologen.Business.Domain.Enumerations;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Business.Utility;
using Spinit.Wpc.Synologen.Data.Extensions;
using Spinit.Wpc.Synologen.Presentation.Intranet.Code;
using Spinit.Wpc.Utility.Business;
using Globals = Spinit.Wpc.Synologen.Business.Globals;

namespace Spinit.Wpc.Synologen.Presentation.Intranet.Wpc.Synologen.ContractSales
{
  
    public partial class EditOrder : SynologenSalesUserControl
    {
        private int _orderId;
        private Order _order;
        private const bool ActiveArticles = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["id"] != null)
            {
                _orderId = Convert.ToInt32(Request.Params["id"]);
                _order = Provider.GetOrder(_orderId);
            }
            CheckUserPermission();
            CheckEnableSaveAndAbortOrder();
            CheckDisplayHaltedButtons();

            if (Page.IsPostBack) return;

            InitSessionContext();
            PopulateContracts();
            PopulateItemNumbers();
            PopulateOriginalShoppingCart();
            PopulateOrder();

        }

        private static void InitSessionContext()
        {
            SynologenSessionContext.OrderItemsMarkedForDeletion = new List<int>();
            SynologenSessionContext.EditOrderItemsInCart = new List<CartOrderItem>();
        }

        #region Population Mehtods
        private void PopulateContracts()
        {
            if (MemberShopId == 0)
            {
                return;
            }

            drpContracts.DataSource = Provider.GetContracts(FetchCustomerContract.AllPerShop, 0, (int)MemberShopId, true);
            drpContracts.DataBind();
            drpContracts.Items.Insert(0, new ListItem("-- V�lj avtal --", "0"));
        }
        private void PopulateCompanies()
        {
            var contractId = Int32.Parse(drpContracts.SelectedValue);
            drpCompany.DataSource = Provider.GetCompanies(0, contractId, null, ActiveFilter.Active);
            drpCompany.DataBind();
            drpCompany.Items.Insert(0, new ListItem("-- V�lj f�retag --", "0"));
            drpCompany.Enabled = true;
        }

        private void PopulateArticles()
        {
            var contractId = Int32.Parse(drpContracts.SelectedValue);
            drpArticle.DataSource = Provider.GetContractArticleConnections(0, contractId, ActiveArticles, "tblSynologenContractArticleConnection.cId");
            drpArticle.DataBind();
            drpArticle.Items.Insert(0, new ListItem("-- V�lj Artikel --", "0"));
            drpArticle.Enabled = true;
        }
        private void PopulateItemNumbers()
        {
            drpNumberOfItems.DataSource = GetNumberOfItemsItemList();
            drpNumberOfItems.DataBind();
            drpNumberOfItems.Items.Insert(0, new ListItem("-- V�lj Antal --", "0"));
            drpNumberOfItems.Enabled = true;
        }
        private void PopulateOriginalShoppingCart()
        {
            SynologenSessionContext.EditOrderItemsInCart = General.ParseList(Provider.GetOrderItemsList(_orderId, 0, null));
            gvOrderItemsCart.DataSource = SynologenSessionContext.EditOrderItemsInCart;
            gvOrderItemsCart.DataBind();
            ltTotalPrice.Text = GetTotalCartPrice(SynologenSessionContext.EditOrderItemsInCart).ToString();
        }
        private void PopulateShoppingCart()
        {
            gvOrderItemsCart.DataSource = SynologenSessionContext.EditOrderItemsInCart;
            gvOrderItemsCart.DataBind();
            ltTotalPrice.Text = GetTotalCartPrice(SynologenSessionContext.EditOrderItemsInCart).ToString();
        }
        private void PopulateOrder()
        {
            txtCompanyUnit.Text = _order.CompanyUnit;
            txtCustomerFirstName.Text = _order.CustomerFirstName;
            txtCustomerLastName.Text = _order.CustomerLastName;
            txtPersonalIDNumber.Text = _order.PersonalIdNumber;
            txtEmail.Text = _order.Email;
            txtPhone.Text = _order.Phone;
            txtCustomerOrderNumber.Text = _order.CustomerOrderNumber;

            var selectListCompany = Provider.GetCompanyRow(_order.CompanyId);

            if (selectListCompany.DerivedFromCompanyId.HasValueAndPositive())
            {
                selectListCompany = Provider.GetCompanyRow((int) selectListCompany.DerivedFromCompanyId);
            }

            TrySetContract(selectListCompany.ContractId);
            PopulateCompanies();
            PopulateArticles();
            TrySetCompany(selectListCompany);

            txtRST.Text = _order.RstText;
            ltCreatedDate.Text = _order.CreatedDate.ToShortDateString();
            if (_order.UpdateDate != DateTime.MinValue)
            {
                ltUpdatedDate.Text = _order.UpdateDate.ToShortDateString();
            }
            ltOrderNumber.Text = _orderId.ToString();
            ltMarkedAsPayed.Text = _order.MarkedAsPayedByShop ? "Ja" : "Nej";
            //var salesPerson = Provider.GetSynologenMember(_order.SalesPersonMemberId, LocationId, LanguageId);
            //ltSalesPersonName.Text = salesPerson.ContactFirst + " " + salesPerson.ContactLast;
            var user = Provider.GetUserRow(_order.SalesPersonMemberId);
            ltSalesPersonName.Text = user.FirstName + " " + user.LastName;

            if (drpContracts.SelectedValue != "0")
            {
                var contract = Provider.GetContract(Convert.ToInt32(drpContracts.SelectedValue));
                if (contract.ForceCustomAddress)
                {
                    invoiceAddressFields.Visible = true;
                    var companyReferfence = Provider.GetCompanyRow(_order.CompanyId);
                    SetInvoiceAddress(companyReferfence);
                }
                else
                {
                    invoiceAddressFields.Visible = false;
                    ClearInvoiceAddress();
                }

            }
            CheckEnableSaveAndAbortOrder();
        }

        #endregion

        #region Events
        protected void drpContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpContracts.SelectedValue != "0")
            {
                var contract = Provider.GetContract(Convert.ToInt32(drpContracts.SelectedValue));

                if (contract.ForceCustomAddress)
                {
                    invoiceAddressFields.Visible = true;
                    ClearInvoiceAddress();
                }
                else
                {
                    invoiceAddressFields.Visible = false;
                }

                PopulateCompanies();
                PopulateArticles();
                PopulateItemNumbers();
            }
            else
            {
                drpCompany.Enabled = false;
                drpArticle.Enabled = false;
                drpNumberOfItems.Enabled = false;
            }
        }

        protected void drpCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCompany.SelectedValue == "0") return;

            var selectedCompany = Provider.GetCompanyRow(Convert.ToInt32(drpCompany.SelectedValue));
            var orderCompany = Provider.GetCompanyRow(_order.CompanyId);
            if (selectedCompany.Id == orderCompany.DerivedFromCompanyId)
            {
                SetInvoiceAddress(orderCompany);
            }
            else
            {
                ClearInvoiceAddress();
            }

            PopulateValidationRules(selectedCompany, Controls);
        }


        //TODO: Each selection hits the database, fix by caching or store whole objects in drop down(if possible).
        protected void drpArticle_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var connectionId = Int32.Parse(drpArticle.SelectedValue);
            var contractArticle = Provider.GetContractCustomerArticleRow(connectionId);
            plManualPrice.Visible = contractArticle.EnableManualPriceOverride;
        }

        protected void gvOrderItemsCart_Deleting(object sender, GridViewDeleteEventArgs e)
        {
            var index = e.RowIndex;
            var temporaryId = (int)gvOrderItemsCart.DataKeys[index].Values["TemporaryId"];
            var orderItemId = (int)gvOrderItemsCart.DataKeys[index].Values["Id"];

            if (temporaryId > 0)
            {
                RemoveTemporaryOrderItemFromCart(temporaryId);
            }
            else
            {
                RemoveExistingOrderItemFromCart(orderItemId);
            }

            PopulateShoppingCart();
        }

        protected void OrderItemsValidation(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (SynologenSessionContext.EditOrderItemsInCart.Count > 0);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Page.Validate("vldAdd");
            if (!Page.IsValid) return;

            var item = new CartOrderItem();
            var connectionId = Int32.Parse(drpArticle.SelectedValue);
            var contractArticle = Provider.GetContractCustomerArticleRow(connectionId);

            item.ArticleDisplayName = contractArticle.ArticleName;
            item.ArticleDisplayNumber = contractArticle.ArticleNumber;
            item.ArticleId = contractArticle.ArticleId;

            if (contractArticle.EnableManualPriceOverride && !String.IsNullOrEmpty(txtManualPrice.Text))
            {
                item.SinglePrice = float.Parse(txtManualPrice.Text);
            }
            else
            {
                item.SinglePrice = contractArticle.Price;
            }

            item.NumberOfItems = Int32.Parse(drpNumberOfItems.SelectedValue);
            item.DisplayTotalPrice = item.SinglePrice * item.NumberOfItems;
            item.Notes = txtNotes.Text;

            AddOrderItemToCart(item);
            ClearItemInputControls();
            PopulateShoppingCart();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (drpCompany.SelectedValue != null && drpCompany.SelectedValue != "0")
            {
                var company = Provider.GetCompanyRow(Convert.ToInt32(drpCompany.SelectedValue));
                PopulateValidationRules(company, Controls);
                Page.Validate(btnSave.ValidationGroup);
            }
            if (!Page.IsValid) return;
            SaveOrder();
            SaveOrderItems(_order.Id);
            DeleteOrderItemsMarkedForDeletion();
            //TODO: Replace string below with resource
            Provider.AddOrderHistory(_orderId, "Ordern uppdaterades av " + CurrentUser);
            Response.Redirect(SynologenSessionContext.SalesListPage);
        }

        protected void btnMarkAsPayed_Click(object sender, EventArgs e)
        {
            _order.MarkedAsPayedByShop = true;
            SaveOrder();
            //TODO: Replace string below with resource
            Provider.AddOrderHistory(_orderId, "Ordern markerades som utbetald av " + CurrentUser);
            Response.Redirect(SynologenSessionContext.SalesListPage);
        }

        protected void btnAbort_Click(object sender, EventArgs e)
        {
            var newStatusId = Globals.AbortStatusId;
            Provider.ChangeOrderStatus(_orderId, newStatusId);
            //TODO: Replace string below with resource
            Provider.AddOrderHistory(_orderId, "Ordern avbr�ts av " + CurrentUser);
            Response.Redirect(SynologenSessionContext.SalesListPage);
        }

        protected void btnHalt_Click(object sender, EventArgs e)
        {
            var newStatusId = Globals.HaltedStatusId;
            Provider.ChangeOrderStatus(_orderId, newStatusId);
            //TODO: Replace string below with resource
            Provider.AddOrderHistory(_orderId, "Ordern sattes som vilande av " + CurrentUser);
            Response.Redirect(SynologenSessionContext.SalesListPage);
        }

        protected void btnAbortHalt_Click(object sender, EventArgs e)
        {
            var newStatusId = Globals.DefaultNewOrderStatus;
            if (_order.InvoiceNumber > 0) newStatusId = Globals.DefaultOrderStatusAfterSPCSInvoice;
            Provider.ChangeOrderStatus(_orderId, newStatusId);
            //TODO: Replace string below with resource
            Provider.AddOrderHistory(_orderId, "Ordern �ter-aktiverades av " + CurrentUser);
            Response.Redirect(SynologenSessionContext.SalesListPage);
        }
        #endregion

        #region Helper Methods

        private void CheckUserPermission()
        {
            var statusOK = Globals.EditableOrderStatusList.Contains(_order.StatusId);
            var userPermissionOK = MemberShopId == _order.SalesPersonShopId;
            if (statusOK && userPermissionOK) return;

            plEditOrder.Visible = false;
            plNoAccessMessage.Visible = true;
        }

        private void SaveOrder()
        {
            _order.PersonalIdNumber = txtPersonalIDNumber.Text.Replace("-", "");
            _order.CompanyUnit = txtCompanyUnit.Text;
            _order.CustomerFirstName = txtCustomerFirstName.Text;
            _order.CustomerLastName = txtCustomerLastName.Text;
            _order.Phone = txtPhone.Text;
            _order.Email = txtEmail.Text;
            _order.SalesPersonMemberId = MemberId;
            _order.SalesPersonShopId = (int)MemberShopId;
            _order.RstText = txtRST.Text;
            _order.CustomerOrderNumber = txtCustomerOrderNumber.Text;

            var selectedCompany = Provider.GetCompanyRow(Convert.ToInt32(drpCompany.SelectedValue));
            var orderCompany = Provider.GetCompanyRow(_order.CompanyId);

            if (selectedCompany.Id == orderCompany.DerivedFromCompanyId)
            {
                orderCompany.Name = txtCompanyName.Text;
                orderCompany.PostBox = txtPostBox.Text;
                orderCompany.StreetName = txtStreetName.Text;
                orderCompany.Zip = txtZip.Text;
                orderCompany.City = txtCity.Text;

                Provider.AddUpdateDeleteCompany(Enumerations.Action.Update, ref orderCompany);
                _order.CompanyId = orderCompany.Id;
            }
            else
            {
                var newReferenceCompany = Provider.CreateReferenceCompanyFromCompany(selectedCompany, txtCompanyName.Text, txtPostBox.Text, txtStreetName.Text, txtZip.Text, txtCity.Text);
                _order.CompanyId = newReferenceCompany.Id;
            }

            Provider.AddUpdateDeleteOrder(Enumerations.Action.Update, ref _order);

        }

        private void SaveOrderItems(int id)
        {
            foreach (var item in SynologenSessionContext.EditOrderItemsInCart)
            {
                if (!item.IsTemporary) continue;
                const Enumerations.Action action = Enumerations.Action.Create;
                IOrderItem tempOrder = item;
                tempOrder.OrderId = id;
                Provider.AddUpdateDeleteOrderItem(action, ref tempOrder);
            }
        }

        private void DeleteOrderItemsMarkedForDeletion()
        {
            foreach (var orderItemId in SynologenSessionContext.OrderItemsMarkedForDeletion)
            {
                const Enumerations.Action action = Enumerations.Action.Delete;
                IOrderItem item = new OrderItem { Id = orderItemId };
                Provider.AddUpdateDeleteOrderItem(action, ref item);
            }
            SynologenSessionContext.OrderItemsMarkedForDeletion = new List<int>();
        }



        private void ClearItemInputControls()
        {
            txtNotes.Text = String.Empty;
            drpArticle.SelectedIndex = 0;
            drpNumberOfItems.SelectedIndex = 0;
            txtManualPrice.Text = String.Empty;
            plManualPrice.Visible = false;
        }

        private static void AddOrderItemToCart(CartOrderItem item)
        {
            var cart = SynologenSessionContext.EditOrderItemsInCart;
            item.TemporaryId = GetNewTemporaryIdForCart(cart);
            cart.Add(item);
            SynologenSessionContext.EditOrderItemsInCart = cart;
        }

        private static void RemoveTemporaryOrderItemFromCart(int itemTemporaryId)
        {
            var cart = new List<CartOrderItem>(SynologenSessionContext.EditOrderItemsInCart);
            cart.RemoveAll(x => x.TemporaryId == itemTemporaryId);
            SynologenSessionContext.EditOrderItemsInCart = cart;
        }

        private static void RemoveExistingOrderItemFromCart(int orderItemId)
        {
            var cart = new List<CartOrderItem>(SynologenSessionContext.EditOrderItemsInCart);
            cart.RemoveAll(x => x.Id == orderItemId);
            SynologenSessionContext.EditOrderItemsInCart = cart;

            //Add item to a list of already existing orderItems to be deleted at save
            var deleteOrderItems = SynologenSessionContext.OrderItemsMarkedForDeletion;
            deleteOrderItems.Add(orderItemId);
            SynologenSessionContext.OrderItemsMarkedForDeletion = deleteOrderItems;
        }

        public void CheckEnableSaveAndAbortOrder()
        {
            try
            {
                var contractOK = !drpContracts.SelectedValue.Equals("0") && !drpContracts.SelectedValue.Equals("");
                var companyOK = !drpCompany.SelectedValue.Equals("0") && !drpCompany.SelectedValue.Equals("");
                var notMarkedAsPayed = !_order.MarkedAsPayedByShop;

                btnSave.Enabled = (contractOK && companyOK && notMarkedAsPayed);
                btnAbort.Enabled = Business.Utility.General.IsOrderEditable(_order.StatusId);
            }
            catch
            {
                btnSave.Enabled = false;
            }
        }


        public void CheckDisplayHaltedButtons()
        {
            if (Business.Utility.General.IsOrderHalted(_order.StatusId))
            {
                btnHalt.Visible = false;
                btnAbortHalt.Visible = true;
                return;
            }
            if (Business.Utility.General.IsOrderEditable(_order.StatusId))
            {
                btnHalt.Visible = true;
                btnAbortHalt.Visible = false;
                return;
            }
            btnHalt.Visible = false;
            btnAbortHalt.Visible = false;
        }

        private void TrySetContract(int contractId)
        {
            try
            {
                drpContracts.SelectedValue = contractId.ToString();
            }
            catch
            {
                drpContracts.SelectedValue = "0";
            }
        }

        private void TrySetCompany(Company company)
        {
            try
            {
                drpCompany.SelectedValue = company.Id.ToString();
                PopulateValidationRules(company, Controls);
            }
            catch
            {
                drpCompany.SelectedValue = "0";
            }
        }

        protected void IsValuePostBoxOrStreetName(object source, ServerValidateEventArgs args)
        {
            if (!string.IsNullOrEmpty(txtPostBox.Text) || !string.IsNullOrEmpty(txtStreetName.Text))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        private void SetInvoiceAddress(Company company)
        {
            txtCompanyName.Text = company.Name;
            txtPostBox.Text = company.PostBox;
            txtStreetName.Text = company.StreetName;
            txtZip.Text = company.Zip;
            txtCity.Text = company.City;
        }

        private void ClearInvoiceAddress()
        {
            txtCompanyName.Text = string.Empty;
            txtPostBox.Text = string.Empty;
            txtStreetName.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtCity.Text = string.Empty;
        }

        #endregion
    }
}