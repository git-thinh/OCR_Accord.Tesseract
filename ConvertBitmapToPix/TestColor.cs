using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesseract;

namespace ConvertBitmapToPix
{

    public class TestColor
    {
        public static void CastColorToNetColor()
        {
            var color = new PixColor(100, 150, 200);
            var castColor = (System.Drawing.Color)color;
            //Assert.That(castColor.R, Is.EqualTo(color.Red));
            //Assert.That(castColor.G, Is.EqualTo(color.Green));
            //Assert.That(castColor.B, Is.EqualTo(color.Blue));
            //Assert.That(castColor.A, Is.EqualTo(color.Alpha));
        }

        public static void ConvertRgb555ToPixColor()
        {
            ushort originalVal = 0x39EC;
            var convertedValue = BitmapHelper.ConvertRgb555ToRGBA(originalVal);
            //Assert.That(convertedValue, Is.EqualTo(0x737B63FF));
        }

        //[TestCase(0xB9EC, 0x737B63FF)]
        //[TestCase(0x39EC, 0x737B6300)]
        public static void ConvertArgb555ToPixColor(int originalVal, int expectedVal)
        {
            var convertedValue = BitmapHelper.ConvertArgb1555ToRGBA((ushort)originalVal);
            //Assert.That(convertedValue, Is.EqualTo((uint)expectedVal));
        }

        public static void ConvertRgb565ToPixColor()
        {
            ushort originalVal = 0x73CC;
            var convertedValue = BitmapHelper.ConvertRgb565ToRGBA(originalVal);
            //Assert.That(convertedValue, Is.EqualTo(0x737963FF));
        }
    }

}
