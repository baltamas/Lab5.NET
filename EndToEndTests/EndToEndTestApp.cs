using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndToEndTests
{
    public class EndToEndTestApp
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetupDriver()
        {
            _driver = new ChromeDriver("C:/DRIVERS");
        }


        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }

        [Test]
        public void loginAndGoToReservations()
        {
            _driver.Url = "http://localhost:4200";



            var emailInput = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-item[1]/ion-input/input"));
            emailInput.SendKeys("test@test.com");
            System.Threading.Thread.Sleep(1000);

            var password = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-item[2]/ion-input/input"));
            System.Threading.Thread.Sleep(1000);
            password.SendKeys("Tamaska123(#");
            

            var loginBtn = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-button"));
            loginBtn.Click();
            System.Threading.Thread.Sleep(3000);

            var findNavMenu = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-movies/app-navbar/ion-header/ion-toolbar/ion-buttons/ion-menu-button"));
            findNavMenu.Click();
            System.Threading.Thread.Sleep(2000);

            var findReservations = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/app-side-menu/ion-menu/ion-content/ion-list/ion-menu-toggle[2]/ion-item"));
            findReservations.Click();
            System.Threading.Thread.Sleep(2000);
        } 
    }
}
