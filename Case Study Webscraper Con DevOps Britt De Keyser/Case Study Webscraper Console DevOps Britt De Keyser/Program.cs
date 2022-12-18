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

namespace Case_Study_Webscraper_Console_DevOps_Britt_De_Keyser
{
    class Scraper
    {
        static void Main(string[] args)
        {
            Console.WriteLine("What would you like to search for? ");
            Console.WriteLine("1. A YouTube Video");
            Console.WriteLine("2. A Job Vacancy");
            Console.WriteLine("3. A Product on Bol.com");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    YouTubeScraping();
                    break;
                case "2":
                    ICTJobScraping();
                    break;
                case "3":
                    BolcomScraping();
                    break;
                case "4":
                    break;
            }

            Console.Write("Press any key to close the console app...");
            Console.ReadKey();
        }

        //ICT Job Webscraper
        static void ICTJobScraping()
        {
            //Variables
            String url = "https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=BI";
            Int32 vcount = 1;
            IWebDriver driver = new ChromeDriver();

            // Local Selenium WebDriver
            driver = new ChromeDriver();
            //Maximize browser window
            driver.Manage().Window.Maximize();
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            //Wait until the document is ready
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Suspends the current thread for 5 seconds
            Thread.Sleep(5000);

            By elem_job_info = By.ClassName("job-info");
            ReadOnlyCollection<IWebElement> jobs = driver.FindElements(elem_job_info);

            //Write to an .csv file
            var csv = new StringBuilder();

            /* Go through the Jobs List and scrap the same to get the attributes of the jobs */
            foreach (IWebElement job in jobs.Take(5))
            {
                string str_link, str_title, str_comp, str_loc, str_keywords;
                IWebElement elem_video_li = job.FindElement(By.CssSelector("[class = 'job-title search-item-link']"));
                str_link = elem_video_li.GetAttribute("href");

                IWebElement elem_job_title = job.FindElement(By.ClassName("job-title"));
                str_title = elem_job_title.Text;

                IWebElement elem_job_comp = job.FindElement(By.ClassName("job-company"));
                str_comp = elem_job_comp.Text;

                IWebElement elem_job_loc = job.FindElement(By.ClassName("job-location"));
                str_loc = elem_job_loc.Text;

                IWebElement elem_job_keywords = job.FindElement(By.ClassName("job-keywords"));
                str_keywords = elem_job_keywords.Text;


                //Write the output to the screen for the end-user
                Console.WriteLine("******* Job " + vcount + " *******");
                var line1 = ("******* Job " + vcount + " *******");
                //Append line to the .csv file
                csv.AppendLine(line1);
                Console.WriteLine("Job Title: " + str_title);
                var line2 = ("Job Title: " + str_title);
                csv.AppendLine(line2);
                Console.WriteLine("Job Company: " + str_comp);
                var line3 = ("Job Company: " + str_comp);
                csv.AppendLine(line3);
                Console.WriteLine("Job Location: " + str_loc);
                var line4 = ("Job Location: " + str_loc);
                csv.AppendLine(line4);
                Console.WriteLine("Keywords: " + str_keywords);
                var line5 = ("Keywords: " + str_keywords);
                csv.AppendLine(line5);
                Console.WriteLine("Detail Page: " + str_link);
                var line6 = ("Detail Page: " + str_link);
                csv.AppendLine(line6);
                Console.WriteLine("\n");
                var line7 = ("\n");
                csv.AppendLine(line7);
                vcount++;
            }
            Console.WriteLine("Scraping Data from ICT Job Search Passed");
            File.WriteAllText("C:/Users/britt/source/repos/ICTJob.csv", csv.ToString());
        }


        //YouTube Webscraper
        //Give the test a name
        [Test(Description = "Web Scraping YouTube"), Order(1)]
        static void YouTubeScraping()
        {
            //Variables
            String url = "https://www.youtube.com/results?search_query=ed+sheeran";
            Int32 vcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            //Wait until the document is ready (loaded)
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Suspends the current thread for 5 seconds
            Thread.Sleep(5000);

            By elem_video_link = By.TagName("ytd-video-renderer");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);

            //Write to an .csv file
            var csv = new StringBuilder();

            /* Go through the Videos List and scrap the same to get the attributes of the videos in the channel */
            foreach (IWebElement video in videos.Take(5))
            {
                string str_title, str_views, str_upl, str_link, str_duur;
                IWebElement elem_video_li = video.FindElement(By.CssSelector("[class = 'yt-simple-endpoint style-scope ytd-video-renderer']"));
                str_link = elem_video_li.GetAttribute("href");

                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;

                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;

                IWebElement elem_video_uploader = video.FindElement(By.ClassName("long-byline"));
                str_upl = elem_video_uploader.Text;

                IWebElement elem_video_duur = video.FindElement(By.ClassName("yt-simple-endpoint"));
                str_duur = elem_video_duur.Text;

                //Write the output to the screen for the end-user
                Console.WriteLine("******* Video " + vcount + " *******");
                var line1 = ("******* Video " + vcount + " *******");
                //Append line to the .csv file
                csv.AppendLine(line1);
                Console.WriteLine("Video Link: " + str_link);
                var line2 = ("Video Link: " + str_link);
                csv.AppendLine(line2);
                Console.WriteLine("Video Title: " + str_title);
                var line3 = ("Video Title: " + str_title);
                csv.AppendLine(line3);
                Console.WriteLine("Video Views: " + str_views);
                var line4 = ("Video Views: " + str_views);
                csv.AppendLine(line4);
                Console.WriteLine("Video Publisher: " + str_upl);
                var line5 = ("Video Publisher: " + str_upl);
                csv.AppendLine(line5);
                Console.WriteLine("Video Duur: " + str_duur);
                var line6 = ("Video Duur: " + str_duur);
                csv.AppendLine(line6);
                Console.WriteLine("\n");
                var line7 = ("\n");
                csv.AppendLine(line7);
                vcount++;
            }
            Console.WriteLine("Scraping Data from YouTube Search Passed");
            File.WriteAllText("C:/Users/britt/source/repos/YouTube.csv", csv.ToString());
        }

        //Bol.com Webscraper
        //Give the test a name
        [Test(Description = "Web Scraping Bol.com"), Order(1)]
        static void BolcomScraping()
        {
            //Variables
            String url = "https://www.bol.com/be/nl/s/?searchtext=laptop";
            Int32 vcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            //Wait until the document is ready (loaded)
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Suspends the current thread for 5 seconds
            Thread.Sleep(5000);

            By elem_product_item = By.ClassName("product-item--row");
            ReadOnlyCollection<IWebElement> products = driver.FindElements(elem_product_item);

            //Write to an .csv file
            var csv = new StringBuilder();

            /* Go through the Videos List and scrap the same to get the attributes of the videos in the channel */
            foreach (IWebElement product in products.Take(5))
            {
                string str_link, str_title, str_creator, str_subtitle, str_prices, str_details;
                IWebElement elem_product_li = product.FindElement(By.CssSelector("[class = 'product-title px_list_page_product_click list_page_product_tracking_target']"));
                str_link = elem_product_li.GetAttribute("href");

                IWebElement elem_product_creator = product.FindElement(By.ClassName("product-creator"));
                str_creator = elem_product_creator.Text;

                IWebElement elem_product_title = product.FindElement(By.ClassName("product-title"));
                str_title = elem_product_title.Text;

                IWebElement elem_product_subtitle = product.FindElement(By.ClassName("product-subtitle"));
                str_subtitle = elem_product_subtitle.Text;

                IWebElement elem_product_prices = product.FindElement(By.ClassName("product-prices"));
                str_prices = elem_product_prices.Text;

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
            Console.WriteLine("Scraping Data from Bol.com Search Passed");
            File.WriteAllText("C:/Users/britt/source/repos/Bolcom.csv", csv.ToString());
        }
    }
}







