//
//  EduProgramProfileViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgram.ViewModels
{
    public class EduProgramProfileViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }
        
        public EduProgramProfileViewModel (IEduProgramProfile model, EduProgramModuleViewModel rootViewModel): base (model)
        {
            RootViewModel = rootViewModel;
        }

        #region Bindable properties

        public string Title_String
        {
            get {
                var title = FormatHelper.FormatEduProgramTitle (
                    Model.ProfileCode,
                    Model.ProfileTitle
                );

                if (string.IsNullOrWhiteSpace (title)) {
                    title = FormatHelper.FormatEduProgramTitle (
                        Model.EduProgram.Code,
                        Model.EduProgram.Title
                    );
                }
                    
                return title;
            }
        }

        public bool AccreditedToDate_Visible
        {
            get { return Model.AccreditedToDate != null; }
        }

        public string AccreditedToDate_String
        {
            get { 
                return Model.AccreditedToDate != null ?
                    Model.AccreditedToDate.Value.ToShortDateString () :
                    string.Empty;
            }
        }

        public bool CommunityAccreditedToDate_Visible
        {
            get { return Model.CommunityAccreditedToDate != null; }
        }

        public string CommunityAccreditedToDate_String
        {
            get { 
                return Model.CommunityAccreditedToDate != null ?
                    Model.CommunityAccreditedToDate.Value.ToShortDateString () :
                    string.Empty;
            }
        }

        public string EduLevel_Title
        {
            get { return Model.EduLevel.Title; }
        }

        public bool EduForms_Visible
        {
            get { return !Model.EduProgramProfileForms.IsNullOrEmpty (); }
        }

        public string EduForms_String
        {
            get {
                if (Model.EduProgramProfileForms != null) {
                    var sb = new StringBuilder ();
                    foreach (var eppf in Model.EduProgramProfileForms) {
                        sb.AppendFormat ("<li>{0} &ndash; {1}</li>", 
                            Localization.GetString ("TimeToLearn" + eppf.EduForm.Title + ".Text", Context.LocalResourceFile),
                            FormatHelper.FormatTimeToLearn (eppf.TimeToLearn,
                                "TimeToLearnYears.Format", "TimeToLearnMonths.Format", Context.LocalResourceFile)
                        );
                    }

                    return string.Format ("<ul>{0}</ul>", sb);
                }

                return null;
            }
        }

        public string Edit_Url
        {
            get {
                return Context.Module.EditUrl (
                    "eduprogramprofile_id",
                    Model.EduProgramProfileID.ToString (),
                    "EditEduProgramProfile"
                );
            }
        }

        public string CssClass
        {
            get { return Model.IsPublished () ? string.Empty : "u8y-not-published"; }
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

