using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;

namespace ConvertBitmapToPix
{
    public unsafe class TestPix : TestBase
    {
        const int Width = 59, Height = 53;
        //[TestCase(1)]
        //[TestCase(2)]
        //[TestCase(4)]
        //[TestCase(8)]
        //[TestCase(16)]
        //[TestCase(32)]
        public static void test_001_CanReadAndWriteData(int depth)
        {
            using (var pix = Pix.Create(Width, Height, depth))
            {
                var pixData = pix.GetData();

                for (int y = 0; y < Height; y++)
                {
                    uint* line = (uint*)pixData.Data + (y * pixData.WordsPerLine);
                    for (int x = 0; x < Width; x++)
                    {
                        uint val = (uint)((y * Width + x) % (1 << depth));
                        uint readVal;
                        if (depth == 1)
                        {
                            PixData.SetDataBit(line, x, val);
                            readVal = PixData.GetDataBit(line, x);
                        }
                        else if (depth == 2)
                        {
                            PixData.SetDataDIBit(line, x, val);
                            readVal = PixData.GetDataDIBit(line, x);
                        }
                        else if (depth == 4)
                        {
                            PixData.SetDataQBit(line, x, val);
                            readVal = PixData.GetDataQBit(line, x);
                        }
                        else if (depth == 8)
                        {
                            PixData.SetDataByte(line, x, val);
                            readVal = PixData.GetDataByte(line, x);
                        }
                        else if (depth == 16)
                        {
                            PixData.SetDataTwoByte(line, x, val);
                            readVal = PixData.GetDataTwoByte(line, x);
                        }
                        else if (depth == 32)
                        {
                            PixData.SetDataFourByte(line, x, val);
                            readVal = PixData.GetDataFourByte(line, x);
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }

                        //Assert.That(readVal, Is.EqualTo(val));
                    }
                }
            }
        }

        //=========================================================================================
        //=========================================================================================

        /// <summary>
        /// LeptonicaPerformanceTests.cs
        /// Leptonica Performance Tests
        /// </summary>
        public static void test_002_ConvertToBitmap(string sourceFilePath = "photo_palette_8bpp.tif")
        {
            const double BaseRunTime = 793.382;
            const int Runs = 1000;

            //var sourceFilePath = Path.Combine("./Data/Conversion", "photo_palette_8bpp.tif");
            using (var bmp = new Bitmap(sourceFilePath))
            {
                // Don't include the first conversion since it will also handle loading the library etc (upfront costs).
                using (var pix = PixConverter.ToPix(bmp))
                {
                }

                // copy 100 times take the average
                Stopwatch watch = new Stopwatch();
                watch.Start();
                for (int i = 0; i < Runs; i++)
                {
                    using (var pix = PixConverter.ToPix(bmp))
                    {
                    }
                }
                watch.Stop();

                var delta = watch.ElapsedTicks / (BaseRunTime * Runs);
                Console.WriteLine("Delta: {0}", delta);
                Console.WriteLine("Elapsed Ticks: {0}", watch.ElapsedTicks);
                Console.WriteLine("Elapsed Time: {0}ms", watch.ElapsedMilliseconds);
                Console.WriteLine("Average Time: {0}ms", (double)watch.ElapsedMilliseconds / Runs);

                //Assert.That(delta, Is.EqualTo(1.0).Within(0.25));
            }
        }

        //=========================================================================================
        //=========================================================================================

        // Test for [Issue #166](https://github.com/charlesw/tesseract/issues/166)
        //[Test]
        public static unsafe void test_003_Convert_ScaledBitmapToPix(string sourceFilePath = "photo_rgb_32bpp.tif")
        {
            //var sourceFilePath = TestFilePath("Conversion/photo_rgb_32bpp.tif");
            var bitmapConverter = new BitmapToPixConverter();
            using (var source = new Bitmap(sourceFilePath))
            {
                using (var scaledSource = new Bitmap(source, new Size(source.Width * 2, source.Height * 2)))
                {
                    //Assert.That(BitmapHelper.GetBPP(scaledSource), Is.EqualTo(32));
                    using (var dest = bitmapConverter.Convert(scaledSource))
                    {
                        //dest.Save(TestResultRunFile("Conversion/ScaledBitmapToPix_rgb_32bpp.tif"), ImageFormat.Tiff);
                        dest.Save("_/ScaledBitmapToPix_rgb_32bpp.tif", ImageFormat.Tiff);

                        AssertAreEquivalent(scaledSource, dest, true);
                    }
                }
            }
        }

        //[Test]
        //[TestCase(1)] // Note: 1bpp will not save pixmap when writing out the result, this is a limitation of leptonica (see pixWriteToTiffStream)
        //[TestCase(4, Ignore = "4bpp images not supported.")]
        //[TestCase(8)]
        //[TestCase(32)]
        public static unsafe void test_004_Convert_BitmapToPix(int depth)
        {
            string pixType;
            if (depth < 16) pixType = "palette";
            else if (depth == 16) pixType = "grayscale";
            else pixType = "rgb";

            //var sourceFile = String.Format("Conversion/photo_{0}_{1}bpp.tif", pixType, depth);
            var sourceFile = String.Format("photo_{0}_{1}bpp.tif", pixType, depth);
            var sourceFilePath = TestFilePath(sourceFile);
            var bitmapConverter = new BitmapToPixConverter();
            using (var source = new Bitmap(sourceFilePath))
            {
                //Assert.That(BitmapHelper.GetBPP(source), Is.EqualTo(depth));
                using (var dest = bitmapConverter.Convert(source))
                {
                    var destFilename = String.Format("_/BitmapToPix_{0}_{1}bpp.tif", pixType, depth);
                    //dest.Save(TestResultRunFile(destFilename), ImageFormat.Tiff);
                    dest.Save(destFilename, ImageFormat.Tiff);

                    AssertAreEquivalent(source, dest, true);
                }
            }
        }

        /// <summary>
        /// Test case for https://github.com/charlesw/tesseract/issues/180
        /// </summary>
        //[Test]
        public static unsafe void test_005_Convert_BitmapToPix_Format8bppIndexed(string sourceFile = "photo_palette_8bpp.png")
        {
            //var sourceFile = TestFilePath("Conversion/photo_palette_8bpp.png");
            var bitmapConverter = new BitmapToPixConverter();
            using (var source = new Bitmap(sourceFile))
            {
                //Assert.That(BitmapHelper.GetBPP(source), Is.EqualTo(8));
                //Assert.That(source.PixelFormat, Is.EqualTo(PixelFormat.Format8bppIndexed));
                using (var dest = bitmapConverter.Convert(source))
                {
                    //var destFilename = TestResultRunFile("Conversion/BitmapToPix_palette_8bpp.png"); 
                    var destFilename = "_/BitmapToPix_palette_8bpp.png";
                    dest.Save(destFilename, ImageFormat.Png);

                    AssertAreEquivalent(source, dest, true);
                }
            }
        }

        //[Test]
        //[TestCase(1, true, false)]
        //[TestCase(1, false, false)]
        //[TestCase(4, false, false, Ignore = "4bpp images not supported.")]
        //[TestCase(4, true, false, Ignore = "4bpp images not supported.")]
        //[TestCase(8, false, false)]
        //[TestCase(8, true, false, Ignore = "Haven't yet created a 8bpp grayscale test image.")]
        //[TestCase(32, false, true)]
        //[TestCase(32, false, false)]
        public static unsafe void test_006_Convert_PixToBitmap(int depth, bool isGrayscale, bool includeAlpha)
        {
            bool hasPalette = depth < 16 && !isGrayscale;
            string pixType;
            if (isGrayscale) pixType = "grayscale";
            else if (hasPalette) pixType = "palette";
            else pixType = "rgb";

            //var sourceFile = TestFilePath(String.Format("Conversion/photo_{0}_{1}bpp.tif", pixType, depth));
            var sourceFile = String.Format("photo_{0}_{1}bpp.tif", pixType, depth);
            var converter = new PixToBitmapConverter();
            using (var source = Pix.LoadFromFile(sourceFile))
            {
                //Assert.That(source.Depth, Is.EqualTo(depth));
                if (hasPalette)
                {
                    //Assert.That(source.Colormap, Is.Not.Null, "Expected source image to have color map\\palette.");
                }
                else
                {
                    //Assert.That(source.Colormap, Is.Null, "Expected source image to be grayscale.");
                }
                using (var dest = converter.Convert(source, includeAlpha))
                {
                    //var destFilename = TestResultRunFile(String.Format("Conversion/PixToBitmap_{0}_{1}bpp.tif", pixType, depth));
                    var destFilename = String.Format("_/PixToBitmap_{0}_{1}bpp.tif", pixType, depth);
                    dest.Save(destFilename, System.Drawing.Imaging.ImageFormat.Tiff);

                    AssertAreEquivalent(dest, source, includeAlpha);
                }
            }
        }

        static void AssertAreEquivalent(Bitmap bmp, Pix pix, bool checkAlpha)
        {
            ////// verify img metadata
            ////Assert.That(pix.Width, Is.EqualTo(bmp.Width));
            ////Assert.That(pix.Height, Is.EqualTo(bmp.Height));
            //////Assert.That(pix.Resolution.X, Is.EqualTo(bmp.HorizontalResolution));
            //////Assert.That(pix.Resolution.Y, Is.EqualTo(bmp.VerticalResolution));

            // do some random sampling over image
            var height = pix.Height;
            var width = pix.Width;
            for (int y = 0; y < height; y += height)
            {
                for (int x = 0; x < width; x += width)
                {
                    PixColor sourcePixel = (PixColor)bmp.GetPixel(x, y);
                    PixColor destPixel = GetPixel(pix, x, y);
                    if (checkAlpha)
                    {
                        //Assert.That(destPixel, Is.EqualTo(sourcePixel), "Expected pixel at <{0},{1}> to be same in both source and dest.", x, y);
                    }
                    else
                    {
                        //Assert.That(destPixel, Is.EqualTo(sourcePixel).Using<PixColor>((c1, c2) => (c1.Red == c2.Red && c1.Blue == c2.Blue && c1.Green == c2.Green) ? 0 : 1), "Expected pixel at <{0},{1}> to be same in both source and dest.", x, y);
                    }
                }
            }
        }

        static unsafe PixColor GetPixel(Pix pix, int x, int y)
        {
            var pixDepth = pix.Depth;
            var pixData = pix.GetData();
            var pixLine = (uint*)pixData.Data + pixData.WordsPerLine * y;
            uint pixValue;
            if (pixDepth == 1)
            {
                pixValue = PixData.GetDataBit(pixLine, x);
            }
            else if (pixDepth == 4)
            {
                pixValue = PixData.GetDataQBit(pixLine, x);
            }
            else if (pixDepth == 8)
            {
                pixValue = PixData.GetDataByte(pixLine, x);
            }
            else if (pixDepth == 32)
            {
                pixValue = PixData.GetDataFourByte(pixLine, x);
            }
            else
            {
                throw new ArgumentException(String.Format("Bit depth of {0} is not supported.", pix.Depth), "pix");
            }

            if (pix.Colormap != null)
            {
                return pix.Colormap[(int)pixValue];
            }
            else
            {
                if (pixDepth == 32)
                {
                    return PixColor.FromRgba(pixValue);
                }
                else
                {
                    byte grayscale = (byte)(pixValue * 255 / ((1 << 16) - 1));
                    return new PixColor(grayscale, grayscale, grayscale);
                }
            }

            //return new PixColor(0, 0, 0);
        }

        //=========================================================================================
        //=========================================================================================

        public static void test_009_CanCreatePixArray()
        {
            using (var pixA = PixArray.Create(0))
            {
                //Assert.That(pixA.Count, Is.EqualTo(0));
            }
        }

        public static void test_010_CanAddPixToPixArray(string sourcePixPath = "phototest.tif")
        {
            //var sourcePixPath = TestFilePath(@"Ocr\phototest.tif");
            using (var pixA = PixArray.Create(0))
            {
                using (var sourcePix = Pix.LoadFromFile(sourcePixPath))
                {
                    pixA.Add(sourcePix);
                    //Assert.That(pixA.Count, Is.EqualTo(1));
                    using (var targetPix = pixA.GetPix(0))
                    {
                        //Assert.That(targetPix, Is.EqualTo(sourcePix));
                    }
                }
            }
        }

        public static void test_011_CanRemovePixFromArray(string sourcePixPath = "phototest.tif")
        {
            //var sourcePixPath = TestFilePath(@"Ocr\phototest.tif");
            using (var pixA = PixArray.Create(0))
            {
                using (var sourcePix = Pix.LoadFromFile(sourcePixPath))
                {
                    pixA.Add(sourcePix);
                }

                pixA.Remove(0);
                //Assert.That(pixA.Count, Is.EqualTo(0));
            }
        }

        public static void test_012_CanClearPixArray(string sourcePixPath = "phototest.tif")
        {
            //var sourcePixPath = TestFilePath(@"Ocr\phototest.tif");
            using (var pixA = PixArray.Create(0))
            {
                using (var sourcePix = Pix.LoadFromFile(sourcePixPath))
                {
                    pixA.Add(sourcePix);
                }

                pixA.Clear();

                //Assert.That(pixA.Count, Is.EqualTo(0));
            }
        }
    }
}
