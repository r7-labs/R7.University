//
//  InterfaceInheritanceTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using Xunit;

namespace R7.University.Tests.Models
{
    public interface ITestValueReadable
    {
        string Value { get; }
    }

    public interface ITestValue:  ITestValueReadable
    {
        new string Value { get; set; }
    }

    public class TestValue: ITestValue
    {
        public string Value { get; set; }
    }

    public class InterfaceInheritanceTests
    {
        [Fact]
        public void InterfaceInheritanceTest ()
        {
            const string value1 = "Value";
            const string value2 = "Another value";

            var a = new TestValue { Value = value1 };

            Assert.Equal (value1, a.Value);
            Assert.Equal (value1, ((ITestValue) a).Value);
            Assert.Equal (value1, ((ITestValueReadable) a).Value);

            a.Value = value2;

            Assert.Equal (value2, ((ITestValueReadable) a).Value);
            Assert.Equal (value2, ((ITestValue) a).Value);
            Assert.Equal (value2, a.Value);
        }
    }
}
