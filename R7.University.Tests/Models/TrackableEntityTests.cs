//
//  TrackableEntityTests.cs
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

using System;
using R7.University.Models;
using Xunit;

namespace R7.University.Tests.Models
{
    public class TestTrackableEntity: ITrackableEntityWritable 
    {
        public int LastModifiedByUserId { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }
    }

    public class TrackableEntityTests
    {
        [Fact]
        public void TrackableEntityTest ()
        {
            const int value1 = 1;
            const int value2 = 2;

            var entity = new TestTrackableEntity { LastModifiedByUserId = value1 };

            Assert.Equal (value1, entity.LastModifiedByUserId);
            Assert.Equal (value1, ((ITrackableEntityWritable) entity).LastModifiedByUserId);
            Assert.Equal (value1, ((ITrackableEntity) entity).LastModifiedByUserId);

            entity.LastModifiedByUserId = value2;

            Assert.Equal (value2, ((ITrackableEntity) entity).LastModifiedByUserId);
            Assert.Equal (value2, ((ITrackableEntityWritable) entity).LastModifiedByUserId);
            Assert.Equal (value2, entity.LastModifiedByUserId);
        }
    }
}
