//
// SettingsEmployee.ascx.cs
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
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University;
using R7.University.Data;

namespace R7.University.Employee
{
    public partial class SettingsEmployee : ModuleSettingsBase<EmployeeSettings>
    {
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // bind employees to the combobox
            comboEmployees.DataSource = UniversityRepository.Instance.DataProvider.GetObjects<EmployeeInfo> ().OrderBy (em => em.LastName);
            comboEmployees.DataBind ();

            // add default item
            comboEmployees.Items.Insert (
                0,
                new ListItem (
                    LocalizeString ("NotSelected.Text"),
                    Null.NullInteger.ToString ()));
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (AJAX.IsInstalled ())
                    AJAX.RegisterScriptManager ();

                if (!IsPostBack) {
                    if (!Null.IsNull (Settings.EmployeeID))
                        comboEmployees.SelectByValue (Settings.EmployeeID);
                    else
                        comboEmployees.SelectedIndex = 0;

                    checkAutoTitle.Checked = Settings.AutoTitle;
                    checkShowCurrentUser.Checked = Settings.ShowCurrentUser;
					
                    if (!Null.IsNull (Settings.PhotoWidth))
                        textPhotoWidth.Text = Settings.PhotoWidth.ToString ();
					
                    if (!Null.IsNull (Settings.DataCacheTime))
                        textDataCacheTime.Text = Settings.DataCacheTime.ToString ();
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings ()
        {
            try {
                Settings.ShowCurrentUser = checkShowCurrentUser.Checked;

                Settings.EmployeeID = int.Parse (comboEmployees.SelectedValue);

                Settings.AutoTitle = checkAutoTitle.Checked;

                if (!string.IsNullOrWhiteSpace (textPhotoWidth.Text))
                    Settings.PhotoWidth = int.Parse (textPhotoWidth.Text);
                else
                    Settings.PhotoWidth = Null.NullInteger;
				
                if (!string.IsNullOrWhiteSpace (textDataCacheTime.Text))
                    Settings.DataCacheTime = int.Parse (textDataCacheTime.Text);
                else
                    Settings.DataCacheTime = Null.NullInteger;
				
                ModuleController.SynchronizeModule (ModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

