using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_imageComparer
{
    public class ConverterNonStatic
    {
        public ICompare comparer;
        public ICompareWithTolerance comparer1;
        public IReturnSortedColors comparer2;
        public ConverterNonStatic(ICompare comparer)
        {
            this.comparer = comparer;
        }
        public OutputWrapper Compare(Bitmap img1, Bitmap img2)
        {
            return comparer.Compare(img1, img2);
        }
        public ConverterNonStatic(ICompareWithTolerance comparer1)
        {
            this.comparer1 = comparer1;
        }
        public OutputWrapper CompareWithTolerance(Bitmap img1, Bitmap img2, int areaRadius, int percentageOfTruth)
        {
            return comparer1.CompareWithTolerance( img1,  img2,  areaRadius,  percentageOfTruth);
        }
        public ConverterNonStatic(IReturnSortedColors comparer2)
        {
            this.comparer2 = comparer2;
        }
        public OutputWrapper ReturnSortedColors(OutputWrapper _outputWrapper)
        {
            return comparer2.ReturnSortedColors(_outputWrapper);
        }
    }
}
