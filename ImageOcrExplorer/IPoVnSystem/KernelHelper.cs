using System;
using System.Collections.Generic;
using System.Text;

namespace IPoVnSystem
{
    public class KernelHelper
    {
        unsafe public static void CalcOffsets(
            int imageStride, int kWidth, int kHeight,
            ref int offsetT_1L_1, ref int offsetT_1R, ref int offsetB_L_1, ref int offsetBR)
        {
            offsetT_1L_1    = -imageStride - 1;
            offsetT_1R      = -imageStride + kWidth - 1;
            offsetB_L_1     = (kHeight - 1) * imageStride - 1;
            offsetBR        = (kHeight - 1) * imageStride + kWidth - 1;
        }
    }
}
