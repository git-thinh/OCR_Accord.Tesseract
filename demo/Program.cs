using Accord.Imaging;
using Accord.Imaging.Filters;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Tesseract;

namespace demo
{
    class oID
    {
        public string Text { set; get; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            string f;
            f = "11.jpg";
            f = "12.jpg";//???
            f = "13.jpg";
            f = "14.jpg";

            f = "1.jpg";
            f = "15.jpg";
            //f = "16.jpg";
            //f = "17.jpg";
            //f = "18.jpg";
            //f = "19.jpg";


            //Bitmap image = fun_filterImage_Basic(f);
            Bitmap image = fun_filterImage_v1(f);
            fid_process_getNoID(f, image);


            Console.WriteLine("\n\nPress ENTER/RETURN to exit");
            Console.ReadKey(true);
        }


        #region [ PROCESS IMAGE ]

        static void f_print_stopWatch(Stopwatch stopWatch)
        {
            TimeSpan ts;

            if (stopWatch == null) ts = DateTime.Now.TimeOfDay;
            else ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Time = " + elapsedTime);
        }

        static Bitmap fun_filterImage_Basic(string pathImage)
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
            f_print_stopWatch(stopWatch);

            result.Save(filename + "_filter.bmp");
            return result;
        }

        static Bitmap fun_filterImage_v1(string pathImage)
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
            f_print_stopWatch(stopWatch);

            result.Save(filename + "_filter.bmp");
            return result;
        }

        #endregion

        #region [ NUMBER NO ID ]

        static void fid_process_getNoID(string file, Bitmap image)
        {
            string s = fid_getText(file, fid_imageCrop(image), true);
            string l = fid_getLineFirst_NoId(s);
            string id = fid_getNumber_NoId(l);

            Console.WriteLine(file);
            Console.WriteLine(l);
            if (id.Length == 0)
                Console.WriteLine("\n\n\n\n{0}\n\n\n\n", s);
            else
                Console.WriteLine("\n\n\n\n{0}\n\n\n\n", id);
        }

        static Bitmap fid_imageCrop(Bitmap bitmap)
        {
            int h = (int)(bitmap.Height * 0.3);
            int top = (int)(h / 5) * 2;
            int w = (int)(bitmap.Width * 0.55);
            Crop filter = new Crop(new Rectangle(bitmap.Width - w, top, w, h));
            bitmap = filter.Apply(bitmap);

            h = bitmap.Height;
            w = bitmap.Width;
            top = (int)(h * 0.333);
            filter = new Crop(new Rectangle(0, top, w, h - top));
            bitmap = filter.Apply(bitmap);

            return bitmap;
        }

        static string fid_getText(string pathImage, Bitmap image, Boolean saveImageOcr = false)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string filename = pathImage.Split('.')[0];
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
                        f_print_stopWatch(stopWatch);

                        if (saveImageOcr)
                        {
                            var boxes = page.GetSegmentedRegions(PageIteratorLevel.Symbol).ToArray();

                            Bitmap rez = new Bitmap(image);
                            using (Graphics g = Graphics.FromImage(rez))
                            {
                                Pen p = new Pen(Brushes.Red, 1.0F);
                                foreach (Rectangle r in boxes)
                                {
                                    //Console.WriteLine(r);
                                    g.DrawRectangle(p, r);
                                }
                                g.DrawImage(rez, 0, 0);
                            }
                            rez.Save(filename + "_ok.bmp");
                        }

                        return text;
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
            return string.Empty;
        }

        const int SIZE_NO_ID = 8;
        static string fid_getLineFirst_NoId(string text)
        {
            //Regex regex = new Regex("[^0-9\\s]");
            //string s = regex.Replace(text, string.Empty);
            string[] a = text.Split(new string[] { "\r", "\n" }, StringSplitOptions.None).Where(x => x.Length > 6).ToArray();
            if (a.Length > 0)
            {
                string line;
                int k, ks;
                for (int i = 0; i < a.Length; i++)
                {
                    line = a[i].Trim();
                    if (line[line.Length - 1] == ':') line = line.Substring(0, line.Length - 1).Trim();

                    k = 0;
                    ks = 0;
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (Char.IsDigit(line[x])) k++;
                        if (line[x] == ' ') ks++;
                    }

                    if (k >= SIZE_NO_ID && ks <= SIZE_NO_ID - 3)
                    {
                        if (line.Contains(":") == false)
                            return line;
                        if (line.Contains(":") && line.IndexOf(':') < 4)
                            return line;
                    }
                }
            }

            return string.Empty;
        }

        static string fid_getNumber_NoId(string s)
        {
            if (s.Length >= SIZE_NO_ID)
            {
                if (s.Contains(":"))
                {
                    string[] a = s.Split(':');
                    s = s.Substring(a[0].Length + 1, s.Length - a[0].Length - 1);
                }
                else
                {
                    if (Char.IsDigit(s[2]) == false) s = s.Substring(2);
                    else if (Char.IsDigit(s[1]) == false) s = s.Substring(1);
                }

                //Regex regex = new Regex("[^0-9\\s]");
                Regex regex = new Regex("[^0-9]");
                s = regex.Replace(s, string.Empty);
                return s;
            }
            return string.Empty;
        }

        #endregion

    }
}
