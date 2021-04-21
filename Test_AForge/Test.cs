using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Test_AForge
{
    public static class test
    {
        //public const string PATH_IN = @"C:\TEMP\grey\";
        //public const string PATH_IN = @"C:\Git\OCR\OCR_Accord.Tesseract\data-test\rectangle\";
        public const string PATH_IN = @"C:\ocr-images\";
        const string PATH_OUT = @"C:\TEMP\";

        public static void t001_detect_rectangles(string file = "YfXPD.png", string folder_out = "")
        {
            //https://stackoverflow.com/questions/5945156/c-sharp-detect-rectangles-in-image

            string path_out = PATH_OUT + (folder_out.Length == 0 ? "" : folder_out + @"\");

            // Open your image
            //string path = "test.png";
            string path = PATH_IN + file;
            Bitmap image = (Bitmap)Bitmap.FromFile(path);

            // locating objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;

            blobCounter.ProcessImage(image);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // check for rectangles
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            foreach (var blob in blobs)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                List<IntPoint> cornerPoints;

                // use the shape checker to extract the corner points
                if (shapeChecker.IsQuadrilateral(edgePoints, out cornerPoints))
                {
                    // only do things if the corners form a rectangle
                    if (shapeChecker.CheckPolygonSubType(cornerPoints) == PolygonSubType.Rectangle)
                    {
                        // here i use the graphics class to draw an overlay, but you
                        // could also just use the cornerPoints list to calculate your
                        // x, y, width, height values.

                        int i = 0;
                        System.Drawing.Point[] a = new System.Drawing.Point[cornerPoints.Count];
                        //List<AForge.Point> Points = new List<AForge.Point>();
                        foreach (var point in cornerPoints)
                        {
                            //Points.Add(new AForge.Point(point.X, point.Y));
                            a[i] = new System.Drawing.Point(point.X, point.Y);
                            i++;
                        }

                        Graphics g = Graphics.FromImage(image);
                        //g.DrawPolygon(new Pen(Color.Red, 5.0f), Points.ToArray());
                        g.DrawPolygon(new Pen(Color.Red, 5.0f), a);

                        image.Save(path_out + file);
                    }
                }
            }
        }

        public static void t002_Grey_Image(string file = "YfXPD.png", string folder_out = "")
        {
            string path_file = PATH_IN + file;
            Bitmap image = (Bitmap)Bitmap.FromFile(path_file);
            string path_out = PATH_OUT + (folder_out.Length == 0 ? "" : folder_out + @"\");
            if (!Directory.Exists(path_out)) Directory.CreateDirectory(path_out);

            Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grImage = grayFilter.Apply(image);
            grImage.Save(path_out + file);
        }

        public static void t003_detect_rectangles(string file = "0.jpg", string folder_out = "")
        {
            string path_file = PATH_IN + file;
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(path_file);

            // lock image
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(0, 64);
            colorFilter.Green = new IntRange(0, 64);
            colorFilter.Blue = new IntRange(0, 64);
            colorFilter.FillOutsideRange = false;

            colorFilter.ApplyInPlace(bitmapData);


            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;

            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Graphics g = Graphics.FromImage(bitmap);
            Pen redPen = new Pen(Color.Red, 2);
            Pen yellowPen = new Pen(Color.Yellow, 2);
            Pen greenPen = new Pen(Color.Green, 2);
            Pen bluePen = new Pen(Color.Blue, 2);

            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints =
                    blobCounter.GetBlobsEdgePoints(blobs[i]);

                AForge.Point center;
                float radius;

                if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                {
                    g.DrawEllipse(yellowPen,
                        (float)(center.X - radius), (float)(center.Y - radius),
                        (float)(radius * 2), (float)(radius * 2));
                }
                else
                {
                    List<IntPoint> corners;

                    if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                    {
                        if (shapeChecker.CheckPolygonSubType(corners) ==
                            PolygonSubType.Rectangle)
                        {
                            g.DrawPolygon(greenPen, ToPointsArray(corners));
                        }
                        else
                        {
                            g.DrawPolygon(bluePen, ToPointsArray(corners));
                        }
                    }
                    else
                    {
                        corners = PointsCloud.FindQuadrilateralCorners(edgePoints);
                        g.DrawPolygon(redPen, ToPointsArray(corners));
                    }
                }
            }

            redPen.Dispose();
            greenPen.Dispose();
            bluePen.Dispose();
            yellowPen.Dispose();
            g.Dispose();

            string path_out = PATH_OUT + (folder_out.Length == 0 ? "" : folder_out + @"\");
            if (!Directory.Exists(path_out)) Directory.CreateDirectory(path_out);
            bitmap.Save(path_out + file);
        }

        // Conver list of AForge.NET's points to array of .NET points
        private static System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }

            return array;
        }





        static Bitmap ResizeImage(System.Drawing.Image image, int[] a)
        {
            int x = a[1], y = a[2], width = a[3], height = a[4];

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void t002_detect_rectangles(string file = "YfXPD.png", string folder_out = "")
        {
            string lang = "eng";
            //lang = "vie";
            string time = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            int _w = 0;
            int _text_width_min = 0;
            int _padding_top = 2;
            int _padding_bottom = 3;
            int _padding_left = 3;
            int _x_min = -1;

            string path_out = PATH_OUT + (folder_out.Length == 0 ? "" : folder_out + @"\");
            if (!Directory.Exists(path_out)) Directory.CreateDirectory(path_out);
            string path = PATH_IN + file;
            var image = System.Drawing.Image.FromFile(path);

            //string s = new WebClient().DownloadString("http://192.168.10.54:54321?file=" + file);
            string s = new WebClient().DownloadString("http://127.0.0.1:54321?lang=" + lang + "&file=" + file);
            var ls = JsonConvert.DeserializeObject<List<int[]>>(s);
            var arr = ls.ToArray();


            //_w = image.Width;
            //_text_width_min = int.Parse((_w / 3).ToString().Split('.')[0]);

            //arr = arr.Where(x => x[3] > _text_width_min).ToArray();

            //var xa = arr.Select(x => x[1]).Where(x => x > _w / 3 && x < _w * 2 / 3).ToArray();
            //if (xa.Length > 0)
            //{
            //    _x_min = xa.Min() - _padding_left;
            //    arr = arr.Where(x => x[1] + x[3] > _x_min).ToArray();
            //    arr = arr.Select(x => { x[1] = _x_min; return x; }).ToArray();
            //}

            //var shadowBrush = new SolidBrush(Color.FromArgb(50, Color.Red));
            //using (Graphics g = Graphics.FromImage(image))
            //    foreach (var a in arr)
            //        g.FillRectangle(shadowBrush, new RectangleF(a[1], a[2] - _padding_top, a[3], a[4] + _padding_bottom));
            //image.Save(path_out + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg");
            //////image.Save(path_out + file);


            foreach (var a in arr)
            {
                //Rectangle cropRect = new Rectangle(a[1], a[2] - _padding_top, a[3], a[4] + _padding_bottom);
                Rectangle cropRect = new Rectangle(a[1], a[2], a[3], a[4]);
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
                using (Graphics g = Graphics.FromImage(target))
                    g.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                target.Save(path_out + time + "_" + string.Join("-", a.Select(x => x.ToString()).ToArray()) + ".jpg");
            }

        }

    }
}
