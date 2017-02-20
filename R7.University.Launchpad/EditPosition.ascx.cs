//
//  EditPosition.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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

using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditPosition : EditPortalModuleBase<PositionInfo, int>
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

        protected EditPosition () : base ("position_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (PositionInfo item)
        {
            txtTitle.Text = item.Title;
            txtShortTitle.Text = item.ShortTitle;
            txtWeight.Text = item.Weight.ToString ();
            checkIsTeacher.Checked = item.IsTeacher;
        }

        protected override void BeforeUpdateItem (PositionInfo item)
        {
            item.Title = txtTitle.Text.Trim ();
            item.ShortTitle = txtShortTitle.Text.Trim ();
            item.Weight = TypeUtils.ParseToNullable<int> (txtWeight.Text) ?? 0;
            item.IsTeacher = checkIsTeacher.Checked;
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override PositionInfo GetItem (int itemId)
        {
            return ModelContext.Get<PositionInfo> (itemId);
        }

        protected override void AddItem (PositionInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (PositionInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (PositionInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion

    }
}

