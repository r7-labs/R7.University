//
// VCard.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using System.Text;
using System.Collections.Generic;

namespace R7.University
{
	public class VCard
	{
		public VCard ()
		{
			Names = new List<string> ();
			Emails = new List<string> ();
			Phones = new List<Phone> ();
		}

		#region Example

		/*
		BEGIN:VCARD
		VERSION:3.0
		TEL:+7 (927) 530-87-50
		EMAIL:support.vgsha@gmail.com
		ORG:Volgograd SAU
		END:VCARD
		*/ 

		#endregion

		#region Properties

		public const string Version = "3.0";

		// TODO: Must provide unique product id
		public const string ProductID = "-//ONLINE DIRECTORY//NONSGML Version 1//EN";

		public string FormattedName { get; set; }

		public List<string> Names { get; set; }

		public string Nickname { get; set; }

		public string Photo { get; set; }

		public DateTime BirthDay { get; set; }

		public string DeliveryAddres { get; set; }

		public string AddressLabel { get; set; }

		public List<Phone> Phones { get; set; }

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

		public override string ToString ()
		{
			var vcard = new StringBuilder ();
			vcard.AppendLine ("BEGIN:VCARD");

			// version
			vcard.AppendLine ("VERSION:" + Version);

			// formatted name
			if (!string.IsNullOrWhiteSpace (FormattedName))
				vcard.AppendLine ("FN:" + FormattedName);

			// names
			// NOTE: Last element must contain additional names, comma separated
			if (Names.Count > 0)
				vcard.AppendLine ("N:" + Utils.FormatList (";", Names.ToArray()));

			// phone
			foreach (var phone in Phones)
			{
				if (phone.Type == PhoneType.None)
					vcard.AppendLine ("TEL:" + phone.Number);
				else
					vcard.AppendLine (
						string.Format ("TEL;TYPE={0}:{1}", 
							GetPhoneTypeString (phone.Type).ToUpperInvariant (), phone.Number));
			}

			// emails
			var firstEmail = true;
			foreach (var email in Emails)
			{		if (firstEmail)
				{	
					vcard.AppendLine ("EMAIL;TYPE=PREF:" + email);
					firstEmail = false;
				}
				else
					vcard.AppendLine ("EMAIL:" + email);
			}

			// url
			if (!string.IsNullOrWhiteSpace(Url))
				vcard.AppendLine ("URL:" + Url);

			// title
			if (!string.IsNullOrWhiteSpace(Title))
				vcard.AppendLine ("TITLE:" + Title);

			vcard.AppendLine ("END:VCARD");

			return vcard.ToString ();
		}

		private string GetPhoneTypeString (PhoneType type)
		{
			var types = new List<string>();

			if ((type & PhoneType.Home) > 0)
				types.Add(PhoneType.Home.ToString());

			if ((type & PhoneType.Msg) > 0)
				types.Add(PhoneType.Msg.ToString());

			if ((type & PhoneType.Work) > 0)
				types.Add(PhoneType.Work.ToString());

			if ((type & PhoneType.Pref) > 0)
				types.Add(PhoneType.Pref.ToString());

			if ((type & PhoneType.Voice) > 0)
				types.Add(PhoneType.Voice.ToString());

			if ((type & PhoneType.Fax) > 0)
				types.Add(PhoneType.Fax.ToString());

			if ((type & PhoneType.Cell) > 0)
				types.Add(PhoneType.Cell.ToString());

			if ((type & PhoneType.Video) > 0)
				types.Add(PhoneType.Video.ToString());

			if ((type & PhoneType.Pager) > 0)
				types.Add(PhoneType.Pager.ToString());

			if ((type & PhoneType.Bbs) > 0)
				types.Add(PhoneType.Bbs.ToString());

			if ((type & PhoneType.Modem) > 0)
				types.Add(PhoneType.Modem.ToString());

			if ((type & PhoneType.Car) > 0)
				types.Add(PhoneType.Car.ToString());

			if ((type & PhoneType.Isdn) > 0)
				types.Add(PhoneType.Isdn.ToString());

			if ((type & PhoneType.Pcs) > 0)
				types.Add(PhoneType.Pcs.ToString());

			return Utils.FormatList (",", types.ToArray ());
		}
	}
}
