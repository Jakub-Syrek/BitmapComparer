using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_imageComparer
{
    public class OutputWrapper : IDisposable
    {
        private Bitmap _bitmap;
        private dynamic _numeric;
        private bool[,] _boolArr;
        private int[,] _intArr;

        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }       

        public dynamic Numeric
        {
            get { return _numeric; }
            set { _numeric = value; }
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

        public void Dispose()
        {

            _bitmap = null;
            _numeric = null;
            _boolArr = null;
            _intArr = null;
        }
    }
}
