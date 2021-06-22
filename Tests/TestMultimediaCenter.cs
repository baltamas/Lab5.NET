using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Tests
{
    public class Tests
    {
        private IWebDriver _driver;
        [SetUp]
        public void SetupDriver()
        {
            _driver = new ChromeDriver("C:\\DRIVERS");
        }

        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

        [Test]
        public void MovieListExists()
        {
            _driver.Url = "http://localhost:4200/movies";

            try
            {
                _driver.FindElement(By.XPath("//ion-list"));
                Assert.Pass();
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Movies list not found!");
            }
        }

        [Test]
        public void LoginFormExists()
        {
            _driver.Url = "http://localhost:4200/login";

            try
            {
                IWebElement name = _driver.FindElement(By.XPath("//ion-input[@name='email']"));
                IWebElement password = _driver.FindElement(By.XPath("//ion-input[@name='password']"));
                Assert.Pass();

            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Login inputs not found!");
            }
        }
    }
}