//
//  AchievementsTable.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015 Roman M. Yagodin
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
using System.Data;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using R7.University.Data;

namespace R7.University.Launchpad
{
    public class AchievementsTable: LaunchpadTableBase
    {
        public AchievementsTable () : base ("Achievements")
        {
        }

        public override DataTable GetDataTable (PortalModuleBase module, UniversityDbRepository repository, string search)
        {
            var dt = new DataTable ();
            DataRow dr;

            dt.Columns.Add (new DataColumn ("AchievementID", typeof (int)));
            dt.Columns.Add (new DataColumn ("Title", typeof (string)));
            dt.Columns.Add (new DataColumn ("ShortTitle", typeof (string)));
            dt.Columns.Add (new DataColumn ("AchievementType", typeof (string)));

            foreach (DataColumn column in dt.Columns)
                column.AllowDBNull = true;

            var achievements = UniversityRepository.Instance.DataProvider.FindObjects<AchievementInfo> (
                                   @"WHERE CONCAT ([Title], ' ', [ShortTitle]) LIKE N'%{0}%'", search, false
                               );

            foreach (var achievement in achievements) {
                var col = 0;
                dr = dt.NewRow ();

                dr [col++] = achievement.AchievementID;
                dr [col++] = achievement.Title;
                dr [col++] = achievement.ShortTitle;
                dr [col++] = Localization.GetString (
                    AchievementTypeInfo.GetResourceKey (achievement.AchievementType),
                    module.LocalResourceFile);

                dt.Rows.Add (dr);
            }

            return dt;
        }
    }
}

