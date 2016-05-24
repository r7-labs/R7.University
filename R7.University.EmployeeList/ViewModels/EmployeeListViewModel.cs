//
// EmployeeListViewModel.cs
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
using System.Linq;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.EmployeeList.Components;
using R7.University.Models;

namespace R7.University.EmployeeList.ViewModels
{
    public class EmployeeListViewModel
    {
        public ViewModelContext<EmployeeListSettings> Context { get; protected set; }

        public EmployeeListViewModel (IEnumerable<IEmployee> employees, 
            IDivision division, ViewModelContext<EmployeeListSettings> context)
        {
            Employees = employees;
            Division = division;
            Context = context;
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
                return employees.Where (e => !hideHeadEmployee || !e.OccupiedPositions
                    .Any (op => op.DivisionID == divisionId && op.PositionID == Division.HeadPositionID));
            }
            set { employees = value; }
        }
    }
}

