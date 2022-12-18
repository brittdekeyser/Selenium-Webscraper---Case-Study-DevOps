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


namespace Case_Study_Webscraper_Job_DevOps_Britt_De_Keyser
{
    public class ScrapingTest
    {
        //Variables
        //Specify url
        String url = "https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=BI";
        //Count and display number of videos found based on the search term
        static Int32 vcount = 1;
        //Creating an instance of the Webdriver interface and casting it to browser driver class
        IWebDriver driver = new ChromeDriver();

        //Open ICT Job via Google Chrome
        public void Open()
        {
            // Local Selenium WebDriver 
            driver = new ChromeDriver();
            //Maximize browser window
            driver.Manage().Window.Maximize();
        }

        //Give a name to the test
        [Test(Description = "Web Scraping ICT Job"), Order(1)]
        public void ICTJobScraping()
        {
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            //Wait until the document is ready
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Suspends the current thread for 5 seconds
            Thread.Sleep(5000);

            //Retrieve a list of the different jobs
            By elem_job_info = By.ClassName("job-info");
            ReadOnlyCollection<IWebElement> jobs = driver.FindElements(elem_job_info);

            //Write to an .csv file
            var csv = new StringBuilder();

            /* Go through the Jobs List and scrap the same to get the attributes of the jobs */
            foreach (IWebElement job in jobs.Take(5))
            {
                string str_link, str_title, str_comp, str_loc, str_keywords;
                //Get link from job
                IWebElement elem_video_li = job.FindElement(By.CssSelector("[class = 'job-title search-item-link']"));
                str_link = elem_video_li.GetAttribute("href");

                //Get title from job
                IWebElement elem_job_title = job.FindElement(By.ClassName("job-title"));
                str_title = elem_job_title.Text;

                //Get company from job
                IWebElement elem_job_comp = job.FindElement(By.ClassName("job-company"));
                str_comp = elem_job_comp.Text;

                //Get location from job
                IWebElement elem_job_loc = job.FindElement(By.ClassName("job-location"));
                str_loc = elem_job_loc.Text;

                //Get keywords from job
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
            //Show message to enduser if test is complete    
            Console.WriteLine("Scraping Data from ICT Job Search Passed");
            //Path to .csv file
            File.WriteAllText("C:/Users/britt/source/repos/ICTJob.csv", csv.ToString());
        }

        //Close the browser
        [TearDown]
        public void Close_Browser()
        {
            driver.Quit();
        }
    }
}