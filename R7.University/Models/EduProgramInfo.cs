//
//  EduProgramInfo.cs
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
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Models
{
    public class EduProgramInfo: IEduProgram
    {
        public EduProgramInfo ()
        {
            Documents = new HashSet<DocumentInfo> ();
            EduProgramProfiles = new HashSet<EduProgramProfileInfo> ();
        }

        #region IEduProgram implementation

        public int EduProgramID { get; set; }

        public int EduLevelID { get; set; }

        public int? DivisionId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Generation { get; set; }

        public string HomePage { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserID { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual EduLevelInfo EduLevel { get; set; }

        public virtual DivisionInfo Division { get; set; }

        public virtual ICollection<DocumentInfo> Documents { get; set; }

        public virtual ICollection<EduProgramProfileInfo> EduProgramProfiles { get; set; }

        #endregion

        // TODO: Move to viewmodel
        public string EduProgramString
        {
            get { return TextUtils.FormatList (" ", Code, Title); }
        }
    }
}
