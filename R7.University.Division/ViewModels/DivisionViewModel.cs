//
//  DivisionViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Division.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;
using DotNetNuke.Common;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Portals;
using System.Linq;
using System.Net.Http.Headers;

namespace R7.University.Division.ViewModels
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

            if (Division.SubDivisions != null) {
                _subDivisionViewModels = Division.SubDivisions.Select (d => new DivisionViewModel (d, Context)).ToList (); 
            }
            else {
                _subDivisionViewModels = new List<DivisionViewModel> ();
            }
        }

        #region IDivision implementation

        public string Address {
            get { return Division.Address; }
            set { throw new NotImplementedException (); }
        }

        public int CreatedByUserID {
            get { return Division.CreatedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime CreatedOnDate {
            get { return Division.CreatedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public int DivisionID {
            get { return Division.DivisionID; }
            set { throw new NotImplementedException (); }
        }

        public int? DivisionTermID {
            get { return Division.DivisionTermID; }
            set { throw new NotImplementedException (); }
        }

        public string DocumentUrl {
            get { return Division.DocumentUrl; }
            set { throw new NotImplementedException (); }
        }

        public string Email {
            get { return Division.Email; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? EndDate {
            get { return Division.EndDate; }
            set { throw new NotImplementedException (); }
        }

        public string Fax {
            get { return Division.Fax; }
            set { throw new NotImplementedException (); }
        }

        public int? HeadPositionID {
            get { return Division.HeadPositionID; }
            set { throw new NotImplementedException (); }
        }

        public string HomePage {
            get { return Division.HomePage; }
            set { throw new NotImplementedException (); }
        }

        public bool IsInformal {
            get { return Division.IsInformal; }
            set { throw new NotImplementedException (); }
        }

        public bool IsVirtual {
            get { return Division.IsVirtual; }
            set { throw new NotImplementedException (); }
        }

        public int LastModifiedByUserID {
            get { return Division.LastModifiedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime LastModifiedOnDate {
            get { return Division.LastModifiedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public int Level {
            get { return Division.Level; }
            set { throw new NotImplementedException (); }
        }

        public string Location {
            get { return Division.Location; }
            set { throw new NotImplementedException (); }
        }

        public int? ParentDivisionID {
            get { return Division.ParentDivisionID; }
            set { throw new NotImplementedException (); }
        }

        public string Path
        {
            get { return Division.Path; }
            set { throw new NotImplementedException (); }
        }

        public string Phone {
            get { return Division.Phone; }
            set { throw new NotImplementedException (); }
        }

        public string SecondaryEmail {
            get { return Division.SecondaryEmail; }
            set { throw new NotImplementedException (); }
        }

        public string ShortTitle {
            get { return Division.ShortTitle; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? StartDate {
            get { return Division.StartDate; }
            set { throw new NotImplementedException (); }
        }

        public ICollection<DivisionInfo> SubDivisions {
            get { return Division.SubDivisions; }
            set { throw new NotImplementedException (); }
        }

        public string Title {
            get { return Division.Title; }
            set { throw new NotImplementedException (); }
        }

        public string WebSite {
            get { return Division.WebSite; }
            set { throw new NotImplementedException (); }
        }

        public string WebSiteLabel {
            get { return Division.WebSiteLabel; }
            set { throw new NotImplementedException (); }
        }

        public string WorkingHours {
            get { return Division.WorkingHours; }
            set { throw new NotImplementedException (); }
        }

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
                if (ModelHelper.HasUniqueShortTitle (Division.ShortTitle, Division.Title)) {
                    return Division.Title + $" ({Division.ShortTitle})";
                }
                return Division.Title;
            }
        }

        public string DisplayFax {
            get {
                return string.Format (Localization.GetString ("Fax.Format", Context.LocalResourceFile), Division.Fax);
            }
        }

        string _location;
        public string DisplayLocation {
            get {
                if (_location == null) {
                    _location = Context.Settings.ShowAddress
                                       ? TextUtils.FormatList (", ", Division.Address, Division.Location)
                                       : Division.Location;
                }

                return _location;
            }
        }

        public string BarcodeImageUrl {
            get {
                var barcodeWidth = UniversityConfig.Instance.Barcode.DefaultWidth;
                return UniversityUrlHelper.FullUrl (string.Format (
                    "/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
                    barcodeWidth, barcodeWidth,
                    HttpUtility.UrlEncode (Division.GetVCard ().ToString ()
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
            get { return FormatHelper.FormatWebSiteUrl (Division.WebSite); }
        }

        public string DisplayWebSiteLabel {
            get { return FormatHelper.FormatWebSiteLabel (Division.WebSite, Division.WebSiteLabel); }
        }
    }
}
