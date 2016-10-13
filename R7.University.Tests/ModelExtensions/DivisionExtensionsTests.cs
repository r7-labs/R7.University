//
//  DivisionExtensionsTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Linq;
using R7.University.ModelExtensions;
using R7.University.Models;
using Xunit;

namespace R7.University.Tests.ModelExtensions
{
    public class DivisionExtensionsTests
    {
        [Fact]
        public void CalculateLevelAndPathTest ()
        {
            var root1 = new DivisionInfo { DivisionID = 1, Title = "Root1" };
            var root1_child1 = new DivisionInfo { DivisionID = 2, ParentDivisionID = 1, Title = "Root1.Child1" };
            var root1_child2 = new DivisionInfo { DivisionID = 3, ParentDivisionID = 2, Title = "Root1.Child2" };
            var root1_child1_child1 = new DivisionInfo
            {
                DivisionID = 4,
                ParentDivisionID = 1,
                Title = "Root1.Child1.Child1"
            };

            root1.SubDivisions = new List<DivisionInfo> { root1_child1, root1_child2 };
            root1_child1.SubDivisions = new List<DivisionInfo> { root1_child1_child1 };

            var divisions = new List<DivisionInfo> {
                root1, root1_child1, root1_child2, root1_child1_child1
            };

            var calculatedDivisions = divisions.CalculateLevelAndPath ();

            // check paths
            Assert.Equal ("/0000000001", calculatedDivisions.Single (cd => cd.DivisionID == 1).Path);
            Assert.Equal ("/0000000001/0000000002", calculatedDivisions.Single (cd => cd.DivisionID == 2).Path);
            Assert.Equal ("/0000000001/0000000003", calculatedDivisions.Single (cd => cd.DivisionID == 3).Path);
            Assert.Equal ("/0000000001/0000000002/0000000004", calculatedDivisions.Single (cd => cd.DivisionID == 4).Path);

            // check level
            Assert.Equal (0, calculatedDivisions.Single (cd => cd.DivisionID == 1).Level);
            Assert.Equal (2, calculatedDivisions.Single (cd => cd.DivisionID == 4).Level);
        }
    }
}
