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

namespace R7.University
{
	public class VCard
	{
		public VCard ()
		{
		}

		/*
		 * BEGIN:VCARD
VERSION:3.0
TEL:+7 (927) 530-87-50
EMAIL:support.vgsha@gmail.com
ORG:Volgograd SAU
END:VCARD
		 */ 

		public const string Version = "3.0";

		// TODO: Must provide unique product id
		public const string ProductID = "-//ONLINE DIRECTORY//NONSGML Version 1//EN";

		public string FormattedName { get; set; }

		public string [] Names { get; set; }

		public string Nickname { get; set; }

		public string Photo { get; set; }

		public DateTime BirthDay { get; set; }

		public string DeliveryAddres { get; set; }

		public string AddressLabel { get; set; }

		public string Phone { get; set; }

		public string [] Emails { get; set; }

		public string EmailProgram  { get; set; }

		public TimeZoneInfo TimeZone { get; set; }

		public string GlobalPositioning { get; set; }

		public string Title { get; set; }

		public string RoleOrOccupation { get; set; }

		public string Logo  { get; set; }

		public VCard Agent  { get; set; }

		public string OrganizationName { get; set; }

		public string [] Categories { get; set; }

		public string Note { get; set; }

		public DateTime LastRevision { get; set; }

		public string SortString { get; set; }

		public string Sound { get; set; }

		public Guid UniqueIdentifier { get; set; }

		public string Url { get; set; }

		//PUBLIC PRIVATE CONFIDENTIAL
		public string AccessClassification { get; set; }

		public string PublicKey { get; set; }
	}
}

