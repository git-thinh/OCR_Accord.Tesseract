using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AForge.Imaging.Filters;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace System
{
    public static class ___IMAGE_DIRECTION
    {
        static HaarCascade cascade = new FaceHaarCascade(); 
         
        static Dictionary<string, Func<object, Bitmap, Bitmap>> M_SCRIPTS = new Dictionary<string, Func<object, Bitmap, Bitmap>>()
        {
            #region [ Filters AForge ]

            { "ACCORD_DIRECTION_BY_FACE_POSSITION", (config, Imagem) => {
                int minSize = 30;
                var detector = new HaarObjectDetector(cascade, minSize);

                detector.SearchMode = ObjectDetectorSearchMode.NoOverlap;
                detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater;
                detector.ScalingFactor = 1.5f;
                detector.UseParallelProcessing = true;
                detector.Suppression = 2;

                // Process frame to detect objects
                Rectangle[] objects = detector.ProcessFrame(Imagem);
                

                //SaturationCorrection filter = new SaturationCorrection();
                //Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                //Imagem = filter.Apply(Imagem);
                return Imagem;
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

        static bool Exist(this string name) => M_SCRIPTS.ContainsKey(name);

        #endregion
    }
}
