//
//  OccupiedPositionExtensionsTests.cs
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
    public class OccupiedPositionExtensionsTests
    {
        [Fact]
        public void GroupByDivisionTest ()
        {
            var gops = GetOccupiedPositions ().GroupByDivision ().ToList ();

            Assert.Equal (2, gops.Count);

            Assert.Equal ("Chief, Consultant", gops [0].Title);
            Assert.Equal ("Manager (main)", gops [1].Title);
        }

        protected IEnumerable<OccupiedPositionInfo> GetOccupiedPositions ()
        {
            var division1 = new DivisionInfo { DivisionID = 1 };
            var division2 = new DivisionInfo { DivisionID = 2 };

            var position11 = new PositionInfo { PositionID = 11, Title = "Director", ShortTitle = "Chief" };
            var position12 = new PositionInfo { PositionID = 12, Title = "Manager" };
            var position13 = new PositionInfo { PositionID = 13, Title = "Consultant" };

            return new List<OccupiedPositionInfo> {
                new OccupiedPositionInfo {
                    DivisionID = 1,
                    Division = division1,
                    PositionID = 11,
                    Position = position11
                },

                new OccupiedPositionInfo {
                    DivisionID = 2,
                    Division = division2,
                    PositionID = 12,
                    Position = position12,
                    TitleSuffix = "(main)"
                },

                new OccupiedPositionInfo {
                    DivisionID = 1,
                    Division = division1,
                    PositionID = 13,
                    Position = position13
                }
            };
        }
    }
}
