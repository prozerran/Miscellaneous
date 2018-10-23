using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Drawing;

namespace TestWEB
{
    public partial class Canvas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static System.Drawing.Image CallWebMethod(string name)
        {
            System.Drawing.Image img = null;
            string filename = @"C:\Users\Admin\Documents\Code\trunk\ASPnetTest\TestWEB\img_the_scream.jpg";

            if (File.Exists(filename))
            {
                img = System.Drawing.Image.FromFile(filename);
            }
            return img;
        }

        //public static MemoryStream CallWebMethod(string name)
        //{
        //    MemoryStream ms = null;
        //    string filename = @"C:\Users\Admin\Documents\Code\trunk\ASPnetTest\TestWEB\img_the_scream.jpg";

        //    if (File.Exists(filename))
        //    {
        //        byte[] img = File.ReadAllBytes(filename);
        //        ms = new MemoryStream(img);
        //    }
        //    return ms;
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}