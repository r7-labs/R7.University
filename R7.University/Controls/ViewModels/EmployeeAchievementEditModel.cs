//
//  EmployeeAchievementEditModel.cs
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
using System.Xml.Serialization;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Controls.ViewModels
{
    [Serializable]
    public class EmployeeAchievementEditModel: IEmployeeAchievementWritable, IEditControlViewModel<EmployeeAchievementInfo>
    {
        #region IEmployeeAchievementWritable implementation

        public int EmployeeAchievementID { get; set; }

        public int EmployeeID { get; set; }

        public int? AchievementID { get; set; }

        public int? AchievementTypeId { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string Description { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        [XmlIgnore]
        [Obsolete ("Use AchievementTypeId and Type properties directly", true)]
        public AchievementTypeInfo AchievementType { get; set; }

        [XmlIgnore]
        [Obsolete ("Use AchievementTypeId and Type properties directly", true)]
        public AchievementInfo Achievement { get; set; }

        public string Type { get; set; }

        #endregion

        #region Bindable properties

        public string Years_String
        {
            get {
                return FormatHelper.FormatYears (YearBegin, YearEnd, 
                                                 Localization.GetString ("AtTheMoment.Text", Context.LocalResourceFile));
            }
        }

        public string AchievementType_String
        {
            get {
                
                // TODO: Don't create new object here?
                var achievementType = (AchievementTypeId != null) ? new AchievementTypeInfo { Type = Type } : null;
                return achievementType.Localize (Context.LocalResourceFile); 
            }
        }

        public string Title_String
        { 
            get { return Title + " " + TitleSuffix; }
        }

        public string DocumentUrl_Link
        {
            get {
                if (!string.IsNullOrWhiteSpace (DocumentURL)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                                          UniversityUrlHelper.LinkClickIdnHack (DocumentURL, Context.Module.TabId, Context.Module.ModuleId),
                                          Localization.GetString ("DocumentUrl.Text", Context.LocalResourceFile));
                }

                return string.Empty;
            }
        }

        #endregion

        #region IEditControlViewModel implementation

        public int ViewItemID { get; set; }

        [XmlIgnore]
        public ViewModelContext Context { get; set; }

        ModelEditState _editState;
        public ModelEditState EditState {
            get { return _editState; }
            set { PrevEditState = _editState; _editState = value; }
        }

        public ModelEditState PrevEditState { get; set; }

        public void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EmployeeID = targetItemId;
        }

        [XmlIgnore]
        public string CssClass {
            get {
                var cssClass = string.Empty;
                if (EditState == ModelEditState.Deleted) {
                    cssClass += " u8y-deleted";
                } else if (EditState == ModelEditState.Added) {
                    cssClass += " u8y-added";
                } else if (EditState == ModelEditState.Modified) {
                    cssClass += " u8y-updated";
                }

                return cssClass.TrimStart ();
            }
        }

        public EmployeeAchievementInfo CreateModel ()
        {
            var achievement = new EmployeeAchievementInfo ();
            CopyCstor.Copy<IEmployeeAchievementWritable> (this, achievement);

            if (achievement.AchievementID != null) {
                achievement.Title = null;
                achievement.ShortTitle = null;
                achievement.AchievementTypeId = null;
            }

            return achievement;
        }

        public IEditControlViewModel<EmployeeAchievementInfo> Create (EmployeeAchievementInfo model, ViewModelContext context)
        {
            var viewModel = new EmployeeAchievementEditModel ();
            CopyCstor.Copy<IEmployeeAchievementWritable> (model, viewModel);
            viewModel.Context = context;

            if (model.Achievement != null) {
                viewModel.Title = model.Achievement.Title;
                viewModel.ShortTitle = model.Achievement.ShortTitle;
                viewModel.AchievementTypeId = model.Achievement.AchievementTypeId;
                if (model.Achievement.AchievementType != null) {
                    viewModel.Type = model.Achievement.AchievementType.Type;
                }
            }
            else if (model.AchievementType != null) {
                viewModel.Type = model.AchievementType.Type;
            }

            return viewModel;
        }

        #endregion

        public EmployeeAchievementEditModel ()
        {
            ViewItemID = ViewNumerator.GetNextItemID ();
        }
    }
}
