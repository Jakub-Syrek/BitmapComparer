using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTestProject1  
{
    [TestClass]
    public class UnitTest12
    {
        [TestMethod]
        public static async  Task<OutputWrapper> TestMethodAsync(dynamic x, dynamic y)
        {
            var result = Task.Run(() => UnitTestProject1.ConverterImg.CompareWithTolerance(x, y, 3, 0.5f));
            var zz = await result;
            if (!(zz.Equals(null)))
            {

                return zz;
            }
            else
            {
                return null;
            }
        }
    }
}
