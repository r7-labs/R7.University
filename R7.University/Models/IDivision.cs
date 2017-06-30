//
//  IDivision.cs
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
    public interface IDivision: ITrackableEntity
    {
        string Title { get; }

        string ShortTitle { get; }

        int DivisionID { get; }

        int? ParentDivisionID  { get; }

        int? DivisionTermID  { get; }

        string HomePage { get; }

        string WebSite { get; }

        string WebSiteLabel { get; }

        string Phone { get; }

        string Fax { get; }

        string Email { get; }

        string SecondaryEmail { get; }

        string Address { get; }

        string Location { get; }

        string WorkingHours { get; }

        string DocumentUrl { get; }

        bool IsVirtual { get; }

        bool IsInformal { get; }

        int? HeadPositionID { get; }

        DateTime? StartDate { get; }

        DateTime? EndDate { get; }

        ICollection<DivisionInfo> SubDivisions { get; }

        int Level { get; }

        string Path { get; }
    }

    public interface IDivisionWritable: IDivision, ITrackableEntityWritable
    {
        new string Title { get; set; }

        new string ShortTitle { get; set; }

        new int DivisionID { get; set; }

        new int? ParentDivisionID  { get; set; }

        new int? DivisionTermID  { get; set; }

        new string HomePage { get; set; }

        new string WebSite { get; set; }

        new string WebSiteLabel { get; set; }

        new string Phone { get; set; }

        new string Fax { get; set; }

        new string Email { get; set; }

        new string SecondaryEmail { get; set; }

        new string Address { get; set; }

        new string Location { get; set; }

        new string WorkingHours { get; set; }

        new string DocumentUrl { get; set; }

        new bool IsVirtual { get; set; }

        new bool IsInformal { get; set; }

        new int? HeadPositionID { get; set; }

        new DateTime? StartDate { get; set; }

        new DateTime? EndDate { get; set; }

        new ICollection<DivisionInfo> SubDivisions { get; set; }

        new int Level { get; set; }

        new string Path { get; set; }
    }
}
