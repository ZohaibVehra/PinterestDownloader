using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinterestGetPhotos;
using System.Diagnostics;
using System.Collections;
using System.Windows.Automation;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace PinterestGetPhotos
{
    internal class Program
    {
        static void Main(string[] args)
        {

           



            Logic controller = new Logic();




            string path = @"c:\Users\Zohaib\Desktop\getPics.txt";

            List<string> strList = new List<string>();
            foreach (string line in System.IO.File.ReadLines(path))
            {
                strList.Add(line);
            }

            string SavePath = strList[0];
            strList.RemoveAt(0);
            //Console.WriteLine($"Save Path is {SavePath}");

           // fixName(@"D:\art prac\Perspective Ref\Complex Scenery", SavePath);
            


            foreach (string url in strList)
            {
                string htmlcode = controller.goToWebPage(url);
                
                string imageURL = FindImageUrl(htmlcode);


                SavePath = fixName(SavePath.Substring(0, SavePath.Length - 5), SavePath);


                saveImage(imageURL, SavePath);
            }

            //resetting getpics txt file to just the path
            string tempPath = "";
            List<string> tempL = new List<string>();
            foreach (string line in System.IO.File.ReadLines(path))
            {
                tempL.Add(line);
            }
            tempPath = tempL[0];

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, tempPath);







            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            string fixName(string photosfolder, string pathtoAdjust)
            {
                int x = 1;

                string retstr = SavePath;
                    DirectoryInfo di = new DirectoryInfo(photosfolder);
                    FileInfo[] files = di.GetFiles("*.png");
                if (files.Length ==0)
                {

                }
                else
                {
                    foreach (FileInfo file in files)
                    {
                        string name = file.Name;
                       // Console.WriteLine(file.Name);
                        name = name.Substring(0, name.IndexOf(".png"));
                        //Console.WriteLine(name);

                        int picnum= Int32.Parse(name);
                        if(x<=picnum)
                            x=picnum+1;
                    }
                    // Console.WriteLine($"x is {x}");
                    retstr = SavePath.Substring(0, SavePath.Length - 5);
                    retstr = retstr + "" + x + ".png";
                    //Console.WriteLine(retstr);
                }
                return retstr;   
                     
                    
                

            }


            void saveImage(string imageurl, string pathtosave)
            {
                try
                {
                    controller.SaveImage(imageurl, pathtosave, ImageFormat.Png);
                }

                catch (ExternalException)
                {
                    Console.WriteLine("Something wrong with format");
                    // applicable here
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("error when saving image");
                }
            }

            string FindImageUrl(string htmlcode)
            {
                List<string> potentialurls = new List<string>();
                string match = "https://i.pinimg.com/564x";

                string str = htmlcode.Substring(htmlcode.IndexOf(match));

                int end =  str.IndexOf(".jpg");
                str = str.Substring(0, end+4);

                return str;
               
            }
        }
    }
}
