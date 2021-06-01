using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace AForgeDetectObjectImage
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "jpg",
                Filter = "JPG files (*.jpg)|*.jpg",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (d.ShowDialog() == DialogResult.OK)
            {
                string file = d.FileName;
                var bitmap = (Bitmap)Bitmap.FromFile(file);
                pictureBox1.Image = bitmap;

                //var bmp = openFile1(file);
                //var bmp = openFile2(file);
                var bmp = openFile3(file);
                __execute(file, bmp);
                //LockUnlockBitsExample(file);

                //pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                //pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
                //pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;

            }
        }

        void __execute(string file, Bitmap bmp)
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;
            blobCounter.MaxHeight = 5000;
            blobCounter.MaxWidth = 5000;
            blobCounter.ProcessImage(bmp);

            blobCounter.ProcessImage(bmp);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            var ls = blobs.Select(x => new XYX1Y1(x.Rectangle)).ToArray();
            var rec = XYX1Y1.Merge(ls);

            var result = System.Drawing.Image.FromFile(file);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawRectangle(new Pen(Color.Red, 1.0f), rec);
            }
            pictureBox3.Image = result;
        }

        Bitmap openFile1(string file)
        {
            var image = (Bitmap)Bitmap.FromFile(file);

            //Those are AForge filters "using Aforge.Imaging.Filters;"
            Grayscale gfilter = new Grayscale(0.2125, 0.7154, 0.0721);
            Invert ifilter = new Invert();
            BradleyLocalThresholding thfilter = new BradleyLocalThresholding();
            var bmp = gfilter.Apply(image);
            thfilter.ApplyInPlace(bmp);
            ifilter.ApplyInPlace(bmp);

            pictureBox2.Image = bmp;

            return bmp;


            //pictureBox1.Height = result.Height;
            //pictureBox2.Height = result.Height;
            //pictureBox3.Height = result.Height;

            ;


            //// create filter
            //EuclideanColorFiltering filter = new EuclideanColorFiltering();
            //// set center colol and radius
            //filter.CenterColor = new AForge.Imaging.RGB(Color.FromArgb(215, 30, 30));
            //filter.Radius = 100;
            //// apply the filter
            //filter.ApplyInPlace(image);
            //pictureBox2.Image = image;


            //BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            //// grayscaling
            //UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            //// unlock image
            //image.UnlockBits(objectsData);



            //blobCounter.MinWidth = 5;
            //blobCounter.MinHeight = 5;
            //blobCounter.FilterBlobs = true;
            //blobCounter.ProcessImage(grayImage);
            //Rectangle[] rects = blobCounter.GetObjectRectangles();
            //foreach (Rectangle recs in rects)
            //    if (rects.Length > 0)
            //    {
            //        foreach (Rectangle objectRect in rects)
            //        {

            //            Graphics g = Graphics.FromImage(image);

            //            using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
            //            {
            //                g.DrawRectangle(pen, objectRect);
            //            }

            //            g.Dispose();
            //        }

            //    }

        }

        Bitmap openFile3(string file)
        {
            var image = (Bitmap)Bitmap.FromFile(file);

            //Those are AForge filters "using Aforge.Imaging.Filters;"
            //Grayscale gfilter = new Grayscale(0.2125, 0.7154, 0.0721);
            Grayscale gfilter = new Grayscale(0.9125, 0.7154, 0.0721);
            Invert ifilter = new Invert();
            BradleyLocalThresholding thfilter = new BradleyLocalThresholding();
            var bmp = gfilter.Apply(image);
            thfilter.ApplyInPlace(bmp);
            ifilter.ApplyInPlace(bmp);

            pictureBox2.Image = bmp;

            return bmp;
        }

        private void openFile2(string file)
        {
            var img = (Bitmap)Bitmap.FromFile(file);
            Bitmap imagem = new Bitmap(file);
            imagem = imagem.Clone(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Erosion erosion = new Erosion();
            Dilatation dilatation = new Dilatation();
            Invert inverter = new Invert();
            ColorFiltering cor = new ColorFiltering();
            cor.Blue = new AForge.IntRange(200, 255);
            cor.Red = new AForge.IntRange(200, 255);
            cor.Green = new AForge.IntRange(200, 255);
            Opening open = new Opening();
            BlobsFiltering bc = new BlobsFiltering();
            Closing close = new Closing();
            GaussianSharpen gs = new GaussianSharpen();
            ContrastCorrection cc = new ContrastCorrection();
            bc.MinHeight = 10;
            FiltersSequence seq = new FiltersSequence(gs, inverter, open, inverter, bc, inverter, open, cc, cor, bc, inverter);
            pictureBox3.Image = seq.Apply(imagem);
        }

        private void LockUnlockBitsExample(string file)
        {
            // Create a new bitmap.
            Bitmap bmp = new Bitmap(file);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            //pictureBox4.Image = bmp;


            // Draw the modified image.
            //e.Graphics.DrawImage(bmp, 0, 150);
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {

        }

        private void fMain_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
        }
    }
}
