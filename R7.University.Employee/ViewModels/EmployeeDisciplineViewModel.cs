//
//  EmployeeDisciplineViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employee.ViewModels
{
    internal class EmployeeDisciplineViewModel: IEmployeeDiscipline
    {
        public IEmployeeDiscipline Model { get; protected set; }

        public EmployeeDisciplineViewModel (IEmployeeDiscipline model)
        {
            Model = model;
        }

        #region IEmployeeDiscipline implementation

        public long EmployeeDisciplineID
        {
            get { return Model.EmployeeDisciplineID; }
            set { throw new InvalidOperationException (); }
        }

        public int EmployeeID
        {
            get { return Model.EmployeeID; }
            set { throw new InvalidOperationException (); }
        }

        public int EduProgramProfileID
        {
            get { return Model.EduProgramProfileID; }
            set { throw new InvalidOperationException (); }
        }

        public string Disciplines
        {
            get { return Model.Disciplines; }
            set { throw new InvalidOperationException (); }
        }

        public EmployeeInfo Employee
        {
            get { return Model.Employee; }
            set { throw new InvalidOperationException (); }
        }

        public EduProgramProfileInfo EduProgramProfile
        {
            get { return Model.EduProgramProfile; }
            set { throw new InvalidOperationException (); }
        }

        #endregion

        public string EduProgramProfile_String
        {
            get {
                return FormatHelper.FormatEduProgramProfileTitle (
                    Model.EduProgramProfile.EduProgram.Code,
                    Model.EduProgramProfile.EduProgram.Title,
                    Model.EduProgramProfile.ProfileCode,
                    Model.EduProgramProfile.ProfileTitle
                );
            }
        }
    }
}
    