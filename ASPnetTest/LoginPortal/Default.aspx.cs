using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;

namespace LoginPortal
{
    public static class Extensions
    {
        public static string GetDomain(this IIdentity identity)
        {
            string s = identity.Name;
            int stop = s.IndexOf("\\");
            return (stop > -1) ? s.Substring(0, stop) : string.Empty;
        }

        public static string GetLogin(this IIdentity identity)
        {
            string s = identity.Name;
            int stop = s.IndexOf("\\");
            return (stop > -1) ? s.Substring(stop + 1, s.Length - stop - 1) : string.Empty;
        }
    }

    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Test using:
            // http://192.168.8.99/LoginPortal/

            string username = HttpContext.Current.User.Identity.Name;

            byte[] encoded_str = UTF8Encoding.UTF8.GetBytes(username);
            string encoded_username = Convert.ToBase64String(encoded_str);

            string url = string.Format("http://192.168.8.99/WCFWinService/Service1.svc/login/{0}",
                encoded_username);

            Response.AddHeader("WindowsLogin", HttpContext.Current.User.Identity.Name);
            Response.Redirect(url);

            //Server.Transfer("DisplayLogin.htm");
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    string username = HttpContext.Current.User.Identity.Name;
        //    byte[] encoded_str = UTF8Encoding.UTF8.GetBytes(username);
        //    string encoded_username = Convert.ToBase64String(encoded_str);

        //    NameValueCollection data = new NameValueCollection();
        //    data.Add("name", encoded_username);
        //    HttpHelper.RedirectAndPOST(this.Page, "http://192.168.8.99/WCFWinService/Service1.svc/postlogin", data);
        //}
    }
}
