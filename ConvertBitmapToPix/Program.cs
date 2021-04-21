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
        public const string _root = @"D:\Ocr\data-test\";
        public const string _result = @"D:\Ocr\data-test\_\";

        static void run_test_color() {
            TestColor.test_001_CastColorToNetColor();
            TestColor.test_002_ConvertRgb555ToPixColor();
            TestColor.test_003_ConvertArgb555ToPixColor(0xB9EC, 0x737B63FF);
            TestColor.test_003_ConvertArgb555ToPixColor(0x39EC, 0x737B6300);
            TestColor.test_004_ConvertRgb565ToPixColor();        
        }

        static void run_test_pix() {
            TestPix.test_001_CanReadAndWriteData(1);
            TestPix.test_001_CanReadAndWriteData(2);
            TestPix.test_001_CanReadAndWriteData(4);
            TestPix.test_001_CanReadAndWriteData(8);
            TestPix.test_001_CanReadAndWriteData(16);
            TestPix.test_001_CanReadAndWriteData(32);
            TestPix.test_002_ConvertToBitmap();
            TestPix.test_003_Convert_ScaledBitmapToPix();
            TestPix.test_004_Convert_BitmapToPix(1); //Note: 1bpp will not save pixmap when writing out the result, this is a limitation of leptonica (see pixWriteToTiffStream)
            TestPix.test_004_Convert_BitmapToPix(4); //Ignore = "4bpp images not supported."
            TestPix.test_004_Convert_BitmapToPix(8);
            TestPix.test_004_Convert_BitmapToPix(32);
            TestPix.test_005_Convert_BitmapToPix_Format8bppIndexed();
            TestPix.test_006_Convert_PixToBitmap(1, true, false);
            TestPix.test_006_Convert_PixToBitmap(1, false, false);
            TestPix.test_006_Convert_PixToBitmap(4, false, false);//4bpp images not supported.
            TestPix.test_006_Convert_PixToBitmap(4, true, false);//4bpp images not supported.
            TestPix.test_006_Convert_PixToBitmap(8, false, false);
            TestPix.test_006_Convert_PixToBitmap(8, true, false);//Haven't yet created a 8bpp grayscale test image.
            TestPix.test_006_Convert_PixToBitmap(32, false, true);
            TestPix.test_006_Convert_PixToBitmap(32, false, false);
            TestPix.test_009_CanCreatePixArray();
            TestPix.test_010_CanAddPixToPixArray();
            TestPix.test_011_CanRemovePixFromArray();
            TestPix.test_012_CanClearPixArray();
        }

        static void run_test_get_text() {
        }

        static void run_test_process_iamge() {
        }

        static void Main(string[] args)
        {
            //if (Directory.Exists(_result) == false) Directory.CreateDirectory(_result);
            //if (Directory.Exists(_root) == false) Directory.CreateDirectory(_root);

            //run_test_color();
            //run_test_pix();
            //run_test_get_text();
            //run_test_process_iamge(); 


            //TestProcessImage.test_007_RemoveLinesTest();
            TestProcessImage.test_005_ConvertRGBToGrayTest();

            Console.WriteLine("DONE ...");
            Console.ReadLine();
        }
    }

}
