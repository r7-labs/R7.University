using System;

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

		#endregion
	}
}

