//
//  EditDivisions.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules;
using R7.University.Controls.ViewModels;
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

        protected override void OnInitControls ()
        {
            InitControls (gridDivisions, hiddenDivisionItemID,
                          buttonAddDivision, buttonUpdateDivision, buttonCancelEditDivision, buttonResetForm);
        }

        protected override void OnLoadItem (EduProgramDivisionEditModel item)
        {
            divisionSelector.DivisionId = item.DivisionId;
            textDivisionRole.Text = item.DivisionRole;
        }

        protected override void OnUpdateItem (EduProgramDivisionEditModel item)
        {
            item.DivisionId = (int) divisionSelector.DivisionId;
            item.DivisionTitle = divisionSelector.DivisionTitle;
            item.DivisionRole = textDivisionRole.Text.Trim ();
        }

        protected override void OnResetForm ()
        {
            divisionSelector.DivisionId = null;
            textDivisionRole.Text = string.Empty;
        }

        #endregion
    }
}
