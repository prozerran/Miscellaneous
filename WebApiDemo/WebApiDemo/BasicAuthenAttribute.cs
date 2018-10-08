using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace WebApiDemo
{
    public class BasicAuthenAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                // username:password
                string authentoken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authentoken));
                string[] userpassarray = decodedtoken.Split(':');
                string username = userpassarray[0];
                string password = userpassarray[1];

                // set username to current running thread
                if (AuthenSecurity.Login(username, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            //base.OnAuthorization(actionContext);
        }
    }
}