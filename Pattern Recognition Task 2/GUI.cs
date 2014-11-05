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
        private Bitmap source;
        GenImage.Slice[] slices;
        private int H, W;
        private int currentClass;

        public GUI()
        {
            InitializeComponent();
            currentClass = 0;
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

            slices = new GenImage.Slice[4];     
            comboBox1.SelectedIndex = 0;
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
            RMuTextBox.Text = slices[comboBox1.SelectedIndex].R_mu.ToString();
            RSigmaTextBox.Text = slices[comboBox1.SelectedIndex].R_sigma.ToString();
            GMuTextBox.Text = slices[comboBox1.SelectedIndex].G_mu.ToString();
            GSigmaTextBox.Text = slices[comboBox1.SelectedIndex].G_sigma.ToString();
            BMuTextBox.Text = slices[comboBox1.SelectedIndex].B_mu.ToString();
            BSigmaTextBox.Text = slices[comboBox1.SelectedIndex].B_sigma.ToString();
        }

   

        private void RSigmaTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                slices[comboBox1.SelectedIndex].R_sigma = double.Parse(RSigmaTextBox.Text);

            }
            catch (Exception)
            { }

        }


        private void GSigmaTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                slices[comboBox1.SelectedIndex].G_sigma = double.Parse(GSigmaTextBox.Text);
            }
            catch (Exception)
            { }
        }

     

        private void BSigmaTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                slices[comboBox1.SelectedIndex].B_sigma = double.Parse(BSigmaTextBox.Text);
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
                //source = GenImage.GenImg(slices, Height, Width);
                generatedImagePictureBox.Image = GenImage.GenImg(slices, H, W);
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
            p.Y = (int)((me.Location.Y) * ((double)greyScaleImage.Height) / ((double)greyScalePictureBox.Width));
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

        private void RMuTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                slices[comboBox1.SelectedIndex].R_mu = double.Parse(RMuTextBox.Text);
            }
            catch (Exception)
            { }
        }

        private void GMuTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                slices[comboBox1.SelectedIndex].G_mu = double.Parse(GMuTextBox.Text);
            }
            catch (Exception)
            { }
        }

        private void BMuTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                slices[comboBox1.SelectedIndex].B_mu = double.Parse(BMuTextBox.Text);
            }
            catch (Exception)
            { }

        }

    

    }
}
