using Leadtools;
using Leadtools.Codecs;
using Leadtools.ImageProcessing.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int differentPixels = 0;
            int allPixels = 0;

            var img1 = new Bitmap(filePath1);
            var img2 = new Bitmap(filePath2);

            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                ParallelLoopResult X = Parallel.For(0, img1.Width, i =>
                {
                      lock (img1)
                      {
                          for (int j = 0; j < img1.Height; j++)
                          {
                                  if (img1.GetPixel(i, j) != img2.GetPixel(i, j))
                                  {
                                      differentPixels++;
                                      break;
                                  }
                                  allPixels++;                           
                          }
                      }
                  
                });                
            }
                
            int a = allPixels - differentPixels;
            var b = Decimal.Divide(a, allPixels);
            var c = b * 100;
            var d = Decimal.Round(c, 4);
            textBox1.AppendText($"{d} % simmilarity{Environment.NewLine}out of {allPixels} pixels {differentPixels} are different{Environment.NewLine}");
            textBox1.AppendText($"Parallel execution in {stopwatch.Elapsed}{Environment.NewLine}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                }
            }
            int a = allPixels - differentPixels;
            var b = Decimal.Divide(a, allPixels);
            var c = b * 100;
            var d = Decimal.Round(c, 4);
            textBox2.AppendText($"{d} % simmilarity{Environment.NewLine}out of {allPixels} pixels {differentPixels} are different{Environment.NewLine}");
            textBox2.AppendText($"Sequential execution in {stopwatch.Elapsed}{Environment.NewLine}");
        }
    }
}
