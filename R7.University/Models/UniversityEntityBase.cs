using System;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University
{
    public abstract class UniversityEntityBase: IAuditable
    {
        public UniversityEntityBase ()
        {
        }

        #region IAuditable implementation

        public int LastModifiedByUserID { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        // REVIEW: Make CreatedOnDate a [ReadOnlyColumn]?

        #endregion
    }
}

