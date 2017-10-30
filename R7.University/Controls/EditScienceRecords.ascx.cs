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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.University.Controls.ViewModels;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditScienceRecords : GridAndFormControlBase<ScienceRecordInfo, ScienceRecordEditModel>
    {
        public string ForModel { get; set; }

        public void OnInit (PortalModuleBase module, IEnumerable<ScienceRecordTypeInfo> scienceRecordTypes)
        {
            Module = module;
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (ScienceRecordEditModel item)
        {
            hiddenScienceRecordID.Value = item.ScienceRecordId.ToString ();
        }

        protected override void OnUpdateItem (ScienceRecordEditModel item)
        {
        }

        protected override void OnResetForm ()
        {
        }

        protected override void BindItems (IEnumerable<ScienceRecordEditModel> items)
        {
            base.BindItems (items);

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion
    }
}
