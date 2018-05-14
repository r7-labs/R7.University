//
//  EmployeeViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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

using DotNetNuke.Common.Utilities;
using R7.Dnn.Extensions.Text;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Launchpad.ViewModels
{
    internal class EmployeeViewModel: EmployeeViewModelBase
    {
        public EmployeeViewModel (IEmployee model): base (model)
        {
        }

        #region Bindable properties

        public string WebSite_String
        {
            get { return FormatHelper.FormatList (": ", Employee.WebSiteLabel, Employee.WebSite); }
        }

        public string Biography_String
        {
            get { 
                return !string.IsNullOrWhiteSpace (Employee.Biography) ? 
                    HtmlUtils.Shorten (Employee.Biography, 16, "...") : string.Empty;
            }
        }

        #endregion
    }
}

