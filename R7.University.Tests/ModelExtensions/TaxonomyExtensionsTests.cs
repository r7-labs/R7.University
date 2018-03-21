//
//  TaxonomyExtensionsTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018 Roman M. Yagodin
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

using R7.University.ModelExtensions;
using Xunit;

namespace R7.University.Tests.ModelExtensions
{
    public class TaxonomyExtensionsTests
    {
        [Fact]
        public void GetSafeTermNameTest ()
        {
            Assert.Equal ("FNS", TaxonomyExtensions.GetSafeTermName ("FNS", "Faculty of Natural Sciencies"));
            Assert.Equal ("Natural Sciencies Faculty", TaxonomyExtensions.GetSafeTermName (string.Empty, "\"Natural Sciencies\" Faculty"));
        }
    }
}
