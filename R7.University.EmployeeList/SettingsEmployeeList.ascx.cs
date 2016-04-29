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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common.Utilities;
using R7.University;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Data;

namespace R7.University.EmployeeList
{
    public partial class SettingsEmployeeList : ModuleSettingsBase<EmployeeListSettings>
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
			try
			{
				if (!IsPostBack)
				{
			        // select node and expand tree to it
                    Utils.SelectAndExpandByValue (treeDivisions, Settings.DivisionID.ToString ());

					// check / uncheck IncludeSubdivisions
					checkIncludeSubdivisions.Checked = Settings.IncludeSubdivisions;

					comboSortType.Select (Settings.SortType.ToString (), false);

					if (!Null.IsNull (Settings.PhotoWidth))
						textPhotoWidth.Text = Settings.PhotoWidth.ToString ();
					
					if (!Null.IsNull (Settings.DataCacheTime))
						textDataCacheTime.Text = Settings.DataCacheTime.ToString ();
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
				Settings.DivisionID = int.Parse (treeDivisions.SelectedValue);
				Settings.IncludeSubdivisions = checkIncludeSubdivisions.Checked;
				Settings.SortType = int.Parse (comboSortType.SelectedValue);

				if (!string.IsNullOrWhiteSpace (textPhotoWidth.Text))
					Settings.PhotoWidth = int.Parse (textPhotoWidth.Text);
				else
					Settings.PhotoWidth = Null.NullInteger;
				
				if (!string.IsNullOrWhiteSpace (textDataCacheTime.Text))
					Settings.DataCacheTime = int.Parse (textDataCacheTime.Text);
				else
					Settings.DataCacheTime = Null.NullInteger;

				Utils.SynchronizeModule (this);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

