//
//  AchievementInfo.cs
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

using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University.Models
{
    [TableName ("University_Achievements")]
    [PrimaryKey ("AchievementID", AutoIncrement = true)]
    public class AchievementInfo: IAchievement
    {
        #region IAchievement implementation

        public int AchievementID { get; set; }

        public string Title { get; set; }

        public string ShortTitle  { get; set; }

        [IgnoreColumn]
        public AchievementType AchievementType
        {
            get { return (AchievementType) AchievementTypeString [0]; }
            set { AchievementTypeString = ((char) value).ToString (); }
        }

        #endregion

        [ColumnName ("AchievementType")]
        public string AchievementTypeString { get; set; }
    }
}
