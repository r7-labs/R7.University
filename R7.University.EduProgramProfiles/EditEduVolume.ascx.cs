//
//  EditEduProgramProfile.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using R7.University.Commands;
using R7.University.Models;
using R7.University.Modules;
using R7.Dnn.Extensions.Utilities;

namespace R7.University.EduProgramProfiles
{
    public partial class EditEduVolume : UniversityEditPortalModuleBase<EduVolumeInfo>
    {
        public enum EditEduVolumeTab
        {
            Common,
            Years,
            Practices
        }

        #region Properties

        protected EditEduVolumeTab SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                /*
                if (!string.IsNullOrEmpty (eventTarget)) {
                    if (eventTarget.Contains ("$" + <practicesControl>.ID)) {
                        ViewState ["SelectedTab"] = EditEduVolumeTab.Practices;
                        return EditEduVolumeTab.Practices;
                    }
                }
                */

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditEduVolumeTab) obj : EditEduVolumeTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        #endregion

        protected EditEduVolume () : base ("eduvolume_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);
        }

        private int? GetQueryId (string key)
        {
            var idParam = Request.QueryString [key];
            if (idParam != null) {
                int Id;
                if (int.TryParse (idParam, out Id)) {
                    return Id;
                }
            }

            return null;
        }

        protected override void LoadItem (EduVolumeInfo item)
        {
            var ev = GetItemWithDependencies (ItemId.Value);

            textTimeToLearnHours.Text = ev.TimeToLearnHours.ToString ();
            textTimeToLearnYears.Text = (ev.TimeToLearnMonths / 12).ToString ();
            textTimeToLearnMonths.Text = (ev.TimeToLearnMonths % 12).ToString ();

            textYear1Cu.Text = ev.Year1Cu.ToString ();
            textYear2Cu.Text = ev.Year2Cu.ToString ();
            textYear3Cu.Text = ev.Year3Cu.ToString ();
            textYear4Cu.Text = ev.Year4Cu.ToString ();
            textYear5Cu.Text = ev.Year5Cu.ToString ();
            textYear6Cu.Text = ev.Year6Cu.ToString ();

            textPracticeType1Cu.Text = ev.PracticeType1Cu.ToString ();
            textPracticeType2Cu.Text = ev.PracticeType2Cu.ToString ();
            textPracticeType3Cu.Text = ev.PracticeType3Cu.ToString ();
        }

        protected override void BeforeUpdateItem (EduVolumeInfo item)
        {
            item.TimeToLearnHours = int.Parse (textTimeToLearnHours.Text);
            item.TimeToLearnMonths = int.Parse (textTimeToLearnYears.Text) * 12 + int.Parse (textTimeToLearnMonths.Text);

            item.Year1Cu = TypeUtils.ParseToNullable<int> (textYear1Cu.Text);
            item.Year2Cu = TypeUtils.ParseToNullable<int> (textYear2Cu.Text);
            item.Year3Cu = TypeUtils.ParseToNullable<int> (textYear3Cu.Text);
            item.Year4Cu = TypeUtils.ParseToNullable<int> (textYear4Cu.Text);
            item.Year5Cu = TypeUtils.ParseToNullable<int> (textYear5Cu.Text);
            item.Year6Cu = TypeUtils.ParseToNullable<int> (textYear6Cu.Text);

            item.PracticeType1Cu = TypeUtils.ParseToNullable<int> (textPracticeType1Cu.Text);
            item.PracticeType2Cu = TypeUtils.ParseToNullable<int> (textPracticeType2Cu.Text);
            item.PracticeType3Cu = TypeUtils.ParseToNullable<int> (textPracticeType3Cu.Text);
        }

        protected override EduVolumeInfo GetItemWithDependencies (int itemId)
        {
            return ModelContext.Get<EduVolumeInfo> (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override void AddItem (EduVolumeInfo item)
        {
            if (SecurityContext.CanAdd (typeof (EduVolumeInfo))) {
                new AddCommand<EduVolumeInfo> (ModelContext, SecurityContext).Add (item);
                ModelContext.SaveChanges ();
            }
        }

        protected override void UpdateItem (EduVolumeInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduVolumeInfo item)
        {
            new DeleteCommand<EduVolumeInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
