//
//  AchievementType.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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
    public interface IAchievementType: ISystemEntity
    {
        int AchievementTypeId { get; }

        string Type { get; }

        string Description { get; }
    }

    public interface IAchievementTypeWritable: IAchievementType, ISystemEntityWritable
    {
        new int AchievementTypeId { get; set; }

        new string Type { get; set; }

        new string Description { get; set; }
    }

    public class AchievementTypeInfo : IAchievementTypeWritable
    {
        public int AchievementTypeId { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }
    }
}

