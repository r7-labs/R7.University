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
using R7.University.ViewModels;
using R7.University.Models;

namespace R7.University.EduProgramProfileDirectory
{
    public class EduProgramProfileObrnadzorDocumentsViewModel: IEduProgramProfile
    {
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

        public IEduProgramProfile Model { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramProfileObrnadzorDocumentsViewModel (
            IEduProgramProfile model,
            ViewModelContext context,
            ViewModelIndexer indexer)
        {
            Model = model;
            Context = context;
            Index = indexer.GetNextIndex ();
        }

        protected IDocument EduProgramDocument
        {
            get { 
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduProgram); 
            }
        }

        protected IEnumerable<IDocument> EduPlanDocuments
        {
            get {
                return Documents.Where (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduPlan)
                    .OrderBy (d => d.SortIndex);
            }
        }

        protected IEnumerable<IDocument> EduScheduleDocuments
        {
            get {
                return Documents.Where (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduSchedule)
                    .OrderBy (d => d.SortIndex);
            }
        }


        protected IDocument WorkProgramAnnotationDocument
        {
            get {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.WorkProgramAnnotation); 
            }
        }

        protected IEnumerable<IDocument> WorkProgramOfPracticeDocuments
        {
            get {
                return Documents.Where (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.WorkProgramOfPractice)
                    .OrderBy (d => d.SortIndex);
            }
        }

        protected IDocument EduMaterialDocument
        {
            get {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduMaterial); 
            }
        }

        protected IDocument ContingentDocument
        {
            get {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.Contingent); 
            }
        }

        protected IDocument ContingentMovementDocument
        {
            get {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.ContingentMovement); 
            }
        }

        protected string FormatDocumentLinks (IEnumerable<IDocument> documents, string microdata)
        {
            var markupBuilder = new StringBuilder ();

            foreach (var document in documents) {
                var linkMarkup = document.FormatLinkWithMicrodata (
                                     Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile),
                                     true,
                                     Context.Module.TabId,
                                     Context.Module.ModuleId,
                                     microdata
                                 );

                if (!string.IsNullOrEmpty (linkMarkup)) {
                    markupBuilder.AppendLine (linkMarkup);
                }
            }

            var markup = markupBuilder.ToString ();
            if (!string.IsNullOrEmpty (markup)) {
                return markup;
            }

            return string.Empty;
        }

        public string EduProgramLink
        {
            get {
                if (EduProgramDocument != null) {
                    var linkMarkup = EduProgramDocument.FormatLinkWithMicrodata (
                                         FormatHelper.FormatEduProgramProfileTitle (
                                             EduProgram.Title,
                                             ProfileCode,
                                             ProfileTitle), 
                                         false,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemprop=\"OOP_main\""
                                     );

                    if (!string.IsNullOrEmpty (linkMarkup)) {
                        return linkMarkup;
                    }
                }

                return "<span itemprop=\"OOP_main\">"
                + FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                + "</span>";
            }
        }

        public string EduPlanLinks
        {
            get { return FormatDocumentLinks (EduPlanDocuments, "itemprop=\"education_plan\""); }
        }

        public string WorkProgramAnnotationLink
        {
            get {
                if (WorkProgramAnnotationDocument != null) {
                    var linkMarkup = WorkProgramAnnotationDocument.FormatLinkWithMicrodata (
                                         Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                                         true,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemprop=\"education_annotation\""
                                     );

                    if (!string.IsNullOrEmpty (linkMarkup)) {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string EduScheduleLinks
        {
            get { return FormatDocumentLinks (EduScheduleDocuments, "itemprop=\"education_shedule\""); }
        }

        public string WorkProgramOfPracticeLinks
        {
            get {
                var index = 0;
                var markupBuilder = new StringBuilder ();

                foreach (var document in WorkProgramOfPracticeDocuments) {
                    var linkMarkup = document.FormatLinkWithMicrodata (
                                         (++index).ToString (),
                                         false,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemprop=\"EduPr\""
                                     );

                    if (!string.IsNullOrEmpty (linkMarkup)) {
                        markupBuilder.AppendLine (linkMarkup);
                    }
                }

                var markup = markupBuilder.ToString ();
                if (!string.IsNullOrEmpty (markup)) {
                    return markup;
                }

                return string.Empty;
            }
        }

        public string EduMaterialLink
        {
            get {
                if (EduMaterialDocument != null) {
                    var linkMarkup = EduMaterialDocument.FormatLinkWithMicrodata (
                                         Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                                         true,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemprop=\"methodology\""
                                     );

                    if (!string.IsNullOrEmpty (linkMarkup)) {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string ContingentLink
        {
            get {
                if (ContingentDocument != null) {
                    var linkMarkup = ContingentDocument.FormatLinkWithMicrodata (
                                         Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                                         true,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/priem\""
                                     );

                    if (!string.IsNullOrEmpty (linkMarkup)) {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string ContingentMovementLink
        {
            get {
                if (ContingentMovementDocument != null) {
                    var linkMarkup = ContingentMovementDocument.FormatLinkWithMicrodata (
                                         Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                                         true,
                                         Context.Module.TabId,
                                         Context.Module.ModuleId,
                                         "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/Perevod\""
                                     );

                    if (!string.IsNullOrEmpty (linkMarkup)) {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public int Index { get; protected set; }

        public string IndexString
        {
            get { return Index + "."; }
        }

        public string Code
        {
            get { return "<span itemprop=\"EduCode\">" + EduProgram.Code + "</span>"; }
        }

        private static char [] languageCodeSeparator = { ';' };

        public string LanguagesString
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
    }
}
