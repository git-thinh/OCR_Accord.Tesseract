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
        const string _root = "../../../data-test/";

        static Pix f_getPix_1(string file = "1.jpg")
        {
            if (File.Exists(file))
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(_root + file);
                Pix img = PixConverter.ToPix(bmp);
                return img;
            }

            return null;
        }

        static void f_test_CanProcess24bitImage_1(string file = "phototest.tif", string lang = "eng")
        {
            using (var engine = new TesseractEngine(@"./tessdata", lang, EngineMode.Default))
            {
                using (var img = Bitmap.FromFile(_root + file))
                {
                    using (var img2 = new Bitmap(img))
                    {
                        using (var img24bit = img2.Clone(
                            new Rectangle(new Point(0, 0), img2.Size),
                            PixelFormat.Format24bppRgb))
                        {
                            //Assert.That(img24bit.PixelFormat, Is.EqualTo(PixelFormat.Format24bppRgb));
                            using (var page = engine.Process(img24bit))
                            {
                                var thresholdedImg = page.GetThresholdedImage();
                                thresholdedImg.Save(_root + "_/" + file + "_thresholdedImg.tiff"); // <-- upload this file please

                                var text = page.GetText();
                                Console.WriteLine("RESULT = " + text);
                                File.WriteAllText(_root + "_/_text.txt", text, Encoding.UTF8);

                                //const string expectedText = "This is a lot of 12 point text to test the\nocr code and see if it works on all types\nof file format.\n\nThe quick brown dog jumped over the\nlazy fox. The quick brown dog jumped\nover the lazy fox. The quick brown dog\njumped over the lazy fox. The quick\nbrown dog jumped over the lazy fox.\n\n";
                                //Assert.That(text, Is.EqualTo(expectedText));
                            }
                        }
                    }
                }
            }
        }

        static void f_test_CanProcess24bitImage_2(string file = "1.jpg", string lang = "eng")
        { 
            using (var engine = new TesseractEngine(@"./tessdata", lang, EngineMode.Default))
            {
                using (var img = (Bitmap)Bitmap.FromFile(_root + file))
                {
                    using (var page = engine.Process(img))
                    {
                        var thresholdedImg = page.GetThresholdedImage();
                        thresholdedImg.Save(_root + "_/" + file + "_thresholdedImg.tiff"); // <-- upload this file please
                        
                        var text = page.GetText();
                        Console.WriteLine("RESULT = " + text);
                        File.WriteAllText(_root + "_/_text.txt", text, Encoding.UTF8);

                    } 
                }
            }
        }

        static void Main(string[] args)
        {
            if (Directory.Exists(_root + "_") == false) Directory.CreateDirectory(_root + "_");
            //var pix = f_getPix_1();

            //f_test_CanProcess24bitImage_1();
            //f_test_CanProcess24bitImage_2("12.jpg", "eng");
            //f_test_CanProcess24bitImage_2("12.jpg", "vie");

            Console.WriteLine("DONE ...");
            Console.ReadLine();
        }
    }
}
