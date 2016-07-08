//
//  EmployeeAchievementRepository.cs
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
using System.Collections.Generic;
using System.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;

namespace R7.University.Data
{
    [Obsolete]
    public class EmployeeAchievementRepository
    {
        protected Dal2DataProvider DataProvider;

        public EmployeeAchievementRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        #region Singleton implementation

        private static readonly Lazy<EmployeeAchievementRepository> instance = new Lazy<EmployeeAchievementRepository> (
            () => new EmployeeAchievementRepository (UniversityDataProvider.Instance)
        );

        public static EmployeeAchievementRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        public IEnumerable<EmployeeAchievementInfo> GetAchievements_ForEmployees (IEnumerable<int> employeeIds)
        {
            return DataProvider.GetObjects<EmployeeAchievementInfo> (CommandType.Text, 
                @"SELECT * FROM {databaseOwner}[{objectQualifier}vw_University_EmployeeAchievements]
                    WHERE [EmployeeID] IN (" + TextUtils.FormatList (", ", employeeIds) + ")"
            );
        }
    }
}
