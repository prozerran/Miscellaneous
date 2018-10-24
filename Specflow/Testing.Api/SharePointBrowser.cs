using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Api
{
  public class SharePointBrowser : InteractiveBrowser
  {
    #region Constructor(s)
    internal SharePointBrowser(IWebDriver driver) : base(driver)
    {
    }

    public SharePointBrowser(BrowserType type) : base(type)
    {
    }

    public SharePointBrowser(BrowserOptions options) : base(options)
    {

    }
    #endregion Constructor(s)

    #region Login

    public LoginPage LoginPage => this.GetPage<LoginPage>();

    public void GoToXrmUri(Uri xrmUri)
    {
      this.Driver.Navigate().GoToUrl(xrmUri);
      this.Driver.WaitForPageToLoad();
    }

    #endregion Login
  }
}
