using System;
using System.Collections.Generic;
using System.Text;

namespace IPoVnSystem
{
    public class Utilities
    {
        unsafe public static void CalcIntegral(
            ushort* pSrc, int width, int height, double vNormalize, double* pDst)
        {
            // get data's length
            int length = width * height;

            // temporate variables
            int x, y, index;

            // calculate integral image
            {                
                pDst[0] = pSrc[0] - vNormalize;

                // first line
                for (index = 1; index < width; index++)
                {
                    // calc integral
                    pDst[index] = pDst[index - 1] + (pSrc[index] - vNormalize);
                }

                // first column
                for (index = width, y = 1; y < height; y++, index += width)
                {
                    // calc integral
                    pDst[index] = pDst[index - width] + (pSrc[index] - vNormalize);
                }

                // remains
                int x_1y_1 = -width - 1;
                int xy_1 = -width;
                int x_1y = -1;
                for (index = width, y = 1; y < height; y++)
                {
                    index += 1;
                    for (x = 1; x < width; x++, index++)
                    {
                        // calc integral
                        pDst[index] =
                            pDst[index + x_1y] + pDst[index + xy_1] -
                            pDst[index + x_1y_1] + (pSrc[index] - vNormalize);
                    }
                }
            }
        }
    }
}
