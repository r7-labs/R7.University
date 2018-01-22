//
//  EmployeeDisciplineViewModel.cs
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

using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employees.ViewModels
{
    internal class EmployeeDisciplineViewModel: IEmployeeDiscipline
    {
        public IEmployeeDiscipline EmployeeDiscipline { get; protected set; }

        public EmployeeDisciplineViewModel (IEmployeeDiscipline model)
        {
            EmployeeDiscipline = model;
        }

        #region IEmployeeDiscipline implementation

        public long EmployeeDisciplineID => EmployeeDiscipline.EmployeeDisciplineID;

        public int EmployeeID => EmployeeDiscipline.EmployeeID;

        public int EduProgramProfileID => EmployeeDiscipline.EduProgramProfileID;

        public string Disciplines => EmployeeDiscipline.Disciplines;

        public EmployeeInfo Employee => EmployeeDiscipline.Employee;

        public EduProgramProfileInfo EduProgramProfile => EmployeeDiscipline.EduProgramProfile;

        #endregion

        public string EduProgramProfile_String
        {
            get {
                return FormatHelper.FormatEduProgramProfileTitle (
                    EmployeeDiscipline.EduProgramProfile.EduProgram.Code,
                    EmployeeDiscipline.EduProgramProfile.EduProgram.Title,
                    EmployeeDiscipline.EduProgramProfile.ProfileCode,
                    EmployeeDiscipline.EduProgramProfile.ProfileTitle
                );
            }
        }

        public string EduLevel_String {
            get {
                return FormatHelper.FormatShortTitle (
                    EmployeeDiscipline.EduProgramProfile.EduLevel.ShortTitle,
                    EmployeeDiscipline.EduProgramProfile.EduLevel.Title
                );
            }
        }
    }
}
    