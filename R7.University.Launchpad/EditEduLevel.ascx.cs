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
using System.Linq;
using DotNetNuke.Common.Utilities;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Data;

namespace R7.University.Launchpad
{
    public partial class EditEduLevel: EditPortalModuleBase<EduLevelInfo,int>
    {
        protected EditEduLevel () : base ("edulevel_id")
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            var eduProgramLevels = UniversityRepository.Instance.GetEduProgramLevels ().ToList ();
            eduProgramLevels.Insert (0, new EduLevelInfo {
                    EduLevelID = Null.NullInteger,
                    ParentEduLevelId = null,
                    Title = LocalizeString ("NotSelected.Text")
                }
            );

            comboParentEduLevel.DataSource = eduProgramLevels;
            comboParentEduLevel.DataBind ();
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
            return UniversityRepository.Instance.DataProvider.Get<EduLevelInfo> (itemId);
        }

        protected override int AddItem (EduLevelInfo item)
        {
            UniversityRepository.Instance.DataProvider.Add<EduLevelInfo> (item);
            return item.EduLevelID;
        }

        protected override void UpdateItem (EduLevelInfo item)
        {
            UniversityRepository.Instance.DataProvider.Update<EduLevelInfo> (item);
        }

        protected override void DeleteItem (EduLevelInfo item)
        {
            UniversityRepository.Instance.DataProvider.Delete<EduLevelInfo> (item);
        }

        #endregion
    }
}

