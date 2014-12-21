using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pattern_Recognition_Task_2
{
    public partial class GUI : Form
    {
        Bitmap greyScaleImage;
        Bitmap coloredImage;
        private Bitmap source;
        private int H, W,numOfClasses;
        private int currentClass;
        private int currentClick;
        GenImage imageGenerator;
        private MultipleFeaturesPixelClassifier pixelClassifier;
        private double[,] lambda;
        private int[,] confusionMatrix;
        private int numOfClicks;
        private double[][] rValue, gValue, bValue;
        private double[] rMu, gMu, bMu, rSigma, gSigma, bSigma;

        public GUI()
        {
            InitializeComponent();
            widthTextBox.Text = "300";
            hieghtTextBox.Text = "300";
            numOfClassesTextBox.Text = "1";
            NumOfClicksTextBox.Text = "2";
            currentClass = 0;
            H = W = 300;
            imageGenerator = new GenImage();
            Mu1TextBox.Enabled = numericUpDown1.Value >= 1;
            segma1TextBox.Enabled = numericUpDown1.Value >= 1;
            prior1TextBox.Enabled = numericUpDown1.Value >= 1;

            Mu2TextBox.Enabled = numericUpDown1.Value >= 2;
            segma2TextBox.Enabled = numericUpDown1.Value >= 2;
            prior2TextBox.Enabled = numericUpDown1.Value >= 2;

            Mu3TextBox.Enabled = numericUpDown1.Value >= 3;
            segma3TextBox.Enabled = numericUpDown1.Value >= 3;
            prior3TextBox.Enabled = numericUpDown1.Value >= 3;

            Mu4TextBox.Enabled = numericUpDown1.Value >= 4;
            segma4TextBox.Enabled = numericUpDown1.Value >= 4;
            prior4TextBox.Enabled = numericUpDown1.Value >= 4;

            
            comboBox1.SelectedIndex = 0;
            NumOfClassesComboBox.SelectedIndex = 0;

            Random rand = new Random();

            for (int i = 0; i < imageGenerator.slices.Length; i++)
            {
                imageGenerator.slices[i].R_mu = rand.Next() % 256;
                imageGenerator.slices[i].R_sigma = rand.Next() % 30;
                imageGenerator.slices[i].G_mu = rand.Next() % 256;
                imageGenerator.slices[i].G_sigma = rand.Next() % 30;
                imageGenerator.slices[i].B_mu = rand.Next() % 256;
                imageGenerator.slices[i].B_sigma = rand.Next() % 30;
            
            }
            
            Mu1TextBox.Text = "0";
            Mu2TextBox.Text = "0";
            Mu3TextBox.Text = "0";
            Mu4TextBox.Text = "0";

            segma1TextBox.Text = "0";
            segma2TextBox.Text = "0";
            segma3TextBox.Text = "0";
            segma4TextBox.Text = "0";

            prior1TextBox.Text = "0";
            prior2TextBox.Text = "0"; 
            prior3TextBox.Text = "0"; 
            prior4TextBox.Text = "0";
        }

        

        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;*.tif;*.tiff;|All Files (*.*)|*.*";
            openFileDialog1.Title = "Open an Image";
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                greyScaleImage = null;
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                
                greyScaleImage = new Bitmap(OpenedFilePath);
                if (greyScaleImage == null)
                    MessageBox.Show("Image can't be opened !", "Error", MessageBoxButtons.OK);
                else
                {
                    ImagePath.Text = OpenedFilePath;
                    greyScalePictureBox.Image = greyScaleImage;
                }

            }


        }

        private void SegmentImageButton_Click(object sender, EventArgs e)
        {
            
        }

        private void AfterSegmentationPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;|All Files (*.*)|*.*";
            openFileDialog1.Title = "Open an Image";
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string OpenedFilePath = openFileDialog1.FileName;
                source = new Bitmap(OpenedFilePath);
                pictureBox1.Image = source;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RMuTextBox.Text = imageGenerator.slices[comboBox1.SelectedIndex].R_mu.ToString();
            RSigmaTextBox.Text = imageGenerator.slices[comboBox1.SelectedIndex].R_sigma.ToString();
            GMuTextBox.Text = imageGenerator.slices[comboBox1.SelectedIndex].G_mu.ToString();
            GSigmaTextBox.Text = imageGenerator.slices[comboBox1.SelectedIndex].G_sigma.ToString();
            BMuTextBox.Text = imageGenerator.slices[comboBox1.SelectedIndex].B_mu.ToString();
            BSigmaTextBox.Text = imageGenerator.slices[comboBox1.SelectedIndex].B_sigma.ToString();
        }

   

        private void RSigmaTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imageGenerator.slices[comboBox1.SelectedIndex].R_sigma = double.Parse(RSigmaTextBox.Text);

            }
            catch (Exception)
            { }

        }


        private void GSigmaTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imageGenerator.slices[comboBox1.SelectedIndex].G_sigma = double.Parse(GSigmaTextBox.Text);
            }
            catch (Exception)
            { }
        }

     

        private void BSigmaTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imageGenerator.slices[comboBox1.SelectedIndex].B_sigma = double.Parse(BSigmaTextBox.Text);
            }
            catch (Exception)
            { }
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            try
            {
                H = int.Parse(hieghtTextBox.Text);
                W = int.Parse(widthTextBox.Text);
                imageGenerator.setWidth(W);
                imageGenerator.setHeight(H);
                numOfClasses = NumOfClassesComboBox.SelectedIndex + 1;
                imageGenerator.setNumberOfClasses(numOfClasses);
                //source = GenImage.GenImg(slices, Height, Width);
                coloredImage = imageGenerator.GenImg();
                generatedImagePictureBox.Image = coloredImage;
                //Form form = new Form();
                //PictureBox pic = new PictureBox();
                //pic.Image = source;
                //pic.SizeMode = PictureBoxSizeMode.AutoSize;
                //form.Controls.Add(pic);
                //form.Show();
            }
            catch (Exception)
            {

                MessageBox.Show("Please check if you entered all parameters of the generated Image !"
                    , "Error", MessageBoxButtons.OK);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Mu1TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SegmentImageButton_Click_1(object sender, EventArgs e)
        {
            double[] prior, Mu, segma;
            prior = new double[(int)numericUpDown1.Value];
            Mu = new double[(int)numericUpDown1.Value];
            segma = new double[(int)numericUpDown1.Value];
            try
            {
                if (numericUpDown1.Value >= 1)
                {
                    prior[0] = double.Parse(prior1TextBox.Text);
                    Mu[0] = double.Parse(Mu1TextBox.Text);
                    segma[0] = double.Parse(segma1TextBox.Text);
                }

                if (numericUpDown1.Value >= 2)
                {
                    prior[1] = double.Parse(prior2TextBox.Text);
                    Mu[1] = double.Parse(Mu2TextBox.Text);
                    segma[1] = double.Parse(segma2TextBox.Text);
                }

                if (numericUpDown1.Value >= 3)
                {
                    prior[2] = double.Parse(prior3TextBox.Text);
                    Mu[2] = double.Parse(Mu3TextBox.Text);
                    segma[2] = double.Parse(segma3TextBox.Text);
                }

                if (numericUpDown1.Value >= 4)
                {
                    prior[3] = double.Parse(prior4TextBox.Text);
                    Mu[3] = double.Parse(Mu4TextBox.Text);
                    segma[3] = double.Parse(segma4TextBox.Text);
                }

                double sumPrior = 0;
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    sumPrior += prior[i];
                }

                if (sumPrior > 1)
                    throw (new Exception());

                PixelClassifier pc = new PixelClassifier((int)numericUpDown1.Value, prior, Mu, segma);
                Bitmap bm = pc.classifyImage(greyScaleImage);
                AfterSegmentationPictureBox.Image = bm;
            }

            catch (Exception)
            {
                MessageBox.Show("Please check if you entered all parameters Correctly !"
                    , "Error", MessageBoxButtons.OK);
            }
            
           

        }

        private void UploadImageButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;|All Files (*.*)|*.*";
            openFileDialog1.Title = "Open an Image";
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string OpenedFilePath = openFileDialog1.FileName;
                ImagePath.Text = OpenedFilePath;
                greyScaleImage = new Bitmap(OpenedFilePath);
                greyScalePictureBox.Image = greyScaleImage;
            }

        }

        private void AfterSegmentationPictureBox_Click_1(object sender, EventArgs e)
        {
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;|All Files (*.*)|*.*";
            dialog.Title = "Open an Image";
            dialog.CheckPathExists = true;
           
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                generatedImagePictureBox.Image.Save(dialog.FileName,System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            Mu1TextBox.Enabled = numericUpDown1.Value >= 1;
            segma1TextBox.Enabled = numericUpDown1.Value >= 1;
            prior1TextBox.Enabled = numericUpDown1.Value >= 1;

            Mu2TextBox.Enabled = numericUpDown1.Value >= 2;
            segma2TextBox.Enabled = numericUpDown1.Value >= 2;
            prior2TextBox.Enabled = numericUpDown1.Value >= 2;

            Mu3TextBox.Enabled = numericUpDown1.Value >= 3;
            segma3TextBox.Enabled = numericUpDown1.Value >= 3;
            prior3TextBox.Enabled = numericUpDown1.Value >= 3;

            Mu4TextBox.Enabled = numericUpDown1.Value >= 4;
            segma4TextBox.Enabled = numericUpDown1.Value >= 4;
            prior4TextBox.Enabled = numericUpDown1.Value >= 4;


        }

        private void ImagePath_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;|All Files (*.*)|*.*";
            dialog.Title = "Save an Image";
            dialog.CheckPathExists = true;

  
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void greyScalePictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs) e;
            Point p = me.Location;
            p.X = (int) ((me.Location.X) * ((double)greyScaleImage.Width) / ((double)greyScalePictureBox.Width)) ;
            p.Y = (int)((me.Location.Y) * ((double)greyScaleImage.Height) / ((double)greyScalePictureBox.Height));
            Color c = greyScaleImage.GetPixel(p.X, p.Y);
            double x = (c.G + c.B + c.R) / 3.0;
            if(currentClass == 0 && Mu1TextBox.Enabled)
            {
                Mu1TextBox.Text = x.ToString();
                currentClass++;
            }
            else if (currentClass == 1 && Mu2TextBox.Enabled)
            {
                Mu2TextBox.Text = x.ToString();
                currentClass++;
            }
            else if (currentClass == 2 && Mu3TextBox.Enabled)
            {
                Mu3TextBox.Text = x.ToString();
                currentClass++;
            }
            else if (currentClass == 3 && Mu4TextBox.Enabled)
            {
                Mu4TextBox.Text = x.ToString();
                currentClass++; 
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            Mu1TextBox.Text = Mu2TextBox.Text = Mu3TextBox.Text = Mu4TextBox.Text = "0";
            segma1TextBox.Text = segma2TextBox.Text = segma3TextBox.Text = segma4TextBox.Text = "0";
            prior1TextBox.Text = prior2TextBox.Text = prior3TextBox.Text = prior4TextBox.Text = "0";
            currentClass = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;|All Files (*.*)|*.*";
            dialog.Title = "Open an Image";
            dialog.CheckPathExists = true;


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                AfterSegmentationPictureBox.Image.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void RMuTextBox_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                imageGenerator.slices[comboBox1.SelectedIndex].R_mu = double.Parse(RMuTextBox.Text);
            }
            catch (Exception)
            { }
        }

        private void GMuTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imageGenerator.slices[comboBox1.SelectedIndex].G_mu = double.Parse(GMuTextBox.Text);
            }
            catch (Exception)
            { }
        }

        private void BMuTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                imageGenerator.slices[comboBox1.SelectedIndex].B_mu = double.Parse(BMuTextBox.Text);
            }
            catch (Exception)
            { }

        }


        private void NumOfClassesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            numOfClasses = int.Parse(NumOfClassesComboBox.Text);
            imageGenerator.setNumberOfClasses(numOfClasses);
            while (comboBox1.Items.Count < numOfClasses)
            {
                comboBox1.Items.Add(comboBox1.Items.Count + 1);    
            }

            while(comboBox1.Items.Count > numOfClasses)
            {
                comboBox1.Items.RemoveAt(comboBox1.Items.Count - 1);
            }
        
        }

        private void getGeneratedButton_Click(object sender, EventArgs e)
        {
            if (generatedImagePictureBox.Image != null)
            {
                ColoredImagePictureBox.Image = generatedImagePictureBox.Image;
            }
            
            int numOfClasses = int.Parse(NumOfClassesComboBox.Text);
            double[] mur, mug, mub, sigmar, sigmag, sigmab;
            mur = new double[numOfClasses];
            mug = new double[numOfClasses];
            mub = new double[numOfClasses];
            sigmab = new double[numOfClasses];
            sigmag = new double[numOfClasses];
            sigmar = new double[numOfClasses];
            
            for(int i = 0; i < numOfClasses; i++)
            {
                mur[i] = imageGenerator.slices[i].R_mu;
                mub[i] = imageGenerator.slices[i].B_mu;
                mug[i] = imageGenerator.slices[i].G_mu;
                sigmab[i] = imageGenerator.slices[i].B_sigma;
                sigmag[i] = imageGenerator.slices[i].G_sigma;
                sigmar[i] = imageGenerator.slices[i].R_sigma;
            }

            lambda = new double[numOfClasses, numOfClasses + 1];
            LambdaGridView.Columns.Clear();
            for (int i = 0; i <= numOfClasses + 1; i++)
            {
                string header;
                if (i == 0)
                    header = "";
                else
                    header = string.Concat("action ", i.ToString());
                LambdaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = header, CellTemplate = new DataGridViewTextBoxCell() });
            }
                for (int i = 0; i <= numOfClasses; i++)
                LambdaGridView.Rows.Add(new DataGridViewRow());

                for (int i = 0; i < numOfClasses; i++)
                {
                    for (int j = 1; j <= numOfClasses + 1; j++)
                    {
                        LambdaGridView.Rows[i].Cells[j].Value = "0.2";
                    }
                }
            //LambdaGridView.Width = numOfClasses + 2;
            //LambdaGridView.Height = numOfClasses + 1;
            

            for (int i = 1; i <= numOfClasses; i++)
            {
                LambdaGridView[0, i - 1].Value = string.Concat("class ", i.ToString());
            }

            pixelClassifier = new MultipleFeaturesPixelClassifier(int.Parse(NumOfClassesComboBox.Text), mur, mug, mub, sigmar, sigmag, sigmab, false);
            pixelClassifier.slices = imageGenerator.slices;
        }


        private void UploadColordButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;*.tif;*.tiff;|All Files (*.*)|*.*";
            openFileDialog1.Title = "Open an Image";
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                coloredImage = null;
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;

                coloredImage = new Bitmap(OpenedFilePath);
                if (coloredImage == null)
                    MessageBox.Show("Image can't be opened !", "Error", MessageBoxButtons.OK);
                else
                {
                    ImagePath.Text = OpenedFilePath;
                    ColoredImagePictureBox.Image = coloredImage;
                }

            }


        }

        private void LambdaGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ClassifyImageButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= numOfClasses; i++)
            {
                for (int j = 1; j <= numOfClasses + 1; j++)
                {
                    string s = LambdaGridView.Rows[i - 1].Cells[j].Value.ToString();
                    lambda[i - 1, j - 1] = double.Parse(s);
                }
            }
            ClassifiedImagePictureBox.Image = pixelClassifier.classifyColoredImage(coloredImage, lambda);
            overAllAccuracyTextBox.Text = pixelClassifier.getOAAccuracy().ToString();
            AccuracyDataGridView.Columns.Clear();
            for (int i = 0; i < numOfClasses; i++)
                AccuracyDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = string.Concat("Class ", (i + 1).ToString()), CellTemplate = new DataGridViewTextBoxCell() });
            AccuracyDataGridView.Rows.Add(new DataGridViewRow());
            for (int i = 0; i < numOfClasses; i++)
            {
                AccuracyDataGridView[i, 0].Value = pixelClassifier.getAccuracy(i);
            }

            confusionMatrix = pixelClassifier.getConfusionMatrix();
            ConfusionMatrixDataGrid.Columns.Clear();

            for (int i = 0; i <= numOfClasses; i++)
            {
                string header = "";
                if (i > 0)
                    header = string.Concat("class ", i.ToString());
                ConfusionMatrixDataGrid.Columns.Add(new DataGridViewColumn() { HeaderText = header, CellTemplate = new DataGridViewTextBoxCell() });
            }

            for (int i = 0; i < numOfClasses; i++)
            {
                ConfusionMatrixDataGrid.Rows.Add(new DataGridViewRow());
                ConfusionMatrixDataGrid.Rows[i].Cells[0].Value = string.Concat("Class ", (i + 1).ToString());
            }

            for (int i = 0; i < numOfClasses; i++)
            {
                for (int j = 1; j <= numOfClasses; j++)
                {
                    ConfusionMatrixDataGrid.Rows[i].Cells[j].Value = confusionMatrix[i, j - 1];
                }
            }



        
        }

        private void UploadColordButton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;*.tif;*.tiff;|All Files (*.*)|*.*";
            openFileDialog1.Title = "Open an Image";
            openFileDialog1.CheckPathExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                coloredImage = null;
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;

                coloredImage = new Bitmap(OpenedFilePath);
                if (coloredImage == null)
                    MessageBox.Show("Image can't be opened !", "Error", MessageBoxButtons.OK);
                else
                {
                    ImagePath.Text = OpenedFilePath;
                    coloredUploadedPictureBox.Image = coloredImage;
                }

            }

        }

        private void numOfClassesTextBox_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                numOfClasses = int.Parse(numOfClassesTextBox.Text);
            }
            catch (Exception)
            { }
        }

        private void NumOfClicksTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                numOfClicks = int.Parse(NumOfClicksTextBox.Text);
            }
            catch (Exception)
            { }

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            lambda = new double[numOfClasses, numOfClasses + 1];
            currentClass = 0;
            currentClick = 0;
            rValue = new double[numOfClasses][];
            gValue = new double[numOfClasses][];
            bValue = new double[numOfClasses][];
            for (int i = 0; i < numOfClasses; i++)
            {
                rValue[i] = new double[numOfClicks];
                gValue[i] = new double[numOfClicks];
                bValue[i] = new double[numOfClicks];
            }

            rMu = new double[numOfClasses];
            gMu = new double[numOfClasses];
            bMu = new double[numOfClasses];
            rSigma = new double[numOfClasses];
            gSigma = new double[numOfClasses];
            bSigma = new double[numOfClasses];
            //Mu & Sigma GridView
            ///////////////////////////////////////////////////////////////////////////
            MuAndSigmaGridView.Columns.Clear();
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "", CellTemplate = new DataGridViewTextBoxCell() });
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "R Mu", CellTemplate = new DataGridViewTextBoxCell() });
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "R Sigma", CellTemplate = new DataGridViewTextBoxCell() });
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "G Mu", CellTemplate = new DataGridViewTextBoxCell() });
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "G Sigma", CellTemplate = new DataGridViewTextBoxCell() });
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "B Mu", CellTemplate = new DataGridViewTextBoxCell() });
            MuAndSigmaGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "B Sigma", CellTemplate = new DataGridViewTextBoxCell() });
           
            for (int i = 0; i < numOfClasses; i++)
            {
                MuAndSigmaGridView.Rows.Add(new DataGridViewRow());
                MuAndSigmaGridView.Rows[i].Cells[0].Value = string.Concat("Class ", (i + 1).ToString());
            }
            ////////////////////////////////////////////////////////////////////////////

            //Lambda GridView

            LambdaLoadedGridView.Columns.Clear();
            for (int i = 0; i <= numOfClasses + 1; i++)
            {
                string header;
                if (i == 0)
                    header = "";
                else
                    header = string.Concat("action ", i.ToString());
                LambdaLoadedGridView.Columns.Add(new DataGridViewColumn() { HeaderText = header, CellTemplate = new DataGridViewTextBoxCell() });
            }

            for (int i = 0; i < numOfClasses; i++)
            {
                LambdaLoadedGridView.Rows.Add(new DataGridViewRow());
                LambdaLoadedGridView.Rows[i].Cells[0].Value = string.Concat("Class ", (i + 1).ToString());
            }
            for (int i = 0; i < numOfClasses; i++)
            {
                for (int j = 1; j <= numOfClasses + 1; j++)
                {
                    LambdaLoadedGridView.Rows[i].Cells[j].Value = "0.2";
                }
            }


        }

        private double calculateMu(double[] d)
        {
            double ret = 0;
            for (int i = 0; i < d.Length; i++)
            {
                ret += d[i];
            }

            return ret / (double)d.Length;
        }

        private double calculateVariance(double[] d, double Mu)
        {
            double variance = 0;
            for (int i = 0; i < d.Length; i++)
                variance += (d[i] - Mu) * (d[i] - Mu);
            return variance / (double)d.Length;
        }

        private void coloredUploadedPictureBox_Click(object sender, EventArgs e)
        {
            if (currentClass < numOfClasses)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                Point p = me.Location;
                p.X = (int)(((me.Location.X)) * ((double)coloredImage.Width) / ((double)coloredUploadedPictureBox.Width));
                p.Y = (int)((me.Location.Y) * ((double)coloredImage.Height) / ((double)coloredUploadedPictureBox.Height));
                Color c = coloredImage.GetPixel(p.X, p.Y);
                rValue[currentClass][currentClick] = c.R;
                gValue[currentClass][currentClick] = c.G;
                bValue[currentClass][currentClick] = c.B;
                currentClick++;
                if (currentClick == numOfClicks)
                {
                    rMu[currentClass] = calculateMu(rValue[currentClass]);
                    gMu[currentClass] = calculateMu(gValue[currentClass]);
                    bMu[currentClass] = calculateMu(bValue[currentClass]);
                    rSigma[currentClass] = Math.Sqrt(calculateVariance(rValue[currentClass], rMu[currentClass]));
                    gSigma[currentClass] = Math.Sqrt(calculateVariance(gValue[currentClass], gMu[currentClass]));
                    bSigma[currentClass] = Math.Sqrt(calculateVariance(bValue[currentClass], bMu[currentClass]));
                    MuAndSigmaGridView.Rows[currentClass].Cells[1].Value = rMu[currentClass];
                    MuAndSigmaGridView.Rows[currentClass].Cells[2].Value = rSigma[currentClass];
                    MuAndSigmaGridView.Rows[currentClass].Cells[3].Value = gMu[currentClass];
                    MuAndSigmaGridView.Rows[currentClass].Cells[4].Value = gSigma[currentClass];
                    MuAndSigmaGridView.Rows[currentClass].Cells[5].Value = bMu[currentClass];
                    MuAndSigmaGridView.Rows[currentClass].Cells[6].Value = bSigma[currentClass];
                    currentClick = 0;
                    currentClass++;
                }
            
            }
        }

        private void ClassifyColoredUploadedButton_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 1; i <= numOfClasses; i++)
                {
                    for (int j = 1; j <= numOfClasses + 1; j++)
                    {
                        string s = LambdaLoadedGridView.Rows[i - 1].Cells[j].Value.ToString();
                        lambda[i - 1, j - 1] = double.Parse(s);
                    }
                }
            }
            catch(Exception)
            {
            
            }
           
            try
            {
                if (currentClass < numOfClasses)
                    throw new Exception();
                pixelClassifier = new MultipleFeaturesPixelClassifier(numOfClasses, rMu, gMu, bMu, rSigma, gSigma, bSigma, true);
                ClassifiedUploadedPictureBox.Image = pixelClassifier.classifyColoredImage(coloredImage, lambda);
            }
            catch(Exception)
            { 
            
            }
        }

        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All Picture Files |*.bmp;*.jpg;*.jpeg;*.jpe;*.png;|All Files (*.*)|*.*";
            dialog.Title = "Open an Image";
            dialog.CheckPathExists = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ClassifiedUploadedPictureBox.Image.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        
       

    }
}
