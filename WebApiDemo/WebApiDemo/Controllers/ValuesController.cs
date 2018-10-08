using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//  ================= TOP ====================================
// ASP.NET WEB API 1.0 TUTURIAL
// https://www.youtube.com/watch?v=0pcM6teVdKk&list=PL6n9fhu94yhW7yoUOGNOfHurUE6bpOO2b
//
// URL: http://localhost:63609/
// API: http://localhost:63609/Help
// HTM: http://localhost:63609/testapi.html
// ===========================================================

namespace WebApiDemo.Controllers
{
    public class ValuesController : ApiController
    {
        // http://localhost:63609/api/values/
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // http://localhost:63609/api/values/1
        public string Get(int id)
        {
            return "value";
        }

        [BasicAuthen]
        // http://localhost:63609/api/values/5?authen=1
        // requires Basic Authen in header
        public string Get(int id, int authen)
        {
            return "LOGIN OK";
        }

        // http://localhost:63609/api/values/5
        // HEAD request
        [HttpHead]
        public HttpResponseMessage MyHead([FromUri] int id = 1)
        {
            return Request.CreateResponse(HttpStatusCode.Found);
        }

        // POST api/values
        // http://localhost:63609/api/values
        public HttpResponseMessage Post([FromBody]string value)
        {
            try
            {           
                // do the add...

                var message = Request.CreateResponse(HttpStatusCode.Created, value);
                message.Headers.Location = new Uri(Request.RequestUri + "POST");
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT api/values/5
        // http://localhost:63609/api/values/2
        // BODY : 5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            try
            {
                if (id != 1)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "id not found");
                }
                else
                {
                    // do the real update
                    return Request.CreateResponse(HttpStatusCode.OK, value);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/values/5
        // http://localhost:63609/api/values/-1
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id < 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "id not found");
                }
                else
                {
                    // do the real delete
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
