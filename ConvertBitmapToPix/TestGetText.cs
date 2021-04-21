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

    public class TestGetText
    {
        //const string _root = Program._root;

        static Pix GetPix_1(string file = "1.jpg")
        {
            if (File.Exists(file))
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(_root + file);
                Pix img = PixConverter.ToPix(bmp);
                return img;
            }

            return null;
        }

        public const string _root = @"D:\Ocr\data-test\";
        public const string _result = @"D:\Ocr\data-test\_\";

        public static void test_001_CanProcess24bitImage_1(string file = "phototest.tif", string lang = "eng")
        {
            file = _root + @"text\phototest.tif";
            //file = @"C:\temp\1.jpg";
            //file = @"C:\temp\2.jpg";
            //file = @"C:\temp\5.jpg";


            string name = Path.GetFileName(file).Split('.')[0];



            using (var engine = new TesseractEngine(@"./tessdata", lang, EngineMode.Default))
            {
                using (var img = Bitmap.FromFile(file))
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
                                thresholdedImg.Save(_root + "_/" + name + "_thresholdedImg.tiff"); // <-- upload this file please

                                var text = page.GetText();
                                Console.WriteLine("RESULT = " + text);
                                //File.WriteAllText(_root + "_/_text.txt", text, Encoding.UTF8);

                                //const string expectedText = "This is a lot of 12 point text to test the\nocr code and see if it works on all types\nof file format.\n\nThe quick brown dog jumped over the\nlazy fox. The quick brown dog jumped\nover the lazy fox. The quick brown dog\njumped over the lazy fox. The quick\nbrown dog jumped over the lazy fox.\n\n";
                                //Assert.That(text, Is.EqualTo(expectedText));
                            }
                        }
                    }
                }
            }
        }

        public static void test_001_CanProcess24bitImage_2(string file = "1.jpg", string lang = "eng")
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
    }
}
