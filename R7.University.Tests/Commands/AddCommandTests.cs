//
//  AddCommandTests.cs
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
using System.Collections.Generic;
using System.Linq;
using R7.University.Commands;
using R7.University.Models;
using R7.University.Tests.Models;
using R7.University.Tests.Security;
using Xunit;

namespace R7.University.Tests.Commands
{
    public class AddCommandTests
    {
        [Theory]
        [MemberData (nameof (TestData))]
        public void AddCommandTest (bool isAdmin)
        {
            using (var modelContext = new TestModelContext ()) {
                var userId = 100;
                var now = DateTime.Now;
                var entity = new DivisionInfo { DivisionID = 1 };
                var securityContext = new TestSecurityContext (userId, isAdmin);
                var command = new AddCommand<DivisionInfo> (modelContext, securityContext);

                command.Add (entity, now);

                var entityLoaded = modelContext
                    .QueryOne<DivisionInfo> (d => d.DivisionID == entity.DivisionID)
                    .SingleOrDefault ();
                
                Assert.Equal (isAdmin, null != entityLoaded);

                if (entityLoaded != null) {
                    Assert.Equal (now, entityLoaded.CreatedOnDate);
                    Assert.Equal (now, entityLoaded.LastModifiedOnDate);
                    Assert.Equal (userId, entityLoaded.CreatedByUserID);
                    Assert.Equal (userId, entityLoaded.LastModifiedByUserID);
                }
            }
        }

        public static IEnumerable<object []> TestData
        {
            get {
                yield return new object [] { true };
                yield return new object [] { false };
            }
        }
    }
}
