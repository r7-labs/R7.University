//
//  IEduProgramProfileForm.cs
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

namespace R7.University.Models
{
    public interface IEduProgramProfileForm
    {
        long EduProgramProfileFormID { get; }

        int EduProgramProfileID { get; }

        int EduFormID { get; }

        int TimeToLearn { get; }

        int TimeToLearnHours { get; }

        bool IsAdmissive { get; }

        EduFormInfo EduForm { get; }

        // EduProgramProfileInfo EduProgramProfile { get; }
    }

    public interface IEduProgramProfileFormWritable: IEduProgramProfileForm
    {
        new long EduProgramProfileFormID { get; set; }

        new int EduProgramProfileID { get; set; }

        new int EduFormID { get; set; }

        new int TimeToLearn { get; set; }

        new int TimeToLearnHours { get; set; }

        new bool IsAdmissive { get; set; }

        new EduFormInfo EduForm { get; set; }


    }
}

