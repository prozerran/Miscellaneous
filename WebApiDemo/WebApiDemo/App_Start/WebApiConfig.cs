using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

using Newtonsoft.Json.Serialization;

namespace WebApiDemo
{
    // 2nd approach 
    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*
            config.Routes.MapHttpRoute(
                name: "Version2",
                routeTemplate: "api/v2/Students/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "StudentsV2" }
            );

            // TODO: Add controller StudentsV2Controller and implement
            */

            // require authentication
            // require Basic Authen in header request
            //config.Filters.Add(new BasicAuthenAttribute());

            // return only JSON data
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // indented, prettyfy json
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // return JSON instead of XML - 1st approach
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // return JSON instead of XML - 2nd approach
            //config.Formatters.Add(new CustomJsonFormatter());



            // force CORS cross-domain calls using JSONP callback - 1st approach
            // need install NUGET webapicontrib.formatting.jsonp
            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0, jsonpFormatter);

            // use CORS instead of JSONP - 2nd approach
            // need install NUGET Microsoft.aspnet.webapi.cors
            //EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);
        }
    }
}
