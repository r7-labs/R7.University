//
//  EduProgramScienceViewModel.cs
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

        public IHtmlString DirectionsHtml => new HtmlString (GetPopupHtml (EduProgram.Science?.Directions, "perechenNir"));

        public IHtmlString BaseHtml => new HtmlString (GetPopupHtml (EduProgram.Science?.Base, "baseNir"));

        public string Scientists => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Scientists);

        public string Students => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Students);

        public string Monographs => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Monographs);

        public string Articles => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Articles);

        public string ArticlesForeign => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.ArticlesForeign);

        public string Patents => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Patents);

        public string PatentsForeign => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.PatentsForeign);

        public string Certificates => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.Certificates);

        public string CertificatesForeign => UniversityFormatHelper.ValueOrDash (EduProgram.Science?.CertificatesForeign);

        public string FinancingByScientist =>
            UniversityFormatHelper.ValueOrDash (EduProgram.Science?.FinancingByScientist, FormatExtensions.ToDecimalString);

        public string EditUrl =>
            EduProgram.Science != null
                ? Context.Module.EditUrl ("science_id", EduProgram.Science.ScienceId.ToString (), "EditScience")
                : Context.Module.EditUrl ("eduprogram_id", EduProgram.EduProgramID.ToString (), "EditScience");
        
        public string CssClass =>
            EduProgram.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public string HtmlElementId => $"science_{Context.Module.ModuleId}_{EduProgramID}";

        #endregion

        string GetPopupHtml (string html, string itemprop)
        {
            if (!string.IsNullOrEmpty (html)) {
                return $"<span itemprop=\"{itemprop}\" class=\"d-none description\">{HttpUtility.HtmlDecode (html)}</span>"
        			+ "<a type=\"button\" href=\"#\" data-toggle=\"modal\""
        			+ $" data-target=\"#u8y-science-descr-dlg-{Context.Module.ModuleId}\">[&#8230;]</a>";
        	}
            	
        	return $"<span itemprop=\"{itemprop}\">-</span>";
        }
    }
}
