//
//  IEmployee.cs
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

using System;
using System.Collections.Generic;

namespace R7.University.Models
{
    public interface IEmployee: ITrackableEntity
    {
        int EmployeeID { get; }

        int? UserID { get; }

        int? PhotoFileID { get; }

        string Phone { get; }

        string CellPhone { get; }

        string Fax { get; }

        string LastName { get; }

        string FirstName { get; }

        string OtherName { get; }

        string Email { get; }

        string SecondaryEmail { get; }

        string WebSite { get; }

        string WebSiteLabel { get; }

        string Messenger { get; }

        string WorkingPlace { get; }

        string WorkingHours { get; }

        string Biography { get; }

        // employee stage may be not continuous, so using starting date is not possible
        int? ExperienceYears { get; }

        // employee ExpYearsBySpec even more unbinded to dates
        int? ExperienceYearsBySpec { get; }

        DateTime? StartDate { get; }

        DateTime? EndDate { get; }

        bool ShowBarcode { get; }

        ICollection<EmployeeAchievementInfo> Achievements { get; }

        ICollection<EmployeeDisciplineInfo> Disciplines { get; }

        ICollection<OccupiedPositionInfo> Positions { get; }
    }

    public interface IEmployeeWritable: IEmployee, ITrackableEntityWritable
    {
        new int EmployeeID { get; set; }

        new int? UserID { get; set; }

        new int? PhotoFileID { get; set; }

        new string Phone { get; set; }

        new string CellPhone { get; set; }

        new string Fax { get; set; }

        new string LastName { get; set; }

        new string FirstName { get; set; }

        new string OtherName { get; set; }

        new string Email { get; set; }

        new string SecondaryEmail { get; set; }

        new string WebSite { get; set; }

        new string WebSiteLabel { get; set; }

        new string Messenger { get; set; }

        new string WorkingPlace { get; set; }

        new string WorkingHours { get; set; }

        new string Biography { get; set; }

        // employee stage may be not continuous, so using starting date is not possible
        new int? ExperienceYears { get; set; }

        // employee ExpYearsBySpec even more unbinded to dates
        new int? ExperienceYearsBySpec { get; set; }

        new DateTime? StartDate { get; set; }

        new DateTime? EndDate { get; set; }

        new bool ShowBarcode { get; set; }

        new ICollection<EmployeeAchievementInfo> Achievements { get; set; }

        new ICollection<EmployeeDisciplineInfo> Disciplines { get; set; }

        new ICollection<OccupiedPositionInfo> Positions { get; set; }
    }
}
