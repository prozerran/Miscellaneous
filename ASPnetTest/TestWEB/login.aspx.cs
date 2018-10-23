using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Web.Security;
using System.Diagnostics;

namespace TestWEB
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TextBox1.Text = WindowsIdentity.GetCurrent().Name;
            this.TextBox2.Text = HttpContext.Current.User.Identity.Name;
            this.TextBox3.Text = Environment.UserName;
            this.TextBox4.Text = Page.User.Identity.Name;
            this.TextBox5.Text = Request.LogonUserIdentity.Name;
            this.TextBox6.Text = Context.User.Identity.Name;
            this.TextBox7.Text = HttpContext.Current.Request.ServerVariables["AUTH_USER"];
            this.TextBox8.Text = HttpContext.Current.Request.ServerVariables[5];
            this.TextBox9.Text = HttpContext.Current.Request.ServerVariables["REMOTE_USER"];
            this.TextBox10.Text = HttpContext.Current.Request.ServerVariables["LOGON_USER"];

            // SLICK way to do it!
            //this.TextBox11.Text = HttpContext.Current.Request.ServerVariables["TTP_X_FORWARDED_FOR"];
            string ip_addr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //Process compiler = new Process();
            //compiler.StartInfo.FileName = "wmic.exe";
            //compiler.StartInfo.Arguments = String.Format("/node:{0} ComputerSystem Get UserName", ip_addr);
            //compiler.StartInfo.UseShellExecute = false;
            //compiler.StartInfo.RedirectStandardOutput = true;
            //compiler.Start();
            //this.TextBox11.Text = compiler.StandardOutput.ReadToEnd();
            //compiler.WaitForExit();
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)        
        {
        }

        protected void Login1_Init(object sender, EventArgs e)
        {
            this.Login1.UserName = Page.User.Identity.Name;
        }
    }
}