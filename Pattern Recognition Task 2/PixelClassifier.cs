using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pattern_Recognition_Task_2
{
    class PixelClassifier
    {
       
        private int num_of_classes;
        private int num_of_actions;
        //classes properties (size of each array = num_of_classes) 
        private double [] prior;
        private double[] Mu;
        private double[] segma;
        private double[] postrior;
        private double[] likleihood;
        private double evidence;
        private Bitmap oldImage;
        private Bitmap newImage;
        //private FastImage oldImageFast;
        //private FastImage newImageFast;

        public PixelClassifier()
        { }

        public PixelClassifier(int num_of_classes, double[] prior, double[] Mu, double[] segma)
        {
            this.num_of_classes = num_of_classes;
            this.prior = prior;
            this.Mu = Mu;
            this.segma = segma;
            postrior = new double[num_of_classes];
            likleihood = new double[num_of_classes];
        }

        
        private double normalFunction(double Segma, double mu, double x)
        {
            return (1.0 / (Math.Sqrt(44.0 / 7.0) * Segma)) * Math.Exp(-((x - mu) * (x - mu)) / (2 * Segma * Segma)); 
        }

        private int getPixelClass(Color pixelColor)
        {
            evidence = 0;
             for (int i = 0; i < num_of_classes; i++)
            {
                likleihood[i] = normalFunction(segma[i], Mu[i], (pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);
                evidence += likleihood[i] * prior[i];
            }

            double maxPosterior = 0;
            for (int i = 0; i < num_of_classes; i++)
            {
                postrior[i] = likleihood[i] / evidence;
                maxPosterior = Math.Max(maxPosterior, postrior[i]);
            }

            for (int i = 0; i < num_of_classes; i++)
            {
                if (postrior[i] == maxPosterior)
                    return i;
            }

            return -1;
        }



        private void generatePixels()
        {
            for (int row = 0; row < newImage.Width; row++)
            {
                for (int column = 0; column < newImage.Height; column++)
                {
                    //int temp = getPixelClass(oldImageFast.GetPixel(row, column));
                    int temp = getPixelClass(oldImage.GetPixel(row, column));
                    if (temp == 0)
                    {
                        newImage.SetPixel(row, column, Color.Red);
                        //newImageFast.Img.SetPixel(row, column, Color.Red);
                    }
                    else if (temp == 1)
                    {
                        newImage.SetPixel(row, column, Color.Lime);
                        //newImageFast.Img.SetPixel(row, column, Color.Lime);
                    }
                    else if (temp == 2)
                    {
                        newImage.SetPixel(row, column, Color.Blue);
                        //newImageFast.Img.SetPixel(row, column, Color.Blue);
                    }
                    else
                    {
                        newImage.SetPixel(row, column, Color.Yellow);
                        //newImageFast.Img.SetPixel(row, column, Color.Yellow);
                    }
                }
                
            }
        
        }

        public Bitmap classifyImage(Bitmap bm)
        {
            oldImage = bm;
            //oldImageFast = new FastImage(oldImage);
            newImage = new Bitmap(oldImage.Width, oldImage.Height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //newImageFast = new FastImage(newImage);
            generatePixels();
            return newImage;
        }



    }
}
