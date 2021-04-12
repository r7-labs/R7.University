using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Divisions.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Divisions.ViewModels
{
    public class DivisionViewModel : IDivision
    {
        protected IDivision Division;

        protected ViewModelContext<DivisionSettings> Context;

        public DivisionViewModel ()
        {
        }

        public DivisionViewModel (IDivision division, ViewModelContext<DivisionSettings> context)
        {
            Division = division;
            Context = context;

            _subDivisionViewModels = Division.SubDivisions.Select (d => new DivisionViewModel (d, Context)).ToList ();
        }

        #region IDivision implementation

        public string Address => Division.Address;

        public int CreatedByUserId => Division.CreatedByUserId;

        public DateTime CreatedOnDate => Division.CreatedOnDate;

        public int DivisionID => Division.DivisionID;

        public int? DivisionTermID => Division.DivisionTermID;

        public string DocumentUrl {
            get {
                if (!string.IsNullOrEmpty (Division.DocumentUrl)) {
                    return Globals.LinkClick (Division.DocumentUrl, Context.Module.TabId, Context.Module.ModuleId);
                }

                return string.Empty;
            }
        }

        public string Email => Division.Email;

        public DateTime? EndDate => Division.EndDate;

        public string Fax => Division.Fax;

        public int? HeadPositionID => Division.HeadPositionID;

        public string HomePage => Division.HomePage;

        public bool IsInformal => Division.IsInformal;

        public bool IsSingleEntity => Division.IsSingleEntity;

        public bool IsGoverning => Division.IsGoverning;

        public int LastModifiedByUserId => Division.LastModifiedByUserId;

        public DateTime LastModifiedOnDate => Division.LastModifiedOnDate;

        public int Level => Division.Level;

        public string Location => Division.Location;

        public int? ParentDivisionID => Division.ParentDivisionID;

        public string Path => Division.Path;

        public string Phone => Division.Phone;

        public string SecondaryEmail => Division.SecondaryEmail;

        public string ShortTitle => Division.ShortTitle;

        public DateTime? StartDate => Division.StartDate;

        public ICollection<DivisionInfo> SubDivisions => Division.SubDivisions;

        public ICollection<OccupiedPositionInfo> OccupiedPositions => Division.OccupiedPositions;

        public string Title => Division.Title;

        public string WebSite => Division.WebSite;

        public string WebSiteLabel => Division.WebSiteLabel;

        public string WorkingHours => Division.WorkingHours;

        #endregion

        public bool IsEmpty {
            get { return Division == null; }
        }

        ICollection<DivisionViewModel> _subDivisionViewModels;

        public ICollection<DivisionViewModel> SubDivisionViewModels {
            get {
                var now = HttpContext.Current.Timestamp;
                return _subDivisionViewModels.Where (d => Context.Module.IsEditable || d.IsPublished (now))
                    .OrderBy (d => d.Title)
                    .ToList ();
            }
        }

        public string DisplayTitle {
            get {
                if (UniversityModelHelper.HasUniqueShortTitle (Division.ShortTitle, Division.Title)) {
                    return Division.Title + $" ({Division.ShortTitle})";
                }

                return Division.Title;
            }
        }

        public string DocumentFileExtension => GetCachedDocumentFile ()?.Extension;

        private IFileInfo _documentFile;

        IFileInfo GetCachedDocumentFile () =>
            _documentFile ?? (_documentFile = UniversityFileHelper.Instance.GetFileByUrl (Division.DocumentUrl));

        public string DocumentSignatureFileUrl {
            get {
                var sigFile = UniversityFileHelper.Instance.GetSignatureFile (GetCachedDocumentFile ());
                if (sigFile == null) {
                    return null;
                }
                return UniversityUrlHelper.LinkClickFile (sigFile.FileId, Context.Module.TabId, Context.Module.ModuleId);
            }
        }

        public string DisplayFax {
            get {
                return string.Format (Localization.GetString ("Fax.Format", Context.LocalResourceFile), Division.Fax);
            }
        }

        public string BarcodeImageUrl {
            get {
                var barcodeWidth = UniversityConfig.Instance.Barcode.DefaultWidth;
                return UniversityUrlHelper.FullUrl (string.Format (
                    "/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
                    barcodeWidth, barcodeWidth,
                    HttpUtility.UrlEncode (Division.VCard ().ToString ()
                                           // fix for "+" signs in phone numbers
                                          .Replace ("+", "%2b"))
                ));
            }
        }

        string _termUrl;
        public string DisplayTermUrl {
            get {
                if (_termUrl == null) {
                    if (Division.DivisionTermID != null) {
                        var termCtrl = new TermController ();
                        var term = termCtrl.GetTerm (Division.DivisionTermID.Value);
                        if (term != null) {
                            // add raw tag to Globals.NavigateURL to allow search work independently of current friendly urls settings
                            _termUrl = Globals.NavigateURL (PortalSettings.Current.SearchTabId) + "?tag=" + term.Name;
                        }
                        else {
                            _termUrl = string.Empty;
                        }
                    }
                }
                return _termUrl;
            }
        }

        public bool HasHomePage {
            get {
                int homeTabId;
                return int.TryParse (Division.HomePage, out homeTabId);
            }
        }

        public string HomePageUrl {
            get { return Globals.NavigateURL (int.Parse (Division.HomePage)); }
        }

        public string CssClass
        {
            get { return !Division.IsPublished (HttpContext.Current.Timestamp) ? "u8y-not-published-element" : string.Empty; }
        }

        public string WebSiteUrl {
            get { return UniversityFormatHelper.FormatWebSiteUrl (Division.WebSite); }
        }

        public string DisplayWebSiteLabel {
            get { return UniversityFormatHelper.FormatWebSiteLabel (Division.WebSite, Division.WebSiteLabel); }
        }
    }
}
