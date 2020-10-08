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
using R7.University.Core.Markdown;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Templates
{
    public class EmployeeToTemplateBinder: UniversityModelToTemplateBinderBase
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
            ResourceFileRoot = resourceFileRoot;

            var now = DateTime.Now;

            Positions = new List<OccupiedPositionInfo> (Model.Positions
                .Where (op => op.Division.IsPublished (now)));

            Disciplines = new List<EmployeeDisciplineInfo> (Model.Disciplines
                .Where (ed => ed.EduProgramProfile.EduProgram.IsPublished (now) && ed.EduProgramProfile.IsPublished (now)));

            Education = Model.EducationAchievements ().ToList ();
            Training = Model.TrainingAchievements ().ToList ();
            Achievements = Model.OtherAchievements ().ToList ();

            ConfigureBindings ();
        }

        void ConfigureBindings ()
        {
            AddBinding (() => Model.FirstName);
            AddBinding (() => Model.LastName);
            AddBinding (() => Model.OtherName);
            AddBinding (() => Model.Phone);
            AddBinding (() => Model.CellPhone);
            AddBinding (() => Model.Fax);
            AddBinding (() => Model.Email);
            AddBinding (() => Model.SecondaryEmail);
            AddBinding (() => Model.Messenger);
            AddBinding (() => Model.WebSite);
            AddBinding (() => Model.WebSiteLabel);
            AddBinding (() => Model.WorkingPlace);
            AddBinding (() => Model.WorkingHours);

            AddBinding (NameOf (() => Model.EmployeeID), () => Model.EmployeeID.ToString ());
            AddBinding (NameOf (() => Model.ScienceIndexAuthorId), () => Model.ScienceIndexAuthorId.ToString ());
            AddBinding (NameOf (() => Model.ExperienceYears), () => Model.ExperienceYears.ToString ());
            AddBinding (NameOf (() => Model.ExperienceYearsBySpec), () => Model.ExperienceYearsBySpec.ToString ());

            AddBinding ("PhotoUrl", () => GetFullFileUrl (Model.PhotoFileID));
            AddBinding ("AltPhotoUrl", () => GetFullFileUrl (Model.AltPhotoFileId));
            AddBinding ("UserName", () => GetUserName (Model.UserID, PortalSettings));
            AddBinding ("About", () => GetAboutText (Model.Biography));
        }

        public override string Eval (string objectName)
        {
            return base.Eval (objectName);
        }

        string GetUserName (int? userId, PortalSettings portalSettings)
        {
            if (userId != null) {
                var user = UserController.Instance.GetUserById (portalSettings.PortalId, userId.Value);
                if (user != null) {
                    return user.Username;
                }
            }
            return null;
        }

        string GetAboutText (string htmlAbout)
        {
            if (!string.IsNullOrEmpty (htmlAbout)) {
                return HtmlUtils.StripTags (MarkdownHelper.PreprocessHtml (HttpUtility.HtmlDecode (HttpUtility.HtmlDecode (htmlAbout))), true);
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
                    return position.FormatTitle ();
                }
                if (objectName == "DivisionTitle") {
                    return position.Division.Title;
                }
                if (objectName == NameOf (() => position.IsPrime)) {
                    return Positions [index].IsPrime ? GetString ("Yes") : string.Empty;
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
            // TODO: Bind via viewmodel?
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
                return achievement.IsTitle ? GetString ("Yes") : string.Empty;
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
