//
//  EditScienceRecordType.cs
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

using R7.University.Models;
using R7.University.Modules;

namespace R7.University.Launchpad
{
    public partial class EditScienceRecordType: UniversityEditPortalModuleBase<ScienceRecordTypeInfo>
    {
        protected EditScienceRecordType () : base ("sciencerecordtype_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (ScienceRecordTypeInfo item)
        {
            textType.Text = item.Type;
            checkIsSystem.Checked = item.IsSystem;
            checkDescriptionIsRequired.Checked = item.DescriptionIsRequired;
            textNumOfValues.Text = item.NumOfValues.ToString ();
            textSortIndex.Text = item.SortIndex.ToString ();

            // disable key fields for system types
            if (item.IsSystem) {
                textType.Enabled = false;
                textNumOfValues.Enabled = false;
                checkDescriptionIsRequired.Enabled = false;
            }
        }

        protected override void BeforeUpdateItem (ScienceRecordTypeInfo item)
        {
            item.SortIndex = int.Parse (textSortIndex.Text);

            // don't update key fields for system types, also don't update IsSystem value at all
            if (!item.IsSystem) {
                item.Type = textType.Text.Trim ();
                item.NumOfValues = int.Parse (textNumOfValues.Text);
                item.DescriptionIsRequired = checkDescriptionIsRequired.Checked;
            }
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override void AddItem (ScienceRecordTypeInfo item)
        {
            ModelContext.Add<ScienceRecordTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (ScienceRecordTypeInfo item)
        {
            ModelContext.Update<ScienceRecordTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (ScienceRecordTypeInfo item)
        {
            ModelContext.Remove<ScienceRecordTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
