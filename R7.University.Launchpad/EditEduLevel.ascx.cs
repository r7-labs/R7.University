//
//  EditEduLevel.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2020 Roman M. Yagodin
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
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public partial class EditEduLevel: UniversityEditPortalModuleBase<EduLevelInfo>
    {
        protected EditEduLevel () : base ("edulevel_id")
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            comboParentEduLevel.DataSource = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboParentEduLevel.DataBind ();
            comboParentEduLevel.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (EduLevelInfo item)
        {
            return item?.Title;
        }

        protected override void LoadItem (EduLevelInfo item)
        {
            base.LoadItem (item);

            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            textSortIndex.Text = item.SortIndex.ToString ();
            comboParentEduLevel.SelectByValue (item.ParentEduLevelId);
        }

        protected override void BeforeUpdateItem (EduLevelInfo item, bool isNew)
        {
            item.Title = textTitle.Text.Trim ();
            item.ShortTitle = textShortTitle.Text.Trim ();
            item.SortIndex = ParseHelper.ParseToNullable<int> (textSortIndex.Text) ?? 0;
            item.ParentEduLevelId = ParseHelper.ParseToNullable<int> (comboParentEduLevel.SelectedValue, true);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EduLevelInfo item) => item.EduLevelID;

        protected override void AddItem (EduLevelInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (EduLevelInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduLevelInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}

