using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.ViewModels
{
    internal class EduProgramProfileEduFormsViewModel: EduProfileViewModelBase
    {
        public EduProgramProfileDirectoryEduFormsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext<EduProgramProfileDirectorySettings> Context => RootViewModel.Context;

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProgramProfileEduFormsViewModel (
            IEduProfile model,
            EduProgramProfileDirectoryEduFormsViewModel rootViewModel,
            ViewModelIndexer indexer): base (model)
        {
            RootViewModel = rootViewModel;
            Indexer = indexer;
        }

        public int Order => Indexer.GetNextIndex ();

        public string Code => Span (EduProgram.Code, "eduCode");

        public string Title => Span (
            UniversityFormatHelper.FormatEduProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle)
                .Append (IsAdopted? Context.LocalizeString ("IsAdopted.Text") : null, " - "),
            "eduName"
        );

        public string EduLevelString => Span (EduLevel.Title, "eduLevel");

        public string AccreditedToDateString => (AccreditedToDate != null)
            ? Span (AccreditedToDate.Value.ToShortDateString (), "dateEnd")
            : string.Empty;

        public string CommunityAccreditedToDateString => (CommunityAccreditedToDate != null)
            ? Span (Span (CommunityAccreditedToDate.Value.ToShortDateString (), "dateEnd"), "eduPOAccred")
            : string.Empty;

        public string EduForms_String
        {
        	get {
                var formYears = GetImplementedEduFormYears ();
                if (!formYears.IsNullOrEmpty ()) {
                    return "<ul itemprop=\"learningTerm\">" + formYears
                        .Select (eppfy => (eppfy.IsPublished (_now) ? "<li>" : "<li class=\"u8y-not-published-element\">")
                                 + LocalizationHelper.GetStringWithFallback ("EduForm_" + eppfy.EduForm.Title + ".Text", Context.LocalResourceFile, eppfy.EduForm.Title).ToLower ()
                                 + ((eppfy.EduVolume != null && (eppfy.EduVolume.TimeToLearnMonths != 0 || eppfy.EduVolume.TimeToLearnHours != 0))
                                    ? ("&nbsp;- " + UniversityFormatHelper.FormatTimeToLearn (eppfy.EduVolume.TimeToLearnMonths, eppfy.EduVolume.TimeToLearnHours, Context.Settings.TimeToLearnDisplayMode, "TimeToLearn", Context.LocalResourceFile))
                                    : string.Empty)
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
                    return $"<span itemprop=\"language\">{HttpUtility.HtmlEncode (FormatHelper.JoinNotNullOrEmpty (", ", languages))}</span>";
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

        string Span (string text, string itemprop)
        {
	        return $"<span itemprop=\"{itemprop}\">{text}</span>";
        }
    }
}
