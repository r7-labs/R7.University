//
// EditEduLevel.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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
using R7.University;
using DotNetNuke.R7;

namespace R7.University.Launchpad
{
    public partial class EditEduForm: EditModuleBase<LaunchpadController,LaunchpadSettings,EduFormInfo>
	{
        protected EditEduForm (): base ("eduform_id")
        {}

        protected override void OnInitControls () 
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnLoadItem (EduFormInfo item)
        {
            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            checkIsSystem.Checked = item.IsSystem;

            // disable fields for system items
            textTitle.Enabled = !item.IsSystem;
        }

        protected override void OnUpdateItem (EduFormInfo item)
        {
            // don't update fields for system items,
            // also don't update IsSystem value at all
            if (!item.IsSystem)
            {
                item.Title = textTitle.Text.Trim ();
            }

            item.ShortTitle = textShortTitle.Text.Trim ();
        }

        protected override bool CanDeleteItem (EduFormInfo item)
        {
            return !item.IsSystem;
        }
	}
}
