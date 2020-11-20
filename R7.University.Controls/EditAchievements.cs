using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.FileSystem;
using R7.Dnn.Extensions.Text;
using R7.University.Controls.EditModels;
using R7.University.Controls.SerializationModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditAchievements: GridAndFormControlBase<EmployeeAchievementInfo,EmployeeAchievementEditModel>
    {
        protected AchievementSerializationModel GetAchievement (int achievementId)
        {
            return Json.Deserialize<List<AchievementSerializationModel>> ((string) ViewState ["achievements"])
                                         .Single (ach => ach.AchievementID == achievementId);
        }

        protected AchievementTypeSerializationModel GetAchievementType (int? achievementTypeId)
        {
            return Json.Deserialize<List<AchievementTypeSerializationModel>> ((string) ViewState ["achievementTypes"])
                                         .SingleOrDefault (acht => acht.AchievementTypeId == achievementTypeId);
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            // HACK: Try to fix GH-348 DnnUrlControl looses its state on postback -
            // this will preserve currently selected folder between edits, but broke general URLs (GH-335)
            if (Page.IsPostBack) {
                urlDocumentUrl.Url = urlDocumentUrl.Url;
            }
        }

        string GetDocumentUrl ()
        {
            if (!string.IsNullOrEmpty (txtDocumentUrl.Text.Trim ())) {
                return txtDocumentUrl.Text.Trim ();
            }
            return urlDocumentUrl.Url;
        }

        void SetDocumentUrl (string url)
        {
            var urlType = Globals.GetURLType (url);
            if (urlType == TabType.Tab || urlType == TabType.File) {
                urlDocumentUrl.Url = url;
                txtDocumentUrl.Text = string.Empty;
            }
            else {
                txtDocumentUrl.Text = url;
            }
        }

        public void OnInit (PortalModuleBase module, IEnumerable<AchievementTypeInfo> achievementTypes, IEnumerable<AchievementInfo> achievements)
        {
            Module = module;

            ViewState ["achievements"] = Json.Serialize (
                achievements.Select (ach => new AchievementSerializationModel (ach)).ToList ());
            ViewState ["achievementTypes"] = Json.Serialize (
                achievementTypes.Select (acht => new AchievementTypeSerializationModel (acht)).ToList ());

            comboAchievement.DataSource = achievements.Select (a => new {
                Value = a.AchievementID,
                Text = !string.IsNullOrEmpty (a.ShortTitle) ? $"{a.Title} ({a.ShortTitle})" : a.Title
            });

            comboAchievement.DataBind ();
            comboAchievement.InsertDefaultItem (LocalizeString ("CustomAchievement.Text"));
            comboAchievement.SelectedIndex = 0;

            comboAchievementTypes.DataSource = achievementTypes.Select (at => new {
                Value = at.AchievementTypeId,
                Text = at.Localize (LocalResourceFile)
            });

            comboAchievementTypes.DataBind ();
            comboAchievementTypes.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
            comboAchievementTypes.SelectedIndex = 0;

            var lastFolderId = FolderHistory.GetLastFolderId (Request, Module.PortalId);
            if (lastFolderId != null) {
                urlDocumentUrl.SelectFolder (lastFolderId.Value);
            }
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (EmployeeAchievementEditModel item)
        {
            if (item.AchievementID != null) {
                comboAchievement.SelectByValue (item.AchievementID);
            }
            else {
                comboAchievement.SelectedIndex = 0;
                textAchievementTitle.Text = item.Title;
                textAchievementShortTitle.Text = item.ShortTitle;
                comboAchievementTypes.SelectByValue (item.AchievementTypeId);
            }

            textAchievementTitleSuffix.Text = item.TitleSuffix;
            textAchievementDescription.Text = item.Description;
            textYearBegin.Text = item.YearBegin.ToString ();
            textYearEnd.Text = item.YearEnd.ToString ();
            checkIsTitle.Checked = item.IsTitle;
            txtHours.Text = item.Hours.ToString ();

            txtEduLevelId.Text = item.EduLevelId.ToString ();

            SetDocumentUrl (item.DocumentURL);
        }

        protected override void OnUpdateItem (EmployeeAchievementEditModel item)
        {
            item.AchievementID = ParseHelper.ParseToNullable<int> (comboAchievement.SelectedValue, true);
            if (item.AchievementID == null) {
                item.Title = textAchievementTitle.Text.Trim ();
                item.ShortTitle = textAchievementShortTitle.Text.Trim ();
                item.AchievementTypeId = ParseHelper.ParseToNullable<int> (comboAchievementTypes.SelectedValue, true);
                var achievementType = GetAchievementType (item.AchievementTypeId);
                item.Type = (achievementType != null)? achievementType.Type : string.Empty;
            }
            else {
                var ach = GetAchievement (int.Parse (comboAchievement.SelectedValue));
                item.Title = ach.Title;
                item.ShortTitle = ach.ShortTitle;
                item.AchievementTypeId = ach.AchievementTypeId;
                var achievementType = GetAchievementType (ach.AchievementTypeId);
                item.Type = (achievementType != null) ? achievementType.Type : string.Empty;
            }

            item.TitleSuffix = textAchievementTitleSuffix.Text.Trim ();
            item.Description = textAchievementDescription.Text.Trim ();
            item.IsTitle = checkIsTitle.Checked;
            item.YearBegin = ParseHelper.ParseToNullable<int> (textYearBegin.Text);
            item.YearEnd = ParseHelper.ParseToNullable<int> (textYearEnd.Text);
            item.DocumentURL = GetDocumentUrl ();
            item.Hours = ParseHelper.ParseToNullable<int> (txtHours.Text);

            item.EduLevelId = ParseHelper.ParseToNullable<int> (txtEduLevelId.Text);
            item.EduLevel_String = GetEduLevelTitle (item.EduLevelId);

            FolderHistory.RememberFolderByFileUrl (Request, Response, item.DocumentURL, Module.PortalId);
        }

        string GetEduLevelTitle (int? eduLevelId)
        {
            if (eduLevelId == null) {
                return string.Empty;
            }

            using (var modelContext = new UniversityModelContext ()) {
                var eduLevel = modelContext.Get<EduLevelInfo, int> (eduLevelId.Value);
                if (eduLevel != null) {
                    return eduLevel.FormatTitle ();
                }
            }

            return string.Empty;
        }

        protected override void OnResetForm ()
        {
            OnPartialResetForm ();

            // reset controls
            comboAchievement.SelectedIndex = 0;
            comboAchievementTypes.SelectedIndex = 0;
            textAchievementTitle.Text = string.Empty;

            textYearBegin.Text = string.Empty;
            textYearEnd.Text = string.Empty;
            hiddenViewItemID.Value = string.Empty;
        }

        protected override void OnPartialResetForm ()
        {
            base.OnPartialResetForm ();

            // reset only secondary fields
            checkIsTitle.Checked = false;
            textAchievementShortTitle.Text = string.Empty;
            textAchievementTitleSuffix.Text = string.Empty;
            textAchievementDescription.Text = string.Empty;

            // TODO: Reset hours and edu. level fields
        }

        #endregion

        protected void gridAchievements_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                var employeeAchievement = (IEmployeeAchievement) e.Row.DataItem;
                e.Row.ToolTip = Server.HtmlDecode (employeeAchievement.Description);
            }
        }
    }
}
