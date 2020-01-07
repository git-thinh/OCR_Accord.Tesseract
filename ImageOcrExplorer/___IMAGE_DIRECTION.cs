using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using AForge.Imaging.Filters;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace System
{
    public class IMAGE_DIRECTION
    {
        public bool Ok { get; set; }
        public Bitmap Image { get; set; }
        public Rectangle[] Regions { get; set; }
        public IMAGE_DIRECTION(Bitmap bitmap) {
            Image = bitmap;
            Ok = false;
            Regions = new Rectangle[] { };
        }
    }

    public static class ___IMAGE_DIRECTION
    {
        static HaarCascade cascade = new FaceHaarCascade();

        static Dictionary<string, Func<object, Bitmap, IMAGE_DIRECTION>> M_SCRIPTS = new Dictionary<string, Func<object, Bitmap, IMAGE_DIRECTION>>()
        {
            #region [ Filters AForge ]

            { "ACCORD_DIRECTION_BY_FACE_POSSITION", (config, Imagem) => {
                IMAGE_DIRECTION result = new IMAGE_DIRECTION(Imagem);

                int minSize = 30;
                var detector = new HaarObjectDetector(cascade, minSize);

                detector.SearchMode = ObjectDetectorSearchMode.NoOverlap;
                detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater;
                detector.ScalingFactor = 1.5f;
                detector.UseParallelProcessing = true;
                detector.Suppression = 2;

                // Process frame to detect objects
                result.Regions = detector.ProcessFrame(Imagem);

                if (result.Regions.Length > 0)
                {
                    int max = 0, index = 0;
                    for(int i = 0; i < result.Regions.Length; i++){
                     if(max < result.Regions[i].Width * result.Regions[i].Height) {
                            max = result.Regions[i].Width * result.Regions[i].Height;
                            index = i;
                        }
                    }

                    RectanglesMarker marker = new RectanglesMarker(new Rectangle[]{ result.Regions[index] }, Color.Red);
                    result.Ok = true;
                    result.Image = marker.Apply(Imagem);
                }

                return result;
            }},

            { "ACCORD_DIRECTION_BY_FACE_POSSITION_ALL", (config, Imagem) => {
                IMAGE_DIRECTION result = new IMAGE_DIRECTION(Imagem);

                int minSize = 30;
                var detector = new HaarObjectDetector(cascade, minSize);

                detector.SearchMode = ObjectDetectorSearchMode.NoOverlap;
                detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater;
                detector.ScalingFactor = 1.5f;
                detector.UseParallelProcessing = true;
                detector.Suppression = 2;

                // Process frame to detect objects
                result.Regions = detector.ProcessFrame(Imagem);

                if (result.Regions.Length > 0)
                { 
                    RectanglesMarker marker = new RectanglesMarker( result.Regions, Color.Red);
                    result.Ok = true;
                    result.Image = marker.Apply(Imagem);
                }

                return result;
            }},

            #endregion
        };

        #region [ Methods ]

        public static string[] getScriptNames() { return M_SCRIPTS.Keys.ToArray(); }

        public static IMAGE_DIRECTION Execute(string filterName, object config, Bitmap image)
        {
            IMAGE_DIRECTION result = new IMAGE_DIRECTION(image);
             
            Func<object, Bitmap, IMAGE_DIRECTION> fun;
            if (M_SCRIPTS.ContainsKey(filterName))
            {
                fun = M_SCRIPTS[filterName];
                result = fun(config, image);
            }
            return result;
        }

        public static Bitmap CopyBitmap(this Bitmap source)
        {
            return new Bitmap(source);
        }

        public static Bitmap CloneBitmap(this Bitmap source)
        {
            return new Bitmap(source);
        }

        static IMAGE_DIRECTION Get(this Bitmap Imagem, string name, object config)
        {
            IMAGE_DIRECTION result = new IMAGE_DIRECTION(Imagem);
            if (M_SCRIPTS.ContainsKey(name))
                result = M_SCRIPTS[name](config, Imagem);
            return result;
        }

        public static bool Exist(this string name) => M_SCRIPTS.ContainsKey(name);

        #endregion
    }
}
