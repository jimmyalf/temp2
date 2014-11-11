<%@ Control Language="C#" %>
<%@ Import Namespace="Spinit.Wpc.Forum.Components" %>
<%@ Import Namespace="Spinit.Wpc.Forum.Controls" %>
<table width="100%" cellspacing="1" cellpadding="0">
  <tr>
    <td align="left">
      <a href="<%=UrlPath.Moderation %>" class="menuTitle">
      ASP.NET Forums Moderation
      </a>
    </td>
    <td align="right" valign="center">
      <asp:HyperLink CssClass="menuTextLink" id="HomeMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="SearchMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="ProfileMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="LoginMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="RegisterMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="MemberListMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="MyForumsMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="ModerationMenu" Visible="False" runat="server" />
      <asp:HyperLink CssClass="menuTextLink" id="AdminMenu" Visible="False" runat="server" />
    </td>
  </tr>
</table>
