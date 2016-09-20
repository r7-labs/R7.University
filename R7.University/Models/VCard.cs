//
//  VCard.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014 Roman M. Yagodin
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
using System.Text;
using System.Collections.Generic;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Models
{
    public class VCard
    {
        // TODO: Allow import vcf in Window Contacts correctly

        public VCard ()
        {
            Names = new List<string> ();
            Emails = new List<string> ();
            Phones = new List<VCardPhone> ();
            LastRevision = DateTime.MinValue;
            Encoding = Encoding.UTF8;
        }

        #region Properties

        public const string Version = "3.0";

        // TODO: Must provide unique product id
        public const string ProductID = "-//ONLINE DIRECTORY//NONSGML Version 1//EN";

        public string FormattedName { get; set; }

        public List<string> Names { get; set; }

        public string Nickname { get; set; }

        public string Photo { get; set; }

        public DateTime BirthDay { get; set; }

        public string DeliveryAddress { get; set; }

        public string AddressLabel { get; set; }

        public List<VCardPhone> Phones { get; set; }

        public List<string> Emails { get; set; }

        public string EmailProgram  { get; set; }

        public TimeZoneInfo TimeZone { get; set; }

        public string GlobalPositioning { get; set; }

        public string Title { get; set; }

        public string RoleOrOccupation { get; set; }

        public string Logo  { get; set; }

        public VCard Agent  { get; set; }

        public string OrganizationName { get; set; }

        public List<string> Categories { get; set; }

        public string Note { get; set; }

        public DateTime LastRevision { get; set; }

        public string SortString { get; set; }

        public string Sound { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public string Url { get; set; }

        public string AccessClassification { get; set; }

        public string PublicKey { get; set; }

        #endregion

        public Encoding Encoding { get; set; }

        public override string ToString ()
        {
            var vcard = new StringBuilder ();
		
            var charset = "";
            if (Encoding != Encoding.UTF8)
                charset = ";CHARSET=" + Encoding.WebName;

            vcard.AppendLine ("BEGIN:VCARD");

            // version
            vcard.AppendLine ("VERSION:" + Version);

            // formatted name
            if (!string.IsNullOrWhiteSpace (FormattedName))
                vcard.AppendFormat ("FN{0}:{1}\n", charset, FormattedName);

            // names
            // last element must contain additional names, comma separated
            if (Names.Count > 0)
                vcard.AppendFormat ("N{0}:{1}\n", charset, TextUtils.FormatList (";", Names.ToArray ()));

            // organization
            if (!string.IsNullOrWhiteSpace (OrganizationName))
                vcard.AppendFormat ("ORG{0}:{1}\n", charset, OrganizationName);

            // phone
            foreach (var phone in Phones) {
                if (phone.Type == VCardPhoneType.None)
                    vcard.AppendLine ("TEL:" + phone.Number);
                else
                    vcard.AppendLine (
                        string.Format ("TEL;TYPE={0}:{1}", 
                            GetPhoneTypeString (phone.Type).ToUpperInvariant (), phone.Number));
            }

            // emails
            var firstEmail = true;
            foreach (var email in Emails) {
                if (firstEmail) {	
                    vcard.AppendLine ("EMAIL;TYPE=PREF:" + email);
                    firstEmail = false;
                }
                else
                    vcard.AppendLine ("EMAIL:" + email);
            }

            // url
            if (!string.IsNullOrWhiteSpace (Url))
                vcard.AppendLine ("URL:" + Url);

            // title
            if (!string.IsNullOrWhiteSpace (Title))
                vcard.AppendFormat ("TITLE{0}:{1}\n", charset, Title);

            // address
            if (!string.IsNullOrWhiteSpace (DeliveryAddress))
                vcard.AppendFormat ("ADR{0}:{1}\n", charset, DeliveryAddress);

            // revision
            if (LastRevision != DateTime.MinValue)
                vcard.AppendLine ("REV:" + LastRevision.ToString ("yyyy-MM-dd"));

            // no need to write endline, so using Append()
            vcard.Append ("END:VCARD");

            return vcard.ToString ();
        }

        private string GetPhoneTypeString (VCardPhoneType type)
        {
            var types = new List<string> ();

            if ((type & VCardPhoneType.Home) > 0)
                types.Add (VCardPhoneType.Home.ToString ());

            if ((type & VCardPhoneType.Msg) > 0)
                types.Add (VCardPhoneType.Msg.ToString ());

            if ((type & VCardPhoneType.Work) > 0)
                types.Add (VCardPhoneType.Work.ToString ());

            if ((type & VCardPhoneType.Pref) > 0)
                types.Add (VCardPhoneType.Pref.ToString ());

            if ((type & VCardPhoneType.Voice) > 0)
                types.Add (VCardPhoneType.Voice.ToString ());

            if ((type & VCardPhoneType.Fax) > 0)
                types.Add (VCardPhoneType.Fax.ToString ());

            if ((type & VCardPhoneType.Cell) > 0)
                types.Add (VCardPhoneType.Cell.ToString ());

            if ((type & VCardPhoneType.Video) > 0)
                types.Add (VCardPhoneType.Video.ToString ());

            if ((type & VCardPhoneType.Pager) > 0)
                types.Add (VCardPhoneType.Pager.ToString ());

            if ((type & VCardPhoneType.Bbs) > 0)
                types.Add (VCardPhoneType.Bbs.ToString ());

            if ((type & VCardPhoneType.Modem) > 0)
                types.Add (VCardPhoneType.Modem.ToString ());

            if ((type & VCardPhoneType.Car) > 0)
                types.Add (VCardPhoneType.Car.ToString ());

            if ((type & VCardPhoneType.Isdn) > 0)
                types.Add (VCardPhoneType.Isdn.ToString ());

            if ((type & VCardPhoneType.Pcs) > 0)
                types.Add (VCardPhoneType.Pcs.ToString ());

            return TextUtils.FormatList (",", types.ToArray ());
        }
    }
}
