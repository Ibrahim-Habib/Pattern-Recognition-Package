using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Pattern_Recognition_Task_2
{
    class GenImage
    {
        private static Bitmap Img;
        private static int Height;

        public struct Slice
        {
            public double R_mu, G_mu, B_mu,
                R_sigma, G_sigma, B_sigma;
            public int start, end;
        }

        static public Bitmap GenImg(Slice[] slices, int height, int width)
        {
            Img = new Bitmap(width, height);
            Height = height;
            slices[0].start = 0;
            slices[0].end = width / 4;
            slices[1].start = slices[0].end + 1;
            slices[1].end = width / 2;
            slices[2].start = slices[1].end + 1;
            slices[2].end = (3 * width) / 4;
            slices[3].start = slices[2].end + 1;
            slices[3].end = width - 1;

            for (int i = 0; i < 4; i++)
                FillSlices(slices[i]);
            return Img;
        }

        static private double boxMuller(double R1, double R2)
        {
            return  Math.Sqrt(-2 * Math.Log(R1, Math.E)) * 0.5 * Math.Cos(2 * Math.PI * R2);
        }

        static private void FillSlices(Slice slice)
        {
            Random GRand = new Random();
            Random BRand = new Random();
            Random RRand = new Random();
            double R1, R2, Z;
            int R, G, B;

            for (int row = 0; row < Height; row++)
                for (int col = slice.start; col <= slice.end; col++)
                {
                    R1 = RRand.NextDouble();
                    R2 = RRand.NextDouble();
                    Z = boxMuller(R1, R2);

                    R = (int)(Z * slice.R_sigma + slice.R_mu);
                    if (R > 255)
                        R = 255;
                    else if (R < 0)
                        R = 0;

                    R1 = GRand.NextDouble();
                    R2 = GRand.NextDouble();
                    Z = boxMuller(R1, R2);

                    G = (int)(Z * slice.G_sigma + slice.G_mu);
                    if (G > 255)
                        G = 255;
                    else if (G < 0)
                        G = 0;

                    R1 = BRand.NextDouble();
                    R2 = BRand.NextDouble();
                    Z = boxMuller(R1, R2);

                    B = (int)(Z * slice.B_sigma + slice.B_mu);

                    if (B > 255)
                        B = 255;
                    else if (B < 0)
                        B = 0;

                    Img.SetPixel(col, row, Color.FromArgb(R, G, B));
                }
        }
    }
}
