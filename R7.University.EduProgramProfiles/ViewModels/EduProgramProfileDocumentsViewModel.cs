//
//  EduProgramProfileDocumentsViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
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
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.ViewModels
{
    internal class EduProgramProfileDocumentsViewModel : EduProgramProfileViewModelBase
    {
        public EduProgramProfileDirectoryDocumentsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext<EduProgramProfileDirectorySettings> Context => RootViewModel.Context;

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProgramProfileDocumentsViewModel (
            IEduProgramProfile model,
            EduProgramProfileDirectoryDocumentsViewModel rootViewModel,
            ViewModelIndexer indexer) : base (model)
        {
            RootViewModel = rootViewModel;
            Indexer = indexer;
        }

        #region Bindable properties

        public int Order => Indexer.GetNextIndex ();

        public string Code => Wrap (EduProgram.Code, "eduCode");

        string _eduProgramLinks;
        public string EduProgram_Links => _eduProgramLinks ?? (_eduProgramLinks = GetEduProgramLinks ());

        public string EduLevel_String => Wrap (EduLevel.Title, "eduLevel");

        string _eduPlanLinks;
        public string EduPlan_Links => _eduPlanLinks ?? (_eduPlanLinks = GetEduPlanLinks ());

        string _eduScheduleLinks;
        public string EduSchedule_Links => _eduScheduleLinks ?? (_eduScheduleLinks = GetEduScheduleLinks ());

        string _workProgramAnnotationLinks;
        public string WorkProgramAnnotation_Links => _workProgramAnnotationLinks ?? (_workProgramAnnotationLinks = GetWorkProgramAnnotationLinks ());

        string _eduMaterialLinks;
        public string EduMaterial_Links => _eduMaterialLinks ?? (_eduMaterialLinks = GetEduMaterialLinks ());

        string _workProgramOfPracticeLinks;
        public string WorkProgramOfPractice_Links => _workProgramOfPracticeLinks ?? (_workProgramOfPracticeLinks = GetWorkProgramOfPracticeLinks ());

        public string EduForms_String
        {
            get {
                var formYears = GetImplementedEduFormYears ();
                if (!formYears.IsNullOrEmpty ()) {
                    return "<ul itemprop=\"eduForm\">" + formYears
                        .Select (eppfy => (eppfy.IsPublished (_now) ? "<li>" : "<li class=\"u8y-not-published-element\">")
                                 + LocalizationHelper.GetStringWithFallback ("EduForm_" + eppfy.EduForm.Title + ".Text", Context.LocalResourceFile, eppfy.EduForm.Title).ToLower () + "</li>")
                        .Aggregate ((s1, s2) => s1 + s2) + "</ul>";
                }

                return string.Empty;
            }
        }

        string GetELearningString ()
        {
            if (ELearning || DistanceEducation) {
                return FormatHelper.JoinNotNullOrEmpty (
                    ", ",
                    ELearning? Localization.GetString ("ELearning_ELearning.Text", Context.LocalResourceFile) : null,
                    DistanceEducation? Localization.GetString ("ELearning_DistanceEducation.Text", Context.LocalResourceFile) : null
                );
            }

            return Localization.GetString ("ELearning_No.Text", Context.LocalResourceFile);
        }

        public string ELearning_String => Wrap (GetELearningString (), IsAdopted ? "adEduEl" : "eduEl");

        #endregion

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable).OrderByGroupDescThenSortIndex ();
        }

        string _rowId;
        protected string RowId => _rowId ?? (_rowId = $"m{Context.Module.ModuleId}-epp{EduProgramProfileID}");

        string _groupColumnHeader;
        protected string GroupColumnHeader =>
            _groupColumnHeader ?? (_groupColumnHeader = Localization.GetString ("DocumentGroup.Column", Context.LocalResourceFile));

        string _titleColumnHeader;
        protected string TitleColumnHeader =>
            _titleColumnHeader ?? (_titleColumnHeader = Localization.GetString ("DocumentTitle.Column", Context.LocalResourceFile));

        // TODO: Calculate initial capacity for StringBuilder?
        string FormatDocumentsLinkWithData (IEnumerable<IDocument> documents, string linkText, string columnSlug, string microdata = "")
        {
            var microdataAttrs = !string.IsNullOrEmpty (microdata) ? " " + microdata : string.Empty;
            var docCount = documents.Count ();
            if (docCount > 0) {
                var docCountText = (docCount > 1 || string.IsNullOrEmpty (linkText)) ? " [" + docCount + "]" : string.Empty;
                var table = new StringBuilder (
                    $"<span{microdataAttrs}>"
                    + $"<a type=\"button\" href=\"#\" data-toggle=\"modal\" data-target=\"#u8y-epp-docs-dlg-{Context.Module.ModuleId}\""
                    + $" data-table=\"doct-{RowId}-{columnSlug}\">{(linkText + docCountText).TrimStart ()}</a>"
                    + $"<table id=\"doct-{RowId}-{columnSlug}\" class=\"hidden\">"
                    + $"<thead><tr><th>{TitleColumnHeader}</th><th>{GroupColumnHeader}</th></tr></thead><tbody>"
                );

                foreach (var document in documents) {
                    var docTitle = !string.IsNullOrEmpty (document.Title) ? document.Title : Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile);
                    var docUrl = UniversityUrlHelper.LinkClickIdnHack (document.Url, Context.Module.TabId, Context.Module.ModuleId);
                    var rowCssClassAttr = !document.IsPublished (HttpContext.Current.Timestamp) ? " class=\"u8y-not-published\"" : string.Empty;
                    table.Append ($"<tr{rowCssClassAttr}><td><a href=\"{docUrl}\" target=\"_blank\">{docTitle}</a></td><td>{document.Group}</td></tr>");
                }

                table.Append ("</tbody></table></span>");
                return table.ToString ();
            }

            if (!string.IsNullOrEmpty (linkText)) {
                return $"<span{microdataAttrs}>{linkText}</span>";
            }

            return string.Empty;
        }

        string GetEduProgramLinks ()
        {
            return Wrap (FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduProgram)),
                UniversityFormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                    .Append (IsAdopted ? Context.LocalizeString ("IsAdopted.Text") : null, " - "),
                "oop",
                IsAdopted ? "itemprop=\"adOpMain\"" : "itemprop=\"opMain\""
            ), "eduName");
        }

        string GetEduPlanLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduPlan)),
                string.Empty,
                "epl",
                IsAdopted ? "itemprop=\"adEducationPlan\"" : "itemprop=\"educationPlan\""
            );
        }

        string GetEduScheduleLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduSchedule)),
                string.Empty,
                "esh",
                IsAdopted ? "itemprop=\"adEducationShedule\"" : "itemprop=\"educationShedule\""
            );
        }

        string GetWorkProgramAnnotationLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramAnnotation)),
                string.Empty,
                "wpa",
                IsAdopted ? "itemprop=\"adEducationAnnotation\"" : "itemprop=\"educationAnnotation\""
            );
        }

        string GetEduMaterialLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduMaterial)),
                string.Empty,
                "met",
                IsAdopted ? "itemprop=\"adMethodology\"" : "itemprop=\"methodology\""
            );
        }

        string GetWorkProgramOfPracticeLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramOfPractice)),
                string.Empty,
                "wpp",
                IsAdopted ? "itemprop=\"adEduPr\"" : "itemprop=\"eduPr\""
            );
        }

        DateTime _now => HttpContext.Current.Timestamp;

        IEnumerable<IEduProgramProfileFormYear> GetImplementedEduFormYears ()
        {
            return EduProgramProfileFormYears.Where (eppfy => eppfy.Year == null &&
                                                     (eppfy.IsPublished (_now) || Context.Module.IsEditable))
                                             .OrderBy (eppfy => eppfy.EduForm.SortIndex);
        }

        string Wrap (string text, string itemprop)
        {
	        return $"<span itemprop=\"{itemprop}\">{text}</span>";
        }
    }
}
