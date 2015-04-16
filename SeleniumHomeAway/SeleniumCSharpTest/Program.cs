using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;

class HomeAway
{
    //classes to represent the Product page info elements
    public static class ProductPageClass {

        public static IWebElement AddtoCart(IWebDriver driver)
        {
            //iPhone4s product page - Purchase button
            IWebElement element = driver.FindElement(By.ClassName("wpsc_buy_button"));
            return element;      
        }
       
        public static IWebElement CheckOutButton(IWebDriver driver)
        {
            //iPhone4s product page - Checkout modal dialog
            IWebElement element = driver.FindElement(By.ClassName("go_to_checkout"));
            return element;

        }  
    }


    //classes to represent the Checkout page elements
    public static class CheckoutPageClass
    {

        public static IWebElement ContinueButton(IWebDriver driver)
        {
            //Checkout product page - continue button
            IWebElement element = driver.FindElement(By.ClassName("step2"));
            return element;
        }

        public static IWebElement RemoveButton(IWebDriver driver)
        {
            //Checkout product page - remove button
            IList<IWebElement> allOptions = driver.FindElements(By.XPath("//*[@value='Remove']"));
            return allOptions[0]; //first entry is the calculate button
        }

        public static IWebElement CalculateButton(IWebDriver driver)
        {
            //find 'Calculate' button 
            IList<IWebElement> allOptions = driver.FindElements(By.XPath("//*[@value='Calculate']"));
            return allOptions[0]; //first entry is the calculate button
    
        }
        public static IWebElement TxtbxCurrentCountry(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("current_country"));
            return element;
        }

        public static IWebElement TxtbxEmail(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_9"));
            return element;
        }

        public static IWebElement TxtbxFirstName(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_2"));
            return element;
        }

        public static IWebElement TxtbxLastName(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_3"));
            return element;
        }

        public static IWebElement TxtbxBillingAddress(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_4"));
            return element;
        }

        public static IWebElement TxtbxCity(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_5"));
            return element;
        }

        public static IWebElement TxtbxState(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_6"));
            return element;
        }

          public static IWebElement TxtbxCountry(IWebDriver driver)
        {

            IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_7"));
            return element;
        }

          public static IWebElement TxtbxZip(IWebDriver driver)
          {

              IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_8"));
              return element;
          }

          public static IWebElement TxtbxPhone(IWebDriver driver)
          {

              IWebElement element = driver.FindElement(By.Id("wpsc_checkout_form_18"));
              return element;
          }

          public static IWebElement BuyButton(IWebDriver driver)
          {

              IWebElement element = driver.FindElement(By.ClassName("wpsc_buy_button"));
              return element;
          }

          public static IWebElement CartMessage(IWebDriver driver)
          {

              IWebElement element = driver.FindElement(By.ClassName("entry-content"));
              return element;
          }
    }

  //helper to emulate writing out Error messages for a test case - replace with proper logging
    static void ErrorHandler(string strError, string strActual, string strExpected)
    {
        Console.WriteLine ("Error:" + strError );
        Console.WriteLine("Expected value: " + strExpected);
        Console.WriteLine("Actual value: " + strActual);

    } //ErrorHandler

 
    //Test case - walks though purchase of iPhone
    static void Test1()
    {

        // Create a new instance of the Firefox driver. Ideally, we should make this so we can call with multiple drivers
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
        ProductPageClass.AddtoCart(driver).Click();
        // Wait for the page to load, timeout after 5 seconds
        driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));
        //Confirm purchase
        ProductPageClass.CheckOutButton(driver).Click();     
        //Continue Checkout
        CheckoutPageClass.ContinueButton(driver).Click();
        //Checkout form - fill in address, etc
        CheckoutPageClass.TxtbxCurrentCountry(driver).SendKeys("USA");

        //find 'Calculate' button 
        CheckoutPageClass.CalculateButton(driver).Click();

        //fill in address, etc.   Consider placing values like this in a readable input file (i.e. xml file), particularly with a test suite of similiar pages
        //FillFormWithElement("email@homeaway.com", "wpsc_checkout_form_9", driver);
        CheckoutPageClass.TxtbxEmail(driver).SendKeys("email@homeaway.com");
        CheckoutPageClass.TxtbxFirstName(driver).SendKeys("First NameTest");
        CheckoutPageClass.TxtbxLastName(driver).SendKeys("Lastnametest");
        CheckoutPageClass.TxtbxBillingAddress(driver).SendKeys("One Billing Address Lane");
        CheckoutPageClass.TxtbxCity(driver).SendKeys("Citytest");
        CheckoutPageClass.TxtbxState(driver).SendKeys("TX");
        CheckoutPageClass.TxtbxCountry(driver).SendKeys("USA");
        CheckoutPageClass.TxtbxZip(driver).SendKeys("98052");
        CheckoutPageClass.TxtbxPhone(driver).SendKeys("123 456 7890");

        
        //Confirm purchase
        CheckoutPageClass.BuyButton(driver).Click();
        
         //Close the browser 
        driver.Quit();

    }


    //Test case: Verifies add and removing item from cart will yield empty message
    static void Test2()
    {
        IWebDriver driver = new FirefoxDriver();
        driver.Navigate().GoToUrl("http://store.demoqa.com/products-page/product-category/iphones/apple-iphone-4s-16gb-sim-free-black/");
        // Wait for the page to load, timeout after 5 seconds
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));      
        //Buy phone
        ProductPageClass.AddtoCart(driver).Click();
        // Wait for the page to load, timeout after 5 seconds
        driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));
        //Confirm purchase
        ProductPageClass.CheckOutButton(driver).Click();
        CheckoutPageClass.RemoveButton(driver).Click();
        if (CheckoutPageClass.CartMessage(driver).Displayed.ToString().Contains("oops"))
        {
            ErrorHandler("Error - Cart not empty", "", "");
        }


    }
    

    static void Main(string[] args)
    {
        Test1();  //walk through and purchase an Apple iPhone 4S 16GB SIM-Free
        Test2();  //Verify you can remove item from cart and get an error message
    }
}
