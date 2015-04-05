<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%= Html.ActionLink("English ", "ChangeCulture", "Home",
    new { lang = (int)NhaTroDBFirstVer1.Culture.en, returnUrl =  
    this.Request.RawUrl }, new { @class = "eng_flag" })%>

<%= Html.ActionLink("Việt ", "ChangeCulture", "Home",
    new { lang = (int)NhaTroDBFirstVer1.Culture.vi, returnUrl = 
    this.Request.RawUrl }, new { @class = "vi_flag" })%>
