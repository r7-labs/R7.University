//
//  IEduProgramProfile.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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
    public interface IEduProgramProfile: ITrackableEntity
    {
        int EduProgramProfileID { get; }

        int EduProgramID { get; }

        int EduLevelId { get; }

        string ProfileCode { get; }

        string ProfileTitle { get; }

        string Languages { get; }

        DateTime? AccreditedToDate { get; }

        DateTime? CommunityAccreditedToDate { get; }

        DateTime? StartDate { get; }

        DateTime? EndDate { get; }

        EduProgramInfo EduProgram { get; }

        EduLevelInfo EduLevel { get; }

        ICollection<EduProgramProfileFormInfo> EduProgramProfileForms { get; }

        ICollection<DocumentInfo> Documents { get; }

        ICollection<EduProgramDivisionInfo> Divisions { get; }
    }

    public interface IEduProgramProfileWritable: IEduProgramProfile, ITrackableEntityWritable
    {
        new int EduProgramProfileID { get; set; }

        new int EduProgramID { get; set; }

        new int EduLevelId { get; set; }

        new string ProfileCode { get; set; }

        new string ProfileTitle { get; set; }

        new string Languages { get; set; }

        new DateTime? AccreditedToDate { get; set; }

        new DateTime? CommunityAccreditedToDate { get; set; }

        new DateTime? StartDate { get; set; }

        new DateTime? EndDate { get; set; }

        new EduProgramInfo EduProgram { get; set; }

        new EduLevelInfo EduLevel { get; set; }

        new ICollection<EduProgramProfileFormInfo> EduProgramProfileForms { get; set; }

        new ICollection<DocumentInfo> Documents { get; set; }

        new ICollection<EduProgramDivisionInfo> Divisions { get; set; }
    }
}

