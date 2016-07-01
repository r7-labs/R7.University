//
//  DataRepositoryTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using R7.University.Tests.Models;
using Xunit;

namespace R7.University.Tests.Data
{
    public class DataRepositoryTests
    {
        [Fact]
        public void DataRepositoryTest ()
        {
            var repository = new TestDataRepository ();
            repository.Query<TestEntity> ().ToList ();
            repository.Dispose ();

            // repository call after dispose should throw exception
            Assert.Throws (typeof (InvalidOperationException), () => repository.Query<TestEntity> ().ToList ());
        }

        [Fact]
        public void QueryOneTest ()
        {
            using (var repository = new TestDataRepository ()) {
                repository.Add<TestEntity> (new TestEntity { Id = 1, Title = "Hello, world!" });
                repository.Add<TestEntity> (new TestEntity { Id = 2, Title = "Hello again!" });

                Assert.Equal (2, repository.QueryOne<TestEntity> (e => e.Id == 2).Single ().Id);
            }
        }
    }
}

