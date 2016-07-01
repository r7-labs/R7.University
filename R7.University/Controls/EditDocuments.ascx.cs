//
//  EditDocuments.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;
using R7.University.Models;

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

            var documentTypeViewModels = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext);
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
            item.DocumentTypeID = int.Parse (comboDocumentType.SelectedValue);
            item.DocumentTypeViewModel = GetDocumentType (item.DocumentTypeID);
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
