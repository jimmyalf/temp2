﻿<%@ Control Inherits="System.Web.Mvc.ViewUserControl<FrameBrandListItemView>" %>
<%@ Import Namespace="Spinit.Wpc.Synologen.Core.Domain.Model"%>
<td class="center">
	<% using (Html.BeginForm("DeleteBrand","Frame", new { id = Model.Id }, FormMethod.Post)) { %>
		<%= Html.AntiForgeryToken() %>
		<input type="submit" value="Radera" class="btnSmall delete" title="Radera bågmärke" <%=Model.DisableIf(x => x.DisableDelete) %> />
	</form>
	<% } %>
</td>