//
// SettingsLaunchpad.ascx.cs
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
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University;

namespace R7.University.Launchpad
{
    public partial class SettingsLaunchpad : ModuleSettingsBase<LaunchpadSettings>
    {
        #region Properties

        protected LaunchpadTables LaunchpadTables = new LaunchpadTables ();

        #endregion

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill PageSize combobox
            comboPageSize.AddItem ("10", "10");
            comboPageSize.AddItem ("25", "25");
            comboPageSize.AddItem ("50", "50");
            comboPageSize.AddItem ("100", "100");

            // fill tables list
            foreach (var table in LaunchpadTables.Tables)
                listTables.Items.Add (new Telerik.Web.UI.RadListBoxItem (
                        LocalizeString (table.ResourceKey), table.Name));
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        { 
            try {
                if (!IsPostBack) {
                    // TODO: Allow select nearest pagesize value
                    comboPageSize.Select (Settings.PageSize.ToString (), false);

                    // check table list items
                    foreach (var table in Settings.Tables) {
                        var item = listTables.FindItemByValue (table);
                        if (item != null)
                            item.Checked = true;
                    }
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
                Settings.PageSize = int.Parse (comboPageSize.SelectedValue);
                Settings.Tables = listTables.CheckedItems.Select (i => i.Value).ToList ();

                // remove session variable for active view,
                // since view set may be changed
                Session.Remove ("Launchpad_ActiveView_" + TabModuleId);

                Utils.SynchronizeModule (this);

            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

