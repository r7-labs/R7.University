//
//  EduProgramStandardObrnadzorViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2018 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Web;
using DotNetNuke.Common;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    internal class EduProgramStandardObrnadzorViewModel: EduProgramViewModelBase
    {
        public IIndexer Indexer { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramStandardObrnadzorViewModel (IEduProgram model, ViewModelContext context, IIndexer indexer)
            : base (model)
        {
            Context = context;
            Indexer = indexer;
        }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable).OrderByGroupDescThenSortIndex ();
        }

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string Title_Link
        {
            get {
                if (!string.IsNullOrWhiteSpace (EduProgram.HomePage)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                        Globals.NavigateURL (int.Parse (EduProgram.HomePage)),
                        EduProgram.Title);
                }

                return EduProgram.Title;
            }
        }

        public string EduLevel_String
        {
            get { return UniversityFormatHelper.FormatShortTitle (EduLevel.ShortTitle, EduLevel.Title); }
        }

        public string EduStandard_Links
        {
            get {
                return UniversityFormatHelper.FormatDocumentLinks (
                    GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemprop=\"eduFedDoc\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string ProfStandard_Links
        {
            get {
                return UniversityFormatHelper.FormatDocumentLinks (
                    GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.ProfStandard)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    string.Empty,
                    DocumentGroupPlacement.InTitle
                );
            }
        }
    }
}

