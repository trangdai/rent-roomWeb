using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhaTroDBFirstVer1
{
    public class OAuthConfig
    {
        public static void RegisterProviders()
        {
            OAuthWebSecurity.RegisterGoogleClient(); // that's all :)
        } 
    }
}