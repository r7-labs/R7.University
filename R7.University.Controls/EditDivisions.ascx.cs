//
//  EditDivisions.ascx.cs
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.University.Controls.EditModels;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditDivisions : GridAndFormControlBase<EduProgramDivisionInfo, EduProgramDivisionEditModel>
    {
        public string ForModel { get; set; }

        public void OnInit (PortalModuleBase module, IEnumerable<DivisionInfo> divisions)
        {
            Module = module;

            divisionSelector.DataSource = divisions;
            divisionSelector.DataBind ();
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override string TargetItemKey
        {
            get { return ForModel; }
        }

        protected override void OnLoadItem (EduProgramDivisionEditModel item)
        {
            divisionSelector.DivisionId = item.DivisionId;
            textDivisionRole.Text = item.DivisionRole;
            hiddenDivisionID.Value = item.DivisionId.ToString ();
        }

        protected override void OnUpdateItem (EduProgramDivisionEditModel item)
        {
            item.DivisionId = (int) divisionSelector.DivisionId;
            item.DivisionRole = textDivisionRole.Text.Trim ();

            using (var modelContext = new UniversityModelContext ()) {
                var division = modelContext.Get<DivisionInfo,int> (item.DivisionId);
                item.StartDate = division.StartDate;
                item.EndDate = division.EndDate;
                item.DivisionTitle = division.Title;
            }
        }

        protected override void OnResetForm ()
        {
            divisionSelector.DivisionId = null;
            textDivisionRole.Text = string.Empty;
        }

        protected override void BindItems (IEnumerable<EduProgramDivisionEditModel> items)
        {
            base.BindItems (items);

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion
    }
}
