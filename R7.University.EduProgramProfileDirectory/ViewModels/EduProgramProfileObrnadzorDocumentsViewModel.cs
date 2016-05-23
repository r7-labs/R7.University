//
// EduProgramProfileObrnadzorDocumentsViewModel.cs
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
using System.Globalization;
using System.Linq;
using System.Text;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfileDirectory
{
    public class EduProgramProfileObrnadzorDocumentsViewModel: IEduProgramProfile
    {
        public IEduProgramProfile Model { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProgramProfileObrnadzorDocumentsViewModel (
            IEduProgramProfile model,
            ViewModelContext context,
            ViewModelIndexer indexer)
        {
            Model = model;
            Context = context;
            Indexer = indexer;
        }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents
                .Where (d => Context.Module.IsEditable || d.IsPublished ())
                .OrderBy (d => d.Group)
                .ThenBy (d => d.SortIndex);
        }

        protected delegate string GetDocumentTitle (IDocument document);

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

        #region IEduProgramProfile implementation

        public int EduProgramProfileID
        { 
            get { return Model.EduProgramProfileID; }
            set { throw new NotImplementedException (); }
        }

        public int EduProgramID
        { 
            get { return Model.EduProgramID; }
            set { throw new NotImplementedException (); }
        }

        public string ProfileCode
        { 
            get { return Model.ProfileCode; }
            set { throw new NotImplementedException (); }
        }

        public string ProfileTitle
        { 
            get { return Model.ProfileTitle; }
            set { throw new NotImplementedException (); }
        }

        public string Languages
        { 
            get { return Model.Languages; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? AccreditedToDate
        { 
            get { return Model.AccreditedToDate; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? CommunityAccreditedToDate
        { 
            get { return Model.CommunityAccreditedToDate; }
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

        public IEduProgram EduProgram
        {
            get { return Model.EduProgram; }
            set { throw new NotImplementedException (); }
        }

        public IList<IEduProgramProfileForm> EduProgramProfileForms
        {
            get { return Model.EduProgramProfileForms; }
            set { throw new NotImplementedException (); }
        }

        public IList<IDocument> Documents
        {
            get { return Model.Documents; }
            set { throw new NotImplementedException (); }
        }

        #endregion

        #region Bindable properties

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string Code
        {
            get { return "<span itemprop=\"EduCode\">" + EduProgram.Code + "</span>"; }
        }

        public string EduProgram_Links
        {
            get {
                var eduProgramDocuments = GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduProgram));
                if (!eduProgramDocuments.IsNullOrEmpty ()) {
                    return FormatDocumentLinks (
                        eduProgramDocuments,
                        "itemprop=\"OOP_main\"",
                        DocumentGroupPlacement.AfterTitle,
                        delegate {
                            return FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title,
                                ProfileCode,
                                ProfileTitle);
                        }
                    );
                }

                // show edu. program profile title w/o link
                return "<span itemprop=\"OOP_main\">"
                    + FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                    + "</span>";
            }
        }

        public string EduPlan_Links
        {
            get { 
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduPlan)),
                    "itemprop=\"education_plan\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string EduSchedule_Links
        {
            get { 
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduSchedule)),
                    "itemprop=\"education_shedule\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string WorkProgramAnnotation_Links
        {
            get {
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.WorkProgramAnnotation)),
                    "itemprop=\"education_annotation\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string EduMaterial_Links
        {
            get {
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduMaterial)),
                    "itemprop=\"methodology\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string Contingent_Links
        {
            get {
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.Contingent)),
                    "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/priem\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string ContingentMovement_Links
        {
            get {
                return FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.ContingentMovement)),
                    "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/Perevod\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string WorkProgramOfPractice_Links
        {
            get {
                var wpopDocuments = GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.WorkProgramOfPractice));

                // get all groups
                var groups = wpopDocuments
                    .Select (d => d.Group)
                    .Distinct ();

                var markupBuilder = new StringBuilder ();
                var groupMarkupBuilder = new StringBuilder ();

                var groupCount = 0;
                foreach (var group in groups) {
                    var index = 0;
                    foreach (var document in wpopDocuments.Where (d => d.Group == group)) {
                        var linkMarkup = document.FormatDocumentLink_WithMicrodata (
                                         null,   
                                         (++index).ToString (),
                                         false,
                                         DocumentGroupPlacement.None,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemprop=\"EduPr\""
                                     );

                        if (!string.IsNullOrEmpty (linkMarkup)) {
                            // use AppendLine to add whitespace between <a> tags
                            groupMarkupBuilder.AppendLine (linkMarkup);
                        }
                    }

                    var groupMarkup = groupMarkupBuilder.ToString ();
                    if (!string.IsNullOrEmpty (groupMarkup)) {
                        markupBuilder.Append ("<li>" + TextUtils.FormatList (": ", group, groupMarkup) + "</li>");
                        groupCount++;
                    }

                    // reuse StringBuilder
                    groupMarkupBuilder.Clear ();
                }

                var markup = markupBuilder.ToString ();
                if (!string.IsNullOrEmpty (markup)) {
                    return ((groupCount == 1)? "<ul class=\"list-inline\">" : "<ul>") + markup + "</ul>";
                }

                return string.Empty;
            }
        }

        private static char [] languageCodeSeparator = { ';' };

        public string Languages_String
        {
            get {
                if (Languages != null) {
                    var languages = Languages
                        .Split (languageCodeSeparator, StringSplitOptions.RemoveEmptyEntries)
                        .Select (l => CultureInfo.GetCultureInfoByIetfLanguageTag (l).NativeName)
                        .ToList ();

                    if (languages.Count > 0) {
                        return "<span itemprop=\"language\">" + TextUtils.FormatList (", ", languages) + "</span>";
                    }
                }

                return string.Empty;
            }
        }

        #endregion
    }
}
