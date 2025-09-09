using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace AppiumTests
{
    [TestFixture]
    public class ApiDemosTests
    {
        private AndroidDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new AppiumOptions();
            options.PlatformName = "Android";
            options.AutomationName = "UiAutomator2";
            options.DeviceName = "emulator-5554"; // match "adb devices"

            // Instead of AppPackage / AppActivity properties:
            options.AddAdditionalAppiumOption("appPackage", "io.appium.android.apis");
            options.AddAdditionalAppiumOption("appActivity", ".ApiDemos");
            options.AddAdditionalAppiumOption("appWaitActivity", "*");

            driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), options, TimeSpan.FromSeconds(60));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }


        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose(); // ensures cleanup of unmanaged resources
                driver = null;
            }
        }


        [Test]
        public void LaunchesAppSuccessfully()
        {
            var currentActivity = driver.CurrentActivity;
            Console.WriteLine("Current Activity: " + currentActivity);

            Assert.IsTrue(currentActivity.Contains("ApiDemos"),
                $"Expected ApiDemos but got {currentActivity}");
        }

        [Test]
        public void OpenPreferenceDependencies()
        {
            // Click on "Preference"
            var preference = driver.FindElement(By.Id("android:id/text1"));
            preference.Click();

            // Click on "3. Preference dependencies"
            var dependencies = driver.FindElement(By.Id("android:id/text1"));
            dependencies.Click();

            // Validate checkbox exists
            //var checkbox = driver.FindElement(MobileBy.XPath("\t\r\n//android.widget.TextView[@resource-id=\"android:id/title\" and @text=\"WiFi\"]"));
            //Assert.That(checkbox.Displayed, Is.True);
        }
    }
}
