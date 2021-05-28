using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace AForgeDetectObjectImage
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fMain());
        }
    }

    public class XYX1Y1
    {
        public int x { get; }
        public int y { get; }
        public int x1 { get; }
        public int y1 { get; }
        public XYX1Y1(System.Drawing.Rectangle r)
        {
            this.x = r.X;
            this.y = r.Y;
            this.x1 = r.X + r.Width;
            this.y1 = r.Y + r.Height;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", x, y, x1, y1);
        }

        public static System.Drawing.Rectangle Merge(XYX1Y1[] arr) {
            var r = new System.Drawing.Rectangle();
            int x = arr.Min(o => o.x);
            int y = arr.Min(o => o.y);
            int x1 = arr.Max(o => o.x1);
            int y1 = arr.Max(o => o.y1);

            r.X = x;
            r.Y = y;
            r.Width = x1 - x;
            r.Height = y1 - y;

            return r;
        }
    }
}
