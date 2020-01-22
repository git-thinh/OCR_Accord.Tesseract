using AForge.Imaging.Filters;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace System
{
    public static class ___IMAGE_FILTERS
    {
        static Dictionary<string, Func<object, Bitmap, Bitmap>> M_SCRIPTS = new Dictionary<string, Func<object, Bitmap, Bitmap>>()
        {
            #region [ Filters AForge ]
            
            { "01_AF_Grey_Image", (config, Imagem) => {
                Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap grImage = grayFilter.Apply(Imagem);
                //grImage.Save("./grey_image.png");
                return grImage;
            }},
            { "01_AF_Invert", (config, Imagem) => {
                Invert filter = new Invert();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},

            { "SaturationCorrection", (config, Imagem) => {
                SaturationCorrection filter = new SaturationCorrection();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "ContrastCorrection", (config, Imagem) => {
                ContrastCorrection filter = new ContrastCorrection();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "BlobsFiltering", (config, Imagem) => {
                BlobsFiltering filter = new BlobsFiltering();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "BilateralSmoothing", (config, Imagem) => {
                BilateralSmoothing filter = new BilateralSmoothing();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "BrightnessCorrection", (config, Imagem) => {
                BrightnessCorrection filter = new BrightnessCorrection();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Mean", (config, Imagem) => {
                Mean filter = new Mean();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Blur", (config, Imagem) => {
                Blur filter = new Blur();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Median", (config, Imagem) => {
                Median filter = new Median();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "ConvolutionTest", (config, Imagem) => {
                int[,] kernel = {
                    { 5,-1, 5 },
                    {-1,20,-1 },
                    { 5,-1, 5 }
                };
                int divisor = 4;
                Convolution filter = new Convolution(kernel, divisor);
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Convolution", (config, Imagem) => {
                int[,] kernel = {

                    { 2,-1, 2 },
                    {-1,10,-1 },
                    { 2,-1, 2 }
                };
                int[,] kernel2 =
                {
                    {0 ,0 ,0 ,0 ,0},
                    {0 ,2 ,0 ,2 ,0},
                    {0 ,-1, 2,-1,0},
                    {0 ,2 ,-1,2 ,0},
                    {0 ,0 ,-1,0 ,0}
                };
                int divisor = 4;
                Convolution filter = new Convolution(kernel, divisor);
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Sharpen", (config, Imagem) => {
                Sharpen filter = new Sharpen();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Opening", (config, Imagem) => {
                Opening filter = new Opening();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Shrink", (config, Imagem) => {
                Shrink filter = new Shrink();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Median2", (config, Imagem) => {
                Median filter = new Median(2);
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Dilatation", (config, Imagem) => {
                Dilatation filter = new Dilatation();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Erosion", (config, Imagem) => {
                Erosion filter = new Erosion();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "GaussianSharpen", (config, Imagem) => {
                GaussianSharpen filter = new GaussianSharpen();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},
            { "Closing", (config, Imagem) => {
                Closing filter = new Closing();
                Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Imagem = filter.Apply(Imagem);
                return Imagem;
            }},

            #endregion

            #region [ Filters ]
            { "01_NO_PegarAzul", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Color c = default(Color);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        c = Imagem.GetPixel(X, Y);
                        if (!(c.R == 0 && c.G == 0 && c.B == 0) && !(c.B > c.G + 35 || c.B > c.R + 35))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},

            { "TirarBordaTRT21", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X < Imagem.Width - 1 & Y <= 4) // cima
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < Imagem.Width - 1 & Y >= Imagem.Height - 7) // baixo
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < 7 & Y < Imagem.Height - 1) //esquerda
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X >= Imagem.Width - 7 & Y <= Imagem.Height - 1) //direita
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "TirarBorda", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X < Imagem.Width - 1 & Y <= 7) // cima
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < Imagem.Width - 1 & Y >= Imagem.Height - 7) // baixo
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < 7 & Y < Imagem.Height - 1) //esquerda
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X >= Imagem.Width - 1 & Y <= Imagem.Height - 1) //direita
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "TirarBordaTRF4", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X < Imagem.Width - 1 & Y <= 7) // cima
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < Imagem.Width - 1 & Y >= Imagem.Height - 5) // baixo
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < 7 & Y < Imagem.Height - 5) //esquerda
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X >= Imagem.Width - 10 & Y <= Imagem.Height - 5) //direita
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "TirarBordaBaixa", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X == 0 || Y == 0)
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X == Imagem.Width - 1 || Y == Imagem.Height - 1)
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},
            { "TirarBordaCima", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X < Imagem.Width - 1 & Y <= 12) // cima
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < Imagem.Width - 1 & Y >= Imagem.Height - 3) // baixo
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < 7 & Y < Imagem.Height - 5) //esquerda
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X >= Imagem.Width - 10 & Y <= Imagem.Height - 5) //direita
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "CorrigirTransparencia", (config, Imagem) => {
                string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                using (var b = new Bitmap(Imagem.Width, Imagem.Height))
                {
                    b.SetResolution(Imagem.HorizontalResolution, Imagem.VerticalResolution);

                    using (var g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        g.DrawImageUnscaled(Imagem, 0, 0);
                    }
                    if (fileName.Contains(".png"))
                        b.Save(fileName.Replace(".png", ".bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
                    else if (fileName.Contains(".jpg"))
                        b.Save(fileName.Replace(".jpg", ".bmp"), System.Drawing.Imaging.ImageFormat.Bmp);
                }

                return Imagem;
            }},
            { "FA_TirarPixelPretoSozinhoNaHorizontal", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c3 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X, Y - 1);
                        }
                        if ((((c1.R == 255 & c1.G == 255 & c1.B == 255) & (c2.R == 255 & c2.G == 255 & c2.B == 255) & (C.R == 0 & C.G == 0 & C.B == 0)) & ((c3.R == 255 & c3.G == 255 & c3.B == 255) | (c4.R == 255 & c4.G == 255 & c4.B == 255))))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},
            { "FA_TirarPixelPretoSozinhoNaVertical", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c2 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X, Y - 1);
                        }
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c3 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X - 1, Y);
                        }

                        if ((((c1.R == 255 & c1.G == 255 & c1.B == 255) & (c2.R == 255 & c2.G == 255 & c2.B == 255) & (C.R == 0 & C.G == 0 & C.B == 0)) & ((c3.R == 255 & c3.G == 255 & c3.B == 255) | (c4.R == 255 & c4.G == 255 & c4.B == 255))))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},
            { "PintarBrancoEntrePretos", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                {
                    for (int X = 0; X <= (Imagem.Width) - 1; X++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c2 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 > 0)
                        {
                            c1 = Imagem.GetPixel(X, Y - 1);
                        }
                        if (((c1.R == 0 & c1.G == 0 & c1.B == 0) & (c2.R == 0 & c2.G == 0 & c2.B == 0) & (C.R == 255 & C.G == 255 & C.B == 255)))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 0, 0, 0));
                        }
                    }
                }
                return Imagem;
            }},
            { "PintarPixelPretoDeBranco", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Color c = default(Color);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        c = Imagem.GetPixel(X, Y);
                        if ((c.R + c.G + c.B > 210))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                        else
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 0, 0, 0));
                        }
                    }
                }

                return Imagem;
            }},
            { "PintarPixelPretoDeBranco2", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Color c = default(Color);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        c = Imagem.GetPixel(X, Y);
                        if (c.R >= 80 || c.G >= 80 || c.B >= 80)
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                        else
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 0, 0, 0));
                        }
                    }
                }

                return Imagem;
            }},
            { "engrossarLinhaPreto", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                //engrossa as linhas pretas
                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X - 1 > 0)
                        {
                            if ((C.R == 0 | C.G == 0 | C.B == 0))
                            {
                                Color pixelanterior = Imagem.GetPixel(X - 1, Y);
                                if (pixelanterior.R == 255 & pixelanterior.G == 255 & pixelanterior.B == 255)
                                {
                                    Imagem.SetPixel(X - 1, Y, Color.FromArgb(C.A, 0, 0, 0));
                                }
                            }
                        }
                    }
                }
                return Imagem;
            }},
            { "TirarPixelSozinho", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c3 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X, Y - 1);
                        }

                        if (((c1.R == 255 & c1.G == 255 & c1.B == 255) && (c2.R == 255 & c2.G == 255 & c2.B == 255) && (c3.R == 255 & c3.G == 255 & c3.B == 255) && (c4.R == 255 & c4.G == 255 & c4.B == 255) && (C.R == 0 & C.G == 0 & C.B == 0)))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "RetirarTodasAsCoresEColocarNaImagemFinal", (config, Imagem) => {

                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Bitmap ImagemColorida = new Bitmap(Imagem.Width, Imagem.Height);
                Bitmap imagemPreta = new Bitmap(Imagem.Width, Imagem.Height);
                Bitmap imagemFinal = new Bitmap(Imagem.Width, Imagem.Height);

                Color c = default(Color);

                //*** BEGIN MULTICOLOR ***
                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        c = Imagem.GetPixel(X, Y);
                        if (((c.R == 0 & c.G == 0 & c.B == 0) | (c.R == 255 & c.G == 255 & c.B == 255)))
                        {
                            ImagemColorida.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                        else
                        {
                            ImagemColorida.SetPixel(X, Y, Color.FromArgb(c.A, 0, 0, 0));
                        }
                    }
                }

                //*** END MULTICOLOR ***

                //*** BEGIN BLACK ***
                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        c = Imagem.GetPixel(X, Y);
                        if ((c.R < 10 & c.G < 10 & c.B < 10))
                        {
                            imagemPreta.SetPixel(X, Y, Color.FromArgb(c.A, 0, 0, 0));
                        }
                        else
                        {
                            imagemPreta.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                    }
                }
                //*** END BLACK ***

                //*** BEGIN FINAL IMAGE ***
                Color colorido = default(Color);

                Color preto = default(Color);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        colorido = ImagemColorida.GetPixel(X, Y);
                        preto = imagemPreta.GetPixel(X, Y);
                        if (colorido.R == 0 | preto.R == 0)
                        {
                            imagemFinal.SetPixel(X, Y, Color.FromArgb(c.A, 0, 0, 0));
                        }
                        else
                        {
                            imagemFinal.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                    }
                }

                //*** END FINAL IMAGE ***
                return imagemFinal;
            }},
            { "TirarPixelPretoSozinhoHorizontal", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c3 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X, Y - 1);
                        }
                        if (((c1.R == 255 & c1.G == 255 & c1.B == 255) & (c2.R == 255 & c2.G == 255 & c2.B == 255) & (C.R == 0 & C.G == 0 & C.B == 0)))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},
            { "TirarBordaRS", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        if (X < Imagem.Width - 1 & Y <= 3) // cima
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < Imagem.Width - 1 & Y >= Imagem.Height - 3) // baixo
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X < 10 & Y < Imagem.Height - 0) //esquerda
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                        if (X >= Imagem.Width - 10 & Y <= Imagem.Height - 0) //direita
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "PintarPixelPretoDeBrancoTRT17", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Color c = default(Color);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        c = Imagem.GetPixel(X, Y);
                        if ((c.R + c.G + c.B > 150))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 255, 255, 255));
                        }
                        else
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(c.A, 0, 0, 0));
                        }
                    }
                }

                return Imagem;
            }},
            { "Limpar2PX", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int Y = 10; Y <= (Imagem.Height) - 5; Y++)
                {
                    for (int X = 10; X <= (Imagem.Width) - 5; X++)
                    {
                        Color C = Imagem.GetPixel(X, Y); //preto
                        Color C2 = Imagem.GetPixel(X, Y - 1); //preto
                        Color c2 = Imagem.GetPixel(X, Y - 2); //branco
                        Color c1 = Imagem.GetPixel(X, Y + 1); //branco
                        if ((C.R == 0 || C.G == 0 || C.B == 0) && (C2.R == 0 || C2.G == 0 || C2.B == 0) && (c1.R == 255 || c1.G == 255 || c1.B == 255) && (c2.R == 255 || c2.G == 255 || c2.B == 255))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                            Imagem.SetPixel(X, Y - 1, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
                for (int Y = 10; Y <= (Imagem.Height) - 10; Y++)
                {
                    for (int X = 10; X <= (Imagem.Width) - 10; X++)
                    {
                        Color C = Imagem.GetPixel(X, Y); //preto
                        Color C2 = Imagem.GetPixel(X - 1, Y); //preto
                        Color c2 = Imagem.GetPixel(X - 2, Y); //branco
                        Color c1 = Imagem.GetPixel(X + 1, Y); //branco
                        if ((C.R == 0 || C.G == 0 || C.B == 0) && (C2.R == 0 || C2.G == 0 || C2.B == 0) && (c1.R == 255 || c1.G == 255 || c1.B == 255) && (c2.R == 255 || c2.G == 255 || c2.B == 255))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                            Imagem.SetPixel(X, Y - 1, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "TirarPixelPretoSozinhoVertical", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c2 = default(Color);
                        Color c1 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c1 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (Y + 1 <= Imagem.Height - 1)
                        {
                            c3 = Imagem.GetPixel(X, Y + 1);
                        }
                        if (Y - 1 >= 0)
                        {
                            c4 = Imagem.GetPixel(X, Y - 1);
                        }

                        if (((c3.R == 255 & c3.G == 255 & c3.B == 255) & (c4.R == 255 & c4.G == 255 & c4.B == 255) & (C.R == 0 & C.G == 0 & C.B == 0)))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }

                return Imagem;
            }},
            { "TirarPixelPretoEntreBranco2Horizontal", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c1 = default(Color);
                        Color c2 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c1 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X + 2 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 2, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c3 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (X - 2 >= 0)
                        {
                            c4 = Imagem.GetPixel(X - 2, Y);
                        }
                        if ((C.R == 0 & C.G == 0 & C.B == 0) & (c1.R == 0 & c1.G == 0 & c1.B == 0) & (c2.R == 255 & c2.G == 255 & c2.B == 255) & (c3.R == 255 & c3.G == 255 & c3.B == 255))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                            Imagem.SetPixel(X + 1, Y, Color.FromArgb(c1.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},
            { "TirarPixelPretoEntreBranco3Horizontal", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 1; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 1; Y++)
                    {
                        Color C = Imagem.GetPixel(X, Y);
                        Color c1 = default(Color);
                        Color c2 = default(Color);
                        Color c3 = default(Color);
                        Color c4 = default(Color);
                        if (X + 1 <= Imagem.Width - 1)
                        {
                            c1 = Imagem.GetPixel(X + 1, Y);
                        }
                        if (X + 2 <= Imagem.Width - 1)
                        {
                            c2 = Imagem.GetPixel(X + 2, Y);
                        }
                        if (X - 1 >= 0)
                        {
                            c3 = Imagem.GetPixel(X - 1, Y);
                        }
                        if (X - 2 >= 0)
                        {
                            c4 = Imagem.GetPixel(X - 2, Y);
                        }
                        if ((C.R == 0 & C.G == 0 & C.B == 0) & (c1.R == 0 & c1.G == 0 & c1.B == 0) & (c3.R == 0 & c3.G == 0 & c3.B == 0) & (c4.R == 255 & c4.G == 255 & c4.B == 255) & (c2.R == 255 & c2.G == 255 & c2.B == 255))
                        {
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                            Imagem.SetPixel(X + 1, Y, Color.FromArgb(c1.A, 255, 255, 255));
                            Imagem.SetPixel(X + 2, Y, Color.FromArgb(c2.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},
            { "RetirarBolinha", (config, Imagem) => {
                Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                for (int X = 0; X <= (Imagem.Width) - 2; X++)
                {
                    for (int Y = 0; Y <= (Imagem.Height) - 2; Y++)
                    {
                        if (X >= 10 && X <= 16 && Y >= 8 && Y <= 13)
                        {
                            Color C = Imagem.GetPixel(X, Y);
                            Imagem.SetPixel(X, Y, Color.FromArgb(C.A, 255, 255, 255));
                        }
                    }
                }
                return Imagem;
            }},

            #endregion

            #region [ TRT21, TRF4 ]

            { "TRT21", (config, Imagem) => {
                Bitmap img = Imagem.Get("TirarBordaTRT21", config);
                img = img.Get("PintarPixelPretoDeBranco", config);
                img = img.Get("Opening", config);
                return img;
            }},
            { "TRF4", (config, Imagem) => {
                Bitmap img = Imagem;
                for (int j = 0; j < 4; j++)
                {
                    img = img.Get("BrightnessCorrection", config);
                    for (int i = 0; i < 5; i++)
                        img = img.Get("ContrastCorrection", config);
                }

                img = img.Get("ContrastCorrection", config);
                img = img.Get("TirarBordaTRF4", config);
                img = img.Get("RetirarBolinha", config);

                return img;
            }},

            #endregion
        };

        #region [ Methods ]

        public static string[] getScriptNames() { return M_SCRIPTS.Keys.ToArray(); }

        public static Bitmap Execute(string filterName, object config, Bitmap image)
        {
            Bitmap img = image;
            Func<object, Bitmap, Bitmap> fun;
            if (M_SCRIPTS.ContainsKey(filterName))
            {
                fun = M_SCRIPTS[filterName];
                img = fun(config, image);
            }
            return img;
        }

        public static Bitmap CopyBitmap(this Bitmap source)
        {
            return new Bitmap(source);
        }

        public static Bitmap CloneBitmap(this Bitmap source)
        {
            return new Bitmap(source);
        }

        static Bitmap Get(this Bitmap Imagem, string name, object config)
        {
            if (M_SCRIPTS.ContainsKey(name))
            {
                var img = M_SCRIPTS[name](config, Imagem);
                return img;
            }
            return Imagem;
        }

        public static bool Exist(this string name) => M_SCRIPTS.ContainsKey(name);

        #endregion
    }
}
