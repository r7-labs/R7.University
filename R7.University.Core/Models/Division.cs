using System;
using System.Collections.Generic;

namespace R7.University.Models
{
    public interface IDivision: ITrackableEntity, IPublishableEntity
    {
        string Title { get; }

        string ShortTitle { get; }

        int DivisionID { get; }

        int? ParentDivisionID  { get; }

        int? DivisionTermID  { get; }

        string HomePage { get; }

        string WebSite { get; }

        string WebSiteLabel { get; }

        string Phone { get; }

        string Fax { get; }

        string Email { get; }

        string SecondaryEmail { get; }

        string Address { get; }

        string Location { get; }

        string WorkingHours { get; }

        string DocumentUrl { get; }

        bool IsSingleEntity { get; }

        bool IsInformal { get; }

        bool IsGoverning { get; }

        int? HeadPositionID { get; }

        ICollection<DivisionInfo> SubDivisions { get; }

        ICollection<OccupiedPositionInfo> OccupiedPositions { get; }

        int Level { get; }

        string Path { get; }
    }

    public interface IDivisionWritable: IDivision, ITrackableEntityWritable, IPublishableEntityWritable
    {
        new string Title { get; set; }

        new string ShortTitle { get; set; }

        new int DivisionID { get; set; }

        new int? ParentDivisionID  { get; set; }

        new int? DivisionTermID  { get; set; }

        new string HomePage { get; set; }

        new string WebSite { get; set; }

        new string WebSiteLabel { get; set; }

        new string Phone { get; set; }

        new string Fax { get; set; }

        new string Email { get; set; }

        new string SecondaryEmail { get; set; }

        new string Address { get; set; }

        new string Location { get; set; }

        new string WorkingHours { get; set; }

        new string DocumentUrl { get; set; }

        new bool IsSingleEntity { get; set; }

        new bool IsInformal { get; set; }

        new bool IsGoverning { get; set; }

        new int? HeadPositionID { get; set; }

        new ICollection<DivisionInfo> SubDivisions { get; set; }

        new ICollection<OccupiedPositionInfo> OccupiedPositions { get; set; }

        new int Level { get; set; }

        new string Path { get; set; }
    }

    public class DivisionInfo: IDivisionWritable
    {
        /// <summary>
        /// Empty division to use as default item with lists and treeviews
        /// </summary>
        /// <param name="title">Title.</param>
        public static DivisionInfo DefaultItem (string title = "")
        {
            return new DivisionInfo
            {
                Title = title,
                DivisionID = -1,
                ParentDivisionID = null
            };
        }

        #region IDivision implementation

        public int DivisionID { get; set; }

        public int? ParentDivisionID  { get; set; }

        public int? DivisionTermID  { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string HomePage { get; set; }

        public string WebSite { get; set; }

        public string WebSiteLabel { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string SecondaryEmail { get; set; }

        public string Address { get; set; }

        public string Location { get; set; }

        public string WorkingHours { get; set; }

        public string DocumentUrl { get; set; }

        public bool IsSingleEntity { get; set; }

        public bool IsInformal { get; set; }

        public bool IsGoverning { get; set; }

        public int? HeadPositionID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserId { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual ICollection<DivisionInfo> SubDivisions { get; set; } = new HashSet<DivisionInfo> ();

        public virtual ICollection<OccupiedPositionInfo> OccupiedPositions { get; set; } = new HashSet<OccupiedPositionInfo> ();

        public int Level { get; set; }

        public string Path { get; set; }

        #endregion
    }
}
