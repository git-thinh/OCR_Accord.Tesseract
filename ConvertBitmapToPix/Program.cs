using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Tesseract;

namespace ConvertBitmapToPix
{
    class Program
    {
       public const string _root = "../../../data-test/";
       public const string _result = "../../../data-test/_/";

        static void Main(string[] args)
        {
            if (Directory.Exists(_result) == false) Directory.CreateDirectory(_result);

            //var pix = TestGetText.GetPix_1();
            //TestGetText.CanProcess24bitImage_1();
            //TestGetText.CanProcess24bitImage_2("12.jpg", "eng");
            //TestGetText.CanProcess24bitImage_2("12.jpg", "vie");


            Console.WriteLine("DONE ...");
            Console.ReadLine();
        }
    }

}
