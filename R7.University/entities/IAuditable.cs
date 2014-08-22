using System;

namespace R7.University
{
	public interface IAuditable
	{
		int LastModifiedByUserID { get; set; }

		DateTime LastModifiedOnDate { get; set; }

		int CreatedByUserID { get; set; }

		DateTime CreatedOnDate { get; set; }
	}
}
