<%@ Page Language="C#" MasterPageFile="~/Wpc/Forum/Themes/MasterPageTemplate.master" Title="Forum" %>
<%@ Import Namespace="Spinit.Wpc.Forum.Components" %>
<%@ Register TagPrefix="Forums" Namespace="Spinit.Wpc.Forum.Controls" Assembly="Spinit.Wpc.Forum.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <Forums:ThreadView runat="server" />
</asp:Content>
