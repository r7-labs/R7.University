//
// SettingsEmployee.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2015 Roman M. Yagodin
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
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.Employee
{
	public partial class SettingsEmployee : EmployeeModuleSettingsBase
	{
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // bind employees to the combobox
            comboEmployees.DataSource = EmployeeController.GetObjects<EmployeeInfo> ().OrderBy (em => em.LastName);
            comboEmployees.DataBind ();

            // add default item
            comboEmployees.Items.Insert (0, new ListItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ()));
        }

		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{
			try
			{
                if (DotNetNuke.Framework.AJAX.IsInstalled ())
                    DotNetNuke.Framework.AJAX.RegisterScriptManager ();

				if (!IsPostBack)
				{
					if (!Null.IsNull (EmployeeSettings.EmployeeID))
                        comboEmployees.SelectByValue (EmployeeSettings.EmployeeID);
                    else
                        comboEmployees.SelectedIndex = 0;

					checkAutoTitle.Checked = EmployeeSettings.AutoTitle;
					checkShowCurrentUser.Checked = EmployeeSettings.ShowCurrentUser;
					
					if (!Null.IsNull (EmployeeSettings.PhotoWidth))
						textPhotoWidth.Text = EmployeeSettings.PhotoWidth.ToString ();
					
					if (!Null.IsNull (EmployeeSettings.DataCacheTime))
						textDataCacheTime.Text = EmployeeSettings.DataCacheTime.ToString ();
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		/// <summary>
		/// handles updating the module settings for this control
		/// </summary>
		public override void UpdateSettings ()
		{
			try
			{
				EmployeeSettings.ShowCurrentUser = checkShowCurrentUser.Checked;

				EmployeeSettings.EmployeeID = int.Parse (comboEmployees.SelectedValue);

				EmployeeSettings.AutoTitle = checkAutoTitle.Checked;

				if (!string.IsNullOrWhiteSpace (textPhotoWidth.Text))
					EmployeeSettings.PhotoWidth = int.Parse (textPhotoWidth.Text);
				else
					EmployeeSettings.PhotoWidth = Null.NullInteger;
				
				if (!string.IsNullOrWhiteSpace (textDataCacheTime.Text))
					EmployeeSettings.DataCacheTime = int.Parse (textDataCacheTime.Text);
				else
					EmployeeSettings.DataCacheTime = Null.NullInteger;
				
				Utils.SynchronizeModule (this);

			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

