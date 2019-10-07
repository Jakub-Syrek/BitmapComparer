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
                                var pixel1 = img1.GetPixel(i, j);
                                var pixel2 = img2.GetPixel(i, j);
                                if (!(pixel1.Equals(pixel2)))
                                {
                                    differentPixels++;
                                }
                                else
                                {
                                    allPixels++;
                                }
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

            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        var pixel1 = img1.GetPixel(i, j);
                        var pixel2 = img2.GetPixel(i, j);

                        if (!(pixel1.Equals(pixel2)))
                        {
                            differentPixels++;

                        }
                        else
                        {
                            allPixels++;
                        }
                    }                    
                }
            }
            int a = allPixels - differentPixels;
            var b = Decimal.Divide(a, allPixels);
            var c = b * 100;
            var d = Decimal.Round(c, 4);
            textBox1.AppendText($"{d} % simmilarity{Environment.NewLine}out of {allPixels} pixels {differentPixels} are different{Environment.NewLine}");
            textBox1.AppendText($"Sequential execution in {stopwatch.Elapsed}{Environment.NewLine}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 0;
            
            Bitmap img1 = new Bitmap(filePath1);
            Bitmap img2 = new Bitmap(filePath2);

            int radius = 0;
            

            if (comboBox1.SelectedItem == null)
            {
                radius = 1;
            }
            else
            {
                radius = Convert.ToInt32(comboBox1.SelectedItem);
            }
            OutputWrapper outputWrapper = Converter.Compare(img1, img2, radius);

            var rowCountY = outputWrapper.BoolArr.GetLength(1);
            var rowLengthX = outputWrapper.BoolArr.GetLength(0);

            for (int i = 0; i < rowLengthX; i++)
            {                
                dataGridView1.Columns.Add($"{i}", $"{i}");
                dataGridView1.Columns[i].Width = i.ToString().Length * 10;               
            }

            for (int i = 0; i < outputWrapper.BoolArr.GetLength(1); i++)
            {
                var row = new DataGridViewRow();

                for (int j = 0; j < outputWrapper.BoolArr.GetLength(0); j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = Convert.ToInt16(outputWrapper.BoolArr[j, i])
                    });
                }
                dataGridView1.Rows.Add(row);

            }
            dataGridView1.Refresh();            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int radius = 0;
            float percentageOfTruth = 0;            

            if (comboBox1.SelectedItem == null)
            {
                radius = 1;
            }
            else
            {
                radius = Convert.ToInt32(comboBox1.SelectedItem);
            }
            if (comboBox2.SelectedItem == null)
            {
                percentageOfTruth = 0.01f;
            }
            else
            {
                percentageOfTruth = float.Parse(comboBox2.SelectedItem.ToString());
            }
            

            dataGridView1.ColumnCount = 0;
            Bitmap img1 = new Bitmap(filePath1);
            Bitmap img2 = new Bitmap(filePath2);

            OutputWrapper outputWrapper = Converter.CompareWithTolerance(
                img1: img1,
                img2: img2,
                areaRadius: radius,
                percentageOfTruth: percentageOfTruth /100);


            pictureBox3.Image = outputWrapper.Bitmap;


            var rowCountY = outputWrapper.BoolArr.GetLength(1);
            var rowLengthX = outputWrapper.BoolArr.GetLength(0);

            for (int i = 0; i < rowLengthX; i++)
            {
                dataGridView1.Columns.Add($"{i}", $"{i}");
                dataGridView1.Columns[i].Width = i.ToString().Length * 10;
            }

            for (int i = 0; i < outputWrapper.BoolArr.GetLength(1); i++)
            {
                var row = new DataGridViewRow();

                for (int j = 0; j < outputWrapper.BoolArr.GetLength(0); j++)
                {
                    row.Cells.Add(new DataGridViewTextBoxCell()
                    {
                        Value = Convert.ToInt16(outputWrapper.BoolArr[j, i])
                    });
                }
                dataGridView1.Rows.Add(row);

            }
            dataGridView1.Refresh();
            textBox1.AppendText($"Pixel radius = {radius} with percentage of true valued cells:{percentageOfTruth}%{Environment.NewLine}");
            //textBox1.AppendText($"{outputWrapper.Numeric1} % simmilarity{Environment.NewLine}");
            textBox1.AppendText($"{outputWrapper.Percentage} % simmilarity{Environment.NewLine}out of {outputWrapper.AllPixels} pixels {outputWrapper.DifferentPixels} are different{Environment.NewLine}");
            //textBox1.AppendText($"{outputWrapper.Colors.Count} different colors detected in picture{Environment.NewLine}");
            
            //outputWrapper.ColorsSorted = Converter.ReturnSortedColors(outputWrapper).ColorsSorted;
            //outputWrapper.ColorsSorted = OutputWrapper.ReturnSortedColors(outputWrapper).ColorsSorted;
            //outputWrapper = new ConverterDependency().ReturnSortedColors(outputWrapper);
           
            //foreach (var item in outputWrapper.ColorsSorted)
            //{                
            //    textBox1.AppendText($"{item}{Environment.NewLine}");
            //}


            textBox1.AppendText($"Parallel execution in {stopwatch.Elapsed}{Environment.NewLine}");
        }

        
    }
}
