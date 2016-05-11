//
// DocumentViewModel.cs
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
using System.Xml.Serialization;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Utilities;
using R7.University.ViewModels;
using R7.University.Models;
using R7.University.Data;

namespace R7.University.Controls
{
    [Serializable]
    public class DocumentViewModel: IDocument, IEditControlViewModel<DocumentInfo>
    {
        #region IDocument implementation

        public int DocumentID { get; set; }

        public int? DocumentTypeID { get; set; }

        [XmlIgnore]
        public IDocumentType DocumentType { get; set; }

        /// <summary>
        /// XML-serializeable boilerplate for <see cref="DocumentViewModel.DocumentType" /> property
        /// </summary>
        /// <value>The document type view model.</value>
        public DocumentTypeViewModel DocumentTypeViewModel
        {
            get { return (DocumentTypeViewModel) DocumentType; }
            set { DocumentType = value; }
        }

        public string ItemID { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int SortIndex { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #endregion

        [XmlIgnore]
        public string LocalizedType
        { 
            get {
                if (DocumentType != null) {
                    var localizedType = Localization.GetString ("SystemDocumentType_" + DocumentType.Type + ".Text", 
                                            Context.LocalResourceFile);
                    
                    return (!string.IsNullOrEmpty (localizedType)) ? localizedType : DocumentType.Type;
                }

                return string.Empty;
            }
        }

        [XmlIgnore]
        public string FormattedUrl
        {
            get { 
                if (!string.IsNullOrWhiteSpace (Url)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                        UrlUtils.LinkClickIdnHack (Url, Context.Module.TabId, Context.Module.ModuleId),
                        Localization.GetString ("DocumentUrlLabel.Text", Context.LocalResourceFile)
                    );
                }

                return string.Empty;
            }
        }

        #region IEditControlViewModel implementation

        public int ViewItemID { get; set; }

        [XmlIgnore]
        public ViewModelContext Context { get; set; }

        public IEditControlViewModel<DocumentInfo> FromModel (DocumentInfo model, ViewModelContext viewContext)
        {
            var viewModel = new DocumentViewModel ();
            CopyCstor.Copy<IDocument> (model, viewModel);

            // FIXME: Context not updated for referenced viewmodels
            if (model.DocumentType != null) {
                viewModel.DocumentType = new DocumentTypeViewModel (model.DocumentType, viewContext);
            }

            viewModel.Context = viewContext;

            return viewModel;
        }

        public DocumentInfo ToModel ()
        {
            var model = new DocumentInfo ();
            CopyCstor.Copy<IDocument> (this, model);

            return model;
        }

        public void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            ItemID = targetItemKey + targetItemId;
        }

        #endregion

        public DocumentViewModel ()
        {
            ViewItemID = ViewNumerator.GetNextItemID ();
        }

        /*
        public static void BindToView (IEnumerable<DocumentViewModel> documents, ViewModelContext context)
        {
            foreach (var document in documents)
            {
                document.Context = context;
            }
        }*/
    }
}

