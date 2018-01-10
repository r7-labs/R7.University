//
//  EduProgramProfileEduFormsViewModel.cs
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
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.ViewModels
{
    internal class EduProgramProfileEduFormsViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramProfileDirectoryEduFormsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext<EduProgramProfileDirectorySettings> Context => RootViewModel.Context;

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProgramProfileEduFormsViewModel (
            IEduProgramProfile model,
            EduProgramProfileDirectoryEduFormsViewModel rootViewModel,
            ViewModelIndexer indexer): base (model)
        {
            RootViewModel = rootViewModel;
            Indexer = indexer;
        }

        protected IEduVolume FullTimeFormVolume => GetEduProgramProfileFormYears ()
            .FirstOrDefault (eppfy => eppfy.EduForm.GetSystemEduForm () == SystemEduForm.FullTime)?.EduVolume; 

        protected IEduVolume PartTimeFormVolume => GetEduProgramProfileFormYears ()
            .FirstOrDefault (eppfy => eppfy.EduForm.GetSystemEduForm () == SystemEduForm.PartTime)?.EduVolume; 

        protected IEduVolume ExtramuralFormVolume => GetEduProgramProfileFormYears ()
            .FirstOrDefault (eppfy => eppfy.EduForm.GetSystemEduForm () == SystemEduForm.Extramural)?.EduVolume; 
        
        IEnumerable<EduProgramProfileFormYearInfo> GetEduProgramProfileFormYears ()
        {
            return EduProgramProfileFormYears
                .Where (eppfy => !eppfy.Year.AdmissionIsOpen && (eppfy.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable))
                .OrderByDescending (eppfy => eppfy.Year.Year);
        }

        protected string TimeToLearnApplyMarkup (string eduFormResourceKey, string timeToLearn)
        {
            return "<span class=\"hidden\" itemprop=\"EduForm\">"
            + Localization.GetString (eduFormResourceKey, Context.LocalResourceFile)
            + "</span>" + "<span itemprop=\"LearningTerm\">" + timeToLearn + "</span>";
        }

        public string TimeToLearnFullTimeString
        {
            get { 
                if (FullTimeFormVolume == null) {
                    return string.Empty; 
                }

                return TimeToLearnApplyMarkup (
                    "TimeToLearnFullTime.Column",
                    FormatHelper.FormatTimeToLearn (FullTimeFormVolume.TimeToLearnMonths, FullTimeFormVolume.TimeToLearnHours, Context.Settings.TimeToLearnDisplayMode, "TimeToLearn", Context.LocalResourceFile)
                );
            }
        }

        public string TimeToLearnPartTimeString
        {
            get {
                if (PartTimeFormVolume == null) {
                    return string.Empty; 
                }

                return TimeToLearnApplyMarkup (
                    "TimeToLearnPartTime.Column",
                    FormatHelper.FormatTimeToLearn (PartTimeFormVolume.TimeToLearnMonths, PartTimeFormVolume.TimeToLearnHours, Context.Settings.TimeToLearnDisplayMode, "TimeToLearn", Context.LocalResourceFile)
                );
            }
        }

        public string TimeToLearnExtramuralString
        {
            get {
                if (ExtramuralFormVolume == null) {
                    return string.Empty; 
                }

                return TimeToLearnApplyMarkup (
                    "TimeToLearnExtramural.Column",
                    FormatHelper.FormatTimeToLearn (ExtramuralFormVolume.TimeToLearnMonths, ExtramuralFormVolume.TimeToLearnHours, Context.Settings.TimeToLearnDisplayMode, "TimeToLearn", Context.LocalResourceFile)
                );
            }
        }

        public int Order => Indexer.GetNextIndex ();

        public string Code => "<span itemprop=\"EduCode\">" + EduProgram.Code + "</span>";

        public string Title => FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle);

        public string EduLevelString => "<span itemprop=\"EduLevel\">" + EduLevel.Title + "</span>";

        public string AccreditedToDateString {
            get { 
                if (AccreditedToDate != null) {
                    return "<span itemprop=\"DateEnd\">" + AccreditedToDate.Value.ToShortDateString () + "</span>";
                }

                return string.Empty;
            }
        }

        string _languagesString;
        public string Languages_String => 
            _languagesString ?? (_languagesString = GetLanguagesString ());

        static char [] languageCodeSeparator = { ';' };

        string GetLanguagesString ()
        {
            if (Languages != null) {
                var languages = Languages
                	.Split (languageCodeSeparator, StringSplitOptions.RemoveEmptyEntries)
                	.Select (L => SafeGetLanguageName (L))
                	.ToList ();

                if (languages.Count > 0) {
                    return $"<span itemprop=\"language\">{HttpUtility.HtmlEncode (TextUtils.FormatList (", ", languages))}</span>";
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
