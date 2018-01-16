//
//  EduProgram.cs
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
    public interface IEduProgram: ITrackableEntity, IPublishableEntity
    {
        int EduProgramID { get; }

        int EduLevelID { get; }

        string Code { get; }

        string Title { get; }

        string Generation { get; }

        string HomePage { get; }

        EduLevelInfo EduLevel { get; }

        ICollection<DocumentInfo> Documents { get; }

        ICollection<EduProgramProfileInfo> EduProgramProfiles { get; }

        ICollection<EduProgramDivisionInfo> Divisions { get; }

        IScience Science { get; }
    }

    public interface IEduProgramWritable: IEduProgram, ITrackableEntityWritable, IPublishableEntityWritable
    {
        new int EduProgramID { get; set; }

        new int EduLevelID { get; set; }

        new string Code { get; set; }

        new string Title { get; set; }

        new string Generation { get; set; }

        new string HomePage { get; set; }

        new EduLevelInfo EduLevel { get; set; }

        new ICollection<DocumentInfo> Documents { get; set; }

        new ICollection<EduProgramProfileInfo> EduProgramProfiles { get; set; }

        new ICollection<EduProgramDivisionInfo> Divisions { get; set; }

        new IScience Science { get; set; }
    }

    public class EduProgramInfo : IEduProgramWritable
    {
        public int EduProgramID { get; set; }

        public int EduLevelID { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Generation { get; set; }

        public string HomePage { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserId { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual EduLevelInfo EduLevel { get; set; }

        public virtual ICollection<DocumentInfo> Documents { get; set; } = new HashSet<DocumentInfo> ();

        public virtual ICollection<EduProgramProfileInfo> EduProgramProfiles { get; set; } = new HashSet<EduProgramProfileInfo> ();

        public virtual ICollection<EduProgramDivisionInfo> Divisions { get; set; } = new HashSet<EduProgramDivisionInfo> ();

        public virtual ScienceInfo Science { get; set; }

        IScience IEduProgram.Science => Science;

        IScience IEduProgramWritable.Science {
            get { return Science; }
            set { Science = (ScienceInfo) value; }
        }
    }
}
