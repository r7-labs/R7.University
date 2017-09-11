//
//  EditDocuments.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditDocuments: GridAndFormControlBase<DocumentInfo,DocumentEditModel>
    {
        #region Control properties

        public string ForModel { get; set; }

        #endregion

        public void OnInit (PortalModuleBase module, IEnumerable<DocumentTypeInfo> documentTypes)
        {
            Module = module;

            var documentTypeViewModels = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext);
            ViewState ["documentTypes"] = Json.Serialize (documentTypeViewModels);

            comboDocumentType.DataSource = documentTypeViewModels.OrderBy (dt => dt.LocalizedType);
            comboDocumentType.DataBind ();

            var filenameFormats = new StringBuilder ();
            var first = true;
            foreach (var documentType in documentTypes) {
                if (!first) {
                    filenameFormats.Append (",");
                }
                first = false;
                filenameFormats.Append ($"{{\"id\":\"{documentType.DocumentTypeID}\",\"match\":\"{documentType.FilenameFormat?.Replace ("\\","\\\\")}\"}}");
            }

            comboDocumentType.Attributes.Add ("data-validation", $"[{filenameFormats}]");
            valDocumentUrl.Attributes.Add ("data-message-template", Localization.GetString ("FileName.Invalid", LocalResourceFile));
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            // HACK: Fix DnnUrlControl looses its state on async postback
            if (Page.IsPostBack) {
                urlDocumentUrl.Url = urlDocumentUrl.Url;
            }
        }

        protected DocumentTypeViewModel GetDocumentType (int? documentTypeId)
        {
            if (documentTypeId != null) {
                var documentTypes = Json.Deserialize<List<DocumentTypeViewModel>> ((string) ViewState ["documentTypes"]);
                return documentTypes.Single (dt => dt.DocumentTypeID == documentTypeId.Value);
            }

            return new DocumentTypeViewModel
            {
                Type = string.Empty,
                DocumentTypeID = Null.NullInteger
            };
        }

        #region Implemented abstract members of GridAndFormEditControlBase

        protected override string TargetItemKey
        {
            get { return ForModel; }
        }

        protected override void OnLoadItem (DocumentEditModel item)
        {
            comboDocumentType.SelectByValue (item.DocumentTypeID);
            textDocumentTitle.Text = item.Title;
            textDocumentGroup.Text = item.Group;
            textDocumentSortIndex.Text = item.SortIndex.ToString ();
            datetimeDocumentStartDate.SelectedDate = item.StartDate;
            datetimeDocumentEndDate.SelectedDate = item.EndDate;
            urlDocumentUrl.Url = item.Url;
        }

        protected override void OnUpdateItem (DocumentEditModel item)
        {
            item.Title = textDocumentTitle.Text.Trim ();
            item.Group = textDocumentGroup.Text.Trim ();
            item.DocumentTypeID = int.Parse (comboDocumentType.SelectedValue);
            item.DocumentTypeViewModel = GetDocumentType (item.DocumentTypeID);
            item.SortIndex = TypeUtils.ParseToNullable<int> (textDocumentSortIndex.Text) ?? 0;
            item.StartDate = datetimeDocumentStartDate.SelectedDate;
            item.EndDate = datetimeDocumentEndDate.SelectedDate;
            item.Url = urlDocumentUrl.Url;
        }

        protected override void OnResetForm ()
        {
            textDocumentTitle.Text = string.Empty;
            textDocumentGroup.Text = string.Empty;
            comboDocumentType.SelectedIndex = 0;
            textDocumentSortIndex.Text = "0";
            datetimeDocumentStartDate.SelectedDate = null;
            datetimeDocumentEndDate.SelectedDate = null;
        }

        #endregion

        public override void SetData (IEnumerable<DocumentInfo> items, int targetItemId)
        {
            base.SetData (
                items.OrderByDescending (d => d.Group, DocumentGroupComparer.Instance)
                .ThenBy (d => d.DocumentType.DocumentTypeID)
                .ThenBy (d => d.SortIndex)
                , targetItemId
            );

            // speedup adding new documents by autoselecting first document's folder
            if (items.Any ()) {
                var firstItem = items.FirstOrDefault (d => Globals.GetURLType (d.Url) == TabType.File);
                if (firstItem != null) {
                    urlDocumentUrl.Url = firstItem.Url;
                }
            }
        }
    }
}
