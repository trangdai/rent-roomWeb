<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        Welcome <b><%: Page.User.Identity.Name %></b>!
        [ <%: Html.ActionLink(ViewRes.Resource.LogOff, "LogOff", "Account") %> ]
<%
    }
    else {
%> 
        [ <%: Html.ActionLink(ViewRes.Resource.LogOn, "LogOn", "Account")%> ]
<%
    }
%>
