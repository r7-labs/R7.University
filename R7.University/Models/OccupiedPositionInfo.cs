//
//  OccupiedPositionInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
    // TODO: Add Unique constraint to OccupiedPositions table FK's?
    public class OccupiedPositionInfo: IOccupiedPositionWritable
    {
        public int OccupiedPositionID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        public virtual PositionInfo Position { get; set; }

        public virtual DivisionInfo Division { get; set; }

        public virtual EmployeeInfo Employee { get; set; }
    }
}
