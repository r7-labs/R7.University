//
// EmployeeInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;

namespace R7.University.Data
{
    // More attributes for class:
    // Set caching for table: [Cacheable("University_Employees", CacheItemPriority.Default, 20)]
    // Explicit mapping declaration: [DeclareColumns]
    // More attributes for class properties:
    // Custom column name: [ColumnName("EmployeeID")]
    // Explicit include column: [IncludeColumn]
    // Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
    [TableName ("University_Employees")]
    [PrimaryKey ("EmployeeID", AutoIncrement = true)]
    public class EmployeeInfo : UniversityBaseEntityInfo
    {
        #region Properties

        public int EmployeeID { get; set; }

        public int? UserID { get; set; }

        public int? PhotoFileID { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Fax { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string OtherName { get; set; }

        public string Email { get; set; }

        public string SecondaryEmail { get; set; }

        public string WebSite { get; set; }

        public string WebSiteLabel { get; set; }

        public string Messenger { get; set; }

        public string WorkingPlace { get; set; }

        public string WorkingHours { get; set; }

        public string Biography { get; set; }

        // employee stage may be not continuous, so using starting date is not possible
        public int? ExperienceYears { get; set; }

        // employee ExpYearsBySpec even more unbinded to dates
        public int? ExperienceYearsBySpec { get; set; }

        public bool IsPublished { get; set; }

        #endregion

        #region Calculated properties

        [IgnoreColumn]
        public string AbbrName
        {
            get {
                if (!string.IsNullOrWhiteSpace (OtherName))
                    return string.Format ("{0} {1}.{2}.", LastName, FirstName.Substring (0, 1), OtherName.Substring (0, 1)); 

                return string.Format ("{0} {1}.", LastName, FirstName.Substring (0, 1));
            }
        }

        public static string GetFileName (string firstName, string lastName, string otherName)
        {
            if (!string.IsNullOrWhiteSpace (otherName))
                return string.Format ("{0}_{1}{2}", lastName, firstName.Substring (0, 1), otherName.Substring (0, 1)); 

            return string.Format ("{0}_{1}", lastName, firstName.Substring (0, 1)); 
        }

        [IgnoreColumn]
        public string FileName
        {
            get { return GetFileName (FirstName, LastName, OtherName); }
        }

        [IgnoreColumn]
        public string FullName
        {
            get { return TextUtils.FormatList (" ", LastName, FirstName, OtherName); }
        }

        [IgnoreColumn]
        public string SearchDocumentText
        {
            get {
                var text = TextUtils.FormatList (", ",
                    FullName,
                    Phone,
                    CellPhone,
                    Fax,
                    Email,
                    SecondaryEmail,
                    WebSite,
                    Messenger,
                    WorkingPlace,
                    WorkingHours,
                    HtmlUtils.ConvertToText (Biography)
                );

                // TODO: Add positions and achievements to the search index

                return text;
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

        #endregion

        [IgnoreColumn]
        public VCard VCard
        {
            get {
                var vcard = new VCard ();

                // names
                vcard.Names = new List<string> ()
                {
                    LastName,
                    FirstName,
                    OtherName
                    // TODO: Add title achievements here
                };

                // formatted name
                // TODO: Add title achievements here
                vcard.FormattedName = FullName;

                // email
                if (!string.IsNullOrWhiteSpace (Email))
                    vcard.Emails.Add (Email);

                // secondary email
                if (!string.IsNullOrWhiteSpace (SecondaryEmail))
                    vcard.Emails.Add (SecondaryEmail);
		
                // phone
                if (!string.IsNullOrWhiteSpace (Phone))
                    vcard.Phones.Add (new VCardPhone () { Number = Phone, Type = VCardPhoneType.Work });

                // cellphone
                if (!string.IsNullOrWhiteSpace (CellPhone))
                    vcard.Phones.Add (new VCardPhone () { Number = CellPhone, Type = VCardPhoneType.Cell });

                // fax
                if (!string.IsNullOrWhiteSpace (Fax))
                    vcard.Phones.Add (new VCardPhone () { Number = Fax, Type = VCardPhoneType.Fax });

                // website
                if (!string.IsNullOrWhiteSpace (WebSite))
                    vcard.Url = WebSite;

                // working place
                if (!string.IsNullOrWhiteSpace (WorkingPlace)) {
                    // TODO: Add division address
                    vcard.DeliveryAddress = WorkingPlace;
                }

                // revision
                vcard.LastRevision = LastModifiedOnDate;

                return vcard;
            }
        }

    }
    // class
}
// namespace
	