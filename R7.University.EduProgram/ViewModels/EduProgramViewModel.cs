//
//  EduProgramViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using System.Web;
using DotNetNuke.Common;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgram.ViewModels
{
    internal class EduProgramViewModel: EduProgramViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }
        
        public EduProgramViewModel (IEduProgram model, EduProgramModuleViewModel rootViewModel): base (model)
        {
            RootViewModel = rootViewModel;
        }

        public IEnumerable<EduProgramProfileViewModel> EduProgramProfileViewModels { get; set; }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            var now = HttpContext.Current.Timestamp;

            return documents
                .Where (d => Context.Module.IsEditable || d.IsPublished (now))
                .OrderBy (d => d.Group)
                .ThenBy (d => d.SortIndex);
        }

        #region Bindable properties

        public string Title_String
        {
            get { return FormatHelper.FormatEduProgramTitle (Model.Code, Model.Title); }
        }

        public string EduLevel_Title
        {
            get { return Model.EduLevel.Title; }
        }

        public bool EduStandard_Visible
        {
            get { return GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduStandard)).Any (); }
        }

        public string EduStandard_Links
        {
            get { 
                return FormatHelper.FormatDocumentLinks (
                    GetDocuments (Model.GetDocumentsOfType (SystemDocumentType.EduStandard)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline u8y-inline\">{0}</ul>",
                    "<ul class=\"list-inline u8y-inline\">{0}</ul>",
                    "itemprop=\"EduStandartDoc\"",
                    DocumentGroupPlacement.InTitle
                );
            }
        }

        public string Edit_Url
        {
            get {
                return Context.Module.EditUrl (
                    "eduprogram_id",
                    Model.EduProgramID.ToString (),
                    "EditEduProgram"
                );
            }
        }

        public string CssClass
        {
            get {
                return Model.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published"; 
            }
        }

        public bool EduProgramProfiles_Visible
        {
            get { 
                return EduProgramProfileViewModels.Any (epp => epp.IsPublished (HttpContext.Current.Timestamp) 
                    || Context.Module.IsEditable
                ); 
            }
        }

        public bool Division_Visible
        {
            get { return Model.Division != null; }
        }

        public string Division_Link
        {
            get { 
                if (Model.Division != null) {
                    if (!string.IsNullOrWhiteSpace (Model.Division.HomePage)) {
                        return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                            // REVIEW: Model.Division.HomePage could not contain tabId
                            Globals.NavigateURL (int.Parse (Model.Division.HomePage)), Model.Division.Title
                        );
                    }
                    return Model.Division.Title;
                }
                return string.Empty;
            }
        }

        #endregion
    }
}

