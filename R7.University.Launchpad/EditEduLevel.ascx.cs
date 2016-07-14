//
//  EditEduLevel.ascx.cs
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
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;
using R7.University.ControlExtensions;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public partial class EditEduLevel: EditPortalModuleBase<EduLevelInfo,int>
    {
        #region Model context

        private UniversityModelContext modelContext;
        protected UniversityModelContext ModelContext
        {
            get { return modelContext ?? (modelContext = new UniversityModelContext ()); }
        }

        public override void Dispose ()
        {
            if (modelContext != null) {
                modelContext.Dispose ();
            }

            base.Dispose ();
        }

        #endregion

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

        protected override void LoadItem (EduLevelInfo item)
        {
            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            textSortIndex.Text = item.SortIndex.ToString ();
            comboParentEduLevel.SelectByValue (item.ParentEduLevelId);
        }

        protected override void OnButtonUpdateClick (object sender, EventArgs e)
        {
            // HACK: Dispose current model context used in load to create new one for update
            modelContext.Dispose ();
            modelContext = null;

            base.OnButtonUpdateClick (sender, e);
        }

        protected override void BeforeUpdateItem (EduLevelInfo item)
        {
            item.Title = textTitle.Text.Trim ();
            item.ShortTitle = textShortTitle.Text.Trim ();
            item.SortIndex = TypeUtils.ParseToNullable<int> (textSortIndex.Text) ?? 0;
            item.ParentEduLevelId = TypeUtils.ParseToNullable<int> (comboParentEduLevel.SelectedValue);
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override EduLevelInfo GetItem (int itemId)
        {
            return ModelContext.Get<EduLevelInfo> (itemId);
        }

        protected override int AddItem (EduLevelInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();

            return item.EduLevelID;
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

