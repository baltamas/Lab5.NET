
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace tests
{
    public class EndToEndTests
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
        public void loginAndAddMovie()
        {
            _driver.Url = "http://localhost:4200";


           
            var emailInput = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-item[1]/ion-input/input"));
            emailInput.SendKeys("test@test.com");
            System.Threading.Thread.Sleep(2000);

            var password = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-item[2]/ion-input/input"));
            System.Threading.Thread.Sleep(2000);
            password.SendKeys("Tamaska123(#");

            var loginBtn = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-login/ion-content/div/form/ion-button"));
            loginBtn.Click();
            System.Threading.Thread.Sleep(2000);

            var addMovieBtn = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-movies/ion-footer/ion-button"));
            addMovieBtn.Click();

            var insertTitle = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[1]/ion-input/input"));
            insertTitle.SendKeys("The Prestige");
            System.Threading.Thread.Sleep(1000);

            var insertDescription = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[2]/ion-input/input"));
            insertDescription.SendKeys("After a tragic accident, two stage magicians engage in a battle to create the ultimate illusion while sacrificing everything they have to outwit each other.");
            System.Threading.Thread.Sleep(1000);

            var insertGenre = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[3]/ion-input/input"));
            insertGenre.SendKeys("Drama, Mystery, Sci-Fi");
            System.Threading.Thread.Sleep(1000);

            var insertDuration = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[4]/ion-input/input"));
            insertDuration.SendKeys("130");
            System.Threading.Thread.Sleep(1000);

            var insertReleaseYear = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[5]/ion-input/input"));
            insertReleaseYear.SendKeys("2006");
            System.Threading.Thread.Sleep(1000);

            var insertDirector = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[6]/ion-input/input"));
            insertDirector.SendKeys("Cristopher Nolan");
            System.Threading.Thread.Sleep(1000);

            var insertRating = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[7]/ion-input/input"));
            insertRating.SendKeys("10");
            System.Threading.Thread.Sleep(1000);

            var checkSeen = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-content/div/form/ion-item[8]"));
            checkSeen.Click();
            System.Threading.Thread.Sleep(2000);

            var addMoviebtn2 = _driver.FindElement(By.XPath("/html/body/app-root/ion-app/ion-router-outlet/app-add-movie/ion-footer/ion-button[1]"));
            addMoviebtn2.Click();
            System.Threading.Thread.Sleep(4000);



        }

    }
}