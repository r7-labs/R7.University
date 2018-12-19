//
//  EditPositions.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Entities.Modules;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Utilities;
using R7.University.Controls.EditModels;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditPositions : GridAndFormControlBase<OccupiedPositionInfo, OccupiedPositionEditModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<PositionInfo> positions, IEnumerable<DivisionInfo> divisions)
        {
            Module = module;

            comboPositions.DataSource = positions.Select (p => new {
                p.PositionID,
                Title = !string.IsNullOrEmpty (p.ShortTitle) ? $"{p.Title} ({p.ShortTitle})" : p.Title
            });

            comboPositions.DataBind ();

            divisionSelector.DataSource = divisions;
            divisionSelector.DataBind ();
        }

        public void SetDivision (int? divisionId)
        {
            divisionSelector.DivisionId = divisionId;
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (OccupiedPositionEditModel item)
        {
            divisionSelector.DivisionId = item.DivisionID;
            comboPositions.SelectByValue (item.PositionID);
            checkIsPrime.Checked = item.IsPrime;
            textPositionTitleSuffix.Text = item.TitleSuffix;
        }

        protected override void OnUpdateItem (OccupiedPositionEditModel item)
        {
            item.PositionID = int.Parse (comboPositions.SelectedValue);
            item.DivisionID = divisionSelector.DivisionId.Value;
            item.IsPrime = checkIsPrime.Checked;
            item.TitleSuffix = textPositionTitleSuffix.Text.Trim ();

            // TODO: Don't call database here
            using (var modelContext = new UniversityModelContext ()) {
                var division = modelContext.Get<DivisionInfo, int> (item.DivisionID);
                item.DivisionTitle = division.Title;
                item.DivisionStartDate = division.StartDate;
                item.DivisionEndDate = division.EndDate;

                var position = modelContext.Get<PositionInfo, int> (item.PositionID);
                item.PositionTitle = position.Title;
            }
        }

        protected override void OnResetForm ()
        {
            var divisionId = Request.QueryString ["division_id"];
            divisionSelector.DivisionId = ParseHelper.ParseToNullable<int> (divisionId);

            comboPositions.SelectedIndex = 0;
            textPositionTitleSuffix.Text = string.Empty;
            checkIsPrime.Checked = false;
        }

        #endregion
    }
}
