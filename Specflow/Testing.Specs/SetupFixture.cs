using NUnit.Framework;
using System;
using Testing.Api;

namespace Testing.Specs
{
  [SetUpFixture]
  public class SetupFixture
  {
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
      LandingPageSteps._browser = new SharePointBrowser(TestSettings.Options);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
      LandingPageSteps._browser.Dispose();
    }
  }
}
