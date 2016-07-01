//
//  EduProgramProfileFormInfo.cs
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
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University.Models
{
    [TableName ("University_EduProgramProfileForms")]
    [PrimaryKey ("EduProgramProfileFormID", AutoIncrement = true)]
    public class EduProgramProfileFormInfo: IEduProgramProfileForm
    {
        #region IEduProgramProfileForm implementation

        public long EduProgramProfileFormID { get; set; }

        public int EduProgramProfileID { get; set; }

        public int EduFormID { get; set; }

        public int TimeToLearn { get; set; }

        // REVIEW: Rename?
        public bool IsAdmissive { get; set; }

        [IgnoreColumn]
        public IEduForm EduForm { get; set; }

        [IgnoreColumn]
        public IEduProgramProfile EduProgramProfile { get; set; }

        #endregion

        public void SetTimeToLearn (int years, int months)
        {
            TimeToLearn = years * 12 + months;
        }
    }
}

