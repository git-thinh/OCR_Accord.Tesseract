using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  System.Drawing
{
    public class ___ImageFilters
    {
        static Dictionary<string, Func<object, Bitmap, Bitmap>> m_Scripts = new Dictionary<string, Func<object, Bitmap, Bitmap>>()
        {
            { "Blur", (config, Imagem) => {
            //Blur filter = new Blur();
            //Imagem = Imagem.Clone(new Rectangle(0, 0, Imagem.Width, Imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //Imagem = filter.Apply(Imagem);
            return Imagem;
            } },
            { "", (config, Imagem) => {
                return Imagem;
            } },
            { "", (config, Imagem) => {
                return Imagem;
            } },
            { "", (config, Imagem) => {
                return Imagem;
            } },
            { "", (config, Imagem) => {
                return Imagem;
            } },
            { "", (config, Imagem) => {
                return Imagem;
            } },
        };

        public static string[] getScriptNames() { return m_Scripts.Keys.ToArray(); }

        public static Bitmap Execute(string filterName, object config, Bitmap image)
        {
            Bitmap img = image;
            Func<object, Bitmap, Bitmap> fun;
            if (m_Scripts.ContainsKey(filterName))
            {
                fun = m_Scripts[filterName];
                img = fun(config, image);
            }
            return img;
        }

        static Bitmap CopyBitmap(Bitmap source)
        {
            return new Bitmap(source);
        }
    }
}
