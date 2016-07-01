//
//  EmployeeDisciplineInfo.cs
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
    [TableName ("University_EmployeeDisciplines")]
    [PrimaryKey ("EmployeeDisciplineID", AutoIncrement = true)]
    [Serializable]
    public class EmployeeDisciplineInfo: IEmployeeDiscipline
    {
        #region IEmployeeDiscipline implementation

        public long EmployeeDisciplineID { get; set; }

        public int EmployeeID { get; set; }

        public int EduProgramProfileID { get; set; }

        public string Disciplines { get; set; }

        #endregion

        public override string ToString ()
        {
            return string.Format (
                "[EmployeeDisciplineInfo: EmployeeDisciplineID={0}, EmployeeID={1}, EduProgramProfileID={2}, Disciplines={3}]",
                EmployeeDisciplineID,
                EmployeeID,
                EduProgramProfileID,
                Disciplines);
        }
    }
}

