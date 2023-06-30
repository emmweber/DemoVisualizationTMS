using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TCPeasy
{
    public class ColorStyles : MonoBehaviour
    {

        public static Color LerpHSV(ColorHSV a, ColorHSV b, float t)
        {
            // Hue interpolation
            float h;
            float d = b.h - a.h;
            if (a.h > b.h)
            {
                // Swap (a.h, b.h)
                var h3 = b.h;
                b.h = a.h;
                a.h = h3;

                d = -d;
                t = 1 - t;
            }

            if (d > 0.5) // 180deg
            {
                a.h = a.h + 1; // 360deg
                h = (a.h + t * (b.h - a.h)) % 1; // 360deg
            }
            else  // 180deg
            {
                h = a.h + t * d;
            }


            ColorHSV colorHSV = new ColorHSV();


            // interpolate the rest ----
            colorHSV.h = h; // H
            colorHSV.s= a.s + t * (b.s - a.s);    // S
            colorHSV.v = a.v + t * (b.v - a.v);    // V
            colorHSV.a = a.a + t * (b.a - a.a);   // A

            return colorHSV;

            
        }



    }
}
