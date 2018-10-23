using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace TestWebService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://localhost/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        // http://localhost:4066/WebService/WebService1.asmx/HelloWorld
        // http://192.168.8.99/WebService/WebService1.asmx

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string HelloWorld()
        {
            return "Hello World from ASMX Web Service!";
        }
    }
}
