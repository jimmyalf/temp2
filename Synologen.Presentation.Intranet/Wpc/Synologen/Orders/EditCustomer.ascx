﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCustomer.ascx.cs" Inherits="Spinit.Wpc.Synologen.Presentation.Intranet.Wpc.Synologen.Orders.EditCustomer" %>
<div class="synologen-control">
	<fieldset class="synologen-form">
		<legend>Redigera kund</legend>
		<p>
			<label for="<%=txtFirstName.ClientID%>">Förnamn</label>
			<asp:TextBox ID="txtFirstName" runat="server" Text='<% #Model.FirstName %>' />
			<asp:RequiredFieldValidator ID="reqtxtFirstName" runat="server" ErrorMessage="Förnamn måste anges" ControlToValidate="txtFirstName" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
		</p>
		<p>
			<label for="<%=txtLastName.ClientID%>">Efternamn</label>
			<asp:TextBox ID="txtLastName" runat="server" Text='<% #Model.LastName %>' />
			<asp:RequiredFieldValidator ID="reqtxtLastName" runat="server" ErrorMessage="Efternamn måste anges" ControlToValidate="txtLastName" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
		</p>
		<p>
			<label for="<%=txtPersonalIdNumber.ClientID%>">Personnummer</label>
			<span>
				<asp:TextBox ID="txtPersonalIdNumber" runat="server" Text='<% #Model.PersonalIdNumber %>' placeholder="ÅÅÅÅMMDDNNNN" />
				<asp:RequiredFieldValidator ID="reqtxtPersonalIdNumber" runat="server" ErrorMessage="Personnummer måste anges" ControlToValidate="txtPersonalIdNumber" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
				<asp:RegularExpressionValidator ID="regextxtPersonalIdNumber" ValidationExpression="^(19|20)(\d){2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{4}$" runat="server" ErrorMessage="Personnummer måste anges som ÅÅÅÅMMDDXXXX" Display="Dynamic" ControlToValidate="txtPersonalIdNumber" CssClass="error-message">*</asp:RegularExpressionValidator>
			</span>
		</p>
		<p>
			<label for="<%=txtEmail.ClientID%>">E-post</label>
			<asp:TextBox ID="txtEmail" runat="server" Text='<% #Model.Email %>' type="email" />
			<asp:RequiredFieldValidator ID="reqtxtEmail" runat="server" ErrorMessage="E-post måste anges" ControlToValidate="txtEmail" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="regextxtEmail" ValidationExpression="^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$" runat="server" ErrorMessage="Ogiltig e-post-adress" Display="Dynamic" ControlToValidate="txtEmail" CssClass="error-message">*</asp:RegularExpressionValidator>	
		</p>
		<p>
			<label for="<%=txtMobilePhone.ClientID%>">Mobiltelefon</label>
			<asp:TextBox ID="txtMobilePhone" runat="server" Text='<% #Model.MobilePhone %>' type="tel" />
			<asp:RegularExpressionValidator ValidationExpression="^[0-9-+() ]+?$" runat="server" ErrorMessage="Telefonnummer får endast innehålla siffor, mellanslag, plus och bindestreck." Display="Dynamic" ControlToValidate="txtMobilePhone" CssClass="error-message">*</asp:RegularExpressionValidator>	
		</p>
		<p>
			<label for="<%=txtPhone.ClientID%>">Telefon</label>
			<asp:TextBox ID="txtPhone" runat="server" Text='<% #Model.Phone %>' type="tel" />
			<asp:RegularExpressionValidator ValidationExpression="^[0-9-+() ]+?$" runat="server" ErrorMessage="Telefonnummer får endast innehålla siffor, mellanslag, plus och bindestreck." Display="Dynamic" ControlToValidate="txtPhone" CssClass="error-message">*</asp:RegularExpressionValidator>	
		</p>
		<p>
			<label for="<%=txtAddressLineOne.ClientID%>">Adress 1</label>
			<asp:TextBox ID="txtAddressLineOne" runat="server" Text='<% #Model.AddressLineOne %>' />
			<asp:RequiredFieldValidator ID="reqtxtAddressLineOne" runat="server" ErrorMessage="Adress 1 måste anges" ControlToValidate="txtAddressLineOne" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
		</p>
		<p>
			<label for="<%=txtAddressLineTwo.ClientID%>">Adress 2</label>
			<asp:TextBox ID="txtAddressLineTwo" runat="server" Text='<% #Model.AddressLineTwo %>' />
		</p>
		<p>
			<label for="<%=txtCity.ClientID%>">Ort</label>
			<asp:TextBox ID="txtCity" runat="server" Text='<% #Model.City %>' />
			<asp:RequiredFieldValidator ID="reqtxtCity" runat="server" ErrorMessage="Ort måste anges" ControlToValidate="txtCity" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
		</p>
		<p>
			<label for="<%=txtPostalCode.ClientID%>">Postnummer</label>
			<asp:TextBox ID="txtPostalCode" runat="server" Text='<% #Model.PostalCode %>' />
			<asp:RequiredFieldValidator ID="reqtxtPostalCode" runat="server" ErrorMessage="Postnummer måste anges" ControlToValidate="txtPostalCode" Display="Dynamic" CssClass="error-message">*</asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="regexPostalCode" ValidationExpression="^\d{5}$" runat="server" ErrorMessage="Ogiltigt postnummer, måste anges som fem siffror utan mellanslag." Display="Dynamic" ControlToValidate="txtPostalCode" CssClass="error-message">*</asp:RegularExpressionValidator>
		</p>
		<p>
			<label for="<%=txtNotes.ClientID%>">Butikens interna notering</label>
			<asp:TextBox ID="txtNotes" TextMode="MultiLine" runat="server" Text='<% #Model.Notes %>' />
		</p>

		<asp:ValidationSummary ID="vldSummary" runat="server" CssClass="error-list" />
		<div class="control-actions">
			<asp:Button ID="btnSave" runat="server" Text="Spara" />
		</div>		
		<p>
			<a href="<%#Model.ReturnUrl %>">Tillbaka</a>
		</p>
	</fieldset>
</div>