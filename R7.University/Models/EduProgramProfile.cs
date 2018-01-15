//
//  EduProgramProfile.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
    public interface IEduProgramProfile: ITrackableEntity, IPublishableEntity
    {
        int EduProgramProfileID { get; }

        int EduProgramID { get; }

        int EduLevelId { get; }

        string ProfileCode { get; }

        string ProfileTitle { get; }

        string Languages { get; }

        bool IsAdopted { get; }

        bool ELearning { get; }

        bool DistanceEducation { get; }

        DateTime? AccreditedToDate { get; }

        DateTime? CommunityAccreditedToDate { get; }

        EduProgramInfo EduProgram { get; }

        EduLevelInfo EduLevel { get; }

        ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears { get; }

        ICollection<DocumentInfo> Documents { get; }

        ICollection<EduProgramDivisionInfo> Divisions { get; }
    }

    public interface IEduProgramProfileWritable: IEduProgramProfile, ITrackableEntityWritable, IPublishableEntityWritable
    {
        new int EduProgramProfileID { get; set; }

        new int EduProgramID { get; set; }

        new int EduLevelId { get; set; }

        new string ProfileCode { get; set; }

        new string ProfileTitle { get; set; }

        new string Languages { get; set; }

        new bool IsAdopted { get; set; }

        new bool ELearning { get; set; }

        new bool DistanceEducation { get; set; }

        new DateTime? AccreditedToDate { get; set; }

        new DateTime? CommunityAccreditedToDate { get; set; }

        new EduProgramInfo EduProgram { get; set; }

        new EduLevelInfo EduLevel { get; set; }

        new ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears { get; set; }

        new ICollection<DocumentInfo> Documents { get; set; }

        new ICollection<EduProgramDivisionInfo> Divisions { get; set; }
    }

    public class EduProgramProfileInfo: IEduProgramProfileWritable
    {
        public int EduProgramProfileID { get; set; }

        public int EduProgramID { get; set; }

        public int EduLevelId { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public string Languages { get; set; }

        public bool IsAdopted { get; set; }

        public bool ELearning { get; set; }

        public bool DistanceEducation { get; set; }

        public DateTime? AccreditedToDate { get; set; }

        public DateTime? CommunityAccreditedToDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserID { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual EduProgramInfo EduProgram { get; set; }

        public virtual EduLevelInfo EduLevel { get; set; }

        public virtual ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears { get; set; } = new HashSet<EduProgramProfileFormYearInfo> ();

        public virtual ICollection<DocumentInfo> Documents { get; set; } = new HashSet<DocumentInfo> ();

        public virtual ICollection<EduProgramDivisionInfo> Divisions { get; set; } = new HashSet<EduProgramDivisionInfo> ();
    }
}
