<%@ Import Namespace="Spinit.Wpc.Forum.Components" %>
<%@ Register TagPrefix="Forums" Namespace="Spinit.Wpc.Forum.Controls" Assembly="Spinit.Wpc.Forum.Controls" %>
<%@ Page Language="C#" MasterPageFile="~/commonresources/templates/Master/Forum Template.master" Title="Forum" Inherits="Spinit.Wpc.Content.Presentation.Site.Code.PageControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
	<Forums:PostFlatView runat="server" SkinFileName="Moderation\View-PostFlatView.ascx" id="PostFlatView" />
<asp:Literal ID="ltPageId" Text="283" Visible="false" runat="server"/>
</asp:Content>
