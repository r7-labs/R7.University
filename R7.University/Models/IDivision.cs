//
//  IDivision.cs
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

namespace R7.University.Models
{
    public interface IDivision: IUniversityBaseEntity
    {
        string Title { get; set; }

        string ShortTitle { get; set; }

        int DivisionID { get; set; }

        int? ParentDivisionID  { get; set; }

        int? DivisionTermID  { get; set; }

        string HomePage { get; set; }

        string WebSite { get; set; }

        string WebSiteLabel { get; set; }

        string Phone { get; set; }

        string Fax { get; set; }

        string Email { get; set; }

        string SecondaryEmail { get; set; }

        string Location { get; set; }

        string WorkingHours { get; set; }

        string DocumentUrl { get; set; }

        bool IsVirtual { get; set; }

        int? HeadPositionID { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
