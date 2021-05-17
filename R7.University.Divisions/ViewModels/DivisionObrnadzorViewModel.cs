using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Divisions.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.UI;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Divisions
{
    internal class DivisionObrnadzorViewModel: DivisionInfo
    {
        const string linkFormat = "<a href=\"{0}\" {2} target=\"_blank\"><i class=\"file \"></i> {1}</a>";

        protected ViewModelContext<DivisionDirectorySettings> Context { get; set; }

        public IOccupiedPosition HeadEmployeePosition { get; set; }

        #region Bindable properties

        public string Order { get; protected set; }

        public string TitleLink {
            get {
                var divisionTitle = Title + ((UniversityModelHelper.HasUniqueShortTitle (Title, ShortTitle))? string.Format (" ({0})", ShortTitle) : string.Empty);
                var divisionString = "<span itemprop=\"name\">" + divisionTitle + "</span>";

                if (!string.IsNullOrWhiteSpace (HomePage)) {
                    divisionString = string.Format (linkFormat,
                        Globals.LinkClick (HomePage, Context.Module.TabId, Context.Module.ModuleId),
                        divisionString, string.Empty);
                }

                return divisionString;
            }
        }

        public string WebSiteLink {
            get {
                if (!string.IsNullOrWhiteSpace (WebSite)) {
                    var webSiteUrl = WebSite.Contains ("://") ? WebSite.ToLowerInvariant () :
                        "http://" + WebSite.ToLowerInvariant ();
                    var webSiteLabel = (!string.IsNullOrWhiteSpace (WebSiteLabel)) ? WebSiteLabel :
                        WebSite.Contains ("://") ? WebSite.Remove (0, WebSite.IndexOf ("://") + 3) : WebSite;

                    return string.Format (linkFormat, webSiteUrl, webSiteLabel, "itemprop=\"site\"");
                }

                return string.Empty;
            }
        }

        public string EmailLink {
            get {
                if (!string.IsNullOrWhiteSpace (Email)) {
                    return string.Format (linkFormat, "mailto:" + Email, Email, "itemprop=\"email\"");
                }

                return string.Empty;
            }
        }

        public string DocumentLink => RenderDocumentLink (DocumentUrl);

        string RenderDocumentLink (string documentUrl)
        {
            if (!string.IsNullOrWhiteSpace (documentUrl)) {
                var fa = FontAwesomeHelper.Instance;
                var documentFile = UniversityFileHelper.Instance.GetFileByUrl (documentUrl);

                var linkMarkup =
                    $"<i class=\"fas fa-file-{fa.GetBaseIconNameByExtension(documentFile.Extension)}\""
                    + $"style=\"color:{fa.GetBrandColorByExtension(documentFile.Extension)}\"></i>"
                    + $" <a href=\"{UniversityUrlHelper.LinkClick (documentUrl, Context.Module.TabId, Context.Module.ModuleId)}\" "
                    + "itemprop=\"divisionClauseDocLink\""
                    + $" target=\"_blank\">{Context.LocalizeString ("Regulations.Text")}</a>";

                var sigFile = UniversityFileHelper.Instance.GetSignatureFile (documentFile);

                if (sigFile != null) {
                    linkMarkup += "<span> + </span>"
                                  + $"<a href=\"{UniversityUrlHelper.LinkClickFile (sigFile.FileId, Context.Module.TabId, Context.Module.ModuleId)}\" "
                                  + $"title=\"{Context.LocalizeString ("Signature.Text")}\"><i class=\"fas fa-signature\"></i></a>";
                }

                return linkMarkup;
            }

            return string.Empty;
        }

        public string LocationString {
            get {
                var location = FormatHelper.JoinNotNullOrEmpty (", ", Address, Location);
                if (!string.IsNullOrWhiteSpace (location)) {
                    return "<span itemprop=\"addressStr\">" + location  + "</span>";
                }

                return string.Empty;
            }
        }

        // TODO: Split data into 2 columns?
        public string HeadEmployeeHtml {
            get {
                if (HeadEmployeePosition != null) {
                    var positionTitle = UniversityFormatHelper.FormatShortTitle (HeadEmployeePosition.Position.ShortTitle, HeadEmployeePosition.Position.Title);
                    var headEmployee =  HeadEmployeePosition.Employee;
                    return $"<a href=\"{Context.Module.EditUrl ("employee_id", headEmployee.EmployeeID.ToString (), "EmployeeDetails")}\"><span itemprop=\"fio\">{UniversityFormatHelper.FullName (headEmployee.FirstName, headEmployee.LastName, headEmployee.OtherName)}</span></a><br />"
                        + $"<span itemprop=\"post\">{FormatHelper.JoinNotNullOrEmpty (" ", positionTitle, HeadEmployeePosition.TitleSuffix)}</span>";
                }

                if (HeadPositionID != null) {
                    return Context.LocalizeString ("HeadPosition_IsVacant.Text");
                }
                return Context.LocalizeString ("HeadPosition_NotApplicable.Text");
            }
        }

        #endregion

        public DivisionObrnadzorViewModel (DivisionInfo division, ViewModelContext<DivisionDirectorySettings> context)
        {
            CopyCstor.Copy<DivisionInfo> (division, this);
            Context = context;
        }

        public static IEnumerable<DivisionObrnadzorViewModel> Create (IEnumerable<DivisionInfo> divisions, ViewModelContext<DivisionDirectorySettings> viewModelContext)
        {
            var now = HttpContext.Current.Timestamp;

            // TODO: If parent division not published, ensure what child divisions also not
            var divisionViewModels = divisions.Select (d => new DivisionObrnadzorViewModel (d, viewModelContext))
                .Where (d => d.IsPublished (now) || viewModelContext.Module.IsEditable)
                .Where (d => !d.IsInformal || viewModelContext.Settings.ShowInformal || viewModelContext.Module.IsEditable)
                .ToList ();

            WithHeadEmployeePositions (divisionViewModels);
            CalculateOrder (divisionViewModels);

            return divisionViewModels;
        }

        protected static void WithHeadEmployeePositions (IEnumerable<DivisionObrnadzorViewModel> divisions)
        {
            var now = HttpContext.Current.Timestamp;
            foreach (var division in divisions) {
                division.HeadEmployeePosition = division.GetHeadEmployeePositions ().FirstOrDefault (hep => hep.Employee.IsPublished (now));
            }
        }

        /// <summary>
        /// Calculates the hierarchical order of divisions.
        /// </summary>
        /// <param name="divisions">Divisions that must be properly sorted before the call.</param>
        protected static void CalculateOrder (IList<DivisionObrnadzorViewModel> divisions)
        {
            // TODO: Get hierarchical data from DB without recalculating it?

            const string separator = ".";
            var orderCounter = 1;
            var orderStack = new List<int> ();
            var returnStack = new Stack<DivisionObrnadzorViewModel> ();

            DivisionObrnadzorViewModel prevDivision = null;

            foreach (var division in divisions) {
                if (prevDivision != null) {
                    if (division.ParentDivisionID == prevDivision.ParentDivisionID) {
                        // moving on same level
                        orderCounter++;
                    }
                    else if (division.ParentDivisionID == prevDivision.DivisionID) {
                        // moving down
                        orderStack.Add (orderCounter);
                        returnStack.Push (prevDivision);
                        orderCounter = 1;
                    }
                    else {
                        // moving up
                        while (returnStack.Count > 0 && orderStack.Count > 0) {
                            orderCounter = orderStack [orderStack.Count - 1];
                            orderStack.RemoveAt (orderStack.Count - 1);

                            if (division.ParentDivisionID == returnStack.Pop ().ParentDivisionID) {
                                break;
                            }
                        }

                        orderCounter++;
                    }
                }

                // format order value
                if (orderStack.Count == 0) {
                    division.Order = orderCounter + separator;
                    division.Level = 0;
                }
                else {
                    division.Order = FormatHelper.JoinNotNullOrEmpty (separator, orderStack.Select (o => o.ToString ())) + separator + orderCounter + separator;
                }

                prevDivision = division;
            }
        }
    }
}

