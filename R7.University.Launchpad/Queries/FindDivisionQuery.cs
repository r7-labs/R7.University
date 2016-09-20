//
//  FindDivisionQuery.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad.Queries
{
    internal class FindDivisionQuery: QueryBase
    {
        public FindDivisionQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<DivisionInfo> List (string search)
        {
            // REVIEW: Cannot set comparison options
            return ((search != null)
                ? ModelContext.Query<DivisionInfo> ()
                    .Where (d => d.Title.Contains (search) 
                        || d.ShortTitle.Contains (search)
                        || d.Location.Contains (search)
                        || d.Phone.Contains (search)
                        || d.Fax.Contains (search)
                        || d.Email.Contains (search)
                        || d.SecondaryEmail.Contains (search)
                        || d.WebSite.Contains (search)
                        || d.WorkingHours.Contains (search))
                : ModelContext.Query<DivisionInfo> ()
            ).ToList ();

        }
    }
}

