//
// IEmployee.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using R7.University.Data;

namespace R7.University.Models
{
    public interface IEmployee
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

        IList<IEmployeeAchievement> Achievements { get; set; }

        IList<OccupiedPositionInfoEx> OccupiedPositions { get; set; }
    }
}
