using Accord.Imaging;
using Accord.Imaging.Filters;
using System;
using System.Diagnostics;
using System.Drawing;
using Tesseract;

namespace demo
{
    class Program
    {
        static Bitmap f_imageFilter(string pathImage)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string filename = pathImage.Split('.')[0];

            Bitmap bitmap = Accord.Imaging.Image.FromFile(pathImage);

            Grayscale gray = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap result = gray.Apply(bitmap);

            BradleyLocalThresholding thres = new BradleyLocalThresholding();
            thres.WindowSize = 80;
            thres.PixelBrightnessDifferenceLimit = 0.1F;
            thres.ApplyInPlace(result);

            Sharpen sharp = new Sharpen();
            sharp.ApplyInPlace(result);

            BilateralSmoothing smooth = new BilateralSmoothing();
            smooth.KernelSize = 7;
            smooth.SpatialFactor = 10;
            smooth.ColorFactor = 60;
            smooth.ColorPower = 0.5;
            smooth.ApplyInPlace(result);

            DocumentSkewChecker skew = new DocumentSkewChecker();
            double angle = skew.GetSkewAngle(result);

            RotateBilinear rot = new RotateBilinear(-angle);
            rot.FillColor = Color.White;
            result = rot.Apply(result);

            stopWatch.Stop();
            f_print(stopWatch);

            result.Save(filename + "_filter.bmp");

            return result;
        }

        static void f_print(Stopwatch stopWatch)
        {
            TimeSpan ts;

            if (stopWatch == null) ts = DateTime.Now.TimeOfDay;
            else ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Time = " + elapsedTime);
        }

        static void f_get_id(Bitmap image)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                using (var engine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default))
                {
                    //engine.SetVariable("tessedit_char_whitelist", "0123456789:");

                    using (var page = engine.Process(image, Rect.Empty))
                    //using (var page = engine.Process(image))
                    {
                        var text = page.GetText();
                        stopWatch.Stop();
                        f_print(stopWatch);

                        Console.WriteLine("Text: {0}", text);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
            }
            finally
            {
            }
        }

        static void Main(string[] args)
        {
            string f;
            f = "1.jpg";

            Bitmap image = f_imageFilter(f);
            f_get_id(image);

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            //{
            //    using (var img = Pix.LoadFromFile(""))
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            Console.WriteLine(page.GetText());
            //        }
            //    }
            //}
            //stopWatch.Stop();
            //f_print(stopWatch);

            Console.WriteLine("\n\nPress ENTER/RETURN to exit");
            Console.ReadKey(true);

        }
    }
}
