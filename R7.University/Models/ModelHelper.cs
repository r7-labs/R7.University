//
//  ModelHelper.cs
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

namespace R7.University.Models
{
    public static class ModelHelper
    {
        public static bool IsPublished (DateTime? startDate, DateTime? endDate)
        {
            var now = DateTime.Now;
            return (startDate == null || now >= startDate) && (endDate == null || now < endDate);
        }

        #region Extension methods

        public static SystemEduForm GetSystemEduForm (this IEduForm eduForm)
        {
            SystemEduForm result;
            return Enum.TryParse<SystemEduForm> (eduForm.Title, out result) ? result : SystemEduForm.Custom;
        }

        #endregion
    }
}

