using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3_WebBrowser
{
    /// <summary>
    /// IMPORTANT: JavaScript callable methods MUST start with lower case
    /// </summary>
    public class JavaSriptCallbackObject
    {
        private MainWindow mw = null;

        public JavaSriptCallbackObject(MainWindow mw)
        {
            this.mw = mw;
        }

        public void callCSharp(string message)
        {
            mw.CallCSharp(message);
        }

        public void callbacktoCS(string message)
        {
            string str = string.Format($"{message} C# Internal -> ");
            mw.CallbacktoCS(str);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChromiumWebBrowser webBrowser = null;
        private JavaSriptCallbackObject jsCallbackObj = null;

        public MainWindow()
        {
            InitializeComponent();
            jsCallbackObj = new JavaSriptCallbackObject(this);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //webBrowser.Address = "http://www.google.com";
            InitBrowser();
        }

        public void InitBrowser()
        {
            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: @"C:\Users\sp\source\repos\WpfApp3_WebBrowser\WpfApp3_WebBrowser\wwwroot",
                    hostName: "cefsharp",
                    defaultPage: "CSWebSampleInterop.html" // will default to index.html
                )
            });

            Cef.Initialize(settings);
            //CefSharpSettings.WcfEnabled = true;

            webBrowser = new ChromiumWebBrowser("localfolder://cefsharp/");
            webBrowser.Height = 400;

            webBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            webBrowser.JavascriptObjectRepository.Register("csObj", jsCallbackObj);
            webPanel.Children.Add(webBrowser);   
        }

        public void CallCSharp(string message)
        {
            csText.Dispatcher.InvokeAsync((Action)delegate
            {
                csText.Text = message;
            });
        }

        public void CallbacktoCS(string message)
        {
            string str = string.Format($"{message} C# External -> JavaScript -> End");

            csText.Dispatcher.InvokeAsync((Action)delegate
            {
                csText.Text = str;
            });

            var objs = new object[] { str };
            webBrowser.ExecuteScriptAsync("CallJavaScript", objs);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var objs = new object[] { DateTime.Now };
            webBrowser.ExecuteScriptAsync("CallJavaScript", objs);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            string str = "This text is set FROM CS --> JavaScript";

            var objs = new object[] { str };
            webBrowser.ExecuteScriptAsync("CallJavaScript", objs);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var objs = new object[] { "Start C# -> " };
            webBrowser.ExecuteScriptAsync("CallJavaScriptWithCallback", objs);
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            var objs = new object[] { DateTime.Now };
            webBrowser.ExecuteScriptAsync("clearAll", objs);
        }
    }
}
