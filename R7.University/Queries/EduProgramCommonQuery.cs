//
//  EduProgramCommonQuery.cs
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
using System.Collections.Generic;
using System.Linq;
using R7.University.Models;

namespace R7.University.Queries
{
    public class EduProgramCommonQuery: QueryBase
    {
        public EduProgramCommonQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<EduProgramInfo> ListByEduLevel (int eduLevelId)
        {
            return ModelContext.Query<EduProgramInfo> ()
                .Where (ep => ep.EduLevelID == eduLevelId)
                .OrderBy (ep => ep.Code)
                .ThenBy (ep => ep.Title)
                .ToList ();
        }
    }
}

