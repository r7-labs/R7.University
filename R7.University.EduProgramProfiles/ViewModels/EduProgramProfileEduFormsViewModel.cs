//
//  EduProgramProfileEduFormsViewModel.cs
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
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

        public int Order => Indexer.GetNextIndex ();

        public string Code => Wrap (EduProgram.Code, "eduCode");

        public string Title => Wrap (
            FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                .Append (IsAdopted? Context.LocalizeString ("IsAdopted.Text") : null, " - "),
            "eduName"
        );

        public string EduLevelString => Wrap (EduLevel.Title, "eduLevel");

        public string AccreditedToDateString => (AccreditedToDate != null)
            ? Wrap (AccreditedToDate.Value.ToShortDateString (), "dateEnd")
            : string.Empty;
        
        public string EduForms_String
        {
        	get {
                var formYears = GetImplementedEduFormYears ();
                if (!formYears.IsNullOrEmpty ()) {
        			return "<ul itemprop=\"learningTerm\">" + formYears
                        .Select (eppfy => (eppfy.IsPublished (_now) ? "<li>" : "<li class=\"u8y-not-published-element\">")
                                 + LocalizationHelper.GetStringWithFallback ("EduForm_" + eppfy.EduForm.Title + ".Text", Context.LocalResourceFile, eppfy.EduForm.Title).ToLower ()
                                 + ((eppfy.EduVolume == null) 
                                    ? string.Empty
                                    : ("&nbsp;- " + FormatHelper.FormatTimeToLearn (eppfy.EduVolume.TimeToLearnMonths, eppfy.EduVolume.TimeToLearnHours, Context.Settings.TimeToLearnDisplayMode, "TimeToLearn", Context.LocalResourceFile)))
                                 + "</li>")
        				.Aggregate ((s1, s2) => s1 + s2) + "</ul>";
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

        DateTime _now => HttpContext.Current.Timestamp;

        IEnumerable<IEduProgramProfileFormYear> GetImplementedEduFormYears ()
        {
            return EduProgramProfileFormYears.Where (eppfy => eppfy.Year == null &&
                                                     (eppfy.IsPublished (_now) || Context.Module.IsEditable))
                                             .OrderBy (eppfy => eppfy.EduForm.SortIndex);
        }

        string Wrap (string text, string itemprop)
        {
	        return $"<span itemprop=\"{itemprop}\">{text}</span>";
        }
    }
}
