using System;
using System.Security;
using TechTalk.SpecFlow;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Testing.Api;
using Microsoft.Dynamics365.UIAutomation.Api;
using OpenQA.Selenium;
using System.IO;

namespace Testing.Specs
{
  [Binding]
  public class LandingPageSteps
  {
    private readonly string _userdomain = System.Configuration.ConfigurationManager.AppSettings["OnlineUserDomain"];
    private readonly SecureString _password = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword"].ToSecureString();
    private readonly Uri _baseUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineBaseUrl"].ToString());

    public static SharePointBrowser _browser;

    [Given(@"I have logged in as '(.*)'")]
    public void GivenIHaveLoggedInAs(string p0)
    {
      _browser.LoginPage.Login(new Uri("https://portal.office.com"), string.Format("{0}@{1}", p0, _userdomain).ToSecureString(), _password, ADFSLoginAction);
    }
    [Given(@"I navigate to '(.*)'")]
    public void GivenINavigateTo(string p0)
    {
      _browser.Navigate(string.Format("{0}{1}", _baseUri, "/sites/pppdev"));
    }
    public void ADFSLoginAction(LoginRedirectEventArgs args)
    {
      var d = args.Driver;
      d.FindElement(By.Id("passwordInput")).SendKeys(args.Password.ToUnsecureString());
      d.ClickWhenAvailable(By.Id("submitButton"), new TimeSpan(0, 0, 2));
      d.WaitForPageToLoad();
    }

    [Given(@"I click button '(.*)'")]
    public void GivenIClickButton(string p0)
    {
    }

    [When(@"I click link '(.*)'")]
    public void WhenIClickLink(string p0)
    {
    }

    [Then(@"I click link '(.*)'")]
    public void ThenIClickLink(string p0)
    {
    }

    [Then(@"I take a screen shot")]
    public void ThenITakeAScreenShot()
    {
      string fileNameTimestap = DateTime.Now.ToString("yyyyMMddhhmmss");

      _browser.Driver.WaitForPageToLoad();
      _browser.TakeWindowScreenShot(
        "C:\\Temp\\screenshot_" + fileNameTimestap + ".png",
        ScreenshotImageFormat.Png);
    }
  }
}
