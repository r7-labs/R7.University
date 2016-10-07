//
//  EditDocumentType.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Launchpad
{
    public partial class EditDocumentType: EditPortalModuleBase<DocumentTypeInfo,int>
    {
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

        protected EditDocumentType () : base ("documenttype_id")
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // HACK: Cancel link setup must be done in the base class
            linkCancel.Attributes.Remove ("onclick");
            linkCancel.NavigateUrl = UrlHelper.GetCancelUrl (UrlHelper.IsInPopup (Request));
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override bool CanDeleteItem (DocumentTypeInfo item)
        {
            return !item.IsSystem;
        }

        protected override void LoadItem (DocumentTypeInfo item)
        {
            textType.Text = item.Type;
            textDescription.Text = item.Description;
            checkIsSystem.Checked = item.IsSystem;

            // disable textType for system types
            textType.Enabled = !item.IsSystem;
        }

        protected override void BeforeUpdateItem (DocumentTypeInfo item)
        {
            // don't update Type for system types,
            // also don't update IsSystem value at all

            if (!item.IsSystem) {
                item.Type = textType.Text.Trim ();
            }

            item.Description = textDescription.Text.Trim ();
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override DocumentTypeInfo GetItem (int itemId)
        {
            return ModelContext.Get<DocumentTypeInfo> (itemId);
        }

        protected override int AddItem (DocumentTypeInfo item)
        {
            ModelContext.Add<DocumentTypeInfo> (item);
            ModelContext.SaveChanges ();

            return item.DocumentTypeID;
        }

        protected override void UpdateItem (DocumentTypeInfo item)
        {
            ModelContext.Update<DocumentTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (DocumentTypeInfo item)
        {
            ModelContext.Remove<DocumentTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
