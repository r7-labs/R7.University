//
//  IEmployeeAchievement.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
        int EmployeeAchievementID { get; set; }

        int EmployeeID  { get; set; }

        int? AchievementID { get; set; }

        string Title { get; set; }

        string ShortTitle  { get; set; }

        string Description { get; set; }

        int? YearBegin { get; set; }

        int? YearEnd { get; set; }

        bool IsTitle { get; set; }

        string DocumentURL { get; set; }

        string TitleSuffix { get; set; }

        AchievementType? AchievementType { get; set; }

        AchievementInfo Achievement { get; set; }
    }
}

