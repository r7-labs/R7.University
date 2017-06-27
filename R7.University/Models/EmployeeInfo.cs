//
//  EmployeeInfo.cs
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
using DotNetNuke.Common.Utilities;
using R7.Dnn.Extensions.Utilities;

namespace R7.University.Models
{
    public class EmployeeInfo: IEmployee
    {
        public EmployeeInfo ()
        {
            Achievements = new HashSet<EmployeeAchievementInfo> ();
            Disciplines = new HashSet<EmployeeDisciplineInfo> ();
            Positions = new HashSet<OccupiedPositionInfo> ();
        }
        
        #region IEmployee implementation

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

        public int? ExperienceYears { get; set; }

        public int? ExperienceYearsBySpec { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserID { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public bool ShowBarcode { get; set; }

        public virtual ICollection<EmployeeAchievementInfo> Achievements { get; set; }

        public virtual ICollection<EmployeeDisciplineInfo> Disciplines { get; set; }

        public virtual ICollection<OccupiedPositionInfo> Positions { get; set; }

        #endregion

        #region Calculated properties

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

        public string FileName
        {
            get { return GetFileName (FirstName, LastName, OtherName); }
        }

        public string FullName
        {
            get { return TextUtils.FormatList (" ", LastName, FirstName, OtherName); }
        }

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

        #endregion

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
}
	