//
// EditDocuments.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;
using R7.University.Data;

namespace R7.University.Controls
{
    public partial class EditDocuments: 
        GridAndFormEditControlBase<DocumentInfo,DocumentViewModel>
    {
        #region Control properties

        public string ForModel { get; set; }

        #endregion

        protected override string TargetItemKey
        {
            get { return ForModel + "ID="; }
        }

        public void OnInit (PortalModuleBase module, IEnumerable<DocumentTypeInfo> documentTypes)
        {
            Module = module;

            var documentTypeViewModels = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext, true);
            ViewState ["documentTypes"] = XmlSerializationHelper.Serialize (documentTypeViewModels);

            comboDocumentType.DataSource = documentTypeViewModels.OrderBy (dt => dt.LocalizedType);
            comboDocumentType.DataBind ();
        }

        protected DocumentTypeViewModel GetDocumentType (int? documentTypeId)
        {
            if (documentTypeId != null) {
                var documentTypes = XmlSerializationHelper.Deserialize<List<DocumentTypeViewModel>> (ViewState ["documentTypes"]);
                return documentTypes.Single (dt => dt.DocumentTypeID == documentTypeId.Value);
            }

            return new DocumentTypeViewModel
            {
                Type = string.Empty,
                DocumentTypeID = Null.NullInteger
            };
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnInitControls ()
        {
            InitControls (gridDocuments, hiddenDocumentItemID, 
                buttonAddDocument, buttonUpdateDocument, buttonCancelEditDocument);
        }

        protected override void OnLoadItem (DocumentViewModel item)
        {
            comboDocumentType.SelectByValue (item.DocumentTypeID);
            textDocumentTitle.Text = item.Title;
            textDocumentGroup.Text = item.Group;
            textDocumentSortIndex.Text = item.SortIndex.ToString ();
            datetimeDocumentStartDate.SelectedDate = item.StartDate;
            datetimeDocumentEndDate.SelectedDate = item.EndDate;
            urlDocumentUrl.Url = item.Url;
        }

        protected override void OnUpdateItem (DocumentViewModel item)
        {
            item.Title = textDocumentTitle.Text.Trim ();
            item.Group = textDocumentGroup.Text.Trim ();
            item.DocumentTypeID = TypeUtils.ParseToNullable<int> (comboDocumentType.SelectedValue);
            item.DocumentType = GetDocumentType (item.DocumentTypeID);
            item.SortIndex = TypeUtils.ParseToNullable<int> (textDocumentSortIndex.Text) ?? 0;
            item.StartDate = datetimeDocumentStartDate.SelectedDate;
            item.EndDate = datetimeDocumentEndDate.SelectedDate;
            item.Url = urlDocumentUrl.Url;
        }

        protected override void OnResetForm ()
        {
            // comboDocumentType.SelectedIndex = 0;
            textDocumentTitle.Text = string.Empty;
            textDocumentSortIndex.Text = "0";
            datetimeDocumentStartDate.SelectedDate = null;
            datetimeDocumentEndDate.SelectedDate = null;
            // urlDocumentUrl.UrlType = "N";
        }

        #endregion
    }
}
