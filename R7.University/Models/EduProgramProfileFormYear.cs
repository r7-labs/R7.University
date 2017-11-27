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

        int? TimeToLearnCu { get; }

        int? TimeToLearnHours { get; }

        int? TimeToLearnMonths { get; }

        int? TrainingPracticeCu { get; }

        int? IndustrialPracticeCu { get; }

        int? UndergraduatePracticeCu { get; }

        DateTime? StartDate { get; }

        DateTime? EndDate { get; }
    }

    public interface IEduProgramProfileFormYearWritable: IEduProgramProfileFormYear
    {
        new int EduProgramProfileFormYearId { get; set; }

        new int EduProgramProfileId { get; set; }

        new int EduFormId { get; set; }

        new int YearId { get; set; }

        new int? TimeToLearnCu { get; set; }

        new int? TimeToLearnHours { get; set; }

        new int? TimeToLearnMonths { get; set; }

        new int? TrainingPracticeCu { get; set; }

        new int? IndustrialPracticeCu { get; set; }

        new int? UndergraduatePracticeCu { get; set; }

        new DateTime? StartDate { get; set; }

        new DateTime? EndDate { get; set; }
    }

    public class EduProgramProfileFormYearInfo: IEduProgramProfileFormYearWritable
    {
        public int EduProgramProfileFormYearId { get; set; }

        public int EduProgramProfileId { get; set; }

        public int EduFormId { get; set; }

        public int YearId { get; set; }

        public int? TimeToLearnCu { get; set; }

        public int? TimeToLearnHours { get; set; }

        public int? TimeToLearnMonths { get; set; }

        public int? TrainingPracticeCu { get; set; }

        public int? IndustrialPracticeCu { get; set; }

        public int? UndergraduatePracticeCu { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
