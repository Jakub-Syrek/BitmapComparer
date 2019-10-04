using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_imageComparer
{
    public static class Converter
    {
        public static OutputWrapper Compare(Bitmap img1, Bitmap img2)
        {
            int differentPixels = 0;
            int allPixels = 0;

            OutputWrapper outputWrapper = new OutputWrapper();

            outputWrapper.BoolArr = new bool[img1.Width, img1.Height];
            //outputWrapper.Bitmap = img1;

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
                                outputWrapper.BoolArr[i, j] = true;
                                //img1.SetPixel(i, j, Color.Red);
                                differentPixels++;
                                break;
                            }
                            else
                            {
                                outputWrapper.BoolArr[i, j] = false;
                            }
                            allPixels++;
                        }
                    }

                });
            }
            //outputWrapper.Bitmap = img1;
            int a = allPixels - differentPixels;
            decimal b = Decimal.Divide(a, allPixels);
            decimal c = b * 100;
            outputWrapper.Numeric = Decimal.Round(c, 4);

            return outputWrapper;
        }

        public static OutputWrapper CompareWithTolerance(Bitmap img1, Bitmap img2, int tolerance)
        {
            int differentPixels = 0;
            int allPixels = 0;
            OutputWrapper reducedDifferences = new OutputWrapper();            
            using (OutputWrapper originalDifferences = Compare(img1, img2))
            {
                reducedDifferences.Bitmap = new Bitmap(img1);
                reducedDifferences.BoolArr = new bool[originalDifferences.BoolArr.GetLength(0), originalDifferences.BoolArr.GetLength(1)];
                ParallelLoopResult Y = Parallel.For(tolerance, originalDifferences.BoolArr.GetLength(0) - tolerance, i =>
                {
                    Parallel.For(tolerance, (originalDifferences.BoolArr.GetLength(1) - tolerance), j =>
                    {
                        int counter = 0;
                        lock (reducedDifferences.Bitmap)
                        {
                            Parallel.For(0 - tolerance, tolerance, k =>
                            {
                                for (int l = 0 - tolerance; l < tolerance; l++)
                                {
                                    if (originalDifferences.BoolArr[i + k, j + l] == true)
                                    {
                                        counter++;
                                    }
                                }
                            });
                            if (counter > tolerance)
                            {
                                differentPixels++;
                                reducedDifferences.BoolArr[i, j] = true;
                                reducedDifferences.Bitmap.SetPixel(i, j, Color.Red);
                            }
                            else
                            {
                                reducedDifferences.BoolArr[i, j] = false;
                            }
                        }
                        allPixels++;
                    });
                });
            }
            int a = allPixels - differentPixels;
            decimal b = Decimal.Divide(a, allPixels);
            decimal c = b * 100;
            reducedDifferences.Numeric = Decimal.Round(c, 4);

            return reducedDifferences;                   
        }
    }
}
////for (int i = tolerance; i < originalDifferences.BoolArr.GetLength(0) - tolerance; i++)
//ParallelLoopResult X = Parallel.For(tolerance, originalDifferences.BoolArr.GetLength(0) - tolerance, i =>
//{
//    for (int j = tolerance; j < originalDifferences.BoolArr.GetLength(0) - tolerance; j++)
//    {
//        int counter = 0;
//        for (int k = 0 - tolerance; k < tolerance; k++)
//        {
//            if (originalDifferences.BoolArr[i + k, j] == true)
//            {
//                counter++;
//            }

//        }
//        for (int l = 0 - tolerance; l < tolerance; l++)
//        {
//            if (originalDifferences.BoolArr[i, j + l] == true)
//            {
//                counter++;
//            }
//        }
//        for (int m = 0 - tolerance; m < tolerance; m++)
//        {
//            if (originalDifferences.BoolArr[i + m, j + m] == true)
//            {
//                counter++;
//            }
//        }
//        for (int n = 0 - tolerance; n < tolerance; n++)
//        {
//            if (originalDifferences.BoolArr[i - n, j - n] == true)
//            {
//                counter++;
//            }
//        }
//        if (counter > tolerance)
//        {
//            differentPixels++;
//            //reducedDifferences.Bitmap.SetPixel(i, j, Color.Red);
//            reducedDifferences.BoolArr[i, j] = true;
//        }
//        else
//        {
//            reducedDifferences.BoolArr[i, j] = false;
//        }
//        allPixels++;
//    }
//});

