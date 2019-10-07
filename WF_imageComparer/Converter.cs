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
        public static OutputWrapper Compare(Bitmap img1, Bitmap img2, int areaRadius)
        {
            int differentPixels = 0;
            int allPixels = 0;
            OutputWrapper outputWrapper = new OutputWrapper();
            outputWrapper.BoolArr = new bool[img1.Width, img1.Height];
            outputWrapper.Bitmap = new Bitmap(img1);
            outputWrapper.PointsList = new List<Point>();
            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                ParallelLoopResult X = Parallel.For(0, img1.Width, i =>
                {
                    lock (img1)
                    {
                        for (int j = 0+ areaRadius; j < img1.Height- areaRadius; j++)
                        {
                            Color pixel1 = img1.GetPixel(i, j);
                            Color pixel2 = img2.GetPixel(i, j);

                            if (!(pixel1.Equals(pixel2)))
                            {
                                outputWrapper.BoolArr[i, j] = true;
                                outputWrapper.Bitmap.SetPixel(i, j, Color.Red);
                                differentPixels++;
                                outputWrapper.PointsList.Add(new Point(i, j));
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
            decimal a = Decimal.Divide(allPixels - differentPixels, allPixels) * 100;
            outputWrapper.Percentage = Decimal.Round(a, 4);
            outputWrapper.DifferentPixels = differentPixels;
            outputWrapper.AllPixels = allPixels;
            return outputWrapper;
        }
        public static OutputWrapper CompareWithTolerance(Bitmap img1, Bitmap img2, int areaRadius, float percentageOfTruth)
        {
            int differentPixels = 0;
            int allPixels = 0;
            OutputWrapper reducedDifferences = new OutputWrapper();            
            using (OutputWrapper originalDifferences = Compare(img1, img2, areaRadius))
            {
                reducedDifferences.Bitmap = new Bitmap(img1);
                reducedDifferences.BoolArr = new bool[originalDifferences.BoolArr.GetLength(0), originalDifferences.BoolArr.GetLength(1)];                
                reducedDifferences.PointsList = new List<Point>();
                Parallel.ForEach(originalDifferences.PointsList, point =>
                {
                    lock (reducedDifferences.Bitmap)
                    {
                        int i = point.X;
                        int j = point.Y;
                        float innerDifferentPixelsAggregated = 0;
                        float innerAllPixelsAggregated = 0;                                               
                        Parallel.For(0 - areaRadius, areaRadius, k =>
                        {                            
                            Parallel.For(0 - areaRadius, areaRadius, l =>
                            {
                                if (originalDifferences.BoolArr[i + k, j + l] == true)
                                {
                                    innerDifferentPixelsAggregated++;
                                }
                                innerAllPixelsAggregated++;
                            });
                        });
                        if ((innerDifferentPixelsAggregated / innerAllPixelsAggregated) * innerAllPixelsAggregated >= (percentageOfTruth * innerAllPixelsAggregated))
                        {
                            differentPixels++;
                            reducedDifferences.PointsList.Add(new Point(i, j));                            
                            reducedDifferences.Bitmap.SetPixel(i, j, Color.Red);
                            reducedDifferences.BoolArr[i, j] = true;
                        }
                        else
                        {
                            reducedDifferences.BoolArr[i, j] = false;
                        }                        
                    }
                });
                allPixels = originalDifferences.AllPixels;
            }            
            decimal a = Decimal.Divide(allPixels - differentPixels, allPixels)*100;            
            reducedDifferences.Percentage = Decimal.Round(a, 4);
            reducedDifferences.DifferentPixels = differentPixels;
            reducedDifferences.AllPixels = allPixels;
            return reducedDifferences;                   
        }

        public static OutputWrapper ReturnSortedColors(OutputWrapper _outputWrapper)
        {            
                List<string> lista = new List<string>();

                foreach (var item in _outputWrapper.Colors)
                {
                    lista.Add($"{item.Name}");
                }
                lista.Sort();
                OutputWrapper outputWrapper = _outputWrapper;

                outputWrapper.ColorsSorted = lista;
                return outputWrapper;            
        }
    }




}

//for (int k = 0 - areaRadius; k < areaRadius; k++)
//{
//    if (originalDifferences.BoolArr[i + k, j] == true)
//    {
//        innerDifferentPixelsAggregated++;
//    }
//    innerAllPixelsAggregated++;
//}
//for (int l = 0 - areaRadius; l < areaRadius; l++)
//{
//    if (originalDifferences.BoolArr[i, j + l] == true)
//    {
//        innerDifferentPixelsAggregated++;
//    }
//    innerAllPixelsAggregated++;
//}
//for (int m = 0 - areaRadius; m < areaRadius; m++)
//{
//    if (originalDifferences.BoolArr[i + m, j + m] == true)
//    {
//        innerDifferentPixelsAggregated++;
//    }
//    innerAllPixelsAggregated++;
//}
//for (int n = 0 - areaRadius; n < areaRadius; n++)
//{
//    if (originalDifferences.BoolArr[i - n, j - n] == true)
//    {
//        innerDifferentPixelsAggregated++;
//    }
//    innerAllPixelsAggregated++;
//}


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

