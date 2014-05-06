//
// SettingsDivision.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.Division
{
	public partial class SettingsDivision : DivisionModuleSettingsBase
	{
		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{
			try
			{
				if (!IsPostBack)
				{
					// get divisions
					var divisions = DivisionController.GetObjects<DivisionInfo>("ORDER BY [Title] ASC").ToList();

					// insert default item
					divisions.Insert (0, new DivisionInfo() { 
						DivisionID = Null.NullInteger, 
						ParentDivisionID = null,
						Title = Localization.GetString("NotSelected.Text", LocalResourceFile),
						ShortTitle = Localization.GetString("NotSelected.Text", LocalResourceFile)
					});

					// bind list to a tree
					treeDivisions.DataSource = divisions;
					treeDivisions.DataBind();

					// select currently stored value
					var treeNode = treeDivisions.FindNodeByValue(DivisionSettings.DivisionID.ToString());
					if (treeNode != null)
					{
						treeNode.Selected = true;

						// expand all parent nodes
						treeNode = treeNode.ParentNode;
						while (treeNode != null)
						{
							treeNode.Expanded = true;
							treeNode = treeNode.ParentNode;
						} 
					}

					textBarcodeWidth.Text = DivisionSettings.BarcodeWidth.ToString();
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
				DivisionSettings.DivisionID = int.Parse(treeDivisions.SelectedValue);
				DivisionSettings.BarcodeWidth = int.Parse (textBarcodeWidth.Text);

				Utils.SynchronizeModule (this);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

