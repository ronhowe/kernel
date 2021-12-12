using ClassLibrary1.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject2_NoAuth
{
    [TestClass]
    public class Test1Base
    {
        [TestInitialize()]
        public async Task TestInitialize()
        {
            var stackTrace = new StackTrace(true);
            var fileName = stackTrace.GetFrame(0).GetFileName();
            Tag.How(fileName);

            await Task.Run(() => Tag.Where("TestInitialize"));
        }
    }
}