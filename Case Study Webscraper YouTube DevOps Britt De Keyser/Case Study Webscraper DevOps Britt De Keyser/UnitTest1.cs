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

namespace Case_Study_Webscraper_YouTube_DevOps_Britt_De_Keyser
{
    public class ScrapingTest
    {
        //Variables
        //Specify url
        String url = "https://www.youtube.com/results?search_query=ed+sheeran"; 
        //Count and display number of videos found based on the search term
        static Int32 vcount = 1;
        //Creating an instance of the Webdriver interface and casting it to browser driver class
        IWebDriver driver = new ChromeDriver();

        //Open YouTube via Google Chrome
        public void Open()
        {
            // Local Selenium WebDriver
            driver = new ChromeDriver();
            //Maximize browser window
            driver.Manage().Window.Maximize();
        }

        //Give the test a name
        [Test(Description = "Web Scraping YouTube"), Order(1)]
        public void YouTubeScraping()
        {
            driver.Url = url;
            /* Explicit Wait to ensure that the page is loaded completely by reading the DOM state */
            var timeout = 10000; /* Maximum wait time of 10 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            //Wait until the document is ready (loaded)
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            //Suspends the current thread for 5 seconds
            Thread.Sleep(5000);

            //Retrieve a list of the different videos
            By elem_video_link = By.TagName("ytd-video-renderer");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);

            //Write to an .csv file
            var csv = new StringBuilder();

            /* Go through the Videos List and scrap the same to get the attributes of the videos in the channel */
            foreach (IWebElement video in videos.Take(5))
            {
                string str_title, str_views, str_upl, str_link, str_duur;
                //Get link from video
                IWebElement elem_video_li = video.FindElement(By.CssSelector("[class = 'yt-simple-endpoint style-scope ytd-video-renderer']"));
                str_link = elem_video_li.GetAttribute("href");

                //Get title from video
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;

                //Get views from video
                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;

                //Get uploader from video
                IWebElement elem_video_uploader = video.FindElement(By.ClassName("long-byline"));
                str_upl = elem_video_uploader.Text;

                //Get duration from video
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
            //Show message to enduser if test is complete
            Console.WriteLine("Scraping Data from YouTube Search Passed");
            //Path to .csv file
            File.WriteAllText("C:/Users/britt/source/repos/YouTube.csv", csv.ToString());
        }

        //Close the browser
        [TearDown]
        public void Close_Browser()
        {
            driver.Quit();
        }
    }
}
