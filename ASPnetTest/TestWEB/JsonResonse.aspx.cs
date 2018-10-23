using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;

using Newtonsoft.Json;

namespace TestWEB
{
    public partial class JsonResonse : System.Web.UI.Page
    {
        // https://192.168.8.99/JsonResonse.aspx?p1=a1&p2=a2

        protected void Page_Load(object sender, EventArgs e)
        {
            string p1 = Request.QueryString.Get("p1");
            string p2 = Request.QueryString.Get("p2");

            Response.AddHeader("p1", p1);
            Response.AddHeader("p2", p2);

            IdentityContract id = new IdentityContract();
            id.Name = "Tim";
            id.Price = 123.45;

            string json = JsonConvert.SerializeObject(id);

            Response.Clear();
            Response.CacheControl = "no-cache";
            //Response.ContentType = "text/xml";
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);
            Response.End();
        }


        // https://192.168.8.99/JsonResonse.aspx/GetData

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetData(string name)
        {
            IdentityContract id = new IdentityContract();
            id.Name = "Tim";
            id.Price = 123.45;

            string json = JsonConvert.SerializeObject(id);
            return json;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetCurrentTime(string name)
        {
            return "Hello " + name + Environment.NewLine + "The Current Time is: "
                + DateTime.Now.ToString();
        }

    }
}