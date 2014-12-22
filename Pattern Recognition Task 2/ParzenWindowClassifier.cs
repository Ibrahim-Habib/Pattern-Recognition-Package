using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pattern_Recognition_Task_2
{
    class ParzenWindowClassifier
    {
        private int num_of_classes;
        private int num_of_samples;
        private double windowSize;
        private bool isGenerated;
        private bool isUploaded;
        private Color[][] takenSamples;
        private Bitmap sourceImage, classifiedImage;
        private Color[] colorOfClass;
        private int[,] confusionMatrix;

        public ParzenWindowClassifier(int num_classes, int num_samples, double window_size, bool isGenerated, Color[][] samples)
        {
            num_of_classes = num_classes;
            num_of_samples = num_samples;
            windowSize = window_size;
            this.isGenerated = isGenerated;
            this.isUploaded = !isGenerated;

            takenSamples = samples;
            colorOfClass = new Color[num_of_classes + 1];
            Random r = new Random();
            for (int i = 0; i < num_of_classes; i++)
            {
                if (i == 0)
                    colorOfClass[i] = Color.FromArgb(r.Next() % 256, r.Next() % 256, r.Next() % 256);
                else
                    colorOfClass[i] = Color.FromArgb((colorOfClass[i - 1].R + 256 / num_of_classes) % 256, (colorOfClass[i - 1].G + 256 / num_of_classes) % 256, (colorOfClass[i - 1].B + 256 / num_of_classes) % 256);
            }

            colorOfClass[num_of_classes] = Color.Black;

            confusionMatrix = new int[num_of_classes, num_of_classes];
        }

        private int getOrigianlPixelClass(int col)
        { 
            return col / (sourceImage.Width / num_of_classes);
        }

        private bool isInside(Color C1, Color C2)
        {
            return Math.Abs(C1.R - C2.R) <= windowSize / 2 && Math.Abs(C1.G - C2.G) <= windowSize / 2 && Math.Abs(C1.B - C2.B) <= windowSize / 2;
        }

        private int getPixelClass(Color C)
        {
            int[] k = new int[num_of_classes];
            for (int i = 0; i < num_of_classes; i++)
                k[i] = 0;
            int maximumCount = 0;
            for (int i = 0; i < num_of_classes; i++)
            {
                for (int j = 0; j < num_of_samples; j++)
                {
                    if (isInside(C, takenSamples[i][j]))
                    {
                        k[i]++;
                        maximumCount = Math.Max(maximumCount, k[i]);
                    }
                }
            }

            int c = 0;
            for (int i = 0; i < num_of_classes; i++)
            {
                if (k[i] == maximumCount)
                    c++;
            }

            if(c > 1)
                return num_of_classes;
            for (int i = 0; i < num_of_classes; i++)
            {
                if (k[i] == maximumCount)
                    return i;
            }
            
            return -1;
                
        }

        private void generateClassifiedImage()
        {
            int originalClass , afterClass;
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    afterClass = getPixelClass(sourceImage.GetPixel(x, y));
                    if (isGenerated)
                    {
                        originalClass = getOrigianlPixelClass(x);
                        if(afterClass < num_of_classes)
                         confusionMatrix[originalClass, afterClass]++;
                    }
                    classifiedImage.SetPixel(x, y, colorOfClass[afterClass]);
                }
            }

        }

        public int[,] getConfusionMatrix()
        {
            return confusionMatrix;
        }

        public Bitmap classifyPixels(Bitmap bm)
        {
            sourceImage = bm;
            classifiedImage = new Bitmap(sourceImage);
            generateClassifiedImage();
            return classifiedImage;
        }
    }
}
