using System;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University
{
	public abstract class ReferenceEntityBase : IReferenceEntity
	{
		public ReferenceEntityBase ()
		{
		}

		#region IReferenceEntity implementation

		public string Title { get; set; }

		public string ShortTitle  { get; set; }

        [IgnoreColumn]
        public string DisplayShortTitle
        {
            get { return !string.IsNullOrWhiteSpace (ShortTitle)? ShortTitle : Title; } 
        }

		#endregion
	}
}

