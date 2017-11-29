//
//  EduProgramProfileObrnadzorDocumentsViewModel.cs
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.ViewModels
{
    internal class EduProgramProfileObrnadzorDocumentsViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramProfileDirectoryDocumentsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext<EduProgramProfileDirectorySettings> Context
        {
            get { return RootViewModel.Context; }
        }

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProgramProfileObrnadzorDocumentsViewModel (
            IEduProgramProfile model,
            EduProgramProfileDirectoryDocumentsViewModel rootViewModel,
            ViewModelIndexer indexer): base (model)
        {
            RootViewModel = rootViewModel;
            Indexer = indexer;
        }

        #region Bindable properties

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string Code
        {
            get { return "<span itemprop=\"EduCode\">" + EduProgram.Code + "</span>"; }
        }

        string _eduProgramLinks;
        public string EduProgram_Links
        {
            get { return _eduProgramLinks ?? (_eduProgramLinks = GetEduProgramLinks ()); }
        }

        public string EduLevel_String
        {
            get { return "<span itemprop=\"EduLevel\">" + EduLevel.Title + "</span>"; }
        }

        string _eduPlanLinks;
        public string EduPlan_Links
        {
            get { return _eduPlanLinks ?? (_eduPlanLinks = GetEduPlanLinks ()); }
        }

        string _eduScheduleLinks;
        public string EduSchedule_Links
        {
            get { return _eduScheduleLinks ?? (_eduScheduleLinks = GetEduScheduleLinks ()); }
        }

        string _workProgramAnnotationLinks;
        public string WorkProgramAnnotation_Links
        {
            get { return _workProgramAnnotationLinks ?? (_workProgramAnnotationLinks = GetWorkProgramAnnotationLinks ()); }
        }

        string _eduMaterialLinks;
        public string EduMaterial_Links
        {
            get { return _eduMaterialLinks ?? (_eduMaterialLinks = GetEduMaterialLinks ()); }
        }

        string _contingentLinks;
        public string Contingent_Links
        {
            get { return _contingentLinks ?? (_contingentLinks = GetContingentLinks ()); }
        }

        string _contingentMovementLinks;
        public string ContingentMovement_Links
        {
            get { return _contingentMovementLinks ?? (_contingentMovementLinks = GetContingentMovementLinks ()); }
        }

        string _workProgramOfPracticeLinks;
        public string WorkProgramOfPractice_Links
        {
            get { return _workProgramOfPracticeLinks ?? (_workProgramOfPracticeLinks = GetWorkProgramOfPracticeLinks ()); }
        }

        string _languagesString;
        public string Languages_String
        {
            get { return _languagesString ?? (_languagesString = GetLanguagesString ()); }
        }

        #endregion

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable).OrderByGroupDescThenSortIndex ();
        }

        string _rowId;
        protected string RowId => _rowId?? (_rowId = $"m{Context.Module.ModuleId}-epp{EduProgramProfileID}");

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
                var docCountText = (docCount > 1 || string.IsNullOrEmpty (linkText))? " [" + docCount + "]" : string.Empty;
                var table = new StringBuilder (
                    $"<span{microdataAttrs}>"
                    + $"<a type=\"button\" href=\"#\" data-toggle=\"modal\" data-target=\"#eduprogram-profile-documents-dialog-{Context.Module.ModuleId}\""
                    + $" data-table=\"doct-{RowId}-{columnSlug}\">{(linkText + docCountText).TrimStart ()}</a>"
                    // TODO: Use class="hidden" instead of inline style?
                    + $"<table id=\"doct-{RowId}-{columnSlug}\" style=\"display:none\">"
                    + $"<thead><tr><th>{TitleColumnHeader}</th><th>{GroupColumnHeader}</th></tr></thead><tbody>"
                );

                foreach (var document in documents) {
                    var docTitle = !string.IsNullOrEmpty (document.Title) ? document.Title : Localization.GetString ("LinkOpen.Text", Context.LocalResourceFile);
                    var docUrl = UniversityUrlHelper.LinkClickIdnHack (document.Url, Context.Module.TabId, Context.Module.ModuleId); 
                    var rowCssClassAttr = !document.IsPublished (HttpContext.Current.Timestamp)? " class=\"u8y-not-published\"" : string.Empty;
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
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduProgram)),
                FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle),
                "oop", "itemprop=\"OOP_main\""
            );
        }

        string GetEduPlanLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduPlan)),
                string.Empty, "epl", "itemprop=\"education_plan\""
            );
        }

        string GetEduScheduleLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduSchedule)),
                string.Empty, "esh", "itemprop=\"education_shedule\""
            );
        }

        string GetWorkProgramAnnotationLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramAnnotation)),
                string.Empty, "wpa", "itemprop=\"education_annotation\""
            );
        }

        string GetEduMaterialLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduMaterial)),
                string.Empty, "met", "itemprop=\"methodology\""
            );
        }

        string GetWorkProgramOfPracticeLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramOfPractice)),
                string.Empty, "wpp", "itemprop=\"EduPr\""
            );
        }

        // TODO: Add more itemprops!
        string GetContingentLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.Contingent)),
                string.Empty, "cnt", "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/priem\""
            );
        }

        // TODO: Add more itemprops!
        string GetContingentMovementLinks ()
        {
            return FormatDocumentsLinkWithData (
                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.ContingentMovement)),
                string.Empty, "cnm",
                "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/Perevod\""
            );
        }

        static char [] languageCodeSeparator = { ';' };

        string GetLanguagesString ()
        {
            if (Languages != null) {
                var languages = Languages
                    .Split (languageCodeSeparator, StringSplitOptions.RemoveEmptyEntries)
                    .Select (L => SafeGetLanguageName (L))
                    .ToList ();

                if (languages.Count > 0) {
                    return "<span itemprop=\"language\">"
                        + HttpUtility.HtmlEncode (TextUtils.FormatList (", ", languages))
                        + "</span>";
                }
            }

            return string.Empty;
        }

        string SafeGetLanguageName (string ietfTag)
        {
            try {
                return CultureInfo.GetCultureInfoByIetfLanguageTag (ietfTag).NativeName;
            }
            catch (CultureNotFoundException) {
                return Localization.GetString ("UnknownLanguage.Text", Context.LocalResourceFile);
            }
        }
    }
}
