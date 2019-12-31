﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScreenshotTaker
{
    /// <summary>
    /// 
    /// </summary>
    public class ScreenshotHelper
    {
        /// <summary>
        /// Returns files from folder
        /// </summary>
        /// <param name="searchFolder"></param>
        /// <param name="filters"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public static String[] GetFilesWithFilters(String searchFolder, String[] filters, bool isRecursive)
        {
            var filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }

        /// <summary>
        /// Gets screenshots by give path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Screenshot> GetScreenshots(string path = "")
        {
            if (path.Equals("")) path = Taker.GetPath();
            var result = new List<Screenshot>();
            var filters = new[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
            var files = GetFilesWithFilters(path, filters, false);

            foreach (var fileInfo in files.Select(file => new FileInfo(file)))
            {
                fileInfo.Refresh();

                result.Add(new Screenshot { Name = fileInfo.Name, Date = fileInfo.CreationTime });
            }

            return result;
        }
    }
}
