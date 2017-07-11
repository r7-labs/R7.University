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
using R7.University.EduProgramProfileDirectory.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfileDirectory.ViewModels
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
            var now = HttpContext.Current.Timestamp;
            return documents
                .Where (d => Context.Module.IsEditable || d.IsPublished (now))
                .OrderBy (d => d.Group)
                .ThenBy (d => d.SortIndex);
        }
        string GetEduProgramLinks ()
        {
            var eduProgramDocuments = GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduProgram));
            if (!eduProgramDocuments.IsNullOrEmpty ()) {
                return FormatHelper.FormatDocumentLinks (
                    eduProgramDocuments,
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
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

        string GetEduPlanLinks ()
        {
            return FormatHelper.FormatDocumentLinks (
                                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduPlan)),
                                Context,
                                "<li>{0}</li>",
                                "<ul class=\"list-inline\">{0}</ul>",
                                "<ul>{0}</ul>",
                                "itemprop=\"education_plan\"",
                                DocumentGroupPlacement.InTitle
                            );
        }

        string GetEduScheduleLinks ()
        {
            return FormatHelper.FormatDocumentLinks (
                                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduSchedule)),
                                Context,
                                "<li>{0}</li>",
                                "<ul class=\"list-inline\">{0}</ul>",
                                "<ul>{0}</ul>",
                                "itemprop=\"education_shedule\"",
                                DocumentGroupPlacement.InTitle
                            );
        }

        string GetWorkProgramAnnotationLinks ()
        {
            return FormatHelper.FormatDocumentLinks (
                                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramAnnotation)),
                                Context,
                                "<li>{0}</li>",
                                "<ul class=\"list-inline\">{0}</ul>",
                                "<ul>{0}</ul>",
                                "itemprop=\"education_annotation\"",
                                DocumentGroupPlacement.InTitle
                            );
        }

        string GetEduMaterialLinks ()
        {
            return FormatHelper.FormatDocumentLinks (
                                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.EduMaterial)),
                                Context,
                                "<li>{0}</li>",
                                "<ul class=\"list-inline\">{0}</ul>",
                                "<ul>{0}</ul>",
                                "itemprop=\"methodology\"",
                                DocumentGroupPlacement.InTitle
                            );
        }

        string GetContingentLinks ()
        {
            return FormatHelper.FormatDocumentLinks (
                                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.Contingent)),
                                Context,
                                "<li>{0}</li>",
                                "<ul class=\"list-inline\">{0}</ul>",
                                "<ul>{0}</ul>",
                                "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/priem\"",
                                DocumentGroupPlacement.InTitle
                            );
        }

        string GetContingentMovementLinks ()
        {
            return FormatHelper.FormatDocumentLinks (
                                GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.ContingentMovement)),
                                Context,
                                "<li>{0}</li>",
                                "<ul class=\"list-inline\">{0}</ul>",
                                "<ul>{0}</ul>",
                                "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/Perevod\"",
                                DocumentGroupPlacement.InTitle
                            );
        }

        string GetWorkProgramOfPracticeLinks ()
        {
            var wpopDocuments = GetDocuments (EduProgramProfile.GetDocumentsOfType (SystemDocumentType.WorkProgramOfPractice));

            // get all groups
            var groups = wpopDocuments
                .Select (d => d.Group)
                .Distinct ();

            var markupBuilder = new StringBuilder ();
            var groupMarkupBuilder = new StringBuilder ();

            var now = HttpContext.Current.Timestamp;

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
                                     "itemprop=\"EduPr\"",
                                     now
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
                return ((groupCount == 1) ? "<ul class=\"list-inline\">" : "<ul>") + markup + "</ul>";
            }

            return string.Empty;
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
