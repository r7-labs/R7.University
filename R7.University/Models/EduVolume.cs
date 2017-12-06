//
//  EduVolume.cs
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

namespace R7.University.Models
{
    public interface IEduVolume
    {
        int EduVolumeId { get; }

        int TimeToLearnHours { get; }

        int TimeToLearnMonths { get; }

        int? Year1Cu { get; }

        int? Year2Cu { get; }

        int? Year3Cu { get; }

        int? Year4Cu { get; }

        int? Year5Cu { get; }

        int? Year6Cu { get; }

        int? PracticeType1Cu { get; }

        int? PracticeType2Cu { get; }

        int? PracticeType3Cu { get; }

        IEduProgramProfileFormYear EduProgramProfileFormYear { get; }
    }

    public interface IEduVolumeWritable: IEduVolume
    {
        new int EduVolumeId { get; set; }

        new int TimeToLearnHours { get; set; }

        new int TimeToLearnMonths { get; set; }

        new int? Year1Cu { get; set; }

        new int? Year2Cu { get; set; }

        new int? Year3Cu { get; set; }

        new int? Year4Cu { get; set; }

        new int? Year5Cu { get; set; }

        new int? Year6Cu { get; set; }

        new int? PracticeType1Cu { get; set; }

        new int? PracticeType2Cu { get; set; }

        new int? PracticeType3Cu { get; set; }

        new IEduProgramProfileFormYear EduProgramProfileFormYear { get; set; }
    }

    public class EduVolumeInfo : IEduVolumeWritable
    {
        public int EduVolumeId { get; set; }

        public int TimeToLearnHours { get; set; }

        public int TimeToLearnMonths { get; set; }

        public int? Year1Cu { get; set; }

        public int? Year2Cu { get; set; }

        public int? Year3Cu { get; set; }

        public int? Year4Cu { get; set; }

        public int? Year5Cu { get; set; }

        public int? Year6Cu { get; set; }

        public int? PracticeType1Cu { get; set; }

        public int? PracticeType2Cu { get; set; }

        public int? PracticeType3Cu { get; set; }

        public virtual EduProgramProfileFormYearInfo EduProgramProfileFormYear { get; set; }

        IEduProgramProfileFormYear IEduVolume.EduProgramProfileFormYear => EduProgramProfileFormYear;

        IEduProgramProfileFormYear IEduVolumeWritable.EduProgramProfileFormYear {
            get { return EduProgramProfileFormYear; }
            set { EduProgramProfileFormYear = (EduProgramProfileFormYearInfo) value; }
        }
    }
}
