//
//  EduVolumeQuery.cs
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
using R7.University.EduProgramProfiles.Models;
using R7.University.Models;
using System.Linq;

namespace R7.University.EduProgramProfiles.Queries
{
    public class EduVolumeQuery
    {
        protected readonly IModelContext ModelContext;

        public EduVolumeQuery (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public IEnumerable<EduVolumeInfo> ListByDivisionAndEduLevels (IEnumerable<int> eduLevelIds,
                                                                      int? divisionId,
                                                                      DivisionLevel divisionLevel)
        {
            // TODO: Implement filtering
            // TODO: Filter out year of admission
            return ModelContext.Query<EduVolumeInfo> ()
                               .Include (ev => ev.EduProgramProfileFormYear)
                               .Include (ev => ev.EduProgramProfileFormYear.EduProgramProfile)
                               .Include (ev => ev.EduProgramProfileFormYear.EduProgramProfile.EduLevel)
                               .Include (ev => ev.EduProgramProfileFormYear.EduProgramProfile.EduProgram)
                               .Include (ev => ev.EduProgramProfileFormYear.EduForm)
                               .Include (ev => ev.EduProgramProfileFormYear.Year).ToList ();
        }
    }
}
