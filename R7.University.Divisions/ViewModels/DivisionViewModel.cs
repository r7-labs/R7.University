//
//  DivisionViewModel.cs
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
    public class DivisionViewModel: IDivision
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

        public string DocumentFileIconUrl {
            get {
                var file = GetCachedDocumentFile ();
                if (file == null) {
                    return null;
                }

                return IconController.GetFileIconUrl (file.Extension);
            }
        }

        public string DocumentFileExtension => GetCachedDocumentFile ()?.Extension;

        private IFileInfo _documentFile;

        IFileInfo GetCachedDocumentFile () => _documentFile ?? (_documentFile = GetDocumentFile (Division.DocumentUrl));

        IFileInfo GetDocumentFile (string url)
        {
            if (Globals.GetURLType (url) != TabType.File) {
                return null;
            }

            var fileId = ParseHelper.ParseToNullable<int> (url.ToLowerInvariant ().Replace ("fileid=", ""));
            if (fileId == null) {
                return null;
            }

            var file = FileManager.Instance.GetFile (fileId.Value);
            return file;
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

        public string DisplayDocumentUrl {
            get { return Globals.LinkClick (Division.DocumentUrl, Context.Module.TabId, Context.Module.ModuleId); }
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
