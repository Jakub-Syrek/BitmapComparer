using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_imageComparer
{
    public interface IOutputWrapper
    {
        OutputWrapper ReturnSortedColors(OutputWrapper _outputWrapper);
    }

    public class OutputWrapper : IDisposable
    {
        private Bitmap _bitmap;
        private dynamic _numeric1;
        private dynamic _numeric2;
        private dynamic _numeric3;
        private bool[,] _boolArr;
        private int[,] _intArr;
        private List<Color> _colors;
        private List<string> _colorsSorted;
        private List<Point> _points;
        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }       

        public dynamic Percentage
        {
            get { return _numeric1; }
            set { _numeric1 = value; }
        }
        public dynamic DifferentPixels
        {
            get { return _numeric2; }
            set { _numeric2 = value; }
        }
        public dynamic AllPixels
        {
            get { return _numeric3; }
            set { _numeric3 = value; }
        }
        public bool[,] BoolArr
        {
            get { return _boolArr; }
            set { _boolArr = value; }
        }
        public int[,] IntArr
        {
            get { return _intArr; }
            set { _intArr = value; }
        }
        public List<Color> Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }
        public List<string> ColorsSorted
        {
            get { return _colorsSorted; }
            set { _colorsSorted = value; }
        }
        public List<Point> PointsList
        {
            get { return _points; }
            set { _points = value; }
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



        public void Dispose()
        {

            _bitmap = null;
            _numeric1 = null;
            _numeric2 = null;
            _numeric3 = null;
            _boolArr = null;
            _intArr = null;
            _colors = null;
            ColorsSorted = null;
        }
    }
}
