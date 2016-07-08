//
//  EduProgramProfileObrnadzorDocumentsViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

namespace R7.University.EduProgramProfileDirectory.ViewModels
{
    public class EduProgramProfileObrnadzorDocumentsViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramProfileDirectoryDocumentsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
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

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents
                .Where (d => Context.Module.IsEditable || d.IsPublished ())
                .OrderBy (d => d.Group)
                .ThenBy (d => d.SortIndex);
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

        public string EduProgram_Links
        {
            get {
                var eduProgramDocuments = GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduProgram));
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
        }

        public string EduLevel_String
        {
            get { return "<span itemprop=\"EduLevel\">" + EduLevel.Title + "</span>"; }
        }

        public string EduPlan_Links
        {
            get { 
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduPlan)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemprop=\"education_plan\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string EduSchedule_Links
        {
            get { 
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduSchedule)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemprop=\"education_shedule\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string WorkProgramAnnotation_Links
        {
            get {
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.WorkProgramAnnotation)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemprop=\"education_annotation\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string EduMaterial_Links
        {
            get {
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduMaterial)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemprop=\"methodology\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string Contingent_Links
        {
            get {
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.Contingent)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemscope=\"\" itemtype=\"http://obrnadzor.gov.ru/microformats/priem\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string ContingentMovement_Links
        {
            get {
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.ContingentMovement)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
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
