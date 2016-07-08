//
//  EmployeesTable.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.Entities.Modules;
using R7.University.Components;
using R7.University.Launchpad.ViewModels;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public class EmployeesTable: LaunchpadTableBase
    {
        public EmployeesTable () : base ("Employees")
        {
        }

        public override DataTable GetDataTable (PortalModuleBase module, UniversityModelContext modelContext, string search)
        {
            // REVIEW: Cannot set comparison options
            var employees = (search == null)
                ? new FlatQuery<EmployeeInfo> (modelContext).List ()
                : new FlatQuery<EmployeeInfo> (modelContext)
                    .ListWhere (e => e.LastName.Contains (search)
                                || e.FirstName.Contains (search)
                                || e.OtherName.Contains (search)
                                || e.CellPhone.Contains (search)
                                || e.Phone.Contains (search)
                                || e.Fax.Contains (search)
                                || e.Email.Contains (search)
                                || e.SecondaryEmail.Contains (search)
                                || e.WebSite.Contains (search)
                                || e.WebSiteLabel.Contains (search)
                                || e.WorkingHours.Contains (search)
                            );

            return DataTableConstructor.FromIEnumerable (employees.Select (e => new EmployeeViewModel (e)));
        }
    }
}
