//Import packages
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.Globalization;
using System.Text;
using OpenQA.Selenium.Safari;
using System.Collections.Generic;
using System.Web;

namespace Case_Study_Webscraper_Bol.com_DevOps_Britt_De_Keyser
{
    public class ScrapingTest
    {
        //Variables
        //Specify url
        String url = "https://www.bol.com/be/nl/s/?searchtext=laptop";
        //Count and display number of videos found based on the search term
        static Int32 vcount = 1;
        //Creating an instance of the Webdriver interface and casting it to browser driver class
        IWebDriver driver = new ChromeDriver();

        //Open Bol.com via Google Chrome
        public void Open()
        {
            // Local Selenium WebDriver
            driver = new ChromeDriver();
            //Maximize browser window
            driver.Manage().Window.Maximize();
        }

        //Give the test a name
        [Test(Description = "Web Scraping Bol.com"), Order(1)]
        public void BolcomScraping()
        {
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            //Wait until the document is ready (loaded)
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Suspends the current thread for 5 seconds
            Thread.Sleep(5000);

            //Retrieve a list of the products
            By elem_product_item = By.ClassName("product-item--row");
            ReadOnlyCollection<IWebElement> products = driver.FindElements(elem_product_item);

            //Write to an .csv file
            var csv = new StringBuilder();

            /* Go through the Products List and scrap the same to get the attributes of the products in the channel */
            foreach (IWebElement product in products.Take(5))
            {
                string str_link, str_title, str_creator, str_subtitle, str_prices, str_details;
                //Get link from product
                IWebElement elem_product_li = product.FindElement(By.CssSelector("[class = 'product-title px_list_page_product_click list_page_product_tracking_target']"));
                str_link = elem_product_li.GetAttribute("href");

                //Get creator from product
                IWebElement elem_product_creator = product.FindElement(By.ClassName("product-creator"));
                str_creator = elem_product_creator.Text;

                //Get title from product
                IWebElement elem_product_title = product.FindElement(By.ClassName("product-title"));
                str_title = elem_product_title.Text;

                //Get subtitle from product
                IWebElement elem_product_subtitle = product.FindElement(By.ClassName("product-subtitle"));
                str_subtitle = elem_product_subtitle.Text;

                //Get prices from product
                IWebElement elem_product_prices = product.FindElement(By.ClassName("product-prices"));
                str_prices = elem_product_prices.Text;

                //Get details from product
                IWebElement elem_product_details = product.FindElement(By.ClassName("product-small-specs"));
                str_details = elem_product_details.Text;


                //Write the output to the screen for the end-user
                Console.WriteLine("******* Product " + vcount + " *******");
                var line1 = ("******* Product " + vcount + " *******");
                //Append line to the .csv file
                csv.AppendLine(line1);
                Console.WriteLine("Product Link: " + str_link);
                var line2 = ("Product Link: " + str_link);
                csv.AppendLine(line2);
                Console.WriteLine("Product Creator: " + str_creator);
                var line3 = ("Product Creator: " + str_creator);
                csv.AppendLine(line3);
                Console.WriteLine("Product Title: " + str_title);
                var line4 = ("Product Title: " + str_title);
                csv.AppendLine(line4);
                Console.WriteLine("Product Subtitle: " + str_subtitle);
                var line5 = ("Product Subtitle: " + str_subtitle);
                csv.AppendLine(line5);
                Console.WriteLine("Product Details: " + str_details);
                var line6 = ("Product Details: " + str_details);
                csv.AppendLine(line6);
                Console.WriteLine("Product Prices: " + str_prices);
                var line7 = ("Product Prices: " + str_prices);
                csv.AppendLine(line7);
                Console.WriteLine("\n");
                var line8 = ("\n");
                csv.AppendLine(line8);
                vcount++;
            }
            //Show message to enduser if test is complete    
            Console.WriteLine("Scraping Data from Bol.com Search Passed");
            //Path to .csv file
            File.WriteAllText("C:/Users/britt/source/repos/Bolcom.csv", csv.ToString());
        }

        //Close the browser
        [TearDown]
        public void Close_Browser()
        {
            driver.Quit();
        }
    }
}
