using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pattern_Recognition_Task_2
{
    class MultipleFeaturesPixelClassifier
    {
        private int num_of_classes;
        private int num_of_actions;
        private double[] muR, muG, muB, sigmaR, sigmaG, sigmaB;
        private double[,] lambda;
        private double[] postrior, likelihood;
        private double prior;
        private double[] risk;
        private int OAAccuracy;
        private int []classAccuracy;
        private Color[] classesColors;
        private Bitmap sourceImage;
        private Bitmap classifiedImage;
        private int[,] confusionMatrix;
        public GenImage.Slice[] slices;
        private bool uploaded;
        private bool generated;

        public MultipleFeaturesPixelClassifier(int numClasses, double[] muR, double[] muG, double[] muB, double[] sigmaR, double[] sigmaG, double[] sigmaB, bool uploaded)
        {
            num_of_classes = numClasses;
            num_of_actions = num_of_classes + 1;
            this.muR = muR;
            this.muG = muG;
            this.muB = muB;
            this.sigmaB = sigmaB;
            this.sigmaG = sigmaG;
            this.sigmaR = sigmaR;
            this.uploaded = uploaded;
            generated = !uploaded;
            if (generated)
            {
                confusionMatrix = new int[num_of_classes, num_of_classes];
                prior = 1.0 / (double)num_of_classes;
                classAccuracy = new int[num_of_classes];
            }
            classesColors = new Color[num_of_classes + 1];
            likelihood = new double[num_of_classes];
            postrior = new double[num_of_classes];
            risk = new double[num_of_actions];
            this.uploaded = uploaded;

            Random Generator = new Random();
            int Red, Green, Blue;
            for (int i = 0; i < num_of_classes; i++)
            {
                Color c;
                Red = Generator.Next() % 256;
                Green = Generator.Next() % 256;
                Blue = Generator.Next() % 256;
                c = Color.FromArgb(Red, Green, Blue);
                classesColors[i] = c;
            }

            classesColors[num_of_classes] = Color.Black;
        }

        public int[,] getConfusionMatrix()
        {
            return confusionMatrix;
        }


        private double normalFunction(double Segma, double mu, double x)
        {
            return (1.0 / (Math.Sqrt(2 * Math.PI) * Segma)) * Math.Exp(-((x - mu) * (x - mu)) / (2 * Segma * Segma));
        }

        private int getPixelClass(Color c)
        {
            double evidence = 0;
            double rLike, gLike, bLike;
            risk = new double[num_of_actions];
            for (int i = 0; i < num_of_classes; i++)
            {
                rLike = normalFunction(sigmaR[i], muR[i], c.R);
                gLike = normalFunction(sigmaG[i], muG[i], c.G);
                bLike = normalFunction(sigmaB[i], muB[i], c.B);
                likelihood[i] = rLike * bLike * gLike;
                if(generated)
                    evidence = evidence + likelihood[i] * prior;
            }

           
            for (int i = 0; i < num_of_classes; i++)
            {
                if (generated)
                    postrior[i] = likelihood[i] * prior / evidence;
                else
                    postrior[i] = likelihood[i];
            }

            double minRisk = 999999;
            

            for (int action = 0; action < num_of_actions; action++)
            {
                risk[action] = 0;
                for (int Class = 0; Class < num_of_classes; Class++)
                {
                    risk[action] += lambda[Class, action] * postrior[Class];
                }
                minRisk = Math.Min(minRisk, risk[action]);    
            }

            
            int ret = num_of_classes;
            for (int i = 0; i < num_of_actions; i++)
            {
                if (risk[i] == minRisk)
                {
                    ret = i;
                }
                   
            }
            
            return ret;
        }

        private int getOriginalPixelClass(int col)
        {
            for (int i = 0; i < num_of_classes; i++)
            {
                if (col >= slices[i].start && col <= slices[i].end)
                    return i;
            }

            return -1;
        }

        private void generatePixels()
        {
            for (int row = 0; row < sourceImage.Width; row++)
            {
                for (int col = 0; col < sourceImage.Height; col++)
                {
                    
                    int afterClassification = getPixelClass(sourceImage.GetPixel(row, col));
                    if (generated)
                    {
                        int beforeClassification = getOriginalPixelClass(row);
                        if (afterClassification >= 0 && afterClassification < num_of_classes)
                            confusionMatrix[beforeClassification, afterClassification]++;
                    }

                    classifiedImage.SetPixel(row, col, classesColors[afterClassification]);
                
                }
            }

            if (generated)
            {
                OAAccuracy = 0;
                for (int i = 0; i < num_of_classes; i++)
                {
                    classAccuracy[i] = confusionMatrix[i, i];
                    OAAccuracy += classAccuracy[i];
                }
            }
                
        }

        public int getOAAccuracy()
        {
            return OAAccuracy;
        }

        public int getAccuracy(int c)
        {
            return classAccuracy[c];
        }

        public Bitmap classifyColoredImage(Bitmap bm, double[,] l)
        {
            this.lambda = l;
            sourceImage = bm;
            classifiedImage = new Bitmap(bm);
            generatePixels();
            
            return classifiedImage;
        }



    }
}
