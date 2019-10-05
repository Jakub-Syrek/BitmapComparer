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
        private dynamic _numeric1;
        private dynamic _numeric2;
        private dynamic _numeric3;
        private bool[,] _boolArr;
        private int[,] _intArr;

        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }       

        public dynamic Numeric1
        {
            get { return _numeric1; }
            set { _numeric1 = value; }
        }
        public dynamic Numeric2
        {
            get { return _numeric2; }
            set { _numeric2 = value; }
        }
        public dynamic Numeric3
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

        public void Dispose()
        {

            _bitmap = null;
            _numeric1 = null;
            _numeric2 = null;
            _numeric3 = null;
            _boolArr = null;
            _intArr = null;
        }
    }
}
