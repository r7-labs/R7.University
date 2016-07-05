//
//  EmployeeAchievementInfo.cs
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

namespace R7.University.Models
{
    // More attributes for class:
    // Set caching for table: [Cacheable("R7.University_Divisions", CacheItemPriority.Default, 20)]
    // Explicit mapping declaration: [DeclareColumns]
    // More attributes for class properties:
    // Custom column name: [ColumnName("DivisionID")]
    // Explicit include column: [IncludeColumn]
    // Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
    [TableName ("University_EmployeeAchievements")]
    [PrimaryKey ("EmployeeAchievementID", AutoIncrement = true)]
    [Serializable]
    public class EmployeeAchievementInfo: IEmployeeAchievement
    {
        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID { get; set; }

        public int EmployeeID  { get; set; }

        public int? AchievementID { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string ShortTitle  { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        [IgnoreColumn]
        public AchievementType? AchievementType
        {
            get { 
                if (!string.IsNullOrWhiteSpace (AchievementTypeString))
                    return (AchievementType) AchievementTypeString [0];

                return null;
            }
            set { 
                if (value != null)
                    AchievementTypeString = ((char) value).ToString ();
                else
                    AchievementTypeString = null;
            }
        }

        #endregion

        [ColumnName ("AchievementType")]
        public string AchievementTypeString { get; set; }

        [IgnoreColumn]
        public virtual AchievementInfo Achievement { get; set; }
    }
}
