using System.Linq;
using R7.University.Core.Templates;
using Xunit;

namespace R7.University.Core.Tests.Templates
{
    public class LiquidHelperTests
    {
        [Fact]
        public void GetLiquidObjectsTest ()
        {
            var liquidObjects = LiquidHelper.GetLiquidObjects ("This is a {{template}} string with {{two}} Liquid objects.");

            Assert.Equal (2, liquidObjects.Count ());
            Assert.Equal ("{{template}}", liquidObjects.ToList () [0]);
            Assert.Equal ("{{two}}", liquidObjects.ToList () [1]);
        }
    }
}
