using Leadtools;
using Leadtools.Codecs;
using Leadtools.ImageProcessing.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF_imageComparer
{
    public partial class Form1 : Form
    {
        string filePath1 = string.Empty;
        string filePath2 = string.Empty;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\jaqbs\source\repos\WF_imageComparer\WF_imageComparer\Bitmaps";
                openFileDialog.Filter = "bmp files (*.bmp)|*.bmp"; //|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath1 = openFileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(filePath1);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\jaqbs\source\repos\WF_imageComparer\WF_imageComparer\Bitmaps";
                openFileDialog.Filter = "bmp files (*.bmp)|*.bmp"; //|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath2 = openFileDialog.FileName;
                    pictureBox2.Image = Image.FromFile(filePath2);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int differentPixels = 0;
            int allPixels = 0;

            var img1 = new Bitmap(filePath1);
            var img2 = new Bitmap(filePath2);

            string img1_ref;
            string img2_ref;


            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            differentPixels++;
                            break;
                        }
                        allPixels++;
                    }
                    //progressBar1.Value++;
                }
            }
            int a = allPixels - differentPixels;
            var b = Decimal.Divide(a, allPixels);
            var c = b * 100;
            textBox1.Text = $"{c} % simmilarity{Environment.NewLine}out of {allPixels} pixels {differentPixels} are different";
        }
    }
}
