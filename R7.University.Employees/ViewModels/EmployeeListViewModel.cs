//
//  EmployeeListViewModel.cs
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

using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Employees.Models;
using R7.University.Models;

namespace R7.University.Employees.ViewModels
{
    internal class EmployeeListViewModel
    {
        public EmployeeListViewModel (IEnumerable<IEmployee> employees, IDivision division)
        {
            Employees = employees;
            Division = division;
        }

        public ViewModelContext<EmployeeListSettings> Context { get; protected set; }

        public EmployeeListViewModel SetContext (ViewModelContext<EmployeeListSettings> context)
        {
            Context = context;
            return this;
        }

        public IDivision Division  { get; protected set; }

        private IEnumerable<IEmployee> employees;
        public IEnumerable<IEmployee> Employees
        {
            get {
                // cache settings properties
                var hideHeadEmployee = Context.Settings.HideHeadEmployee;
                var divisionId = Context.Settings.DivisionID;

                // filter out selected division's head employee (according to settings)
                return employees.Where (e => !hideHeadEmployee || !e.Positions
                    .Any (op => op.DivisionID == divisionId && op.PositionID == Division.HeadPositionID));
            }
            protected set { employees = value; }
        }
    }
}

