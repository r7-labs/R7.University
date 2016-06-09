//
//  EmployeeDisciplineInfoEx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using DotNetNuke.ComponentModel.DataAnnotations;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Data
{
    [TableName ("vw_University_EmployeeDisciplines")]
    [Serializable]
    public class EmployeeDisciplineInfoEx: EmployeeDisciplineInfo
    {
        #region External properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        #endregion

        [IgnoreColumn]
        public string EduProfileString
        {
            get {
                var profileString = TextUtils.FormatList (" ", ProfileCode, ProfileTitle);
                return TextUtils.FormatList (" ", Code, Title) +
                (!string.IsNullOrWhiteSpace (profileString) ? " (" + profileString + ")" : string.Empty);
            }
        }
    }
}
