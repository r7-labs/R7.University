//
//  DeleteCommandTests.cs
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

using System.Collections.Generic;
using System.Linq;
using R7.University.Commands;
using R7.University.Models;
using R7.University.Tests.Models;
using R7.University.Tests.Security;
using Xunit;

namespace R7.University.Tests.Commands
{
    public class DeleteCommandTests
    {
        [Theory]
        [MemberData (nameof (TestData))]
        public void DeleteCommandTest (bool isAdmin)
        {
            using (var modelContext = new TestModelContext ()) {
                var entity = new DivisionInfo { DivisionID = 1 };
                modelContext.Add (entity);

                var securityContext = new TestSecurityContext (1, isAdmin);
                var command = new DeleteCommand<DivisionInfo> (modelContext, securityContext);

                command.Delete (entity);

                Assert.Equal (isAdmin, null == modelContext
                              .QueryWhere<DivisionInfo> (d => d.DivisionID == entity.DivisionID)
                              .SingleOrDefault ());
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
