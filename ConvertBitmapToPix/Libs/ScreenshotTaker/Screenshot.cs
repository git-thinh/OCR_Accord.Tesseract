using System;

namespace ScreenshotTaker
{
    /// <summary>
    /// 
    /// </summary>
    public class Screenshot
    {
        /// <summary>
        /// 
        /// </summary>
        public Screenshot()
        {
            var now = DateTime.Now;
            Name = Taker.GetScreenName(now);
            Date = now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        public Screenshot(DateTime date)
        {
            Name = Taker.GetScreenName(date);
            Date = date;
        }

        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime Date { get; set; }
    }
}
