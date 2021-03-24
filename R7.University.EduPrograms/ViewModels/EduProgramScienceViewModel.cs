using System;
using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduPrograms.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramScienceViewModel: EduProgramViewModelBase
    {
        protected ViewModelContext<ScienceDirectorySettings> Context;

        public EduProgramScienceViewModel (IEduProgram eduProgram, ViewModelContext<ScienceDirectorySettings> context)
            : base (eduProgram)
        {
            Context = context;
        }

        #region Bindable properties

        public IHtmlString DirectionsHtml => new HtmlString (GetPopupHtml (EduProgram.Science?.Directions));

        public IHtmlString ResultsHtml => new HtmlString (GetPopupHtml (EduProgram.Science?.Results));

        public IHtmlString BaseHtml => new HtmlString (GetPopupHtml (EduProgram.Science?.Base));

        public string EduLevelTitle => EduProgram.EduLevel.Title;

        [Obsolete]
        public string Scientists => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Scientists);

        [Obsolete]
        public string Students => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Students);

        [Obsolete]
        public string Monographs => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Monographs);

        [Obsolete]
        public string Articles => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Articles);

        [Obsolete]
        public string ArticlesForeign => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.ArticlesForeign);

        [Obsolete]
        public string Patents => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Patents);

        [Obsolete]
        public string PatentsForeign => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.PatentsForeign);

        [Obsolete]
        public string Certificates => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Certificates);

        [Obsolete]
        public string CertificatesForeign => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.CertificatesForeign);

        [Obsolete]
        public string FinancingByScientist =>
            UniversityFormatHelper.ValueOrDash (EduProgram.Science?.FinancingByScientist, FormatExtensions.ToDecimalString);

        public string EditUrl =>
            EduProgram.Science != null
                ? Context.Module.EditUrl ("science_id", EduProgram.Science.ScienceId.ToString (), "EditScience")
                : Context.Module.EditUrl ("eduprogram_id", EduProgram.EduProgramID.ToString (), "EditScience");

        public string CssClass =>
            EduProgram.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public string HtmlElementId => $"science_{Context.Module.ModuleId}_{EduProgramID}";

        public string ModalTitle => $"{EduProgram.FormatTitle ()} ({EduProgram.EduLevel.Title})";

        #endregion

        string GetPopupHtml (string html)
        {
            if (!string.IsNullOrEmpty (html)) {
                return $"<span class=\"d-none description\">{HttpUtility.HtmlDecode (html)}</span>"
        			+ "<a type=\"button\" class=\"badge badge-secondary\" data-toggle=\"modal\""
        			+ $" data-target=\"#u8y-science-descr-dlg-{Context.Module.ModuleId}\">&hellip;</a>";
        	}

            return "-";
        }
    }
}
