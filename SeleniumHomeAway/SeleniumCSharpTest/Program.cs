using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;

class HomeAway
{

    static void ErrorHandler(string strError, string strActual, string strExpected)
    {
        Console.WriteLine ("Error:" + strError );
        Console.WriteLine("Expected value: " + strExpected);
        Console.WriteLine("Actual value: " + strActual);

    } //ErrorHandler

    static void FillFormWithElement (string sendKeysElement, string byIdText, IWebDriver driver)
    {
        IWebElement select;
        select = driver.FindElement(By.Id(byIdText));
        select.SendKeys(sendKeysElement);

    }
    static void Test1()
    {

        // Create a new instance of the Firefox driver.
        IWebDriver driver = new FirefoxDriver();

        driver.Navigate().GoToUrl("http://store.demoqa.com/products-page/product-category/iphones/apple-iphone-4s-16gb-sim-free-black/");
        // Wait for the page to load, timeout after 5 seconds
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        //Verify product id is expected value  - product id 96
        //Assume this since this is a product key, unlikely that this value will change for the SKU.   
        IList<IWebElement> products = driver.FindElements(By.ClassName("productcol"));
        string matchingElement = "Apple iPhone 4S 16GB SIM-Free";

        if (String.Compare(products[0].Text.Substring(0, matchingElement.Length), matchingElement) != 0)
        {
            ErrorHandler("Phone selection failed", products[0].Text, matchingElement);

        }
        //Buy phone
        driver.FindElement(By.ClassName("wpsc_buy_button")).Click();
        // Wait for the page to load, timeout after 5 seconds
        driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));

        //Confirm purchase
        driver.FindElement(By.ClassName("go_to_checkout")).Click();
        //Continue Checkout
        driver.FindElement(By.ClassName("step2")).Click();

        //navigate to new page - fill in address, etc

        IWebElement select = driver.FindElement(By.Id("current_country"));
        select.SendKeys("USA");
        //find 'Calculate' button 
        IList<IWebElement> allOptions = select.FindElements(By.XPath("//*[@value='Calculate']"));
        allOptions[0].Click();

        //fill in address, etc.   Consider placing values like this in a readable input file (i.e. xml file), particularly with a test suite of similiar pages
        FillFormWithElement("email@homeaway.com", "wpsc_checkout_form_9", driver);
        FillFormWithElement("FirstNameTest", "wpsc_checkout_form_2", driver);
        FillFormWithElement("LastNameTest", "wpsc_checkout_form_3", driver);
        FillFormWithElement("One Billing Address Lane", "wpsc_checkout_form_4", driver);
        FillFormWithElement("CityTest", "wpsc_checkout_form_5", driver);
        FillFormWithElement("TX", "wpsc_checkout_form_6", driver);
        FillFormWithElement("USA", "wpsc_checkout_form_7", driver);
        FillFormWithElement("98052", "wpsc_checkout_form_8", driver);
        FillFormWithElement("425 123 4567", "wpsc_checkout_form_9", driver);
        //Confirm purchase
        driver.FindElement(By.ClassName("wpsc_buy_button")).Click();


         //Close the browser 
        driver.Quit();

    }
    

    static void Main(string[] args)
    {
        Test1();
    }
}
