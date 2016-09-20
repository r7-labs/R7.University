//
//  DivisionInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using System.Text.RegularExpressions;
using DotNetNuke.Common.Utilities;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.ViewModels;

namespace R7.University.Models
{
    public class DivisionInfo: IDivision
    {
        /// <summary>
        /// Empty division to use as default item with lists and treeviews
        /// </summary>
        /// <param name="title">Title.</param>
        public static DivisionInfo DefaultItem (string title = "")
        {
            return new DivisionInfo
            { 
                Title = title,
                DivisionID = Null.NullInteger,
                ParentDivisionID = null
            };
        }

        #region IDivision implementation

        public int DivisionID { get; set; }

        public int? ParentDivisionID  { get; set; }

        public int? DivisionTermID  { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string HomePage { get; set; }

        public string WebSite { get; set; }

        public string WebSiteLabel { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string SecondaryEmail { get; set; }

        public string Location { get; set; }

        public string WorkingHours { get; set; }

        public string DocumentUrl { get; set; }

        public bool IsVirtual { get; set; }

        public int? HeadPositionID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserID { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual ICollection<DivisionInfo> SubDivisions { get; set; }

        public int Level { get; set; }

        public string Path { get; set; }

        #endregion

        public string FileName
        {
			// replace all non-word character with spaces, 
			// trim resulting string and then replace all spaces with single underscore
            get { 
                return Regex.Replace (Regex.Replace (
                    FormatHelper.FormatShortTitle (ShortTitle, Title), @"\W", " ").Trim (), @"\s+", "_"); 
            } 
        }

        public string FormatWebSiteLabel
        {
            get {
                return (!string.IsNullOrWhiteSpace (WebSiteLabel)) ? WebSiteLabel : 
					WebSite.Contains ("://") ? WebSite.Remove (0, WebSite.IndexOf ("://") + 3) : WebSite;
            }
        }

        public string FormatWebSiteUrl
        {
            get {
                return WebSite.Contains ("://") ? WebSite.ToLowerInvariant () : 
					"http://" + WebSite.ToLowerInvariant ();
            }
        }

        public string FormatEmailUrl
        {
            get { return "mailto:" + Email; }
        }

        public VCard VCard
        {
            get {
                var vcard = new VCard ();

                // org. name
                if (!string.IsNullOrWhiteSpace (Title))
                    vcard.OrganizationName = Title;

                // email
                if (!string.IsNullOrWhiteSpace (Email))
                    vcard.Emails.Add (Email);

                // secondary email
                if (!string.IsNullOrWhiteSpace (SecondaryEmail))
                    vcard.Emails.Add (SecondaryEmail);

                // phone
                if (!string.IsNullOrWhiteSpace (Phone))
                    vcard.Phones.Add (new VCardPhone () { Number = Phone, Type = VCardPhoneType.Work });

                // fax
                if (!string.IsNullOrWhiteSpace (Fax))
                    vcard.Phones.Add (new VCardPhone () { Number = Fax, Type = VCardPhoneType.Fax });

                // website
                if (!string.IsNullOrWhiteSpace (WebSite))
                    vcard.Url = WebSite;

                // location
                if (!string.IsNullOrWhiteSpace (Location))
				// TODO: Add organization address
				vcard.DeliveryAddress = Location;

                // revision
                vcard.LastRevision = LastModifiedOnDate;

                return vcard;
            }
        }

        public bool HasUniqueShortTitle
        {
            get { 
                return !string.IsNullOrEmpty (ShortTitle) &&
                !string.IsNullOrEmpty (Title) &&
                ShortTitle.Length < Title.Length &&
                !Title.StartsWith (ShortTitle);
            }
        }

        public string SearchDocumentText
        {
            get {
                var text = TextUtils.FormatList (", ",
                    Title,
                    HasUniqueShortTitle ? ShortTitle : null,
                    Phone,
                    Fax,
                    Email,
                    SecondaryEmail,
                    WebSite,
                    Location,
                    WorkingHours
                );

                return text;
            }
        }
    }
}
