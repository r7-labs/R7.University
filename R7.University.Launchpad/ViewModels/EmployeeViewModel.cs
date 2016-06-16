//
//  EmployeeViewModel.cs
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
using DotNetNuke.Common.Utilities;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Launchpad.ViewModels
{
    public class EmployeeViewModel: EmployeeViewModelBase
    {
        public EmployeeViewModel (IEmployee model): base (model)
        {
        }

        #region Bindable properties

        public string WebSite_String
        {
            get { return TextUtils.FormatList (": ", Model.WebSiteLabel, Model.WebSite); }
        }

        public string Biography_String
        {
            get { 
                return !string.IsNullOrWhiteSpace (Model.Biography) ? 
                    HtmlUtils.Shorten (Model.Biography, 16, "...") : string.Empty;
            }
        }

        #endregion
    }
}

