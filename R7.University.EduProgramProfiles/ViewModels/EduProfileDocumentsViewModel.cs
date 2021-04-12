using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
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
    internal class EduProfileDocumentsViewModel : EduProfileViewModelBase
    {
        public EduProgramProfileDirectoryDocumentsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext<EduProgramProfileDirectorySettings> Context => RootViewModel.Context;

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProfileDocumentsViewModel (
            IEduProfile model,
            EduProgramProfileDirectoryDocumentsViewModel rootViewModel,
            ViewModelIndexer indexer) : base (model)
        {
            RootViewModel = rootViewModel;
            Indexer = indexer;
        }

        #region Bindable properties

        public int Order => Indexer.GetNextIndex ();

        public string Code => Span (EduProgram.Code, "eduCode");

        string _eduProgramLinks;
        public string EduProgram_Links => _eduProgramLinks ?? (_eduProgramLinks = GetEduProgramLinks ());

        public string EduLevel_String => Span (EduLevel.Title, "eduLevel");

        string _eduPlanLinks;
        public string EduPlan_Links => _eduPlanLinks ?? (_eduPlanLinks = GetEduPlanLinks ());

        string _eduScheduleLinks;
        public string EduSchedule_Links => _eduScheduleLinks ?? (_eduScheduleLinks = GetEduScheduleLinks ());

        string _workProgramAnnotationLinks;

        public string WorkProgramAnnotation_Links => _workProgramAnnotationLinks ??
                                                     (_workProgramAnnotationLinks = GetWorkProgramAnnotationLinks ());

        string _eduMaterialLinks;
        public string EduMaterial_Links => _eduMaterialLinks ?? (_eduMaterialLinks = GetEduMaterialLinks ());

        string _workProgramLinks;
        public string WorkProgram_Links => _workProgramLinks ?? (_workProgramLinks = GetWorkProgramLinks ());

        public string EduForms_String {
            get {
                var formYears = GetImplementedEduFormYears ();
                if (!formYears.IsNullOrEmpty ()) {
                    return "<ul itemprop=\"eduForm\">" + formYears
                        .Select (eppfy =>
                            (eppfy.IsPublished (_now) ? "<li>" : "<li class=\"u8y-not-published-element\">")
                            + LocalizationHelper.GetStringWithFallback ("EduForm_" + eppfy.EduForm.Title + ".Text",
                                Context.LocalResourceFile, eppfy.EduForm.Title).ToLower () + "</li>")
                        .Aggregate ((s1, s2) => s1 + s2) + "</ul>";
                }

                return "<span itemprop=\"eduForm\">-</span>";
            }
        }

        string GetELearningString ()
        {
            if (ELearning || DistanceEducation) {
                return FormatHelper.JoinNotNullOrEmpty (
                    ", ",
                    ELearning ? Localization.GetString ("ELearning_ELearning.Text", Context.LocalResourceFile) : null,
                    DistanceEducation
                        ? Localization.GetString ("ELearning_DistanceEducation.Text", Context.LocalResourceFile)
                        : null
                );
            }

            return Localization.GetString ("ELearning_No.Text", Context.LocalResourceFile);
        }

        public string ELearning_String => Span (GetELearningString (), IsAdopted ? "adEduEl" : "eduEl");

        #endregion

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable)
                .OrderByGroupDescThenSortIndex ();
        }

        string _rowId;
        protected string RowId => _rowId ?? (_rowId = $"m{Context.Module.ModuleId}-epp{EduProgramProfileID}");

        string _groupColumnHeader;

        protected string GroupColumnHeader =>
            _groupColumnHeader ?? (_groupColumnHeader =
                Localization.GetString ("DocumentGroup.Column", Context.LocalResourceFile));

        string _titleColumnHeader;

        protected string TitleColumnHeader =>
            _titleColumnHeader ?? (_titleColumnHeader =
                Localization.GetString ("DocumentTitle.Column", Context.LocalResourceFile));

        string _signatureColumnHeader;

        protected string SignatureColumnHeader =>
            _signatureColumnHeader ?? (_signatureColumnHeader =
                Localization.GetString ("DocumentSignature_Column.Text", Context.LocalResourceFile));

        string FormatDocumentsLinkWithData (IEnumerable<IDocument> documents, string columnSlug, string microdata = "",
            string noLinksText = "-")
        {
            var microdataAttrs = !string.IsNullOrEmpty (microdata) ? " " + microdata : string.Empty;
            var docCount = documents.Count ();
            if (docCount > 0) {
                var table = new StringBuilder (
                    $"<span{microdataAttrs}>"
                    + $"<a type=\"button\" class=\"badge badge-secondary\" data-toggle=\"modal\" data-target=\"#u8y-epp-docs-dlg-{Context.Module.ModuleId}\""
                    + $" data-table=\"doct-{RowId}-{columnSlug}\">{docCount}</a>"
                    + $"<table id=\"doct-{RowId}-{columnSlug}\" class=\"d-none\">"
                    + $"<thead><tr><th>{TitleColumnHeader}</th><th>{SignatureColumnHeader}</th><th>{GroupColumnHeader}</th></tr></thead><tbody>"
                );

                foreach (var document in documents) {
                    GenerateDocumentsTableRow (table, document);
                }

                table.Append ("</tbody></table></span>");
                return table.ToString ();
            }

            return $"<span{microdataAttrs}>{noLinksText}</span>";
        }

        void GenerateDocumentsTableRow (StringBuilder table, IDocument document)
        {
            var docTitle = !string.IsNullOrEmpty (document.Title)
                ? document.Title
                : Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile);
            var docUrl =
                UniversityUrlHelper.LinkClick (document.Url, Context.Module.TabId, Context.Module.ModuleId);

            var sigFile =
                UniversityFileHelper.Instance.GetSignatureFile (
                    UniversityFileHelper.Instance.GetFileByUrl (document.Url));
            var sigUrl = sigFile != null
                ? UniversityUrlHelper.LinkClick ("fileid=" + sigFile.FileId, Context.Module.TabId,
                    Context.Module.ModuleId)
                : string.Empty;

            var rowCssClassAttr = !document.IsPublished (HttpContext.Current.Timestamp)
                ? " class=\"u8y-not-published\""
                : string.Empty;

            table.Append ($"<tr{rowCssClassAttr}>");
            table.Append ($"<td><a href=\"{docUrl}\" target=\"_blank\">{docTitle}</a></td>");
            if (sigFile != null) {
                table.Append ($"<td><a href=\"{sigUrl}\">.sig</a></td>");
            }
            else {
                table.Append ("<td></td>");
            }

            table.Append ($"<td>{document.Group}</td></tr>");
        }

        string GetEduProgramLinks ()
        {
            return Span (
                UniversityFormatHelper.FormatEduProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                    .Append (IsAdopted ? Context.LocalizeString ("IsAdopted.Text") : null, " - ")
                + " "
                + FormatDocumentsLinkWithData (
                    GetDocuments (EduProfile.GetDocumentsOfType (SystemDocumentType.EduProgram)),
                    "oop",
                    IsAdopted ? "itemprop=\"adOpMain\"" : "itemprop=\"opMain\"",
                    string.Empty
                ), "eduName");
        }

        string GetEduPlanLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProfile.GetDocumentsOfType (SystemDocumentType.EduPlan)),
                "epl",
                IsAdopted ? "itemprop=\"adEducationPlan\"" : "itemprop=\"educationPlan\""
            );
        }

        string GetEduScheduleLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProfile.GetDocumentsOfType (SystemDocumentType.EduSchedule)),
                "esh",
                IsAdopted ? "itemprop=\"adEducationShedule\"" : "itemprop=\"educationShedule\""
            );
        }

        string GetWorkProgramAnnotationLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramAnnotation)),
                "wpa",
                IsAdopted ? "itemprop=\"adEducationAnnotation\"" : "itemprop=\"educationAnnotation\""
            );
        }

        string GetEduMaterialLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProfile.GetDocumentsOfType (SystemDocumentType.EduMaterial)),
                "met",
                IsAdopted ? "itemprop=\"adMethodology\"" : "itemprop=\"methodology\""
            );
        }

        string GetWorkProgramLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProfile.GetDocumentsOfType (SystemDocumentType.WorkProgram)),
                "wp",
                // TODO: This related to obsolete WorkProgramOfPractice document type
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

        string Span (string text, string itemprop)
        {
	        return $"<span itemprop=\"{itemprop}\">{text}</span>";
        }
    }
}
