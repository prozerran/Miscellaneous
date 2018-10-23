using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServiceTestingHarness.WcfService;
using System.Net;
using System.IO;

namespace ServiceTestingHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // http://192.168.8.99/WebService/WebService1.asmx

        private void button1_Click(object sender, EventArgs e)
        {
            WebService.WebService1SoapClient cli = new WebService.WebService1SoapClient();
            this.label1.Text = cli.HelloWorld();
            cli.Close();
        }

        // http://192.168.8.99/WCFService/Service1.svc

        private void button2_Click(object sender, EventArgs e)
        {
            Service1Client cli = new Service1Client();
            this.label1.Text = cli.GetString("Hello World from WCF Web Service!");
            cli.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = "http://192.168.8.99/WCFService/Service2.svc/getstring/HelloRest";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            this.richTextBox1.Text = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string url = "http://192.168.8.99/WebService/WebService1.asmx/HelloWorld";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            this.richTextBox1.Text = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
        }
    }
}
