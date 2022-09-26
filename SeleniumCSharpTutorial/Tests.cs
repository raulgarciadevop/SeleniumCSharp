using SeleniumCSharpTutorial.Objects;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCSharpTutorial
{
    public class Tests
    {
        IWebDriver driver;
        List<TestCase> testCases;

        // SetUp of the driver configurations for each test
        [SetUp]
        public void Setup()
        {
            // Get test cases values supplied by Excel file
            testCases = ExcelExtractor.GetTestDataFromExcel();
            if (testCases == null)
                Assert.Fail("It wasn't possible to obtain test case values from excel file source.");

            driver = new ChromeDriver();// In this case I'm using Chrome
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);// Default timeout to locate web elements will be set to 30 seconds
            driver.Manage().Window.Maximize();// Window is maximized before test execution
        }

        // Test script
        [Test]
        public void BECUTest()
        {
            /* Working variables definition */
            IWebElement monthlyPayment, downPayment, loanTerm, interestRates, ifram;
            string currentValue, initialValue;

            /* Test execution */

            // Launch BECU.ORG website
            driver.Url = "https://becu.org/";

            // Select LOANS & MORTGAGES
            driver.FindElement(By.XPath("//*/a[@data-id=\"{4EE93736-5B2D-456D-A2D9-00055676C627}\"]")).Click();

            // Select Auto Loans
            driver.FindElement(By.LinkText("Auto Loans")).Click();

            // Expand the Calculator Section under Auto Loan Calculator
            IWebElement expBtn = driver.FindElement(By.XPath("//button[contains(@data-bs-target, 'How-much-v')]"));
            expBtn.SendKeys(Keys.Return);

            // Switch to the iframe that's covering the text fields in order to be able to interact with child elements
            ifram = driver.FindElement(By.XPath("//iframe[@title='Auto_What vehicle can I afford']"));
            driver.SwitchTo().Frame(ifram);

            // Iterate test cases in Excel file
            foreach (TestCase testCase in testCases)
            {
                if (testCase.CaseValues == null)
                {
                    Assert.Fail("Couldn't load test case values successfully.");
                    break;
                }

                // TODO: Check if form values are the same

                // Obtain initial form's value
                initialValue = driver.FindElement(By.XPath("//*/span[@id=\"lf_answer\"]/span[@class=\"answer_highlight\"]")).Text;

                /* Supply new values from the Excel file to the form fields  */

                // Monthly Payment
                monthlyPayment = driver.FindElement(By.Name("Auto_MonthlyPayment"));
                clearField(monthlyPayment);
                monthlyPayment.SendKeys(testCase.CaseValues[0]);

                // Down Payment
                downPayment = driver.FindElement(By.XPath("//*/input[@id=\"lf_Global_AutoDownPayment\"]"));
                clearField(downPayment);
                downPayment.SendKeys(testCase.CaseValues[1]);

                // Loan Term
                loanTerm = driver.FindElement(By.XPath("//*/input[@id=\"lf_Global_AutoLoanTerm\"]"));
                clearField(loanTerm);
                loanTerm.SendKeys(testCase.CaseValues[2]);

                // Interest Rates
                interestRates = driver.FindElement(By.XPath("//*/input[@id=\"lf_Global_AutoInterestRate\"]"));
                clearField(interestRates);
                interestRates.SendKeys(testCase.CaseValues[3]);
                interestRates.SendKeys(Keys.Return);// This is for submitting the form

                // Wait for spinner icon to disappear before interacting with the value
                Thread.Sleep(3000);

                // Obtain current form's value
                currentValue = driver.FindElement(By.XPath("//*/span[@id=\"lf_answer\"]/span[@class=\"answer_highlight\"]")).Text;

                // Verify if value has changed
                Assert.That(currentValue, Is.Not.EqualTo(initialValue));
            }

            Assert.Pass("Test completed successfully.");

        }

        // Test clean up after each test failure/pass
        [TearDown]
        public void CleanUp()
        {
            driver.Quit();// I make sure to quit the Web Driver to avoid memory overflow/overuse
        }

        // I've made this method to reuse the way I found out to clear form fields
        public void clearField(IWebElement field)
        {
            field.SendKeys(Keys.Control + "a");
            field.SendKeys(Keys.Delete);
        }

    }
}