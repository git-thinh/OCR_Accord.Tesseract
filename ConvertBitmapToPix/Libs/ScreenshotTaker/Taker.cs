using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ScreenshotTaker
{
    /// <summary>
    /// Main class
    /// </summary>
    public static class Taker
    {
        private static void SaveScreen(this Image img, string path, string name, ImageFormat format)
        {
            var fullPath = path + @"\" + name + "." + format;
            if (!Directory.Exists(path.TrimEnd('\\')))
            {
                Directory.CreateDirectory(path.TrimEnd('\\'));
            }
            Console.WriteLine("Saving screenshot: " + fullPath);
            img.Save(fullPath, format);
            Console.WriteLine("Saved.");
        }

        /// <summary>
        /// Default name for screenshot
        /// </summary>
        /// <param name="now"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetScreenName(DateTime now, ImageFormat format = null)
        {
            format = format ?? ImageFormat.Png;
            return String.Format("screenshot_{0}.{1}", now.ToString("yyyyMMddHHmmssfff"), format.ToString().ToLower());
        }

        /// <summary>
        /// Method to get currently executing assembly path
        /// </summary>
        /// <returns>Path to the assembly</returns>
        public static string GetPath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            return path + @"\_Screenshots";
        }

        /// <summary>
        /// Method to clean folder with screenshots
        /// </summary>
        public static void DeleteScreenshots(string path = "")
        {
            var folderPath = path.Equals("") ? GetPath() : path;
            var folder = new DirectoryInfo(folderPath);

            foreach (var file in folder.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in folder.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Method to take screenshot from primary screen, change it's creation time and save it into the folder
        /// </summary>
        /// <param name="creationTime"></param>
        /// <param name="screenPath"></param>
        /// <returns></returns>
        public static string TakeScreenshot(DateTime creationTime = default(DateTime), string screenPath = "")
        {
            var format = ImageFormat.Png;
            var now = DateTime.Now;
            creationTime = creationTime.Equals(default(DateTime)) ? now : creationTime;
            var screenName = GetScreenName(creationTime, format);

            using (var bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height))
            {
                using (var g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);

                    var file = (screenPath.Equals("") ? GetPath() : screenPath) + screenName;
                    bmpScreenCapture.Save(file, format);
                    var fileInfo = new FileInfo(file);
                    fileInfo.Refresh();
                    fileInfo.CreationTime = creationTime;

                }
            }
            return screenName;
        }

        /// <summary>
        /// Method to take screenshot from primary screen and save it into the folder
        /// </summary>
        public static void TakeScreenshot(string path = "", string name = "")
        {
            var now = DateTime.Now;
            var screenPath = path.Equals("") ? GetPath() : path;
            var screenName = name.Equals("") ? String.Format("screenshot_{0}", now.ToString("yyyyMMddHHmmssfff")) : name;

            using (var bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height))
            {
                using (var g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                    bmpScreenCapture.SaveScreen(screenPath, screenName, ImageFormat.Png);
                }
            }
        }
    }
}
