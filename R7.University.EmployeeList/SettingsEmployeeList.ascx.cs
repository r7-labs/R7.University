//
// SettingsEmployeeList.ascx.cs
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Data;
using R7.University.EmployeeList.Components;
using R7.University.Utilities;

namespace R7.University.EmployeeList
{
    public partial class SettingsEmployeeList: ModuleSettingsBase<EmployeeListSettings>
    {
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // get divisions
            var divisions = UniversityRepository.Instance.DataProvider.GetObjects<DivisionInfo> ().OrderBy (d => d.Title).ToList ();

            // insert default item
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            // bind divisions to the tree
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();

            // sort type
            comboSortType.AddItem (LocalizeString ("SortTypeByMaxWeight.Text"), "0");
            comboSortType.AddItem (LocalizeString ("SortTypeByTotalWeight.Text"), "1");
            comboSortType.AddItem (LocalizeString ("SortTypeByName.Text"), "2");
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    // select node and expand tree to it
                    Utils.SelectAndExpandByValue (treeDivisions, Settings.DivisionID.ToString ());

                    checkIncludeSubdivisions.Checked = Settings.IncludeSubdivisions;
                    checkHideHeadEmployee.Checked = Settings.HideHeadEmployee;
                    comboSortType.SelectByValue (Settings.SortType);
                    textPhotoWidth.Text = Settings.PhotoWidth.ToString ();
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
                Settings.DivisionID = int.Parse (treeDivisions.SelectedValue);
                Settings.IncludeSubdivisions = checkIncludeSubdivisions.Checked;
                Settings.HideHeadEmployee = checkHideHeadEmployee.Checked;
                Settings.SortType = int.Parse (comboSortType.SelectedValue);
                Settings.PhotoWidth = int.Parse (textPhotoWidth.Text);

                ModuleController.SynchronizeModule (ModuleId);
                CacheHelper.RemoveCacheByPrefix ("//r7_University/Modules/EmployeeList?TabModuleId=" + TabModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

