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
        
        public static void MergeImages(Image img1,Image img2,string outputFilePath,
                                Image img3 = null, //optional
                                Image img4 = null, //optional
                                Image img5 = null) //optional
        {           
            using (Image image1 = img1)
            using (Image image2 = img2)
            using (Image image3 = img3 ?? null)
            using (Image image4 = img4 ?? null)
            using (Image image5 = img5 ?? null)
            {
                int image3Width = (img3 == null) ? 0 : img3.Width;
                int image4Width = (img4 == null) ? 0 : img4.Width;
                int image5Width = (img5 == null) ? 0 : img5.Width;
                int image3Height = (img3 == null) ? 0 : img3.Height;
                int image4Height = (img4 == null) ? 0 : img4.Height;
                int image5Height = (img5 == null) ? 0 : img5.Height;
                int width = image1.Width + image2.Width + image3Width + image4Width + image5Width;
                int height = Math.Max(Math.Max(Math.Max(img1.Height, img2.Height), (Math.Max(image3Height, image4Height))), image5Height);
                using (Bitmap imageOut3 = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(imageOut3))
                    {
                        int x = 0;
                        g.Clear(Color.Black);
                        g.DrawImage(img1, new Point(x, 0));
                        g.DrawImage(img2, new Point(x = x + img1.Width, 0));
                        if (img3 != null) g.DrawImage(img3, new Point(x = x + img2.Width, 0));
                        if (img4 != null) g.DrawImage(img4, new Point(x = x + image3Width, 0));
                        if (img5 != null) g.DrawImage(img5, new Point(x = x + image4Width, 0));
                    }
                    imageOut3.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);                    
                }
            }
        }
        public static void Dispose<T>(this List<T> list) where T : IDisposable
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Dispose();
            }
            list.Clear();
        }
        internal static void MergeImagesList(List<Image> imgArr, string outputFilePath, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            List<Image> images = imgArr;
            int width = 0;
            int height = 0;
            foreach (Image picture in images)
            {
                width += picture.Width;
                height = Math.Max(height, picture.Height);
            }
            using (Bitmap imageOut3 = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(imageOut3))
                {
                    g.Clear(Color.Black);
                    int currentX = 0;
                    for (int i = 0; i < images.Count; i++)
                    {
                        g.DrawImage(images[i], new Point(currentX, 0));
                        currentX += images[i].Width;
                    }
                    imageOut3.Save(outputFilePath, imageFormat);
                }
            }
            images.Dispose();            
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

