﻿<%@ Page MasterPageFile="~/Areas/SynologenAdmin/Views/Shared/SynologenMVC.Master" Inherits="System.Web.Mvc.ViewPage<Spinit.Wpc.Synologen.Presentation.Models.LensSubscription.SubscriptionListView>" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div id="dCompMain" class="Components-Synologen-LensSubscriptions">
	<div class="fullBox">
		<div class="wrap">
			<% using (Html.BeginForm()) {%>
			<%= Html.WpcPager(Model.List)%>
			<div>
				<%= Html.WpcGrid(Model.List).Columns( column => 
					{
 						column.For(x => x.SubscriptionId).Named("ID")
 							.HeaderAttributes(@class => "controlColumn");
 						column.For(x => x.ShopName).Named("Butik");
					    column.For(x => x.CustomerName).Named("Kund");
					    column.For(x => x.Status).Named("Status");
						column.For(x => Html.ActionLink("Visa", "View", "LensSubscription", new {id = x.SubscriptionId}, new object()))
							.SetAsWpcControlColumn("Visa");
 					}).Empty("Inga abonnemang i databasen.")
				%>
				
			<%} %>
			</div>
		</div>     						
	</div>				
</div>
</asp:Content>
