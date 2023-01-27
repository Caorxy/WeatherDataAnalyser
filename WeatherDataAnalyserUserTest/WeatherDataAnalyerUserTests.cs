using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace TestProject2;

[TestFixture]
public class WeatherDataAnalyserUserTests
{
    private WindowsDriver<WindowsElement> _driver = null!;
     
    [SetUp]
    public void TestInit()
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("app", "C:/Users/gabri/RiderProjects/WeatherDataAnalyser/WeatherDataAnalyser/bin/Debug/net7.0-windows/WeatherDataAnalyser.exe");
        options.AddAdditionalCapability("deviceName", "WindowsPC");
        _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }
     
    [TearDown]
    public void TestCleanup()
    {
        _driver.Quit();
    }

    [Test]
    public void TestNavigation()
    {
        // Arrange
        var possibleChoices = new[] {"Analysis", "Save to file", "About" };
        var random = new Random();

        // Simulate user's behaviour
        for (var i = 0; i < 100; i++)
        {
            _driver.FindElement(By.Name(possibleChoices[random.Next(3)])).Click();
            _driver.FindElement(By.Name("Back")).Click();
        }
    }

    [Test]
    public void TestUserWantsToSeeStatisticsForOneLocation()
    {
        // Arrange
        var json = File.ReadAllText("../../../expected1.txt");
        var expectedValues = JsonConvert.DeserializeObject<List<List<string>>>(json);
        var random = new Random();
        
        // Simulate user's behaviour
        _driver.FindElement(By.Name("Analysis")).Click();
        _driver.FindElementByAccessibilityId("LatTyped").SendKeys("0");
        _driver.FindElementByAccessibilityId("LonTyped").SendKeys("0");
        SimulateChoosingDateByUser();
        _driver.FindElement(By.Name("Calculate")).Click();
        
        // Assert whether results are correct
        var id = 0;
        for (var i = 0; i < 10; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(expectedValues!.ElementAt(id).ElementAt(0), Is.EqualTo(_driver.FindElementByAccessibilityId("MaxVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(1), Is.EqualTo(_driver.FindElementByAccessibilityId("MinVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(2), Is.EqualTo(_driver.FindElementByAccessibilityId("AvgVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(3), Is.EqualTo(_driver.FindElementByAccessibilityId("MedVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(4), Is.EqualTo(_driver.FindElementByAccessibilityId("StdDevVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(5), Is.EqualTo(_driver.FindElementByAccessibilityId("VarVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(6), Is.EqualTo(_driver.FindElementByAccessibilityId("ModeVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(8), Is.EqualTo(_driver.FindElementByAccessibilityId("MaxDate").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(9), Is.EqualTo(_driver.FindElementByAccessibilityId("MinDate").Text));

                if (random.Next(2) == 0)
                {
                    id = id == 4 ? 0 : id + 1;
                    _driver.FindElement(By.Name(">")).Click();
                }
                else 
                {
                    id = id == 0 ? 4 : id-1;
                    _driver.FindElement(By.Name("<")).Click();
                }
            });
        }
    }

    [Test]
    public void TestUserWantsToSeeStatisticsForTwoLocations()
    {
        // Arrange
        var json1 = File.ReadAllText("../../../expected1.txt");
        var json2 = File.ReadAllText("../../../expected2.txt");
        var expectedValues1 = JsonConvert.DeserializeObject<List<List<string>>>(json1);
        var expectedValues2 = JsonConvert.DeserializeObject<List<List<string>>>(json2);
        var random = new Random();
         
        // Simulate user's behaviour
        _driver.FindElement(By.Name("Analysis")).Click();
        _driver.FindElementByAccessibilityId("LatTyped").SendKeys("0");
        _driver.FindElementByAccessibilityId("LonTyped").SendKeys("0");
        _driver.FindElement(By.Name("+")).Click();
        _driver.FindElementByAccessibilityId("Lat2Typed").SendKeys("45");
        _driver.FindElementByAccessibilityId("Lon2Typed").SendKeys("-45");
        SimulateChoosingDateByUser();
        _driver.FindElement(By.Name("Calculate")).Click();
        _driver.FindElementByAccessibilityId("MaxVal");
        _driver.FindElementByAccessibilityId("LinesCheckBox").Click();
        _driver.FindElementByAccessibilityId("ColumnsCheckBox").Click();


        // Assert whether results are correct
        var id = 0;
        var expectedValues = expectedValues1;
        for (var i = 0; i < 10; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(expectedValues!.ElementAt(id).ElementAt(0), Is.EqualTo(_driver.FindElementByAccessibilityId("MaxVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(1), Is.EqualTo(_driver.FindElementByAccessibilityId("MinVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(2), Is.EqualTo(_driver.FindElementByAccessibilityId("AvgVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(3), Is.EqualTo(_driver.FindElementByAccessibilityId("MedVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(4), Is.EqualTo(_driver.FindElementByAccessibilityId("StdDevVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(5), Is.EqualTo(_driver.FindElementByAccessibilityId("VarVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(6), Is.EqualTo(_driver.FindElementByAccessibilityId("ModeVal").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(8), Is.EqualTo(_driver.FindElementByAccessibilityId("MaxDate").Text));
                Assert.That(expectedValues!.ElementAt(id).ElementAt(9), Is.EqualTo(_driver.FindElementByAccessibilityId("MinDate").Text));

                var choice = random.Next(4);
                switch (choice)
                {
                    case 0:
                        id = id == 4 ? 0 : id + 1;
                        _driver.FindElementByAccessibilityId("Next").Click();
                        break;
                    case 1:
                        id = id == 0 ? 4 : id-1;
                        _driver.FindElementByAccessibilityId("Back").Click();
                        break;
                    case 2:
                        _driver.FindElementByAccessibilityId("NextStats").Click();
                        expectedValues = expectedValues == expectedValues1 ? expectedValues2 : expectedValues1;
                        break;
                    case 3:
                        _driver.FindElementByAccessibilityId("BackStats").Click();
                        expectedValues = expectedValues == expectedValues1 ? expectedValues2 : expectedValues1;
                        break;
                }
            });
        }
    }

    private void SimulateChoosingDateByUser()
    {
        var element = _driver.FindElementByAccessibilityId("StartDate");
        element.FindElement(By.ClassName("Button")).Click();
        element.FindElement(By.Name("styczeń 2023")).Click();
        element.FindElement(By.Name("2023")).Click();
        element.FindElement(By.Name("Przycisk Poprzedni")).Click();
        element.FindElement(By.Name("Przycisk Poprzedni")).Click();
        element.FindElement(By.Name("2000")).Click();
        element.FindElement(By.Name("sty")).Click();
        element.FindElement(By.Name("1")).Click();
         
         
        var element2 = _driver.FindElementByAccessibilityId("EndDate");
        element2.FindElement(By.ClassName("Button")).Click();
        element2.FindElement(By.Name("styczeń 2023")).Click();
        element2.FindElement(By.Name("2023")).Click();
        element2.FindElement(By.Name("Przycisk Poprzedni")).Click();
        element2.FindElement(By.Name("Przycisk Poprzedni")).Click();
        element2.FindElement(By.Name("2000")).Click();
        element2.FindElement(By.Name("sty")).Click();
        element2.FindElement(By.Name("poniedziałek, 31 stycznia 2000")).Click();
    }
}