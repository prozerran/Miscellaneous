using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Drawing.Imaging;

namespace Testing.Specs
{
    [Binding]
    public class GoogleSearch
    {
        IWebDriver driver = null;

        [Given(@"I go to Google website")]
        public void GivenIGoToGoogleWebsite()
        {
            driver = new ChromeDriver();
            driver.Url = "http://www.google.com/";
            driver.Manage().Window.Maximize();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        [Given(@"I search for Avanade wiki")]
        public void GivenISearchForAvanade()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.Id("lst-ib")).SendKeys("Avanade wiki");

            Thread.Sleep(2000);
            driver.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);
        }

        [Given(@"I click on first link")]
        public void GivenIClickOnFirstLink()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Avanade - Wikipedia")).Click();

            //IWebElement ele = driver.FindElement(By.LinkText("Avanade - Wikipedia"));
            //Actions actions = new Actions(driver);
            //actions.Click(ele).Perform();
        }

        [Then(@"I am at the Avanade wiki page")]
        public void ThenIGoToTheAvanadeWikiPage()
        {
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("firstHeading"));

            // able to land/find this page
        }

        [Then(@"I capture screenshot")]
        public void ThenICaptureScreenshot()
        {
            Thread.Sleep(2000);
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(@"C:\Temp\Screenshot.jpg", ScreenshotImageFormat.Jpeg);
        }
    }
}
