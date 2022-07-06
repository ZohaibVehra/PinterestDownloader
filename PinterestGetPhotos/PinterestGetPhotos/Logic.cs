using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Automation;
using System.Collections;
using System.Net.Http;
using System.Drawing;
using System.Net;
using System.Drawing.Imaging;
using System.IO;

namespace PinterestGetPhotos
{
    public class Logic
    {
        public int x = 5;

        public string TestFunc()
        {
            return "hello";
        }

        public bool isChromeOpen()
        {
            bool result = false;

            Process[] procsChrome = Process.GetProcessesByName("chrome");
            if (procsChrome.Length <= 0)
            {
                Console.WriteLine("Chrome is not running");
            }
            else
            {
                Console.WriteLine("Chrome is running");
                foreach (Process proc in procsChrome)
                {
                    // the chrome process must have a window 
                    if (proc.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }
                    Console.WriteLine("Iteration");

                    // to find the tabs we first need to locate something reliable - the 'New Tab' button 
                    AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
                    Condition condNewTab = new PropertyCondition(AutomationElement.NameProperty, "New Tab");
                    AutomationElement elmNewTab = root.FindFirst(TreeScope.Descendants, condNewTab);
                    // get the tabstrip by getting the parent of the 'new tab' button 
                    TreeWalker treewalker = TreeWalker.ControlViewWalker;
                    AutomationElement elmTabStrip = treewalker.GetParent(elmNewTab);

                    // loop through all the tabs and get the names which is the page title 
                    Condition condTabItem = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                    foreach (AutomationElement tabitem in elmTabStrip.FindAll(TreeScope.Children, condTabItem))
                    {
                        Console.WriteLine(tabitem.Current.Name);
                    }
                }
            }


            return result;
        }






        public string goToWebPage(string url)
        {
            string result;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                         result = content.ReadAsStringAsync().Result;
                    }
                }
            }
            return result;
        }

        public string GetActiveTabUrl()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            string urls = "";
            if (procsChrome.Length <= 0)
                return null;

            foreach (Process proc in procsChrome)
            {
                // the chrome process must have a window 
                if (proc.MainWindowHandle == IntPtr.Zero)
                    continue;
                Console.WriteLine("iter");
                // to find the tabs we first need to locate something reliable - the 'New Tab' button 
                AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
                var SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
                if (SearchBar != null)
                    urls+= (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty);
            }
            return urls;
            //return null;
        }


        public void SaveImage(string imageUrl, string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }


       


    }



}
