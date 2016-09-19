//
//  EduProgramProfileInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

namespace R7.University.Models
{
    public class EduProgramProfileInfo: IEduProgramProfile
    {
        public EduProgramProfileInfo ()
        {
            EduProgramProfileForms = new HashSet<EduProgramProfileFormInfo> ();
            Documents = new HashSet<DocumentInfo> ();
        }

        #region IEduProgramProfile implementation

        public int EduProgramProfileID { get; set; }

        public int EduProgramID { get; set; }

        public int EduLevelId { get; set; }

        public int? DivisionId { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public string Languages { get; set; }

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

        public virtual DivisionInfo Division { get; set; }

        public virtual ICollection<EduProgramProfileFormInfo> EduProgramProfileForms { get; set; }

        public virtual ICollection<DocumentInfo> Documents { get; set; }

        #endregion
    }
}
