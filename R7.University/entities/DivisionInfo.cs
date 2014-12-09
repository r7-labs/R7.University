using System;
using System.Text.RegularExpressions;
using DotNetNuke.Data;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;

namespace R7.University
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
	public class DivisionInfo : EntityBase, IReferenceEntity
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public DivisionInfo ()
		{
		}

        /// <summary>
        /// Empty division to use as default item with lists and treeviews
        /// </summary>
        /// <param name="title">Title.</param>
        public static DivisionInfo DefaultItem (string title = "")
        {
            return new DivisionInfo { 
                Title = title,
                DivisionID = Null.NullInteger,
                ParentDivisionID = null
            };
        }

		#region IReferenceEntity implementation

		public string Title { get; set; }

		public string ShortTitle { get; set; }

        [IgnoreColumn]
        public string DisplayShortTitle
        {
            get { return FormatShortTitle (Title, ShortTitle); }
        }

        public static string FormatShortTitle (string title, string shortTitle)
        {
            return !string.IsNullOrWhiteSpace (shortTitle)? shortTitle : title;
        }

		#endregion

		#region Properties

		public int DivisionID { get; set; }

		public int? ParentDivisionID  { get; set; }

		public int? DivisionTermID  { get; set; }

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

		#endregion

		[IgnoreColumn]
		public string FileName
		{
			// replace all non-word character with spaces, 
			// trim resulting string and then replace all spaces with single underscore
			get { return Regex.Replace (Regex.Replace (ShortTitle, @"\W", " ").Trim (), @"\s+", "_"); } 
		}

		[IgnoreColumn]
		public string FormatWebSiteLabel 
		{
			get
			{
				return (!string.IsNullOrWhiteSpace (WebSiteLabel)) ? WebSiteLabel : 
					WebSite.Contains("://") ? WebSite.Remove (0, WebSite.IndexOf ("://") + 3) : WebSite;
			}
		}

		[IgnoreColumn]
		public string FormatWebSiteUrl
		{
			get
			{
				return WebSite.Contains ("://") ? WebSite.ToLowerInvariant () : 
					"http://" + WebSite.ToLowerInvariant ();
			}
		}

		[IgnoreColumn]
		public VCard VCard
		{
			get
			{
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
			get
			{ 
				return !string.IsNullOrEmpty (ShortTitle) &&
				!string.IsNullOrEmpty (Title) &&
				ShortTitle.Length < Title.Length &&
				!Title.StartsWith (ShortTitle);
			}
		}


		[IgnoreColumn]
		public string SearchDocumentText
		{
			get
			{
				var text = Utils.FormatList (", ",
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

