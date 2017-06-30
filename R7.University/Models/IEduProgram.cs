//
//  IEduProgram.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

namespace R7.University.Models
{
    public interface IEduProgram: ITrackableEntity
    {
        int EduProgramID { get; set; }

        int EduLevelID { get; set; }

        int? DivisionId { get; set; }

        string Code { get; set; }

        string Title { get; set; }

        string Generation { get; set; }

        string HomePage { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }

        EduLevelInfo EduLevel { get; set; }

        DivisionInfo Division { get; set; }

        ICollection<DocumentInfo> Documents { get; set; }

        ICollection<EduProgramProfileInfo> EduProgramProfiles { get; set; }
    }
}
