//
//  PositionInfo.cs
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
using R7.University.Models;

namespace R7.University
{
	[TableName ("University_Positions")]
	[PrimaryKey ("PositionID", AutoIncrement = true)]
	public class PositionInfo: IPosition
	{
        public int PositionID { get; set; }

        public string Title { get; set; }

        public string ShortTitle  { get; set; }

		public int Weight { get; set; }

		public bool IsTeacher { get; set; }
	}
}

