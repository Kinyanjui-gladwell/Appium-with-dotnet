using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
namespace SimpleAppium;
using OpenQA.Selenium.Appium.Android.Enums;
using System.IO;

[TestFixture]
public class Practice
{
    private AndroidDriver driver;


    [SetUp]
    public void Setup()
    {
        var options = new AppiumOptions();

        options.PlatformName = "Android";
        options.AutomationName = "UiAutomator2";
        options.DeviceName = "emulator-5554";

       // options.AddAdditionalAppiumOption("app", "\"C:\\Users\\lenovo\\Downloads\\ApiDemos-debug.apk\"");
        //or
        options.AddAdditionalAppiumOption("appPackage", "io.appium.android.apis");
        options.AddAdditionalAppiumOption("appActivity", ".ApiDemos");
        options.AddAdditionalAppiumOption("autoGrantPermissions", true);
        options.AddAdditionalAppiumOption("noReset", true);
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
            driver.Dispose();
            driver = null;
        }
    }
    [Test]
    public void LocatorsFirstRun()
    {
        var acccessibility = driver.FindElement(MobileBy.AccessibilityId("Accessibility")); //content desc
        acccessibility.Click();
        var accessibilityService = driver.FindElement(MobileBy.AccessibilityId("Accessibility Service"));
        Assert.IsTrue(accessibilityService.Displayed, "Accessibility service is displayed");
        driver.PressKeyCode(new KeyEvent(AndroidKeyCode.Back));
        driver.FindElement(MobileBy.AccessibilityId("Text")).Click();
        driver.FindElement(MobileBy.AccessibilityId("LogTextBox")).Click();
        driver.FindElement(MobileBy.AccessibilityId("Add")).Click();
        string text = driver.FindElement(By.Id("io.appium.android.apis:id/text")).Text;
        Assert.That(text, Is.EqualTo("This is a test\r\n"));
        driver.PressKeyCode(new KeyEvent(AndroidKeyCode.Back));
        driver.PressKeyCode(new KeyEvent(AndroidKeyCode.Back));
        driver.FindElement(MobileBy.AccessibilityId("Views")).Click();
        string textSwitch = "TextSwitcher";
        var textSwitchElement = driver.FindElement(MobileBy.AndroidUIAutomator(
            "new UiScrollable(new UiSelector().scrollable(true))" +
            $".scrollIntoView(new UiSelector().text(\"{textSwitch}\"))"));
        textSwitchElement.Click();
        driver.Navigate().Back();
        driver.Navigate().Back();
        driver.Orientation = ScreenOrientation.Landscape;
        driver.Orientation = ScreenOrientation.Portrait;
        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile(@"C:\tmp\screenshot.png");
        driver.FindElement(MobileBy.AccessibilityId("Accessibility")).Click();
        driver.FindElement(MobileBy.AccessibilityId("Accessibility Node Querying")).Click();
        var takeOutTrash = driver.FindElement(By.Id("io.appium.android.apis:id/tasklist_label"));
        bool isEnabled = takeOutTrash.Enabled;
        takeOutTrash.Click();
        bool isDisabled = !takeOutTrash.Enabled;
        takeOutTrash.Click();
        driver.Navigate().Back();
        driver.Navigate().Back();
        var battery = driver.ExecuteScript("mobile: batteryInfo");
        Console.WriteLine(battery);
        var info = driver.ExecuteScript("mobile: deviceInfo");
        Console.WriteLine(info);
        driver.SetClipboardText("Copied from clipboard", "plaintext");
        var clipBoardText = driver.GetClipboardText();
        Assert.That(clipBoardText, Is.EqualTo("Copied from clipboard"));
        driver.Lock(5);
        bool isLocked = driver.IsLocked();
        driver.ExecuteScript("mobile: shell", new Dictionary<string, object>
        {
            {
                "command", "svc"
            },
            { "args", new List<string> { "wifi", "disable" } }
        });
        driver.ExecuteScript("mobile: shell", new Dictionary<string, object>
        {
            { "command", "svc" },
            { "args", new List<string> { "wifi", "enable" } }
        });

    }
}
