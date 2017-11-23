//
//  EduProgramProfileForm.cs
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

namespace R7.University.Models
{
    public interface IEduProgramProfileForm
    {
        int EduProgramProfileFormID { get; }

        int EduProgramProfileID { get; }

        int EduFormID { get; }

        int TimeToLearn { get; }

        int TimeToLearnHours { get; }

        // TODO: Remove
        bool IsAdmissive { get; }

        EduFormInfo EduForm { get; }
    }

    public interface IEduProgramProfileFormWritable: IEduProgramProfileForm
    {
        new int EduProgramProfileFormID { get; set; }

        new int EduProgramProfileID { get; set; }

        new int EduFormID { get; set; }

        new int TimeToLearn { get; set; }

        new int TimeToLearnHours { get; set; }

        // TODO: Remove
        new bool IsAdmissive { get; set; }

        new EduFormInfo EduForm { get; set; }
    }

    public class EduProgramProfileFormInfo: IEduProgramProfileFormWritable
    {
        public int EduProgramProfileFormID { get; set; }

        public int EduProgramProfileID { get; set; }

        public int EduFormID { get; set; }

        public int TimeToLearn { get; set; }

        public int TimeToLearnHours { get; set; }

        // TODO: Remove
        public bool IsAdmissive { get; set; }

        public virtual EduFormInfo EduForm { get; set; }
    }
}

