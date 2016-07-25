//
//  EduProgramStandardObrnadzorViewModel.cs
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
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramDirectory
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
            var now = HttpContext.Current.Timestamp;

            return documents
                .Where (d => Context.Module.IsEditable || d.IsPublished (now))
                .OrderBy (d => d.Group)
                .ThenBy (d => d.SortIndex);
        }

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string Title_Link
        {
            get {
                if (!string.IsNullOrWhiteSpace (Model.HomePage)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                        Globals.NavigateURL (int.Parse (Model.HomePage)),
                        Model.Title);
                }

                return Model.Title;
            }
        }

        public string EduLevel_String
        {
            get { return FormatHelper.FormatShortTitle (EduLevel.ShortTitle, EduLevel.Title); }
        }

        public string EduStandard_Links
        {
            get { 
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduStandard)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline\">{0}</ul>",
                    "<ul>{0}</ul>",
                    "itemprop=\"EduStandartDoc\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string ProfStandard_Links
        {
            get { 
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.ProfStandard)),
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

