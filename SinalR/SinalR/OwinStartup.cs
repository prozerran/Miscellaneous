using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalR.Startup), "Configuration")]

/* SINALR
 * 
 * Sample:      http://localhost:53935/SignalR.Sample/StockTicker.html
 * Reference:   https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/an-overview-of-project-katana
 * 
 * Once landed on HTML page over HTTP, the HTTP will be upgraded to WebSocket.
 * Then communication will become full-duplex bi-directional with open connection.
 */

namespace SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // simple Hello World Page
            //app.Run(context =>
            //{
            //    context.Response.ContentType = "text/plain";
            //    return context.Response.WriteAsync("Hello World!");
            //});

            // Stock ticker simulation
            Microsoft.AspNet.SignalR.StockTicker.Startup.ConfigureSignalR(app);
        }
    }
}