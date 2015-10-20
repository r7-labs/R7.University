//
// EditDocumentType.ascx.cs
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
    public partial class EditDocumentType: EditModuleBase<LaunchpadController,LaunchpadSettings,DocumentTypeInfo>
	{
        protected EditDocumentType (): base ("documenttype_id")
        {}

        protected override void OnInitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override bool CanDeleteItem (DocumentTypeInfo item)
        {
            return !item.IsSystem;
        }

        protected override void OnLoadItem (DocumentTypeInfo item)
        {
            textType.Text = item.Type;
            textDescription.Text = item.Description;
            checkIsSystem.Checked = item.IsSystem;

            // disable textType for system types
            textType.Enabled = !item.IsSystem;
        }

        protected override void OnUpdateItem (DocumentTypeInfo item)
        {
            // don't update Type for system types,
            // also don't update IsSystem value at all

            if (!item.IsSystem)
            {
                item.Type = textType.Text.Trim ();
            }

            item.Description = textDescription.Text.Trim ();
        }
	}
}

