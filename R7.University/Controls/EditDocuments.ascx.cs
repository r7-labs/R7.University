//
// EditDocuments.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Localization;
using R7.University.ControlExtensions;
using DotNetNuke.Common.Utilities;

namespace R7.University.Controls
{
    public partial class EditDocuments: UserControl
    {
        #region Control properties

        public string ForModel { get; set; }

        #endregion

        #region Bindable icons

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        protected string DeleteIconUrl
        {
            get { return IconController.IconURL ("Delete"); }
        }

        #endregion


        private string localResourceFile;
        public string LocalResourceFile
        {
            get
            {
                if (localResourceFile == null)
                {
                    localResourceFile = DotNetNuke.Web.UI.Utilities.GetLocalResourceFile (this);
                }

                return localResourceFile;
            }
        }

        protected PortalModuleBase Module;

        protected int ItemId;

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this, Module)); }
        }

        protected List<DocumentViewModel> ViewStateDocuments
        {
            get { return XmlSerializationHelper.Deserialize<List<DocumentViewModel>> (ViewState ["documents"]); }
            set { ViewState ["documents"] = XmlSerializationHelper.Serialize<List<DocumentViewModel>> (value); }
        }

        public void OnInit (PortalModuleBase module, IEnumerable<DocumentTypeInfo> documentTypes)
        {
            Module = module;

            SetDocumentTypes (documentTypes);

            comboDocumentType.DataSource = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext, true);
            comboDocumentType.DataBind ();

            gridDocuments.LocalizeColumns (LocalResourceFile);
        }

        public void SetDocuments (int itemId, IEnumerable<DocumentInfo> documentsWithType)
        { 
            ItemId = itemId;

            var documentViewModels = documentsWithType.Select (d => new DocumentViewModel (d, ViewModelContext)).ToList ();
            ViewStateDocuments = documentViewModels;
            gridDocuments.DataSource = DataTableConstructor.FromIEnumerable (documentViewModels);
            gridDocuments.DataBind ();
        }

        public IList<DocumentInfo> GetDocuments ()
        {
            if (ViewStateDocuments != null)
            {
                return ViewStateDocuments.Select (dvm => dvm.NewDocumentInfo ()).ToList ();
            }

            return new List<DocumentInfo> ();    
        }
       
        protected void SetDocumentTypes (IEnumerable<DocumentTypeInfo> documentTypes)
        {
            ViewState ["documentTypes"] = documentTypes.ToList ();
        }

        protected DocumentTypeInfo GetDocumentType (int? documentTypeId)
        {
            if (documentTypeId != null)
            {
                var documentTypes = (List<DocumentTypeInfo>) ViewState ["documentTypes"];
                return documentTypes.Single (dt => dt.DocumentTypeID == documentTypeId.Value);
            }

            return new DocumentTypeInfo { 
                Type = string.Empty,
                DocumentTypeID = Null.NullInteger
            };
        }

        #region Handlers

        protected void buttonAddDocument_Command (object sender, CommandEventArgs e)
        {
            try
            {
                DocumentViewModel document;

                // get documents list from viewstate
                var documents = ViewStateDocuments;

                // creating new list, if none
                if (documents == null)
                    documents = new List<DocumentViewModel> ();

                var command = e.CommandArgument.ToString ();
                if (command == "Add")
                {
                    document = new DocumentViewModel ();
                }
                else
                {
                    // restore ItemID from hidden field
                    var hiddenItemID = int.Parse (hiddenDocumentItemID.Value);
                    document = documents.Find (d => d.ViewItemID == hiddenItemID);
                }

                document.Title = textDocumentTitle.Text.Trim ();
                document.DocumentTypeID = Utils.ParseToNullableInt (comboDocumentType.SelectedValue);
                document.DocumentType = GetDocumentType (document.DocumentTypeID);
                document.SortIndex = int.Parse (textDocumentSortIndex.Text);
                document.StartDate = datetimeDocumentStartDate.SelectedDate;
                document.EndDate = datetimeDocumentEndDate.SelectedDate;
                document.Url = urlDocumentUrl.Url;

                if (command == "Add")
                {
                    document.ItemID = ForModel + "ID=" + ItemId;
                    documents.Add (document);
                }

                ResetEditDocumentForm ();

                // refresh viewstate
                ViewStateDocuments = documents;

                // bind items to the gridview
                DocumentViewModel.BindToView (documents, ViewModelContext);
                gridDocuments.DataSource = DataTableConstructor.FromIEnumerable (documents);
                gridDocuments.DataBind ();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void linkEditDocument_Command (object sender, CommandEventArgs e)
        {
            try
            {
                var documents = ViewStateDocuments;
                if (documents != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find document in a list
                    var document = documents.Find (d => d.ViewItemID.ToString () == itemID);

                    if (document != null)
                    {
                        // fill form
                        Utils.SelectByValue (comboDocumentType, document.DocumentTypeID);
                        textDocumentTitle.Text = document.Title;
                        textDocumentSortIndex.Text = document.SortIndex.ToString ();
                        datetimeDocumentStartDate.SelectedDate = document.StartDate;
                        datetimeDocumentEndDate.SelectedDate = document.EndDate;
                        urlDocumentUrl.Url = document.Url;

                        // store ItemID in the hidden field
                        hiddenDocumentItemID.Value = document.ViewItemID.ToString ();

                        // show / hide buttons
                        buttonAddDocument.Visible = false;
                        buttonUpdateDocument.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void linkDeleteDocument_Command (object sender, CommandEventArgs e)
        {
            try
            {
                var documents = ViewStateDocuments;
                if (documents != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find position in a list
                    var documentIndex = documents.FindIndex (d => d.ViewItemID.ToString () == itemID);

                    if (documentIndex >= 0)
                    {
                        // remove item
                        documents.RemoveAt (documentIndex);

                        // refresh viewstate
                        ViewStateDocuments = documents;

                        // bind to the gridview
                        DocumentViewModel.BindToView (documents, ViewModelContext);
                        gridDocuments.DataSource = DataTableConstructor.FromIEnumerable (documents);
                        gridDocuments.DataBind ();

                        // reset form if we deleting currently edited item
                        if (buttonUpdateDocument.Visible && hiddenDocumentItemID.Value == itemID)
                            ResetEditDocumentForm ();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void buttonCancelEditDocument_Click (object sender, EventArgs e)
        {
            try
            {
                ResetEditDocumentForm ();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        void ResetEditDocumentForm ()
        {
            // restore default buttons visibility
            buttonAddDocument.Visible = true;
            buttonUpdateDocument.Visible = false;

            comboDocumentType.SelectedIndex = 0;
            textDocumentTitle.Text = string.Empty;
            textDocumentSortIndex.Text = "0";
            datetimeDocumentStartDate.SelectedDate = null;
            datetimeDocumentEndDate.SelectedDate = null;
            urlDocumentUrl.UrlType = "N";
        }

        protected void gridDocuments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hide ViewItemID column, also in header
            e.Row.Cells [1].Visible = false;

            // exclude header
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // find edit and delete linkbuttons
                var linkDelete = e.Row.Cells [0].FindControl ("linkDelete") as LinkButton;
                var linkEdit = e.Row.Cells [0].FindControl ("linkEdit") as LinkButton;

                // set recordId to delete
                linkEdit.CommandArgument = e.Row.Cells [1].Text;
                linkDelete.CommandArgument = e.Row.Cells [1].Text;

                // add confirmation dialog to delete link
                linkDelete.Attributes.Add ("onClick", "javascript:return confirm('" 
                    + Localization.GetString ("DeleteItem") + "');");
            }
        }

        #endregion
    }
}
