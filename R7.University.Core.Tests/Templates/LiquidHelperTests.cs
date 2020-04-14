using System.Collections.Generic;
using System.Linq;
using R7.University.Core.Templates;
using Xunit;

namespace R7.University.Core.Tests.Templates
{
    public class LiquidHelperTests
    {
        [Theory]
        [MemberData (nameof (GetTemplates))]
        public void GetLiquidObjectsTest (string template, string [] liquidObjects)
        {
            var liquidObjectsActual = LiquidHelper.GetLiquidObjects (template).ToList ();

            Assert.Equal (liquidObjects.Length, liquidObjectsActual.Count ());

            for (var i = 0; i < liquidObjects.Length; i++) {
                Assert.Equal (liquidObjects [i], LiquidHelper.UnwrapLiquidObject (liquidObjectsActual [i]));
            }
        }
         
        public static IEnumerable<object []> GetTemplates ()
        {
            yield return new object [] { "This is a template with {{one}}  object.", new string [] { "one" } };
            yield return new object [] { "This is a template with {{one}}, {{two}} Liquid objects.", new string [] { "one", "two" } };
            yield return new object [] { "This is a template with {{two}}{{adjacent}} Liquid objects.", new string [] { "two", "adjacent" } };
            yield return new object [] { "This is a template with {{one}} Liquid object and a {syntax error}}", new string [] {"one"} };
        }
    }
}
