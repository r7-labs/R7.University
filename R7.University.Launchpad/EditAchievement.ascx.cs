//
//  EditAchievement.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Launchpad
{
    public partial class EditAchievement : PortalModuleBase
    {
        private int? itemId = null;

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

        #region Handlers

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // set url for Cancel link
            linkCancel.NavigateUrl = UrlHelper.GetCancelUrl (UrlHelper.IsInPopup (Request));

            // add confirmation dialog to delete button
            buttonDelete.Attributes.Add (
                "onClick",
                "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");

            // bind achievement types
            comboAchievementType.DataSource = AchievementTypeInfo.GetLocalizedAchievementTypes (LocalizeString);
            comboAchievementType.DataBind ();
        }

        /// <summary>
        /// Handles Load event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
			
            try {
                // parse querystring parameters
                itemId = TypeUtils.ParseToNullable<int> (Request.QueryString ["achievement_id"]);

                if (!IsPostBack) {
                    // load the data into the control the first time we hit this page

                    // check we have an item to lookup
                    // ALT: if (!Null.IsNull (itemId) 
                    if (itemId.HasValue) {

                        // load the item
                        var item = ModelContext.Get<AchievementInfo> (itemId.Value);

                        if (item != null) {
                            textTitle.Text = item.Title;
                            textShortTitle.Text = item.ShortTitle;
                            comboAchievementType.SelectByValue (item.AchievementType);
                        }
                        else
                            Response.Redirect (Globals.NavigateURL (), true);
                    }
                    else {
                        buttonDelete.Visible = false;
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        /// <summary>
        /// Handles Click event for Update button
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// Event args.
        /// </param>
        protected void buttonUpdate_Click (object sender, EventArgs e)
        {
            try {
                AchievementInfo item;

                // determine if we are adding or updating
                // ALT: if (Null.IsNull (itemId))
                if (!itemId.HasValue) {
                    // add new record
                    item = new AchievementInfo ();
                }
                else {
                    // update existing record
                    item = ModelContext.Get<AchievementInfo> (itemId.Value);
                }

                // fill the object
                item.Title = textTitle.Text.Trim ();
                item.ShortTitle = textShortTitle.Text.Trim ();
                item.AchievementType = (AchievementType) Enum.Parse (typeof (AchievementType), comboAchievementType.SelectedValue);

                if (!itemId.HasValue) {
                    ModelContext.Add<AchievementInfo> (item);
                }
                else {
                    ModelContext.Update<AchievementInfo> (item);
                }

                ModelContext.SaveChanges ();

                ModuleController.SynchronizeModule (ModuleId);

                Response.Redirect (Globals.NavigateURL (), true);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        /// <summary>
        /// Handles Click event for Delete button
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// Event args.
        /// </param>
        protected void buttonDelete_Click (object sender, EventArgs e)
        {
            try {
                if (itemId.HasValue) {
                    
                    var item = ModelContext.Get<AchievementInfo> (itemId.Value);
                    ModelContext.Remove<AchievementInfo> (item);
                    ModelContext.SaveChanges ();

                    Response.Redirect (Globals.NavigateURL (), true);
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion
    }
}

