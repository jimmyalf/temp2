<%@ Page Language="C#"  MasterPageFile="~/Default.master" AutoEventWireup="true" Title="Intran�t" %>
<asp:Content ID="cnt1" ContentPlaceHolderID="Content" Runat="Server">
<asp:Literal ID="ltPageId" Text="190" Visible="false" runat="server"/>
<WpcSynologen:FrameOrderView ID="synologenMvpTestControl" runat="server" EditPageId="700" RedirectAfterSentOrderPageId="700" />
</asp:Content>

