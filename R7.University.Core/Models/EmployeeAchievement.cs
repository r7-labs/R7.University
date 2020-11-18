//
//  EmployeeAchievement.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
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

namespace R7.University.Models
{
    public interface IEmployeeAchievement
    {
        int EmployeeAchievementID { get; }

        int EmployeeID { get; }

        int? AchievementID { get; }

        int? AchievementTypeId { get; }

        string Title { get; }

        string ShortTitle { get; }

        string Description { get; }

        int? YearBegin { get; }

        int? YearEnd { get; }

        bool IsTitle { get; }

        string DocumentURL { get; }

        string TitleSuffix { get; }

        int? Hours { get; }

        IAchievement Achievement { get; }

        IAchievementType AchievementType { get; }
    }

    public interface IEmployeeAchievementWritable: IEmployeeAchievement
    {
        new int EmployeeAchievementID { get; set; }

        new int EmployeeID { get; set; }

        new int? AchievementID { get; set; }

        new int? AchievementTypeId { get; set; }

        new string Title { get; set; }

        new string ShortTitle { get; set; }

        new string Description { get; set; }

        new int? YearBegin { get; set; }

        new int? YearEnd { get; set; }

        new bool IsTitle { get; set; }

        new string DocumentURL { get; set; }

        new string TitleSuffix { get; set; }

        new int? Hours { get; set; }

        new IAchievement Achievement { get; set; }

        new IAchievementType AchievementType { get; set; }
    }

    public class EmployeeAchievementInfo: IEmployeeAchievementWritable
    {
        public int EmployeeAchievementID { get; set; }

        public int EmployeeID  { get; set; }

        public int? AchievementID { get; set; }

        public int? AchievementTypeId { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string ShortTitle  { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        public int? Hours { get; set; }

        public virtual AchievementInfo Achievement { get; set; }

        public virtual AchievementTypeInfo AchievementType { get; set; }

        IAchievementType IEmployeeAchievement.AchievementType => AchievementType;

        IAchievementType IEmployeeAchievementWritable.AchievementType {
            get { return AchievementType; }
            set { AchievementType = (AchievementTypeInfo) value; }
        }

        IAchievement IEmployeeAchievement.Achievement => Achievement;

        IAchievement IEmployeeAchievementWritable.Achievement {
            get { return Achievement; }
            set { Achievement = (AchievementInfo) value; }
        }
    }
}
