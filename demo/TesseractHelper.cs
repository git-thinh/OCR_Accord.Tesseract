using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tesseract
{
    /// <summary>
    /// Wrapper around Tesseract Library
    /// </summary>
    public static class TesseractHelper
    {
        private static Regex regex = new Regex("[^a-zA-Z0-9]");

        /// <summary>
        /// Gets the number from image.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static int GetNumberFromImage(string imagePath)
        {
            var numberValue = 0;
            if (string.IsNullOrWhiteSpace(imagePath) == false && System.IO.File.Exists(imagePath) == true)
            {
                try
                {
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(imagePath))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                if (string.IsNullOrWhiteSpace(text) == false)
                                {
                                    text = regex.Replace(text, string.Empty);                               //remove non alpha numeric characters
                                    text = text.ToLowerInvariant().Replace('i', '1').Replace('o', '0');    //to fix wrong interpretation
                                    if (int.TryParse(text, out numberValue) == false)
                                    {
                                        Console.WriteLine("Unable to process the file : " + imagePath);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected Error: " + ex.Message);
                }
            }
            return numberValue;
        }

        /// <summary>
        /// Gets all the text as string from image.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static string GetStringFromImage(string imagePath)
        {
            var textValue = string.Empty;
            if (string.IsNullOrWhiteSpace(imagePath) == false && System.IO.File.Exists(imagePath) == true)
            {
                try
                {
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(imagePath))
                        {
                            using (var page = engine.Process(img))
                            {
                                textValue = page.GetText();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected Error: " + ex.Message);
                }
            }
            return textValue;
        }

        /// <summary>
        /// Gets all the numbers seperately from image.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static List<int> GetAllNumbersFromImage(string imagePath)
        {
            var numbersList = new List<int>();
            if (string.IsNullOrWhiteSpace(imagePath) == false && System.IO.File.Exists(imagePath) == true)
            {
                try
                {
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(imagePath))
                        {
                            using (var page = engine.Process(img))
                            {
                                if (string.IsNullOrWhiteSpace(page.GetText()) == false)
                                {
                                    page.GetText()
                                        .Split(' ')
                                        .ToList()
                                        .ForEach(text =>
                                                {
                                                    if (string.IsNullOrWhiteSpace(text) == false)
                                                    {
                                                        var numberValue = 0;
                                                        text = regex.Replace(text, string.Empty);                               //remove non alpha numeric characters
                                                        text = text.ToLowerInvariant().Replace('i', '1').Replace('o', '0');    //to fix wrong interpretation
                                                        if (int.TryParse(text, out numberValue) == false)
                                                        {
                                                            Console.WriteLine("Unable to process the file : " + imagePath);
                                                        }
                                                        else
                                                        {
                                                            numbersList.Add(numberValue);
                                                        }
                                                    }
                                                });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected Error: " + ex.Message);
                }
            }
            return numbersList;
        }
    }
}
