using Microsoft.Dynamics365.UIAutomation.Browser;
using Testing.Api;

namespace Testing.Specs
{
  public static class TestSettings
  {
    private static SharePointBrowser _browser;
    public static SharePointBrowser Browser
    {
      get
      {
        if (_browser == null)
        {
          _browser = new SharePointBrowser(TestSettings.Options);
        }
        return _browser;
      }
    }
    public static BrowserOptions Options = new BrowserOptions
    {
      BrowserType = BrowserType.Chrome,
      PrivateMode = true,
      FireEvents = true
    };
  }
}
