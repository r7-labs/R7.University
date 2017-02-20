//
//  EditAchievement.ascx.cs
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

using System;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditAchievement : EditPortalModuleBase<AchievementInfo, int>
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

        protected EditAchievement () : base ("achievement_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            comboAchievementType.DataSource = AchievementTypeInfo.GetLocalizedAchievementTypes (LocalizeString);
            comboAchievementType.DataBind ();
        }

        protected override void LoadItem (AchievementInfo item)
        {
            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            comboAchievementType.SelectByValue (item.AchievementType);
        }

        protected override void BeforeUpdateItem (AchievementInfo item)
        {
            item.Title = textTitle.Text.Trim ();
            item.ShortTitle = textShortTitle.Text.Trim ();
            item.AchievementType = (AchievementType) Enum.Parse (typeof (AchievementType), comboAchievementType.SelectedValue);
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override AchievementInfo GetItem (int itemId)
        {
            return ModelContext.Get<AchievementInfo> (itemId);
        }

        protected override void AddItem (AchievementInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (AchievementInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (AchievementInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}

