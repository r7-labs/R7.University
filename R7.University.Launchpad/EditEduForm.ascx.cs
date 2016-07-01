//
//  EditEduForm.ascx.cs
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
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Data;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditEduForm: EditPortalModuleBase<EduFormInfo,int>
    {
        protected EditEduForm () : base ("eduform_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (EduFormInfo item)
        {
            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            checkIsSystem.Checked = item.IsSystem;

            // disable fields for system items
            textTitle.Enabled = !item.IsSystem;
        }

        protected override void BeforeUpdateItem (EduFormInfo item)
        {
            // don't update fields for system items,
            // also don't update IsSystem value at all
            if (!item.IsSystem) {
                item.Title = textTitle.Text.Trim ();
            }

            item.ShortTitle = textShortTitle.Text.Trim ();
        }

        protected override bool CanDeleteItem (EduFormInfo item)
        {
            return !item.IsSystem;
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override EduFormInfo GetItem (int itemId)
        {
            return UniversityDataProvider.Instance.Get<EduFormInfo> (itemId);
        }

        protected override int AddItem (EduFormInfo item)
        {
            UniversityDataProvider.Instance.Add<EduFormInfo> (item);
            return item.EduFormID;
        }

        protected override void UpdateItem (EduFormInfo item)
        {
            UniversityDataProvider.Instance.Update<EduFormInfo> (item);
        }

        protected override void DeleteItem (EduFormInfo item)
        {
            UniversityDataProvider.Instance.Delete<EduFormInfo> (item);
        }

        #endregion
    }
}
