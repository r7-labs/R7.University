//
// EduProgramStandardObrnadzorViewModel.cs
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Data;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;
using System.Text;

namespace R7.University.EduProgramDirectory
{
    public class EduProgramStandardObrnadzorViewModel: IEduProgram
    {
        public IEduProgram Model { get; protected set; }

        public IIndexer Indexer { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramStandardObrnadzorViewModel (EduProgramInfo model, ViewModelContext context, IIndexer indexer)
        {
            Model = model;
            Context = context;
            Indexer = indexer;
        }

        #region IEduProgram implementation

        public int EduProgramID
        {
            get { return Model.EduProgramID; }
            set { throw new NotImplementedException (); }
        }

        public int EduLevelID
        {
            get { return Model.EduLevelID; }
            set { throw new NotImplementedException (); }
        }

        public string Code
        {
            get { return Model.Code; }
            set { throw new NotImplementedException (); }
        }

        public string Title
        {
            get { return Model.Title; }
            set { throw new NotImplementedException (); }
        }

        public string Generation
        {
            get { return Model.Generation; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? StartDate
        {
            get { return Model.StartDate; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? EndDate
        {
            get { return Model.EndDate; }
            set { throw new NotImplementedException (); }
        }

        public int LastModifiedByUserID
        {
            get { return Model.LastModifiedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime LastModifiedOnDate
        {
            get { return Model.LastModifiedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public int CreatedByUserID
        {
            get { return Model.CreatedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime CreatedOnDate
        {
            get { return Model.CreatedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public IEduLevel EduLevel
        {
            get { return Model.EduLevel; }
            set { throw new NotImplementedException (); }
        }

        public IList<IDocument> Documents
        {
            get { return Model.Documents; }
            set { throw new NotImplementedException (); }
        }

        #endregion

        protected string FormatDocumentLinks (IEnumerable<IDocument> documents, string microdata, DocumentGroupPlacement groupPlacement, GetDocumentTitle getDocumentTitle = null)
        {
            var markupBuilder = new StringBuilder ();
            var count = 0;
            foreach (var document in documents) {
                var linkMarkup = document.FormatDocumentLink_WithMicrodata (
                    (getDocumentTitle == null)? document.Title : getDocumentTitle (document),
                    Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile),
                    true,
                    groupPlacement,
                    Context.Module.TabId,
                    Context.Module.ModuleId,
                    microdata
                );

                if (!string.IsNullOrEmpty (linkMarkup)) {
                    markupBuilder.Append ("<li>" + linkMarkup + "</li>");
                    count++;
                }
            }

            var markup = markupBuilder.ToString ();
            if (!string.IsNullOrEmpty (markup)) {
                return ((count == 1)? "<ul class=\"list-inline\">" : "<ul>") + markup + "</ul>";
            }

            return string.Empty;
        }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents
                .Where (d => Context.Module.IsEditable || d.IsPublished ())
                .OrderBy (d => d.Group)
                .ThenBy (d => d.SortIndex);
        }

        public int Order 
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string EduLevel_String
        {
            get { return FormatHelper.FormatShortTitle (EduLevel.ShortTitle, EduLevel.Title); }
        }

        public string EduStandard_Links
        {
            get { 
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduStandard)),
                    "itemprop=\"EduStandartDoc\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string ProfStandard_Links
        {
            get { 
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.ProfStandard)),
                    string.Empty,
                    DocumentGroupPlacement.InTitle
                );
            }
        }
    }
}

