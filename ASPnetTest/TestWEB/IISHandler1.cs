using System;
using System.Web;
using System.Drawing;
using System.IO;

namespace TestWEB
{
    public class IISHandler1 : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;

            //TODO: link your MyImage to iSource using imageId query parameter...
            Image iSource = null;
            MemoryStream ms = new MemoryStream();
            iSource.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] data = ms.ToArray();
            ms.Dispose();
            //g.Flush();
            //g.Dispose();
            iSource.Dispose();

            context.Response.BinaryWrite(data);
        }

        #endregion
    }
}
