using IPoVn.IPCore;
using OCR.TesseractWrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace IPoVn.OCRer
{
    public enum OCR_ENGINE_MODE
    {
        OEM_TESSERACT_ONLY = 0,
        OEM_CUBE_ONLY = 1,
        OEM_TESSERACT_CUBE_COMBINED = 2,
        OEM_DEFAULT = 3
    }

    class Program
    {
        public const string _root = @"D:\Ocr\data-test\";
        public const string _result = @"D:\Ocr\data-test\_\";





        //const string TessdataFolder = @"C:\IPoVn\Test\tessdata\";
        //const string TessdataFolder = @"tessdata\";
        const string TessdataFolder = @".\tessdata\";

        static string[] Images = new string[] {
            @"phototest.tif",
            @"eurotext.tif",
            @"sample3.png",
            @"sample4.jpg"
        };

        const string InputFolder = "";
        const string OutputFolder = _result;

        static void Simple_Recognize()
        {
            string imageFile = _root + @"text\" + Images[0];

            TesseractProcessor processor = new TesseractProcessor();

            using (var bmp = Bitmap.FromFile(imageFile) as Bitmap)
            {
                var success = processor.Init(TessdataFolder, "eng", (int)eOcrEngineMode.OEM_DEFAULT);
                if (!success)
                {
                    Console.WriteLine("Failed to initialize tesseract.");
                }
                else
                {
                    string text = processor.Recognize(bmp);
                    Console.WriteLine("Text:");
                    Console.WriteLine("*****************************");
                    Console.WriteLine(text);
                    Console.WriteLine("*****************************");
                }
            }

        }

        static void Simple1_Recognize()
        {
            using (TesseractProcessor processor = new TesseractProcessor())
            {
                using (Bitmap bmp = Bitmap.FromFile("phototest.tif") as Bitmap)
                {
                    DateTime started = DateTime.Now;
                    DateTime ended = DateTime.Now;

                    int oem = 0;
                    for (int i = 0; i < 4; i++)
                    //for (int i = 3; i < 4; i++)
                    {
                        oem = i;
                        bool ok = processor.Init(TessdataFolder, "eng", i);
                        if (ok)
                        {
                            string text = "";
                            unsafe
                            {
                                started = DateTime.Now;

                                text = processor.Recognize(bmp);

                                ended = DateTime.Now;

                                Console.WriteLine("Duration recognition: {0} ms\n\n", (ended - started).TotalMilliseconds);
                            }

                            Console.WriteLine(
                                string.Format("RecognizeMode: {1}\nRecognized Text:\n{0}\n++++++++++++++++++++++++++++++++\n", text, ((eOcrEngineMode)oem).ToString()));
                        }
                        else
                        {
                            Console.WriteLine("FAIL " + i.ToString());
                        }
                    }
                }
            }
        }
        static void Simple1_AnalyseLayout()
        {
            string fileName = "";
            fileName = _root + @"text\phototest.tif";
            fileName = @"C:\temp\1.jpg";
            fileName = @"C:\temp\2.jpg";
            fileName = @"C:\temp\3.jpg";
            fileName = @"C:\temp\4.jpg";
            //fileName = @"C:\temp\5.jpg";

            Console.WriteLine("Image: {0}", fileName);

            string imageFile = Path.Combine("", fileName);

            string name = Path.GetFileNameWithoutExtension(imageFile);

            string outFile = Path.Combine(OutputFolder, string.Format("Simple1_{0}_layout.bmp", name));
            string outFile2 = Path.Combine(OutputFolder, string.Format("Simple1_{0}_grey.bmp", name));
            string outFile3 = Path.Combine(OutputFolder, string.Format("Simple1_{0}_bin.bmp", name));

            using (TesseractProcessor processor = new TesseractProcessor())
            {
                processor.InitForAnalysePage();
                //processor.SetPageSegMode(ePageSegMode.PSM_AUTO_ONLY);

                using (Bitmap bmp = Bitmap.FromFile(imageFile) as Bitmap)
                {
                    DateTime started = DateTime.Now;
                    DateTime ended = DateTime.Now;

                    DocumentLayout doc = null;

                    unsafe
                    {
                        started = DateTime.Now;

                        doc = processor.AnalyseLayout(bmp);

                        ended = DateTime.Now;

                        Console.WriteLine("Duration AnalyseLayout: {0} ms", (ended - started).TotalMilliseconds);
                    }
                    Console.WriteLine(outFile);

                    // prevents one-byte index format
                    //using (Image tmp = new Bitmap(bmp.Width, bmp.Height))
                    using (Image tmp = new Bitmap(imageFile))
                    {
                        using (Graphics grph = Graphics.FromImage(tmp))
                        {
                            //Rectangle rect = new Rectangle(0, 0, tmp.Width, tmp.Height);

                            // grph.DrawImage(bmp, rect, rect, GraphicsUnit.Pixel);

                            grph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            foreach (Block block in doc.Blocks)
                            {
                                Render.DrawBlock(grph, block);

                                //if (block.Paragraphs != null && block.Paragraphs.Count > 0)
                                //{
                                //    for (int i = 0; i < block.Paragraphs.Count; i++)
                                //    {
                                //        var lines = block.Paragraphs[i].Lines;
                                //        if (lines != null && lines.Count > 0)
                                //        {
                                //            for (int j = 0; j < lines.Count; j++)
                                //            {

                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }

                        tmp.Save(outFile);
                    }
                }
            }
        }


        static void Simple2_Recognize()
        {
            int n_images = Images.Length;
            int i_image = n_images - 1;
            //i_image = 0;
            string fileName = Images[i_image];

            string imageFile = Path.Combine(InputFolder, fileName);

            string language = "eng";
            int oem = (int)eOcrEngineMode.OEM_DEFAULT;

            using (TesseractProcessor processor = new TesseractProcessor())
            {
                using (Bitmap bmp = Bitmap.FromFile(imageFile) as Bitmap)
                {
                    using (GreyImage greyImage = GreyImage.FromImage(bmp))
                    {

                        ImageThresholder thresholder = new AdaptiveThresholder();
                        using (BinaryImage binImage = thresholder.Threshold(greyImage))
                        {
                            DateTime started = DateTime.Now;
                            DateTime ended = DateTime.Now;

                            int i = 3;
                            //for (i = 0; i < 4; i++)
                            //for (i = 3; i < 4; i++)
                            {
                                oem = i;
                                processor.Init(TessdataFolder, language, oem);

                                string text = "";
                                unsafe
                                {
                                    started = DateTime.Now;

                                    text = processor.RecognizeBinaryImage(
                                        binImage.BinaryData, greyImage.Width, greyImage.Height);

                                    ended = DateTime.Now;

                                    Console.WriteLine("Duration recognition: {0} ms\n\n", (ended - started).TotalMilliseconds);
                                }

                                Console.WriteLine(
                                    string.Format("RecognizeMode: {1}\nRecognized Text:\n{0}\n++++++++++++++++++++++++++++++++\n", text, ((eOcrEngineMode)oem).ToString()));
                            }
                        }
                    }
                }
            }
        }

        static void Simple2_AnalyseLayout()
        {
            string fileName = "";
            fileName = _root + @"text\phototest.tif";
            //fileName = @"C:\temp\1.jpg";
            //fileName = @"C:\temp\2.jpg";
            fileName = @"C:\temp\5.jpg";

            Console.WriteLine("Image: {0}", fileName);

            string imageFile = Path.Combine(InputFolder, fileName);

            string name = Path.GetFileNameWithoutExtension(imageFile);

            string outFile = Path.Combine(OutputFolder, string.Format("Simple2_{0}_layout.bmp", name));
            string outFile2 = Path.Combine(OutputFolder, string.Format("Simple2_{0}_grey.bmp", name));
            string outFile3 = Path.Combine(OutputFolder, string.Format("Simple2_{0}_bin.bmp", name));

            using (TesseractProcessor processor = new TesseractProcessor())
            {
                processor.InitForAnalysePage();
                //processor.SetPageSegMode(ePageSegMode.PSM_AUTO);

                using (Bitmap bmp = Bitmap.FromFile(imageFile) as Bitmap)
                {
                    using (GreyImage greyImage = GreyImage.FromImage(bmp))
                    {
                        greyImage.Save(ImageFormat.Bmp, outFile2);

                        ImageThresholder thresholder = new AdaptiveThresholder();
                        using (BinaryImage binImage = thresholder.Threshold(greyImage))
                        {
                            binImage.Save(ImageFormat.Bmp, outFile3);

                            DateTime started = DateTime.Now;
                            DateTime ended = DateTime.Now;

                            DocumentLayout doc = null;

                            unsafe
                            {
                                started = DateTime.Now;

                                doc = processor.AnalyseLayoutBinaryImage(
                                    binImage.BinaryData, greyImage.Width, greyImage.Height);

                                ended = DateTime.Now;

                                Console.WriteLine("Duration AnalyseLayout: {0} ms", (ended - started).TotalMilliseconds);
                            }
                            Console.WriteLine(doc.ToString());

                            using (Image tmp = new Bitmap(bmp.Width, bmp.Height)) // prevents one-byte index format
                            {
                                using (Graphics grph = Graphics.FromImage(tmp))
                                {
                                    //Rectangle rect = new Rectangle(0, 0, tmp.Width, tmp.Height);
                                    //grph.DrawImage(bmp, rect, rect, GraphicsUnit.Pixel);

                                    grph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                                    foreach (Block block in doc.Blocks)
                                    {
                                        Render.DrawBlock(grph, block);
                                    }
                                }

                                tmp.Save(outFile);
                            }
                        }
                    }
                }
            }
        }

        static void Simple3_Recognize()
        {
            int n_images = Images.Length;
            int i_image = n_images - 1;
            //i_image = 0;
            i_image = 2;
            string fileName = Images[i_image];

            string imageFile = Path.Combine(InputFolder, fileName);

            string language = "eng";
            int oem = (int)eOcrEngineMode.OEM_DEFAULT;

            string name = Path.GetFileNameWithoutExtension(imageFile);
            {
                using (Bitmap bmp = Bitmap.FromFile(imageFile) as Bitmap)
                {
                    using (GreyImage greyImage = GreyImage.FromImage(bmp))
                    {

                        ImageThresholder thresholder = new AdaptiveThresholder();
                        using (BinaryImage binImage = thresholder.Threshold(greyImage))
                        {
                            DateTime started = DateTime.Now;
                            DateTime ended = DateTime.Now;

                            Rectangle[] rois = new Rectangle[] {
                                Rectangle.FromLTRB(807, 43, 1351, 613),
                                Rectangle.FromLTRB(4, 604, binImage.Width - 15, binImage.Height-35)
                            };

                            int nROIs = rois.Length;

                            string[] texts = new string[nROIs];
#if PARALLEL
                            Parallel.For(0, nROIs, delegate(int iROI) 
#else
                            using (TesseractProcessor processor = new TesseractProcessor())
                                for (int iROI = 0; iROI < nROIs; iROI++)
#endif
                                {
#if PARALLEL
                                using (TesseractProcessor processor = new TesseractProcessor())
#endif
                                    {
                                        Rectangle roi = rois[iROI];
                                        {
                                            //oem = (int)eOcrEngineMode.OEM_TESSERACT_CUBE_COMBINED;
                                            processor.Init(TessdataFolder, language, oem);
                                            processor.UseROI = true;
                                            processor.ROI = roi;
                                            unsafe
                                            {
                                                texts[iROI] = processor.RecognizeBinaryImage(
                                                   binImage.BinaryData, binImage.Width, binImage.Height);
                                            }
                                        }
                                    }
                                }
#if PARALLEL
                            );
#endif

                            ended = DateTime.Now;

                            Console.WriteLine("Duration recognition: {0} ms\n\n", (ended - started).TotalMilliseconds);

                            Console.WriteLine("Recognized Text:");
                            for (int i = 0; i < nROIs; i++)
                            {
                                Console.WriteLine(texts[i]);
                            }

                            string txtFile = Path.Combine(
                                OutputFolder, string.Format("Simple3_{0}.txt", name));
                            using (StreamWriter writer = new StreamWriter(txtFile))
                            {
                                for (int i = 0; i < nROIs; i++)
                                {
                                    writer.WriteLine(texts[i]);
                                    writer.WriteLine("\n\n");
                                }
                            }
                            Process.Start(txtFile);
                        }
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            //Simple_Recognize();

            //Simple1_Recognize();
            Simple1_AnalyseLayout();
            //Simple2_AnalyseLayout();

            //Simple2_Recognize();

            //Simple3_Recognize();

            //Console.Write("\n\n\nPress any key to exit...");
            //Console.ReadKey();
        }
    }
}
