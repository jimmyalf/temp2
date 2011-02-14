<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSettlement.ascx.cs" Inherits="Spinit.Wpc.Synologen.Presentation.Site.Wpc.Synologen.ContractSales.ViewSettlement" %>
<div class="synologen-control">
	<fieldset><legend>Utbetalningsuppgifter</legend>
		<label>Utbetalning:</label><span><%#Model.SettlementId %></span><br />
		<label>Butiknummer:</label><span><%#Model.ShopNumber %></span><br />
		<label>Period:</label><span><%#Model.Period %></span><br />
		<label>Avtalsf�rs�ljningsv�rde inkl moms:</label><span><%#Model.ContractSalesValueIncludingVAT %></span><br />
		<label>Linsabonnemangsv�rde inkl moms:</label><span><%#Model.LensSubscriptionsValueIncludingVAT %></span><br />
		<label>Antal linsabonnemang-transaktioner</label><span><%#Model.LensSubscriptionTransactionsCount %></span><br />
		<a href="<%=Spinit.Wpc.Synologen.Presentation.Site.Code.SynologenSessionContext.SettlementListPage %>">&laquo;&nbsp;Tillbaka</a><br /><br />
	</fieldset><br />
	<asp:Button ID="btnSwitchView" runat="server" Text="Visa detaljer" OnClick="btnSwitchView_Click" />
	<asp:Button ID="btnMarkAsPayed" runat="server" Text="Markera som utbetalda" OnClick="btnMarkAsPayed_Click" OnClientClick="return confirm('�r du s�ker p� att du vill markera fakturor som utbetalda?');"  Enabled='<%#Model.MarkAsPayedButtonEnabled %>'/>
	<input type="button" onclick="window.print();return false;" value="Skriv ut"/>
	<br />
	<asp:PlaceHolder ID="plSimpleView" runat="server" Visible='<%#Model.DisplaySimpleView%>'>
	<fieldset><legend>Avtalsf�rs�ljning</legend>
	<asp:Repeater ID="rptSettlementOrderItemsSimple" runat="server" DataSource='<%#Model.SimpleContractSales%>'>
	<HeaderTemplate>
		<table>
			<tr class="synologen-table-headerrow">	
				<th>Artikelnummer</th>
				<th>Artikel</th>
				<th>Antal</th>
				<th>V�rde</th>
				<th>Momsfri</th>
			</tr>			
	</HeaderTemplate>
	<ItemTemplate>
			<tr>
				<td><%# Eval("ArticleNumber") %></td>
				<td><%# Eval("ArticleName") %></td>
				<td><%# Eval("Quantity") %></td>
				<td><%# Eval("ValueExcludingVAT")%></td>
				<td><%# Eval("IsVATFree")%></td>
			</tr>
	</ItemTemplate>	
	<FooterTemplate>
		</table>
	</FooterTemplate>			
	</asp:Repeater>	
	</fieldset>
	</asp:PlaceHolder>
	<asp:PlaceHolder ID="plDetailedView" runat="server" Visible='<%#Model.DisplayDetailedView%>'>
	<fieldset><legend>Avtalsf�rs�ljning</legend>
	<asp:Repeater ID="rptSettlementOrderItemsDetailed" runat="server" DataSource='<%#Model.DetailedContractSales%>'>
	<HeaderTemplate>
		<table>
			<tr class="synologen-table-headerrow">	
				<th>Order Nr</th>
				<th>Avtalsf�retag</th>
				<th>Artikelnummer</th>
				<th>Artikel</th>
				<th>Antal</th>
				<th>V�rde</th>
				<th>Momsfri</th>
			</tr>			
	</HeaderTemplate>
	<ItemTemplate>
			<tr>
				<td><%# Eval("ContractSaleId") %></td>
				<td><%# Eval("ContractCompany") %></td>
				<td><%# Eval("ArticleNumber") %></td>
				<td><%# Eval("ArticleName") %></td>
				<td><%# Eval("Quantity") %></td>
				<td><%# Eval("ValueExcludingVAT")%></td>
				<td><%# Eval("IsVATFree") %></td>
			</tr>
	</ItemTemplate>	
	<FooterTemplate>
		</table>
	</FooterTemplate>			
	</asp:Repeater>	
	</fieldset>
	<fieldset><legend>Linsabonnemang transaktioner</legend>
	<asp:Repeater ID="rptSettlementTransactionItemsDetailed" runat="server" DataSource='<%#Model.DetailedSubscriptionTransactions%>'>
	<HeaderTemplate>
		<table>
			<tr class="synologen-table-headerrow">	
				<th>Abonnemang</th>
				<th>Belopp</th>
				<th>Datum</th>
			</tr>			
	</HeaderTemplate>
	<ItemTemplate>
			<tr>
				<td><a href="<%# Eval("SubscriptionLink")%>" title="Abonnemang"><%# Eval("CustomerName")%></a></td>
				<td><%# Eval("Amount") %></td>
				<td><%# Eval("Date") %></td>
			</tr>
	</ItemTemplate>	
	<FooterTemplate>
		</table>
	</FooterTemplate>			
	</asp:Repeater>	
	</fieldset>
	</asp:PlaceHolder>			
</div>