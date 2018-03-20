//
//  ContingentViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.EduProgramProfiles.ViewModels
{
    public class ContingentViewModel : IEduProgramProfileFormYear
    {
        protected readonly IEduProgramProfileFormYear FormYear;

        protected readonly ViewModelContext<ContingentDirectorySettings> Context;

        public readonly ContingentDirectoryViewModel RootViewModel;

        public ContingentViewModel (IEduProgramProfileFormYear formYear,
                                    ViewModelContext<ContingentDirectorySettings> context,
                                    ContingentDirectoryViewModel rootViewModel)
        {
            FormYear = formYear;
            Context = context;
            RootViewModel = rootViewModel;
        }

        #region IEduProgramProfileFormYear implementation

        public int EduProgramProfileFormYearId => FormYear.EduProgramProfileFormYearId;

        public int EduProgramProfileId => FormYear.EduProgramProfileId;

        public int EduFormId => FormYear.EduFormId;

        public int? YearId => FormYear.YearId;

        public IEduForm EduForm => FormYear.EduForm;

        public IYear Year => FormYear.Year;

        public IEduVolume EduVolume => FormYear.EduVolume;

        public IContingent Contingent => FormYear.Contingent;

        public IEduProgramProfile EduProgramProfile => FormYear.EduProgramProfile;

        public DateTime? StartDate => FormYear.EduProgramProfile.StartDate ?? FormYear.StartDate;

        public DateTime? EndDate => FormYear.EduProgramProfile.EndDate ?? FormYear.EndDate;

        #endregion

        #region Bindable properties

        public string EditIconUrl => FormYear.Contingent != null ? UniversityIcons.Edit : UniversityIcons.AddAlternate;

        public string EditUrl =>
            FormYear.Contingent != null
                    ? Context.Module.EditUrl ("contingent_id", FormYear.Contingent.ContingentId.ToString (), "EditContingent")
                    : Context.Module.EditUrl ("eduprogramprofileformyear_id", FormYear.EduProgramProfileFormYearId.ToString (), "EditContingent");

        public string CssClass =>
            this.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public string EduProgramProfileTitle => FormYear.EduProgramProfile.FormatTitle (withEduProgramCode: false);

        public string EduFormTitle
        {
            get {
                var sysEduForm = FormYear.EduForm.GetSystemEduForm ();
                if (sysEduForm != SystemEduForm.Custom) {
                    return Context.LocalizeString ($"EduForm_{sysEduForm}.Text").ToLower ();
                }
                return FormYear.EduForm.Title.ToLower ();
            }
        }

        public string VacantFB => FormatValue (() => FormYear.Contingent?.VacantFB);

        public string VacantRB => FormatValue (() => FormYear.Contingent?.VacantRB);

        public string VacantMB => FormatValue (() => FormYear.Contingent?.VacantMB);

        public string VacantBC => FormatValue (() => FormYear.Contingent?.VacantBC);

        public string ActualFB => FormatValue (() => FormYear.Contingent?.ActualFB);

        public string ActualRB => FormatValue (() => FormYear.Contingent?.ActualRB);

        public string ActualMB => FormatValue (() => FormYear.Contingent?.ActualMB);

        public string ActualBC => FormatValue (() => FormYear.Contingent?.ActualBC);

        public string AvgAdmScore => FormatValue (() => FormYear.Contingent?.AvgAdmScore);

        public string AdmittedFB => FormatValue (() => FormYear.Contingent?.AdmittedFB);

        public string AdmittedRB => FormatValue (() => FormYear.Contingent?.AdmittedRB);

        public string AdmittedMB => FormatValue (() => FormYear.Contingent?.AdmittedMB);

        public string AdmittedBC => FormatValue (() => FormYear.Contingent?.AdmittedBC);

        public string MovedIn => FormatValue (() => FormYear.Contingent?.MovedIn);

        public string MovedOut => FormatValue (() => FormYear.Contingent?.MovedOut);

        public string Restored => FormatValue (() => FormYear.Contingent?.Restored);

        public string Expelled => FormatValue (() => FormYear.Contingent?.Expelled);

        public string EduLevelVacantItemProp {
            get {
                // HACK: Hardcoded edu. levels 
                var eduLevel = EduProgramProfile.EduLevel.Title.ToLower ();
                if (eduLevel.Contains ("бакалавриат")) {
                    return "bachelorVacant";
                }
                if (eduLevel.Contains ("магистратура")) {
                    return "magistracyVacant";
                }
                if (eduLevel.Contains ("специалитет")) {
                    return "specialityVacant";
                }
                if (eduLevel.Contains ("высшей квалификации") || eduLevel.Contains ("аспирантура")) {
                    return "postgraduateVacant";
                }
                return string.Empty;
            }
        }

        public int? Course => UniversityModelHelper.SafeGetCourse (Year, RootViewModel.LastYear);

        public string HtmlElementId => $"contingent_{Context.Module.ModuleId}_{EduProgramProfileFormYearId}";

        #endregion

        string FormatValue<T> (Func<T?> getValue) where T : struct
        {
            var value = getValue ();
            return value != null ? value.ToString () : "-";
        }
    }
}
