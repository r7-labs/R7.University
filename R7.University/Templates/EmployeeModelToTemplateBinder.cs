//
//  EmployeeModelToTemplateBinder.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2020 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.University.Core.Templates;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Templates
{
    public class EmployeeToTemplateBinder: ModelToTemplateBinderBase
    {
        protected readonly IEmployee Model;

        protected readonly PortalSettings PortalSettings;

        protected readonly string ResourceFileRoot;

        protected readonly IList<OccupiedPositionInfo> Positions;

        protected readonly IList<EmployeeAchievementInfo> Education;

        protected readonly IList<EmployeeAchievementInfo> Training;

        protected readonly IList<EmployeeAchievementInfo> Achievements;

        protected readonly IList<EmployeeDisciplineInfo> Disciplines;

        string GetString (string key)
        {
            return Localization.GetString (key, ResourceFileRoot);
        }

        public EmployeeToTemplateBinder (IEmployee model, PortalSettings portalSettings, string resourceFileRoot)
        {
            Model = model;
            PortalSettings = portalSettings;

            var now = DateTime.Now;

            Positions = new List<OccupiedPositionInfo> (Model.Positions
                .Where (op => op.Division.IsPublished (now)));

            Disciplines = new List<EmployeeDisciplineInfo> (Model.Disciplines
                .Where (ed => ed.EduProgramProfile.EduProgram.IsPublished (now) && ed.EduProgramProfile.IsPublished (now)));

            Education = Model.EducationAchievements ().ToList ();
            Training = Model.TrainingAchievements ().ToList ();
            Achievements = Model.OtherAchievements ().ToList ();
        }

        public override string Eval (string objectName)
        {
            // TODO: Add simple bindings via configuration
            if (objectName == NameOf (() => Model.LastName)) {
                return Model.LastName;
            }
            if (objectName == NameOf (() => Model.FirstName)) {
                return Model.FirstName;
            }
            if (objectName == NameOf (() => Model.OtherName)) {
                return Model.OtherName;
            }
            if (objectName == NameOf (() => Model.Phone)) {
                return Model.Phone;
            }
            if (objectName == NameOf (() => Model.CellPhone)) {
                return Model.CellPhone;
            }
            if (objectName == NameOf (() => Model.Fax)) {
                return Model.Fax;
            }
            if (objectName == NameOf (() => Model.Email)) {
                return Model.Email;
            }
            if (objectName == NameOf (() => Model.SecondaryEmail)) {
                return Model.SecondaryEmail;
            }
            if (objectName == NameOf (() => Model.Messenger)) {
                return Model.Messenger;
            }
            if (objectName == NameOf (() => Model.WebSite)) {
                return Model.WebSite;
            }
            if (objectName == NameOf (() => Model.WebSiteLabel)) {
                return Model.WebSiteLabel;
            }
            if (objectName == NameOf (() => Model.ScienceIndexAuthorId)) {
                return Model.ScienceIndexAuthorId.ToString ();
            }
            if (objectName == "PhotoUrl") {
                return GetFullFileUrl (Model.PhotoFileID);
            }
            if (objectName == "AltPhotoUrl") {
                return GetFullFileUrl (Model.AltPhotoFileId);
            }
            if (objectName == "UserEmail") {
                if (Model.UserID != null) {
                    var user = UserController.Instance.GetUserById (PortalSettings.PortalId, Model.UserID.Value);
                    if (user != null) {
                        return user.Username;
                    }
                }
            }
            if (objectName == NameOf (() => Model.WorkingPlace)) {
                return Model.WorkingPlace;
            }
            if (objectName == NameOf (() => Model.WorkingHours)) {
                return Model.WorkingHours;
            }
            if (objectName == "About") {
                // TODO: Strip also HTML entities?
                return HtmlUtils.StripTags (HttpUtility.HtmlDecode (Model.Biography), true);
            }

            if (objectName == NameOf (() => Model.ExperienceYears)) {
                return Model.ExperienceYears.ToString ();
            }
            if (objectName == NameOf (() => Model.ExperienceYearsBySpec)) {
                return Model.ExperienceYearsBySpec.ToString ();
            }

            return null;
        }

        string GetFullFileUrl (int? fileId)
        {
            var relativeUrl = GetFileUrl (fileId);
            if (relativeUrl != null) {
                return Globals.AddHTTP (PortalSettings.PortalAlias.HTTPAlias) + relativeUrl;
            }

            return null;
        }


        string GetFileUrl (int? fileId)
        {
            if (fileId != null) {
                var file = FileManager.Instance.GetFile (fileId.Value);
                if (file != null) {
                    var url = FileManager.Instance.GetUrl (file);
                    var cacheBusterParamIndex = url.LastIndexOf ("?ver=", StringComparison.Ordinal);
                    if (cacheBusterParamIndex >= 0) {
                        return url.Substring (0, cacheBusterParamIndex);
                    }
                    return url;
                }
            }
            return null;
        }

        public override string Eval (string objectName, string collectionName, int index)
        {
            if (collectionName == nameof (Positions)) {
                var position = Positions [index];
                if (objectName == "PositionTitle") {
                    return position.Position.Title;
                }
                if (objectName == "DivisionTitle") {
                    return position.Division.Title;
                }
                if (objectName == NameOf (() => position.IsPrime)) {
                    return Positions [index].IsPrime ? GetString ("Yes") : GetString ("No");
                }
            }

            if (collectionName == nameof (Disciplines)) {
                var discipline = Disciplines [index];
                var profile = discipline.EduProgramProfile;
                if (objectName == NameOf (() => profile.EduProgram)) {
                    return UniversityFormatHelper.FormatEduProgramTitle (profile.EduProgram.Code, profile.EduProgram.Title);
                }
                if (objectName == NameOf (() => Disciplines [index].EduProgramProfile)) {
                    return UniversityFormatHelper.FormatEduProgramProfilePartialTitle (profile.ProfileCode, profile.ProfileTitle);
                }
                if (objectName == NameOf (() => profile.EduLevel)) {
                    return profile.EduLevel.Title;
                }
                if (objectName == NameOf (() => discipline.Disciplines)) {
                    return discipline.Disciplines;
                }
            }

            if (collectionName == nameof (Education)) {
                var education = Education [index];
                return EvalAchievement (education, objectName);
            }

            if (collectionName == nameof (Training)) {
                var training = Training [index];
                return EvalAchievement (training, objectName);
            }

            if (collectionName == nameof (Achievements)) {
                var achievement = Achievements [index];
                return EvalAchievement (achievement, objectName);
            }

            return null;
        }

        string EvalAchievement (IEmployeeAchievement achievement, string objectName)
        {
            // TODO: Bind via viewmodel 
            // FIXME: Type not localized properly
            if (objectName == "Type") {
                return ((achievement.Achievement != null)? achievement.Achievement.AchievementType : achievement.AchievementType)
                    .Localize (ResourceFileRoot);
            }
            if (objectName == NameOf (() => achievement.Title)) {
                return (achievement.Achievement != null) ? achievement.Achievement.Title : achievement.Title;
            }

            if (objectName == "Years") {
                return UniversityFormatHelper.FormatYears (
                    achievement.YearBegin,
                    achievement.YearEnd,
                    GetString ("AtTheMoment.Text"));
            }
            if (objectName == NameOf (() => achievement.TitleSuffix)) {
                return achievement.TitleSuffix;
            }
            if (objectName == NameOf (() => achievement.IsTitle)) {
                return achievement.IsTitle ? GetString ("Yes") : GetString ("No");
            }
            if (objectName == NameOf (() => achievement.Description)) {
                return achievement.Description;
            }
            if (objectName == "DocumentUrl") {
                switch (Globals.GetURLType (achievement.DocumentURL)) {
                    case TabType.Tab: return Globals.NavigateURL (int.Parse (achievement.DocumentURL));
                    case TabType.File: return GetFullFileUrl (int.Parse (achievement.DocumentURL.ToUpperInvariant ().Replace ("FILEID=", "")));
                    default: return achievement.DocumentURL;
                }
            }

            return null;
        }

        public override int Count (string collectionName)
        {
            if (collectionName == nameof (Positions)) {
                return Positions.Count;
            }
            if (collectionName == nameof (Disciplines)) {
                return Disciplines.Count;
            }
            if (collectionName == nameof (Education)) {
                return Education.Count;
            }
            if (collectionName == nameof (Training)) {
                return Training.Count;
            }
            if (collectionName == nameof (Achievements)) {
                return Achievements.Count;
            }

            return 0;
        }
    }
}
