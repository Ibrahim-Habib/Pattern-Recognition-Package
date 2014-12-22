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
        private Bitmap Img;
        private int Height;
        private int Width;
        private int numOfClasses;
        public Slice[] slices;

        public struct Slice
        {
            public double R_mu, G_mu, B_mu,
                R_sigma, G_sigma, B_sigma;
            public int start, end;
        }

        public void setWidth(int x)
        {
            Width = x;
        }

        public void setHeight(int x)
        {
            Height = x;
        }

        public void setNumberOfClasses(int x) 
        {
            numOfClasses = x;
        }

        public Slice[] getSlices
        {
            get { return slices; }
            set { }
        }


        public GenImage()
        {

            this.Width = 400;
            this.Height = 400;
            this.numOfClasses = 1;
            slices = new Slice[10];
        }

        public GenImage(int width, int height, int numClasses)
        {
            this.Width = width;
            this.Height = height;
            this.numOfClasses = numClasses;
            slices = new Slice[10];
        }

        

        public Bitmap GenImg()
        {
            Img = new Bitmap(Width, Height);
            for (int i = 0; i < numOfClasses; i++)
            {
                if (i == 0)
                    slices[i].start = 0;
                else
                    slices[i].start = slices[i - 1].end + 1;

                slices[i].end =(int)(((double)(i + 1) / numOfClasses) * Width) - 1;
            }

            for (int i = 0; i < numOfClasses; i++)
                FillSlices(slices[i]);
            return Img;
        }

        private double boxMuller(double R1, double R2)
        {
            return  Math.Sqrt(-2 * Math.Log(R1, Math.E)) * 0.5 * Math.Cos(2 * Math.PI * R2);
        }

        private void FillSlices(Slice slice)
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
