//
//  IEmployee.cs
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
using System.Collections.Generic;
using R7.University.Data;

namespace R7.University.Models
{
    public interface IEmployee: IUniversityBaseEntity
    {
        int EmployeeID { get; set; }

        int? UserID { get; set; }

        int? PhotoFileID { get; set; }

        string Phone { get; set; }

        string CellPhone { get; set; }

        string Fax { get; set; }

        string LastName { get; set; }

        string FirstName { get; set; }

        string OtherName { get; set; }

        string Email { get; set; }

        string SecondaryEmail { get; set; }

        string WebSite { get; set; }

        string WebSiteLabel { get; set; }

        string Messenger { get; set; }

        string WorkingPlace { get; set; }

        string WorkingHours { get; set; }

        string Biography { get; set; }

        // employee stage may be not continuous, so using starting date is not possible
        int? ExperienceYears { get; set; }

        // employee ExpYearsBySpec even more unbinded to dates
        int? ExperienceYearsBySpec { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }

        bool ShowBarcode { get; set; }

        IList<IEmployeeAchievement> Achievements { get; set; }

        IList<IEmployeeDiscipline> Disciplines { get; set; }

        IList<OccupiedPositionInfoEx> OccupiedPositions { get; set; }

        ICollection<OccupiedPositionInfo> Positions { get; set; }
    }
}
