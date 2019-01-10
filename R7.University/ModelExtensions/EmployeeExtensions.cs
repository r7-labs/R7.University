//
//  EmployeeExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018-2019 Roman M. Yagodin
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

using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using R7.Dnn.Extensions.Text;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class EmployeeExtensions
    {
        public static string GetSearchUrl (this IEmployee employee, ModuleInfo module, PortalSettings portalSettings)
        {
            return Globals.NavigateURL (module.TabID, false, portalSettings, "",
                portalSettings.PortalAlias.CultureCode, "", "mid", module.ModuleID.ToString ());
        }

        // TODO: Allow custom format
        public static string FullName (this IEmployee e)
        {
            return FormatHelper.JoinNotNullOrEmpty (" ", e.LastName, e.FirstName, e.OtherName);
        }

        // TODO: Allow custom format
        public static string AbbrName (this IEmployee e)
        {
            if (!string.IsNullOrWhiteSpace (e.OtherName))
                return string.Format ("{0} {1}.{2}.", e.LastName, e.FirstName.Substring (0, 1), e.OtherName.Substring (0, 1));
            
            return string.Format ("{0} {1}.", e.LastName, e.FirstName.Substring (0, 1));
        }

        public static string SearchText (this IEmployee employee)
        {
            // TODO: Add positions and achievements to the search
            return FormatHelper.JoinNotNullOrEmpty (", ",
                employee.FullName (),
                employee.Phone,
                employee.CellPhone,
                employee.Fax,
                employee.Email,
                employee.SecondaryEmail,
                employee.WebSite,
                employee.Messenger,
                employee.WorkingPlace,
                employee.WorkingHours,
                HtmlUtils.ConvertToText (employee.Biography)
            );
        }

        public static VCard VCard (this IEmployee e)
        {
            var vcard = new VCard ();

            // TODO: Add title achievements here?
            vcard.Names = new List<string> ()
            {
                e.LastName,
                e.FirstName,
                e.OtherName
            };

            // TODO: Add title achievements here?
            vcard.FormattedName = e.FullName ();

            if (!string.IsNullOrWhiteSpace (e.Email))
                vcard.Emails.Add (e.Email);

            if (!string.IsNullOrWhiteSpace (e.SecondaryEmail))
                vcard.Emails.Add (e.SecondaryEmail);

            if (!string.IsNullOrWhiteSpace (e.Phone))
                vcard.Phones.Add (new VCardPhone () { Number = e.Phone, Type = VCardPhoneType.Work });

            if (!string.IsNullOrWhiteSpace (e.CellPhone))
                vcard.Phones.Add (new VCardPhone () { Number = e.CellPhone, Type = VCardPhoneType.Cell });

            if (!string.IsNullOrWhiteSpace (e.Fax))
                vcard.Phones.Add (new VCardPhone () { Number = e.Fax, Type = VCardPhoneType.Fax });

            if (!string.IsNullOrWhiteSpace (e.WebSite))
                vcard.Url = e.WebSite;

            if (!string.IsNullOrWhiteSpace (e.WorkingPlace)) {
                // TODO: Add division address
                vcard.DeliveryAddress = e.WorkingPlace;
            }

            vcard.LastRevision = e.LastModifiedOnDate;

            return vcard;
        }

        public static string GetFileName (string firstName, string lastName, string otherName)
        {
            if (!string.IsNullOrWhiteSpace (otherName))
                return string.Format ("{0}_{1}{2}", lastName, firstName.Substring (0, 1), otherName.Substring (0, 1));

            return string.Format ("{0}_{1}", lastName, firstName.Substring (0, 1));
        }

        public static string FileName (this IEmployee e)
        {
            return GetFileName (e.FirstName, e.LastName, e.OtherName);
        }
    }
}

