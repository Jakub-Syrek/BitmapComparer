using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_imageComparer
{
    public interface ICompare
    {
        OutputWrapper Compare(Bitmap img1, Bitmap img2);
    }
    public interface ICompareWithTolerance
    {
        OutputWrapper CompareWithTolerance(Bitmap img1, Bitmap img2, int areaRadius, int percentageOfTruth);
    }
    public interface IReturnSortedColors
    {
        OutputWrapper ReturnSortedColors(OutputWrapper _outputWrapper);
    }
    class ConverterDependency : ICompare , ICompareWithTolerance , IReturnSortedColors, IDisposable
    {
        public OutputWrapper Compare(Bitmap img1, Bitmap img2)
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
            outputWrapper.Percentage = Decimal.Round(c, 4);
            outputWrapper.DifferentPixels = differentPixels;
            outputWrapper.AllPixels = allPixels;
            return outputWrapper;
        }

        public OutputWrapper CompareWithTolerance(Bitmap img1, Bitmap img2, int areaRadius, int percentageOfTruth)
        {
            if (areaRadius == 0) areaRadius = 1;

            int differentPixels = 0;
            int allPixels = 0;
            OutputWrapper reducedDifferences = new OutputWrapper();
            using (OutputWrapper originalDifferences = new OutputWrapper ())
            using (ConverterDependency converterDependency = new ConverterDependency())
            {                
                converterDependency.Compare(img1, img2);
                reducedDifferences.Bitmap = new Bitmap(img1);
                reducedDifferences.BoolArr = new bool[originalDifferences.BoolArr.GetLength(0), originalDifferences.BoolArr.GetLength(1)];
                reducedDifferences.Colors = new List<Color>();
                ParallelLoopResult Y = Parallel.For(areaRadius, originalDifferences.BoolArr.GetLength(0) - areaRadius, i =>
                {
                    Parallel.For(areaRadius, (originalDifferences.BoolArr.GetLength(1) - areaRadius), j =>
                    {
                        int innerDifferentPixelsAggregated = 0;
                        int innerAllPixelsAggregated = 0;
                        lock (reducedDifferences.Bitmap)
                        {
                            if (!(reducedDifferences.Colors.Contains(reducedDifferences.Bitmap.GetPixel(i, j))))
                            {
                                Color color = reducedDifferences.Bitmap.GetPixel(i, j);
                                reducedDifferences.Colors.Add(color);

                            }

                            Parallel.For(0 - areaRadius, areaRadius, k =>
                            {
                                for (int l = 0 - areaRadius; l < areaRadius; l++)
                                {
                                    if (originalDifferences.BoolArr[i + k, j + l] == true)
                                    {
                                        innerDifferentPixelsAggregated++;
                                    }
                                    innerAllPixelsAggregated++;
                                }
                            });
                            decimal areaRadiusD = areaRadius;
                            decimal percentageOfTruthD = percentageOfTruth / 10;
                            percentageOfTruthD = percentageOfTruthD / 10;
                            decimal x = areaRadiusD * areaRadiusD * percentageOfTruthD;
                            if (Convert.ToDecimal(innerDifferentPixelsAggregated) > x)
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
            reducedDifferences.Percentage = Decimal.Round(c, 4);
            reducedDifferences.DifferentPixels = differentPixels;
            reducedDifferences.AllPixels = allPixels;
            return reducedDifferences;
        }

        public OutputWrapper ReturnSortedColors(OutputWrapper _outputWrapper)
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


        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
