using System.Text;
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;
using R7.University.ModelExtensions;
using R7.Dnn.Extensions.Text;

namespace R7.University.Employees.ViewModels
{
    internal class EmployeeAchievementViewModel: EmployeeAchievementViewModelBase
    {
        public ViewModelContext Context { get; protected set; }

        public EmployeeAchievementViewModel (IEmployeeAchievement model, ViewModelContext context): base (model)
        {
            Context = context;
        }

        #region Bindable properties

        public string Title_String => FormatHelper.JoinNotNullOrEmpty (" ", Title, TitleSuffix);

        public string MoreInfo_Link {
            get {
                var showMoreInfo = Hours != null || EduLevel != null || !string.IsNullOrWhiteSpace (Description);
                if (!showMoreInfo) {
                    return string.Empty;
                }
                var sb = new StringBuilder ();
                if (!string.IsNullOrWhiteSpace (Description)) {
                    sb.Append ($"<p>{Description}</p>");
                }
                if (EduLevel != null) {
                    sb.Append ($"<p><strong>{Context.LocalizeString ("EduLevel.Text")}</strong> {EduLevel.Title}</p>");
                }
                if (Hours != null) {
                    sb.Append ($"<p><strong>{Context.LocalizeString ("Hours.Text")}</strong> {Hours}</p>");
                }
                return $"<a class=\"btn btn-sm btn-link p-0\" role=\"button\" tabindex=\"0\" data-toggle=\"popover\" data-trigger=\"focus\" title=\"{HttpUtility.HtmlEncode (Title_String)}\" "
                    + $"data-html=\"true\" data-content=\"{HttpUtility.HtmlEncode (sb.ToString ())}\"><i class=\"fas fa-info-circle\"></i></a>";
            }
        }

        public string DocumentUrl_Link
        {
            get {
                if (!string.IsNullOrWhiteSpace (DocumentURL)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                        UniversityUrlHelper.LinkClickIdnHack (DocumentURL, Context.Module.TabId, Context.Module.ModuleId),
                        Localization.GetString ("DocumentUrl.Text",  Context.LocalResourceFile));
                }

                return string.Empty;
            }
        }

        public string Years_String
        {
            get {
                return UniversityFormatHelper.FormatYears (EmployeeAchievement.YearBegin, EmployeeAchievement.YearEnd,
                                                 Localization.GetString ("AtTheMoment.Text", Context.LocalResourceFile));
            }
        }

        public string AchievementType_String
        {
            get { return AchievementType.Localize (Context.LocalResourceFile); }
        }

        public string EduLevel_String => EduLevel?.FormatShortTitle () ?? string.Empty;

        #endregion
    }
}
