//
//  IEmployeeDiscipline.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
    public interface IEmployeeDiscipline
    {
        long EmployeeDisciplineID { get; set; }

        int EmployeeID { get; set; }

        int EduProgramProfileID { get; set; }

        string Disciplines { get; set; }

        EmployeeInfo Employee { get; set; }

        EduProgramProfileInfo EduProgramProfile { get; set; }
    }
}

