//
//  EduProgramProfileViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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
using System.Linq;
using System.Text;
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramProfileViewModel: EduProfileViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context => RootViewModel.Context;

        public EduProgramProfileViewModel (IEduProfile model, EduProgramModuleViewModel rootViewModel) : base (model)
        {
            RootViewModel = rootViewModel;
        }

        #region Bindable properties

        public string Title_String => FormatHelper.JoinNotNullOrEmpty (
            ": ",
            Localization.GetString ("EduProgramProfile.Text", Context.LocalResourceFile),
            UniversityFormatHelper.FormatEduProgramTitle (EduProfile.ProfileCode, EduProfile.ProfileTitle)
        );
        
        public bool AccreditedToDate_Visible => EduProfile.AccreditedToDate != null;

        public string AccreditedToDate_String =>
            EduProfile.AccreditedToDate != null ? EduProfile.AccreditedToDate.Value.ToShortDateString () : string.Empty;

        public bool CommunityAccreditedToDate_Visible => EduProfile.CommunityAccreditedToDate != null;

        public string CommunityAccreditedToDate_String =>
            EduProfile.CommunityAccreditedToDate != null ? EduProfile.CommunityAccreditedToDate.Value.ToShortDateString () : string.Empty;

        public string EduLevel_Title => EduProfile.EduLevel.Title;

        public bool ImplementedEduForms_Visible => !ImplementedEduForms.IsNullOrEmpty ();

        public bool EduFormsForAdmission_Visible => !EduFormsForAdmission.IsNullOrEmpty ();

        public int? YearOfAdmission => EduFormsForAdmission.FirstOrDefault ()?.Year.Year;

        public string ImplementedEduForms_String => FormatEduFormYears (ImplementedEduForms);

        public string EduFormsForAdmission_String => FormatEduFormYears (EduFormsForAdmission);

        public string Edit_Url => Context.Module.EditUrl (
            "eduprogramprofile_id",
            EduProfile.EduProgramProfileID.ToString (),
            "EditEduProgramProfile"
        );

        public string CssClass => EduProfile.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public bool DivisionsVisible =>
            EduProfile.Divisions.Any (epd => epd.Division.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable);

        public IEnumerable<EduProgramDivisionViewModel> DivisionViewModels =>
            EduProfile.Divisions
                             .Where (epd => epd.Division.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable)
                             .Select (epd => new EduProgramDivisionViewModel (epd));

        #endregion

        IEnumerable<IEduProgramProfileFormYear> _implementedEduForms;
        protected IEnumerable<IEduProgramProfileFormYear> ImplementedEduForms =>
            _implementedEduForms ?? (_implementedEduForms = GetImplementedEduFormYears ());

        IEnumerable<IEduProgramProfileFormYear> _eduFormsForAdmission;
        protected IEnumerable<IEduProgramProfileFormYear> EduFormsForAdmission =>
            _eduFormsForAdmission ?? (_eduFormsForAdmission = GetEduFormYearsForAdmission ());

        IEnumerable<IEduProgramProfileFormYear> GetImplementedEduFormYears ()
        {
	        return EduProgramProfileFormYears.Where (eppfy => eppfy.YearId == null)
                                             .OrderBy (eppfy => eppfy.EduForm.SortIndex);
        }

        IEnumerable<IEduProgramProfileFormYear> GetEduFormYearsForAdmission ()
        {
            return EduProfile.EduProgramProfileFormYears
                                    .Where (eppfy => eppfy.Year != null && eppfy.Year.AdmissionIsOpen && (eppfy.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable))
                                    .OrderBy (eppfy => eppfy.EduForm.SortIndex);
        }

        string FormatEduFormYears (IEnumerable<IEduProgramProfileFormYear> eppfys)
        {
            if (eppfys != null) {
                var sb = new StringBuilder ();
                foreach (var eppfy in eppfys) {
                    var eduFormTitle = LocalizationHelper.GetStringWithFallback (
                        "TimeToLearn" + eppfy.EduForm.Title + ".Text",
                        Context.LocalResourceFile,
                        eppfy.EduForm.Title
                    ).ToLower ();
                    sb.AppendFormat (
                       "<li>{0}</li>",
                       FormatHelper.JoinNotNullOrEmpty (
                          " &ndash; ",
                          eduFormTitle,
                          (eppfy.EduVolume != null)
                             ? UniversityFormatHelper.FormatTimeToLearn (eppfy.EduVolume.TimeToLearnMonths, eppfy.EduVolume.TimeToLearnHours, TimeToLearnDisplayMode.Both, "TimeToLearn", Context.LocalResourceFile)
                             : null
                       )
                    );
                }

                return string.Format ("<ul>{0}</ul>", sb);
            }

            return string.Empty;
        }
    }
}
