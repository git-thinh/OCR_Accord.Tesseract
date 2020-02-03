using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Test_AForge
{
    class Program
    {
        static void Main(string[] args)
        {
            //test.t003_detect_rectangles("1.1.jpg");
            test.t002_detect_rectangles("0.jpg", "t002_detect_rectangles");
            //test.t002_detect_rectangles("0.0.jpg", "t002_detect_rectangles");
            //test.t002_detect_rectangles("1.0.jpg", "t002_detect_rectangles");
            //test.t002_detect_rectangles("1.1.jpg", "t002_detect_rectangles");
            //test.t002_detect_rectangles("1.2.jpg", "t002_detect_rectangles");

            //var a = Directory.GetFiles(test.PATH_IN);
            //for (int i = 0; i < a.Length; i++)
            //{
            //    string file = Path.GetFileName(a[i]);
            //    //test.t001_detect_rectangles(file, "grey_rectangle_1");
            //    //test.t002_Grey_Image(file, "grey");
            //    //test.t003_detect_rectangles(file, "grey_rectangle_3");

            //    test.t002_detect_rectangles(file, "t002_detect_rectangles");
            //    Console.WriteLine("OK[" + i + "|" + a.Length + "]: " + file);
            //}


            //Console.WriteLine("DONE ...");
            //Console.ReadLine();
        }
    }
}
