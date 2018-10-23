using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Channels;
using System.Web.Security;
using System.Web;
using System.Configuration;
using System.Security.Principal;
using System.Security.Permissions;
using System.Net;
using System.Security.Cryptography;

namespace WCFWinServiceLib
{
    //[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required)]
    public class Service1 : IService1
    {
        private string saved_login_name = "";

        public string StringRequest(string p1, string p2) 
        {
            //WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Redirect;
            //WebOperationContext.Current.OutgoingResponse.Location = "http://www.google.com";

            return string.Format("Hello : {0}", this.saved_login_name);
        }

        public long Multiply(long x, long y)
        {
            return x * y;
        }

        public string InvokeRestString(string str)
        {
            return str;
        }

        public string InvokeGetString(string str)
        {
            try
            {
                return ServiceSecurityContext.Current.WindowsIdentity.Name;
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }

        public string InvokePostString(string str)
        {
            return str;
        }

        public LoginResponse GetLogin(string name)
        {
            LoginResponse res = new LoginResponse();

            // get IP Address
            String ipaddr = RemoteEndpointMessageProperty.Name;
            OperationContext ctx = OperationContext.Current;
            MessageProperties mesgp = ctx.IncomingMessageProperties;
            RemoteEndpointMessageProperty client_ep = mesgp[ipaddr] as RemoteEndpointMessageProperty;
            res.ipaddress = client_ep.Address;
            res.sessionid = ctx.SessionId;

            //System.ServiceModel.DomainServices.Server.ApplicationServices
            //res.authtype = ServiceSecurityContext.Current.PrimaryIdentity.AuthenticationType;
            //res.username = ctx.ServiceSecurityContext.WindowsIdentity.Name;
            res.encoded_username = name;

            byte[] decoded_str = Convert.FromBase64String(name);
            res.username = Encoding.UTF8.GetString(decoded_str);

            this.saved_login_name = name;

            //get header information
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            res.listString = new List<String>();

            for (int i = 0; i < headers.Count; i++)
            {
                res.listString.Add(headers.Get(i).ToString());
            }               
            return res;
        }

        public LoginResponse GetLoginPost(string name)
        {
            LoginResponse res = new LoginResponse();

            // get IP Address
            String ipaddr = RemoteEndpointMessageProperty.Name;
            OperationContext ctx = OperationContext.Current;
            MessageProperties mesgp = ctx.IncomingMessageProperties;
            RemoteEndpointMessageProperty client_ep = mesgp[ipaddr] as RemoteEndpointMessageProperty;
            res.ipaddress = client_ep.Address;
            res.sessionid = ctx.SessionId;

            res.encoded_username = name;

            byte[] decoded_str = Convert.FromBase64String(name);
            res.username = Encoding.UTF8.GetString(decoded_str);

            //get header information
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            res.listString = new List<String>();

            for (int i = 0; i < headers.Count; i++)
            {
                res.listString.Add(headers.Get(i).ToString());
            }
            return res;
        }
    }
}
