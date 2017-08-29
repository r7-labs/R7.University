//
//  EditEduForms.ascx.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Controls.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.SerializationModels;
using R7.University.Utilities;

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

        public void OnInit (PortalModuleBase module, IEnumerable<AchievementTypeInfo> achievementTypes, IEnumerable<AchievementInfo> achievements)
        {
            Module = module;

            ViewState ["achievements"] = Json.Serialize (
                achievements.Select (ach => new AchievementSerializationModel (ach)).ToList ());
            ViewState ["achievementTypes"] = Json.Serialize (
                achievementTypes.Select (acht => new AchievementTypeSerializationModel (acht)).ToList ());

            comboAchievement.DataSource = achievements
                .Select (ach => new ListItemViewModel (ach.AchievementID, ach.Title));
            comboAchievement.DataBind ();
            comboAchievement.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
            comboAchievement.SelectedIndex = 0;

            comboAchievementTypes.DataSource = achievementTypes
                .Select (acht => new ListItemViewModel (acht.AchievementTypeId, acht.Localize (LocalResourceFile)));
            comboAchievementTypes.DataBind ();
            comboAchievementTypes.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
            comboAchievementTypes.SelectedIndex = 0;
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (EmployeeAchievementEditModel item)
        {
            if (item.AchievementID != null) {
                comboAchievement.SelectByValue (item.AchievementID);

                panelAchievementTitle.Visible = false;
                panelAchievementShortTitle.Visible = false;
                panelAchievementTypes.Visible = false;
            }
            else {
                comboAchievement.SelectedIndex = 0;

                textAchievementTitle.Text = item.Title;
                textAchievementShortTitle.Text = item.ShortTitle;
                comboAchievementTypes.SelectByValue (item.AchievementTypeId);

                panelAchievementTitle.Visible = true;
                panelAchievementShortTitle.Visible = true;
                panelAchievementTypes.Visible = true;
            }

            textAchievementTitleSuffix.Text = item.TitleSuffix;
            textAchievementDescription.Text = item.Description;
            textYearBegin.Text = item.YearBegin.ToString ();
            textYearEnd.Text = item.YearEnd.ToString ();
            checkIsTitle.Checked = item.IsTitle;

            if (!string.IsNullOrWhiteSpace (item.DocumentURL)) {
                urlDocumentURL.Url = item.DocumentURL;
            }
            else {
                urlDocumentURL.UrlType = "N";
            }
        }

        protected override void OnUpdateItem (EmployeeAchievementEditModel item)
        {
            item.AchievementID = TypeUtils.ParseToNullable<int> (comboAchievement.SelectedValue);
            if (item.AchievementID == null) {
                item.Title = textAchievementTitle.Text.Trim ();
                item.ShortTitle = textAchievementShortTitle.Text.Trim ();
                item.AchievementTypeId = TypeUtils.ParseToNullable<int> (comboAchievementTypes.SelectedValue);
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
            item.YearBegin = TypeUtils.ParseToNullable<int> (textYearBegin.Text);
            item.YearEnd = TypeUtils.ParseToNullable<int> (textYearEnd.Text);
            item.DocumentURL = urlDocumentURL.Url;
        }

        protected override void OnCancelEdit (EmployeeAchievementEditModel item)
        {
            // fix for DnnUrlControl looses its state on postback
            urlDocumentURL.Url = item.DocumentURL;

            base.OnCancelEdit (item);
        }

        protected override void OnResetForm ()
        {
            // restore default panels visibility
            panelAchievementTitle.Visible = true;
            panelAchievementShortTitle.Visible = true;
            panelAchievementTypes.Visible = true;

            // reset controls
            comboAchievement.SelectedIndex = 0;
            comboAchievementTypes.SelectedIndex = 0;
            textAchievementTitle.Text = string.Empty;
            textAchievementShortTitle.Text = string.Empty;
            textAchievementTitleSuffix.Text = string.Empty;
            textAchievementDescription.Text = string.Empty;
            textYearBegin.Text = string.Empty;
            textYearEnd.Text = string.Empty;
            checkIsTitle.Checked = false;
            hiddenViewItemID.Value = string.Empty;
            urlDocumentURL.UrlType = "N";
        }

        #endregion

        protected void comboAchievement_SelectedIndexChanged (object sender, EventArgs e)
        {
            try {
                if (((DropDownList) sender).SelectedValue == Null.NullInteger.ToString ()) {
                    panelAchievementTitle.Visible = true;
                    panelAchievementShortTitle.Visible = true;
                    panelAchievementTypes.Visible = true;
                }
                else {
                    panelAchievementTitle.Visible = false;
                    panelAchievementShortTitle.Visible = false;
                    panelAchievementTypes.Visible = false;
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void gridAchievements_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                var employeeAchievement = (IEmployeeAchievement) e.Row.DataItem;
                e.Row.ToolTip = Server.HtmlDecode (employeeAchievement.Description);
            }
        }
    }
}
