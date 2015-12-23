//
// EduProgramProfileObrnadzorDocumentsViewModel.cs
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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.Localization;

namespace R7.University.EduProgramProfileDirectory
{
    public class EduProgramProfileObrnadzorDocumentsViewModel: IEduProgramProfile
    {
        #region IEduProgramProfile implementation

        public int EduProgramProfileID 
        { 
            get { return Model.EduProgramProfileID; }
            set {}
        }

        public int EduProgramID 
        { 
            get { return Model.EduProgramID; }
            set {}
        }
      
        public string ProfileCode 
        { 
            get { return Model.ProfileCode; }
            set {}
        }

        public string ProfileTitle 
        { 
            get { return Model.ProfileTitle; }
            set {}
        }

        public DateTime? AccreditedToDate 
        { 
            get { return Model.AccreditedToDate; }
            set {}
        }

        public DateTime? CommunityAccreditedToDate 
        { 
            get { return Model.CommunityAccreditedToDate; }
            set {}
        }

        public DateTime? StartDate
        { 
            get { return Model.StartDate; }
            set {}
        }

        public DateTime? EndDate 
        {
            get { return Model.EndDate; }
            set {}
        }

        public EduProgramInfo EduProgram
        {
            get { return Model.EduProgram; }
            set {}
        }
       
        public IList<IEduProgramProfileForm> EduProgramProfileForms
        {
            get { return Model.EduProgramProfileForms; }
            set {}
        }

        public IList<IDocument> Documents
        {
            get { return Model.Documents; }
            set {}
        }

        #endregion

        public IEduProgramProfile Model { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramProfileObrnadzorDocumentsViewModel (IEduProgramProfile model, ViewModelContext context, ViewModelIndexer indexer)
        {
            Model = model;
            Context = context;
            Index = indexer.GetNextIndex ();
        }

        protected IDocument EduProgramDocument
        {
            get 
            { 
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduProgram); 
            }
        }

        protected IDocument EduPlanDocument
        {
            get 
            {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduPlan); 
            }
        }

        protected IDocument WorkProgramAnnotationDocument
        {
            get 
            {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.WorkProgramAnnotation); 
            }
        }

        protected IDocument EduScheduleDocument
        {
            get 
            {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduSchedule); 
            }
        }

        protected IList<IDocument> WorkProgramOfPracticeDocuments
        {
            get 
            {
                return Documents.Where (d =>
                        (d.IsPublished () || Context.Module.IsEditable) &&
                        d.DocumentType.GetSystemDocumentType () == SystemDocumentType.WorkProgramOfPractice)
                    .OrderBy (d => d.SortIndex)
                    .ToList ();
            }
        }

        protected IDocument EduMaterialDocument
        {
            get 
            {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduMaterial); 
            }
        }

        protected IDocument ContingentDocument
        {
            get 
            {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.Contingent); 
            }
        }

        protected IDocument ContingentMovementDocument
        {
            get 
            {
                return Documents.FirstOrDefault (d =>
                    (d.IsPublished () || Context.Module.IsEditable) &&
                    d.DocumentType.GetSystemDocumentType () == SystemDocumentType.ContingentMovement); 
            }
        }

        public string EduProgramDocumentLink
        {
            get 
            {
                if (EduProgramDocument != null)
                {
                    var linkMarkup = EduProgramDocument.FormatLinkWithMicrodata (
                        FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle), 
                        false,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemprop=\"OOP_main\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        return linkMarkup;
                    }
                }

                return "<span itemprop=\"OOP_main\">" 
                    + FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                    + "</span>";
            }
        }

        public string EduPlanDocumentLink
        {
            get 
            {
                if (EduPlanDocument != null)
                {
                    var linkMarkup = EduPlanDocument.FormatLinkWithMicrodata (
                        Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                        true,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemprop=\"education_plan\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string WorkProgramAnnotationDocumentLink
        {
            get 
            {
                if (WorkProgramAnnotationDocument != null)
                {
                    var linkMarkup = WorkProgramAnnotationDocument.FormatLinkWithMicrodata (
                        Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                        true,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemprop=\"education_annotation\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string EduScheduleDocumentLink
        {
            get 
            {
                if (EduScheduleDocument != null)
                {
                    var linkMarkup = EduScheduleDocument.FormatLinkWithMicrodata (
                        Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                        true,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemprop=\"education_schedule\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string WorkProgramOfPracticeLinks
        {
            get
            {
                var index = 0;
                var markupBuilder = new StringBuilder ();

                foreach (var document in WorkProgramOfPracticeDocuments)
                {
                    var linkMarkup = document.FormatLinkWithMicrodata (
                         (++index).ToString (),
                         false,
                         Context.Module.TabId,
                         Context.Module.ModuleId,
                         "itemprop=\"EduPr\""
                     );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        markupBuilder.AppendLine (linkMarkup);
                    }
                }

                var markup = markupBuilder.ToString ();
                if (!string.IsNullOrEmpty (markup))
                {
                    return markup;
                }

                return string.Empty;
            }
        }

        public string EduMaterialDocumentLink
        {
            get 
            {
                if (EduMaterialDocument != null)
                {
                    var linkMarkup = EduMaterialDocument.FormatLinkWithMicrodata (
                        Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                        true,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemprop=\"methodology\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string ContingentDocumentLink
        {
            get 
            {
                if (ContingentDocument != null)
                {
                    var linkMarkup = ContingentDocument.FormatLinkWithMicrodata (
                        Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                        true,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/priem\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
                        return linkMarkup;
                    }
                }

                return string.Empty;
            }
        }

        public string ContingentMovementDocumentLink
        {
            get 
            {
                if (ContingentMovementDocument != null)
                {
                    var linkMarkup = ContingentMovementDocument.FormatLinkWithMicrodata (
                        Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile), 
                        true,
                        Context.Module.TabId,
                        Context.Module.ModuleId,
                        "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/Perevod\""
                    );

                    if (!string.IsNullOrEmpty (linkMarkup))
                    {
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

        public string EduLevelString
        {
            get { return "<span itemprop=\"EduLevel\">" + EduProgram.EduLevel.Title + "</span>"; }
        }

        public string EduLanguages
        {
            // TODO: Add language(s) to the IEduProgramProfile model
            get { return "<span itemprop=\"language\">Русский</span>"; }
        }

    }
}
