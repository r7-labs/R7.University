//
//  EduProgramProfileFormYear.cs
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

using System;

namespace R7.University.Models
{
    public interface IEduProgramProfileFormYear
    {
        int EduProgramProfileFormYearId { get; }

        int EduProgramProfileId { get; }

        int EduFormId { get; }

        int YearId { get; }

        DateTime? StartDate { get; }

        DateTime? EndDate { get; }

        IEduForm EduForm { get; }

        IYear Year { get; }
    }

    public interface IEduProgramProfileFormYearWritable: IEduProgramProfileFormYear
    {
        new int EduProgramProfileFormYearId { get; set; }

        new int EduProgramProfileId { get; set; }

        new int EduFormId { get; set; }

        new int YearId { get; set; }

        new DateTime? StartDate { get; set; }

        new DateTime? EndDate { get; set; }

        new IEduForm EduForm { get; set; }

        new IYear Year { get; set; }
    }

    public class EduProgramProfileFormYearInfo: IEduProgramProfileFormYearWritable
    {
        public int EduProgramProfileFormYearId { get; set; }

        public int EduProgramProfileId { get; set; }

        public int EduFormId { get; set; }

        public int YearId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual YearInfo Year { get; set; }

        IYear IEduProgramProfileFormYear.Year => Year;

        IYear IEduProgramProfileFormYearWritable.Year {
            get { return Year; }
            set { Year = (YearInfo) value; }
        }

        public virtual EduFormInfo EduForm { get; set; }

        IEduForm IEduProgramProfileFormYear.EduForm => EduForm;

        IEduForm IEduProgramProfileFormYearWritable.EduForm {
            get { return EduForm; }
            set { EduForm = (EduFormInfo) value; }
        }

    }
}
