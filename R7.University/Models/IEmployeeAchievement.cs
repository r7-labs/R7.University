//
//  IEmployeeAchievement.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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

        AchievementInfo Achievement { get; }

        AchievementTypeInfo AchievementType { get; }
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

        new AchievementInfo Achievement { get; set; }

        new AchievementTypeInfo AchievementType { get; set; }
    }
}

