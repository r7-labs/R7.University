//
//  DivisionInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;
using R7.University.ViewModels;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace R7.University.Data
{
    // More attributes for class:
    // Set caching for table: [Cacheable("R7.University_Divisions", CacheItemPriority.Default, 20)]
    // Explicit mapping declaration: [DeclareColumns]
    // More attributes for class properties:
    // Custom column name: [ColumnName("DivisionID")]
    // Explicit include column: [IncludeColumn]
    // Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
    [TableName ("University_Divisions")]
    [PrimaryKey ("DivisionID", AutoIncrement = true)]
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

        #endregion

        [IgnoreColumn]
        public bool IsPublished
        {
            get {
                var now = DateTime.Now;
                return (StartDate == null || now >= StartDate) && (EndDate == null || now < EndDate);
            }
        }

        [IgnoreColumn]
        public string FileName
        {
			// replace all non-word character with spaces, 
			// trim resulting string and then replace all spaces with single underscore
            get { 
                return Regex.Replace (Regex.Replace (
                    FormatHelper.FormatShortTitle (ShortTitle, Title), @"\W", " ").Trim (), @"\s+", "_"); 
            } 
        }

        [IgnoreColumn]
        public string FormatWebSiteLabel
        {
            get {
                return (!string.IsNullOrWhiteSpace (WebSiteLabel)) ? WebSiteLabel : 
					WebSite.Contains ("://") ? WebSite.Remove (0, WebSite.IndexOf ("://") + 3) : WebSite;
            }
        }

        [IgnoreColumn]
        public string FormatWebSiteUrl
        {
            get {
                return WebSite.Contains ("://") ? WebSite.ToLowerInvariant () : 
					"http://" + WebSite.ToLowerInvariant ();
            }
        }

        [IgnoreColumn]
        public string FormatEmailUrl
        {
            get { return "mailto:" + Email; }
        }

        [IgnoreColumn]
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

        [IgnoreColumn]
        public bool HasUniqueShortTitle
        {
            get { 
                return !string.IsNullOrEmpty (ShortTitle) &&
                !string.IsNullOrEmpty (Title) &&
                ShortTitle.Length < Title.Length &&
                !Title.StartsWith (ShortTitle);
            }
        }


        [IgnoreColumn]
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

    public class DivisionMapping: EntityTypeConfiguration<DivisionInfo>
    {
        public DivisionMapping ()
        {
            HasKey (m => m.DivisionID);
            Property (m => m.DivisionID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);

            Property (m => m.ParentDivisionID).IsOptional ();
            Property (m => m.DivisionTermID).IsOptional ();
            Property (m => m.HeadPositionID).IsOptional ();

            Property (m => m.Title).IsRequired ();
            Property (m => m.ShortTitle);

            Property (m => m.HomePage);
            Property (m => m.WebSite);
            Property (m => m.WebSiteLabel);

            Property (m => m.Phone);
            Property (m => m.Fax);
            Property (m => m.Email);
            Property (m => m.SecondaryEmail);
            Property (m => m.Location);
            Property (m => m.WorkingHours);
            Property (m => m.DocumentUrl);
            Property (m => m.IsVirtual);

            Property (m => m.StartDate).IsOptional ();
            Property (m => m.EndDate).IsOptional ();

            Property (m => m.LastModifiedByUserID);
            Property (m => m.LastModifiedOnDate);
            Property (m => m.CreatedByUserID);
            Property (m => m.CreatedOnDate);
        }
    }
}

