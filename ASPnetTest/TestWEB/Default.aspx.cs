using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.DirectoryServices;

using SQL_cslib;
using SQL_vblib.SQL_vblib;
using System.Web.Services;
using System.Drawing;

namespace TestWEB
{
    public class SqlConProp
    {
        public static string domain = "192.168.8.99";
        public static string database1 = "TestDatabase1";
        public static string database2 = "TestDatabase2";
        public static string login = "Administrator";
        public static string pass = "Pa$$word";
    }

    public partial class _Default : System.Web.UI.Page
    {
#region LoginStuffs

        [DllImport("ADVAPI32.dll", EntryPoint = "LogonUserW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// Parses the string to pull the domain name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must 
        /// contain the domain in either the domain\username or UPN format 
        /// username@domain</param>
        /// <returns>The domain name or "" if not domain is found.</returns>
        public static string GetDomainName(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(0, index);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(index + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Parses the string to pull the user name out.
        /// </summary>
        /// <param name="usernameDomain">The string to parse that must 
        /// contain the username in either the domain\username or UPN format 
        /// username@domain</param>
        /// <returns>The username or the string if no domain is found.</returns>
        public static string GetUsername(string usernameDomain)
        {
            if (string.IsNullOrEmpty(usernameDomain))
            {
                throw (new ArgumentException("Argument can't be null.", "usernameDomain"));
            }
            if (usernameDomain.Contains("\\"))
            {
                int index = usernameDomain.IndexOf("\\");
                return usernameDomain.Substring(index + 1);
            }
            else if (usernameDomain.Contains("@"))
            {
                int index = usernameDomain.IndexOf("@");
                return usernameDomain.Substring(0, index);
            }
            else
            {
                return usernameDomain;
            }
        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            string domainName = GetDomainName(this.TextBox2.Text); // Extract domain name 
            //form provide DomainUsername e.g Domainname\Username
            string userName = GetUsername(this.TextBox2.Text);  // Extract user name 
            //from provided DomainUsername e.g Domainname\Username
            IntPtr token = IntPtr.Zero;

            //userName, domainName and Password parameters are very obvious.
            //dwLogonType (3rd parameter): 
            //    I used LOGON32_LOGON_INTERACTIVE, This logon type is 
            //    intended for users who will be interactively using the computer, 
            //    such as a user being logged on by a terminal server, remote shell, 
            //    or similar process. 
            //    This logon type has the additional expense of caching 
            //    logon information for disconnected operations.
            //    For more details about this parameter please see 
            //    http://msdn.microsoft.com/en-us/library/aa378184(VS.85).aspx
            //dwLogonProvider (4th parameter) :
            //    I used LOGON32_PROVIDER_DEFAUL, This provider use the standard 
            //    logon provider for the system. 
            //    The default security provider is negotiate, unless you pass 
            //    NULL for the domain name and the user name is not in UPN format. 
            //    In this case, the default provider is NTLM. For more details 
            //    about this parameter please see 
            //    http://msdn.microsoft.com/en-us/library/aa378184(VS.85).aspx
            //phToken (5th parameter):
            //    A pointer to a handle variable that receives a handle to a 
            //    token that represents the specified user. We can use this handler 
            //    for impersonation purpose. 
            bool result = LogonUser(userName, domainName, this.TextBox3.Text, 2, 0, ref token);
            if (result)
            {
                ////If Successfully authenticated

                ////When an unauthenticated user try to visit any page of your 
                ////application that is only allowed to view by authenticated users then,
                ////ASP.NET automatically redirect the user to login form and add 
                ////ReturnUrl query string parameter that contain the URL of a page that 
                ////user want to visit, So that we can redirect the user to that page after 
                ////authenticated. FormsAuthentication.RedirectFromLoginPage() method 
                ////not only redirect the user to that page but also generate an 
                ////authentication token for that user.
                //if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                //{
                //    FormsAuthentication.RedirectFromLoginPage(this.TextBox2.Text, false);
                //}
                ////If ReturnUrl query string parameter is not present, 
                ////then we need to generate authentication token and redirect 
                ////the user to any page ( according to your application need). 
                ////FormsAuthentication.SetAuthCookie() 
                ////method will generate Authentication token 
                //else
                //{
                //    FormsAuthentication.SetAuthCookie(this.TextBox2.Text, false);
                //    Response.Redirect("default.aspx");
                //}
                this.TextBox4.Text = "Login Success";
                this.TextBox4.ForeColor = Color.Green;
            }
            else
            {
                //If not authenticated then display an error message
                //Response.Write("Invalid username or password.");
                this.TextBox4.Text = "Login Failed";
                this.TextBox4.ForeColor = Color.Red;
            }
        }

        public bool AuthenticateUser(string domain, string username, string password,string LdapPath, out string Errmsg)
        {
            Errmsg = "";
            //string domainAndUsername = domain + @"\" + username;
            string domainAndUsername = username;
            DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password);

            // Bind to the native AdsObject to force authentication.
            Object obj = entry.NativeObject;
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (null == result)
            {
                return false;
            }

            // Update the new path to the user in the directory
            LdapPath = result.Path;
            string _filterAttribute = (String)result.Properties["cn"][0];
            return true;
        }

        protected void Button14_Click(object sender, EventArgs e)
        {
            string dominName = "Admin-PC";
            string adPath = "LDAP://localhost";     // need LDAP server!
            string userName = this.TextBox2.Text.Trim();
            string strError = string.Empty;

            if (!String.IsNullOrEmpty(dominName) && !String.IsNullOrEmpty(adPath))
            {
                if (true == AuthenticateUser(dominName, userName, this.TextBox3.Text, adPath, out strError))
                {
                    this.TextBox4.Text = "Login Success";
                    this.TextBox4.ForeColor = Color.Green;
                    //Response.Redirect("default.aspx");// Authenticated user redirects to default.aspx
                }
                else
                {
                    this.TextBox4.Text = "Login Failed";
                    this.TextBox4.ForeColor = Color.Red;
                }
            }
        }

#endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.TextBox2.Text = HttpContext.Current.User.Identity.Name;
        }

        [WebMethod]
        public static string ProcessIT(string name)
        {
            return string.Format("Welcome Mr. {0}", name);
        }

        [WebMethod]
        public static void UpdateDatabase1FromJS()
        {
            Random random = new Random();
            SQL_cs_connector cs_con = new SQL_cs_connector(SqlConProp.domain, SqlConProp.database1, SqlConProp.login, SqlConProp.pass);
            cs_con.UpdateTable("Table1", "name", random.Next(100));
            cs_con.Disconnect();
        }

        [WebMethod]
        public static void UpdateDatabase2FromJS()
        {
            Random random = new Random();
            SQL_vb_connector vb_con = new SQL_vb_connector(SqlConProp.domain, SqlConProp.database2, SqlConProp.login, SqlConProp.pass);
            vb_con.UpdateTable("Table1", "name", random.Next(100));
            vb_con.Disconnect();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SQL_cs_connector cs_con = new SQL_cs_connector(SqlConProp.domain, SqlConProp.database1, SqlConProp.login, SqlConProp.pass);
            this.GridView1.DataSource = cs_con.SelectTable("Table1");
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            cs_con.Disconnect();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            SQL_cs_connector cs_con = new SQL_cs_connector(SqlConProp.domain, SqlConProp.database1, SqlConProp.login, SqlConProp.pass);
            cs_con.UpdateTable("Table1", "name", random.Next(100));
            this.GridView1.DataSource = cs_con.SelectTable("Table1");
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            cs_con.Disconnect();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            SQL_cs_connector cs_con = new SQL_cs_connector(SqlConProp.domain, SqlConProp.database1, SqlConProp.login, SqlConProp.pass);
            cs_con.EmptyTable("Table1");
            this.GridView1.DataSource = cs_con.SelectTable("Table1");
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            cs_con.Disconnect();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            this.GridView1.DataSource = null;
            this.GridView1.Visible = false;
        }

        protected void Button6_Click(object sender, EventArgs e)
        {

        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            SQL_vb_connector vb_con = new SQL_vb_connector(SqlConProp.domain, SqlConProp.database2, SqlConProp.login, SqlConProp.pass);
            this.GridView1.DataSource = vb_con.SelectTable("Table1");
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            vb_con.Disconnect();
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            SQL_vb_connector vb_con = new SQL_vb_connector(SqlConProp.domain, SqlConProp.database2, SqlConProp.login, SqlConProp.pass);
            vb_con.UpdateTable("Table1", "name", random.Next(100));
            this.GridView1.DataSource = vb_con.SelectTable("Table1");
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            vb_con.Disconnect();
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            SQL_vb_connector vb_con = new SQL_vb_connector(SqlConProp.domain, SqlConProp.database2, SqlConProp.login, SqlConProp.pass);
            vb_con.EmptyTable("Table1");
            this.GridView1.DataSource = vb_con.SelectTable("Table1");
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            vb_con.Disconnect();
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            this.GridView1.DataSource = null;
            this.GridView1.Visible = false;
        }

        protected void Button11_Click(object sender, EventArgs e)
        {

        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            SQL_cs_connector cs_con = new SQL_cs_connector(SqlConProp.domain, SqlConProp.database1, SqlConProp.login, SqlConProp.pass);
            int nId = Int32.Parse(this.TextBox1.Text);
            this.GridView1.DataSource = cs_con.CallStoreProc("GetTableFromId", nId);
            this.GridView1.DataBind();
            this.GridView1.Visible = true;
            cs_con.Disconnect();
        }
    }
}