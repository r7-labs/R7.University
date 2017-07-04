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
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;
using R7.University.ModelExtensions;

namespace R7.University.Employee.ViewModels
{
    [Serializable]
    public class EmployeeAchievementEditModel: IEmployeeAchievementWritable, IViewModel
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

        #endregion

        public string Type { get; set; }

        #region Bindable properties

        public int ItemID { get; set; }

        public string Years_String
        {
            get { return FormatHelper.FormatYears (YearBegin, YearEnd,
                                                   Localization.GetString ("AtTheMoment.Text", Context.LocalResourceFile)); }
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

        protected ViewModelContext Context;

        public void SetContext (ViewModelContext context)
        {
            Context = context;
        }

        public EmployeeAchievementEditModel ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public EmployeeAchievementEditModel (IEmployeeAchievementWritable achievement) : this ()
        {
            CopyCstor.Copy (achievement, this);

            // use base achievement values
            if (achievement.Achievement != null) {
                Title = achievement.Achievement.Title;
                ShortTitle = achievement.Achievement.ShortTitle;
                AchievementTypeId = achievement.Achievement.AchievementTypeId;
                if (achievement.Achievement.AchievementType != null) {
                    Type = achievement.Achievement.AchievementType.Type;
                }
            }
            else if (achievement.AchievementType != null) {
                Type = achievement.AchievementType.Type;
            }
        }

        public EmployeeAchievementInfo NewEmployeeAchievementInfo ()
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
    }
}
