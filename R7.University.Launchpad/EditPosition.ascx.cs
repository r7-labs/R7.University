//
// EditPosition.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University;
using R7.University.Data;

namespace R7.University.Launchpad
{
    public partial class EditPosition : PortalModuleBase
    {
        // ALT: private int itemId = Null.NullInteger;
        private int? itemId = null;

        #region Handlers

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // set url for Cancel link
            linkCancel.NavigateUrl = Globals.NavigateURL ();

            // add confirmation dialog to delete button
            buttonDelete.Attributes.Add (
                "onClick",
                "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");
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
                itemId = TypeUtils.ParseToNullable<int> (Request.QueryString ["position_id"]);
      
                if (!IsPostBack) {
                    // load the data into the control the first time we hit this page

                    // check we have an item to lookup
                    // ALT: if (!Null.IsNull (itemId) 
                    if (itemId.HasValue) {
                        // load the item
                        var item = UniversityRepository.Instance.DataProvider.Get<PositionInfo> (itemId.Value);

                        if (item != null) {
											
                            txtTitle.Text = item.Title;
                            txtShortTitle.Text = item.ShortTitle;
                            txtWeight.Text = item.Weight.ToString ();
                            checkIsTeacher.Checked = item.IsTeacher;
														
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
                PositionInfo item;

                // determine if we are adding or updating
                // ALT: if (Null.IsNull (itemId))
                if (!itemId.HasValue) {
                    // add new record
                    item = new PositionInfo ();
                }
                else {
                    // update existing record
                    item = UniversityRepository.Instance.DataProvider.Get<PositionInfo> (itemId.Value);
                }

                // fill the object
                item.Title = txtTitle.Text.Trim ();
                item.ShortTitle = txtShortTitle.Text.Trim ();
                item.Weight = TypeUtils.ParseToNullable<int> (txtWeight.Text) ?? 0;
                item.IsTeacher = checkIsTeacher.Checked;

                if (!itemId.HasValue)
                    UniversityRepository.Instance.DataProvider.Add<PositionInfo> (item);
                else
                    UniversityRepository.Instance.DataProvider.Update<PositionInfo> (item);

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
                // ALT: if (!Null.IsNull (itemId))
                if (itemId.HasValue) {
                    UniversityRepository.Instance.DataProvider.Delete<PositionInfo> (itemId.Value);
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

