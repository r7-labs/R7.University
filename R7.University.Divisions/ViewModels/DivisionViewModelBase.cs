using System;
using System.Collections.Generic;
using DotNetNuke.Common;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.UI;
using R7.University.Utilities;

namespace R7.University.Divisions.ViewModels
{
    public abstract class DivisionViewModelBase: IDivision
    {
        protected ViewModelContext Dnn { get; }

        protected IDivision Division { get; }

        protected DivisionViewModelBase (IDivision division, ViewModelContext dnn)
        {
            Division = division;
            Dnn = dnn;
        }

        #region IDivision implementation

        public int LastModifiedByUserId => Division.LastModifiedByUserId;

        public DateTime LastModifiedOnDate => Division.LastModifiedOnDate;

        public int CreatedByUserId => Division.CreatedByUserId;

        public DateTime CreatedOnDate => Division.CreatedOnDate;

        public DateTime? StartDate => Division.StartDate;

        public DateTime? EndDate => Division.EndDate;

        public string Title => Division.Title;

        public string ShortTitle => Division.ShortTitle;

        public int DivisionID => Division.DivisionID;

        public int? ParentDivisionID => Division.ParentDivisionID;

        public int? DivisionTermID => Division.DivisionTermID;

        public string HomePage => Division.HomePage;

        public string WebSite => Division.WebSite;

        public string WebSiteLabel => Division.WebSiteLabel;

        public string Phone => Division.Phone;

        public string Fax => Division.Fax;

        public string Email => Division.Email;

        public string SecondaryEmail => Division.SecondaryEmail;

        public string Address => Division.Address;

        public string Location => Division.Location;

        public string WorkingHours => Division.WorkingHours;

        public string DocumentUrl => Division.DocumentUrl;

        public bool IsSingleEntity => Division.IsSingleEntity;

        public bool IsInformal => Division.IsInformal;

        public bool IsGoverning => Division.IsGoverning;

        public int? HeadPositionID => Division.HeadPositionID;

        public ICollection<DivisionInfo> SubDivisions => Division.SubDivisions;

        public ICollection<OccupiedPositionInfo> OccupiedPositions => Division.OccupiedPositions;

        public int Level => Division.Level;

        public string Path => Division.Path;

        #endregion

        protected const string linkFormat = "<a href=\"{0}\" {2} target=\"_blank\"><i class=\"file \"></i> {1}</a>";

        public string TitleLink {
            get {
                var divisionTitle = Title + ((UniversityModelHelper.HasUniqueShortTitle (Title, ShortTitle))? string.Format (" ({0})", ShortTitle) : string.Empty);
                var divisionString = "<span itemprop=\"name\">" + divisionTitle + "</span>";

                if (!string.IsNullOrWhiteSpace (HomePage)) {
                    divisionString = string.Format (linkFormat,
                        Globals.LinkClick (HomePage, Dnn.Module.TabId, Dnn.Module.ModuleId),
                        divisionString, string.Empty);
                }

                return divisionString;
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

        public string DocumentLink => RenderDocumentLink (DocumentUrl);

        string RenderDocumentLink (string documentUrl)
        {
            if (!string.IsNullOrWhiteSpace (documentUrl)) {
                var fa = FontAwesomeHelper.Instance;
                var documentFile = UniversityFileHelper.Instance.GetFileByUrl (documentUrl);

                if (documentFile == null) {
                    return $" <a href=\"{UniversityUrlHelper.LinkClick (documentUrl, Dnn.Module.TabId, Dnn.Module.ModuleId)}\" "
                           + "itemprop=\"divisionClauseDocLink\""
                           + $" target=\"_blank\">{Dnn.LocalizeString ("Regulations.Text")}</a>";
                }

                var linkMarkup =
                    $"<i class=\"fas fa-file-{fa.GetBaseIconNameByExtension(documentFile.Extension)}\""
                    + $"style=\"color:{fa.GetBrandColorByExtension(documentFile.Extension)}\"></i>"
                    + $" <a href=\"{UniversityUrlHelper.LinkClick (documentUrl, Dnn.Module.TabId, Dnn.Module.ModuleId)}\" "
                    + "itemprop=\"divisionClauseDocLink\""
                    + $" target=\"_blank\">{Dnn.LocalizeString ("Regulations.Text")}</a>";

                var sigFile = UniversityFileHelper.Instance.GetSignatureFile (documentFile);
                if (sigFile != null) {
                    linkMarkup += "<span> + </span>"
                                  + $"<a href=\"{UniversityUrlHelper.LinkClickFile (sigFile.FileId, Dnn.Module.TabId, Dnn.Module.ModuleId)}\" "
                                  + $"title=\"{Dnn.LocalizeString ("Signature.Text")}\"><i class=\"fas fa-signature\"></i></a>";
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
    }
}
