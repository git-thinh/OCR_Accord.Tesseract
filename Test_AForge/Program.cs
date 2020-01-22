using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Test_AForge
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open your image
            //string path = "test.png";
            string path = @"C:\ocr-images\1.1.jpg";
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

                        image.Save("result.png");
                    }
                }
            }

        }
    }
}
