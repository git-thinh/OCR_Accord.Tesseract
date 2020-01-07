using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Tesseract;

namespace ImageOcrExplorer
{
    public static class ___IMAGE_OCR
    {
        //public static string DIR_TESSDATA = @"C:\IPoVn\Test\tessdata\";
        //public static string DIR_TESSDATA = @"tessdata\";
        public static string DIR_TESSDATA = $@"{Environment.CurrentDirectory}\tessdata\";
        //public static string DIR_TESSDATA = @".\tessdata\";

        static Dictionary<string, Func<object, Bitmap, String>> M_SCRIPTS = new Dictionary<string, Func<object, Bitmap, String>>()
        {
            #region [ CAPTCHA ]

            { "captcha_001", (config, bitmap) => {

                try
                {
                    string res = string.Empty;

                    using (var engine = new TesseractEngine(DIR_TESSDATA, "eng"))
                    {
                        string letters = "abcdefghijklmnopqrstuvwxyz";
                        string numbers = "0123456789";

                        engine.SetVariable("tessedit_char_whitelist", $"{numbers}{letters}{letters.ToUpper()}");
                        engine.SetVariable("tessedit_unrej_any_wd", true);
                        engine.SetVariable("tessedit_adapt_to_char_fragments", true);
                        engine.SetVariable("tessedit_redo_xheight", true);
                        engine.SetVariable("chop_enable", true);

                        Bitmap x = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                        using (var page = engine.Process(x, PageSegMode.SingleLine))
                            res = page.GetText().Replace(" ", "").Trim();
                    }

                    return res;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Erro: {ex.Message}");
                    return null;
                }
            }}

            #endregion 
        };

        #region [ Methods ]

        public static string[] getScriptNames() { return M_SCRIPTS.Keys.ToArray(); }

        public static string Execute(string filterName, object config, Bitmap image)
        {
            string result = string.Empty;

            Bitmap img = image;
            Func<object, Bitmap, String> fun;
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

        static string Get(this Bitmap Imagem, string name, object config)
        {
            if (M_SCRIPTS.ContainsKey(name))
            {
                string text = M_SCRIPTS[name](config, Imagem);
                return text;
            }
            return string.Empty;
        }

        public static bool Exist(this string name) => M_SCRIPTS.ContainsKey(name);

        #endregion

    }
}
