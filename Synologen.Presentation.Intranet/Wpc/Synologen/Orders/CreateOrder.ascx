﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateOrder.ascx.cs" Inherits="Spinit.Wpc.Synologen.Presentation.Intranet.Wpc.Synologen.Orders.CreateOrder" %>

<div id="page" class="step3">
    <header>
		<h1>Linsabonnemang demo</h1>
		<span class="customer-name"><b>Kund:</b> <%=Model.CustomerName %></span>
	</header>

	<WpcSynologen:OrderMenu runat="server" />
	<div id="tab-container">

   	<fieldset>
   	    <div class="progress">
   			<label>Steg 3 av 6</label>
	 		<div id="progressbar"></div>
   		</div>
    	<p><label>Välj Kategori</label>
            <asp:DropDownList id="ddlPickCategory" DataSource="<% #Model.Categories %>"  SelectedValue="<%#Model.SelectedCategoryId%>" DataTextField="Text" DataValueField="Value" runat="server" AutoPostBack="true">
            </asp:DropDownList>
            <asp:RequiredFieldValidator 
		        ID="RequiredFieldValidator1" 
		        InitialValue="0" 
		        Runat="server" 
		        ErrorMessage="Obligatoriskt fält" 
		        ControlToValidate="ddlPickCategory" 
		        Display="Dynamic" 
		        CssClass="" 
		        ValidationGroup="vldSubmit"	>&nbsp;*</asp:RequiredFieldValidator>
		</p>
        <p><label>Välj Typ</label>
            <asp:DropDownList id="ddlPickKind" DataSource="<% #Model.ArticleTypes %>" SelectedValue="<%#Model.SelectedArticleTypeId%>" DataTextField="Text" DataValueField="Value" runat="server" AutoPostBack="true">
            </asp:DropDownList>
            <asp:RequiredFieldValidator 
		        ID="RequiredFieldValidator2" 
		        InitialValue="0" 
		        Runat="server" 
		        ErrorMessage="Obligatoriskt fält" 
		        ControlToValidate="ddlPickKind" 
		        Display="Dynamic" 
		        CssClass="" 
		        ValidationGroup="vldSubmit"	>&nbsp;*</asp:RequiredFieldValidator>
    	</p>
    	<p><label>Välj Leverantör</label>
            <asp:DropDownList id="ddlPickSupplier" DataSource="<% #Model.Suppliers %>" SelectedValue="<%#Model.SelectedSupplierId%>" DataTextField="Text" DataValueField="Value" runat="server" AutoPostBack="true">
            </asp:DropDownList>
            <asp:RequiredFieldValidator 
		        ID="RequiredFieldValidator3" 
		        InitialValue="0" 
		        Runat="server" 
		        ErrorMessage="Obligatoriskt fält" 
		        ControlToValidate="ddlPickSupplier" 
		        Display="Dynamic" 
		        CssClass="" 
		        ValidationGroup="vldSubmit"	>&nbsp;*</asp:RequiredFieldValidator>
		</p>
    	<p><label>Välj Artikel</label>
            <asp:DropDownList id="ddlPickArticle" DataSource="<% #Model.OrderArticles %>" SelectedValue="<%#Model.SelectedArticleId%>" DataTextField="Text" DataValueField="Value" runat="server" AutoPostBack="true">
            </asp:DropDownList>
            <asp:RequiredFieldValidator 
		        ID="RequiredFieldValidator4" 
		        InitialValue="0" 
		        Runat="server" 
		        ErrorMessage="Obligatoriskt fält" 
		        ControlToValidate="ddlPickArticle" 
		        Display="Dynamic" 
		        CssClass="" 
		        ValidationGroup="vldSubmit"	>&nbsp;*</asp:RequiredFieldValidator>
    	</p>
      </fieldset>

      <fieldset class="right-eye">
      <legend>H</legend>
          	<p><label>Styrka</label>

            <asp:DropDownList id="ddlRightStrength" DataSource="<% #Model.PowerOptions %>" DataTextField="Text" DataValueField="Value" runat="server">

            </asp:DropDownList>
		</p>
    	<p><label>Baskurva</label>

            <asp:DropDownList id="ddlRightBaskurva" DataSource="<% #Model.BaseCurveOptions %>" DataTextField="Text" DataValueField="Value" runat="server">

            </asp:DropDownList>

		</p>
    	<p><label>Diameter</label>
            <asp:DropDownList id="ddlRightDiameter" DataSource="<% #Model.DiameterOptions %>" DataTextField="Text" DataValueField="Value" runat="server">
            </asp:DropDownList>
    	</p>
        <p><label>Axel</label>
            <asp:DropDownList id="ddlRightAxis" DataSource="<% #Model.AxisOptions %>" DataTextField="Text" DataValueField="Value" runat="server">
            </asp:DropDownList>
    	</p>
        <p><label>Cylinder</label>
            <asp:DropDownList id="ddlRightCylinder" DataSource="<% #Model.CylinderOptions %>" DataTextField="Text" DataValueField="Value" runat="server">
            </asp:DropDownList>
    	</p>
      </fieldset>
      <fieldset class="left-eye">
      <legend>V</legend>
          	<p><label>Styrka</label>

            <asp:DropDownList id="ddlLeftStrength" DataTextField="Text" DataValueField="Value" DataSource="<% #Model.PowerOptions %>" runat="server">

            </asp:DropDownList>
		</p>
    	<p><label>Baskurva</label>

            <asp:DropDownList id="ddlLeftBaskurva" DataSource="<% #Model.BaseCurveOptions %>" DataTextField="Text" DataValueField="Value" runat="server">

            </asp:DropDownList>

		</p>
    	<p><label>Diameter</label>
            
            <asp:DropDownList id="ddlLeftDiameter" DataSource="<% #Model.DiameterOptions %>" DataTextField="Text" DataValueField="Value" runat="server">

            </asp:DropDownList>
    	</p>
        <p><label>Axel</label>
            <asp:DropDownList id="ddlLeftAxis" DataSource="<% #Model.AxisOptions %>" DataTextField="Text" DataValueField="Value" runat="server">
            </asp:DropDownList>
    	</p>
        <p><label>Cylinder</label>
            <asp:DropDownList id="ddlLeftCylinder" DataSource="<% #Model.CylinderOptions %>" DataTextField="Text" DataValueField="Value" runat="server">
            </asp:DropDownList>
    	</p>
      </fieldset>
           
      <fieldset>
      <legend>Leverantörsalternativ</legend>
        <asp:RadioButtonList runat="server" ID="rbShippingOptions" DataSource="<%# Model.ShippingOptions %>" DataTextField="Text" DataValueField="Value" RepeatLayout="Flow" RepeatDirection="Horizontal" />
        <asp:RequiredFieldValidator   
            ID="ReqiredFieldValidator1"  
            runat="server"  
            ControlToValidate="rbShippingOptions"  
            ErrorMessage="*"  
            >  
        </asp:RequiredFieldValidator>  
      </fieldset>
      
   <fieldset>
        <div class="next-step">
            <div class="control-actions">
				
                <asp:Button ID="btnPreviousStep" runat="server" Text="← Föregående steg" CssClass="cancel-button" />
                <asp:Button ID="btnCancel" Text="Avbryt" runat="server" CssClass="cancel-button" />
		        <asp:Button ID="btnNextStep" runat="server" Text="Nästa steg →" />
	        </div>
        </div>
   </fieldset>

  </div>
  </div>