<%@ Control %><%@ OutputCache Duration="1800" VaryByParam="*" VaryByCustom="UrlKey" SqlDependency="Wpc:tblContTree" %>
<%= Html.Action("Menu", "WpcContent", new { area = "WpcContent", settings = new { Id = "Main-Navigation", Class = "navigation", ShowRootLevel = true, ShowDefaultPage = false, StopAtLevel = 0 } }) %>