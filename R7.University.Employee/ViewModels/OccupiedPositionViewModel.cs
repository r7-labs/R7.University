//
//  OccupiedPositionViewModel.cs
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
using R7.University.ViewModels;
using R7.University.Models;

namespace R7.University.Employee.ViewModels
{
    [Serializable]
    public class OccupiedPositionViewModel
    {
        public int ItemID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public string PositionShortTitle { get; set; }

        public string DivisionShortTitle { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        public string PositionShortTitleWithSuffix
        {
            get { return PositionShortTitle + " " + TitleSuffix; }
        }

        public OccupiedPositionViewModel ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public OccupiedPositionInfo NewOccupiedPositionInfo ()
        {
            var opinfo = new OccupiedPositionInfo ();

            opinfo.PositionID = PositionID;
            opinfo.DivisionID = DivisionID;
            opinfo.IsPrime = IsPrime;
            opinfo.TitleSuffix = TitleSuffix;

            return opinfo;
        }

        public OccupiedPositionViewModel (OccupiedPositionInfo op) : this ()
        {
            PositionID = op.PositionID;
            DivisionID = op.DivisionID;
            PositionShortTitle = FormatHelper.FormatShortTitle (op.Position.ShortTitle, op.Position.Title);
            DivisionShortTitle = FormatHelper.FormatShortTitle (op.Division.ShortTitle, op.Division.Title);
            IsPrime = op.IsPrime;
            TitleSuffix = op.TitleSuffix;
        }
    }
}
