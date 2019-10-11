using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF_imageComparer;

namespace WF_imageComparer
{
    public class Proxy 
    {
        private dynamic dynamicProp1;

        public dynamic MyDynamicProperty
        {
            get { return dynamicProp1; }
            set { dynamicProp1 = value; }
        }
        private dynamic dynamicProp;

        public dynamic MyDynamicProperty2
        {
            get { return dynamicProp; }
            set { dynamicProp = value; }
        }

        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources =new System.Resources.ResourceManager(typeof(Form1));
            Bitmap bitmap = (Bitmap)resources.GetObject("$this.BackgroundImage");    
        }

    }
}
