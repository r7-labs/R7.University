//
//  IPosition.cs
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
    public interface IPosition
    {
        int PositionID { get; }

        string Title { get; }

        string ShortTitle  { get; }

        int Weight { get; }

        bool IsTeacher { get; }
    }

    public interface IPositionWritable: IPosition
    {
        new int PositionID { get; set; }

        new string Title { get; set; }

        new string ShortTitle  { get; set; }

        new int Weight { get; set; }

        new bool IsTeacher { get; set; }
    }
}

