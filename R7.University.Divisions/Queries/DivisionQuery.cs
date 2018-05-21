//
//  DivisionQuery.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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

using System.Linq;
using System.Collections.Generic;
using R7.University.Models;
using R7.University.Queries;
using R7.Dnn.Extensions.Models;

namespace R7.University.Divisions.Queries
{
    internal class DivisionQuery: QueryBase
    {
        public DivisionQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<DivisionInfo> ListExcept (int? divisionId)
        {
            return ModelContext.Query<DivisionInfo> ()
                .Where (d => (divisionId == null || divisionId != d.DivisionID))
                .ToList ();
        }

        public DivisionInfo SingleOrDefault (int divisionId)
        {
            return ModelContext.QueryOne<DivisionInfo> (d => d.DivisionID == divisionId)
                .Include2 (d => d.SubDivisions)
                .SingleOrDefault ();
        }
    }
}

