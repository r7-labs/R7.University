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

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.ControlExtensions;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Controls
{
    public partial class EditDocuments: 
        GridAndFormEditControlBase<DocumentInfo,DocumentViewModel>
    {
        #region Control properties

        public string ForModel { get; set; }

        #endregion

        public void OnInit (PortalModuleBase module, IEnumerable<DocumentTypeInfo> documentTypes)
        {
            Module = module;

            var documentTypeViewModels = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext);
            ViewState ["documentTypes"] = XmlSerializationHelper.Serialize (documentTypeViewModels);

            comboDocumentType.DataSource = documentTypeViewModels.OrderBy (dt => dt.LocalizedType);
            comboDocumentType.DataBind ();

            // TODO: Add match to the DocumentType model, generate JSON
            // comboDocumentType.Attributes.Add ("data-validation", "[{\"id\": \"5\", \"match\": \"^annot_[a-z0-9_]+_\\\\d{8}$\" }]");

            valDocumentUrl.Attributes.Add ("data-message-template", Localization.GetString ("FileName.Invalid", LocalResourceFile));
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

        protected void gridDocuments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                var document = (IDocument) e.Row.DataItem;
                if (!document.IsPublished (HttpContext.Current.Timestamp)) {
                    e.Row.CssClass = gridDocuments.GetDataRowStyle (e.Row).CssClass + " u8y-not-published";
                }
            }
        }

        #region Implemented abstract members of GridAndFormEditControlBase

        protected override string TargetItemKey
        {
            get { return ForModel; }
        }

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

        public override void SetData (List<DocumentInfo> items, int targetItemId)
        {
            base.SetData (items, targetItemId);

            // speedup adding new documents by autoselecting first document's folder
            if (items.Count > 0) {
                var firstItem = items [0];
                if (Globals.GetURLType (firstItem.Url) == TabType.File) {
                    urlDocumentUrl.Url = firstItem.Url;
                }
            }
        }
    }
}
