//
//  OccupiedPositionEditModel.cs
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

using System;
using System.Xml.Serialization;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employee.ViewModels
{
    [Serializable]
    public class OccupiedPositionEditModel: IOccupiedPositionWritable
    {
        #region IOccupiedPositionWritable implementation

        public int OccupiedPositionID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        [XmlIgnore]
        public PositionInfo Position { get; set; }

        [XmlIgnore]
        public DivisionInfo Division { get; set; }

        [XmlIgnore]
        public EmployeeInfo Employee { get; set; }

        #endregion

        #region External properties

        public string PositionShortTitle { get; set; }

        public string DivisionShortTitle { get; set; }

        #endregion

        #region Bindable properties

        public string PositionShortTitleWithSuffix
        {
            get { return PositionShortTitle + " " + TitleSuffix; }
        }

        #endregion

        public int ItemID { get; set; }

        public OccupiedPositionEditModel ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public OccupiedPositionInfo NewOccupiedPositionInfo ()
        {
            var op = new OccupiedPositionInfo ();

            op.PositionID = PositionID;
            op.DivisionID = DivisionID;
            op.IsPrime = IsPrime;
            op.TitleSuffix = TitleSuffix;

            return op;
        }

        public OccupiedPositionEditModel (IOccupiedPosition op) : this ()
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
