using System;
using System.Collections.Generic;

namespace R7.University.Models
{
    public interface IEduProfile: ITrackableEntity, IPublishableEntity
    {
        int EduProgramProfileID { get; }

        int EduProgramID { get; }

        int EduLevelId { get; }

        string ProfileCode { get; }

        string ProfileTitle { get; }

        string Languages { get; }

        bool IsAdopted { get; }

        bool ELearning { get; }

        bool DistanceEducation { get; }

        DateTime? AccreditedToDate { get; }

        DateTime? CommunityAccreditedToDate { get; }

        IEduProgram EduProgram { get; }

        IEduLevel EduLevel { get; }

        ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears { get; }

        ICollection<DocumentInfo> Documents { get; }

        ICollection<EduProgramDivisionInfo> Divisions { get; }
    }

    public interface IEduProfileWritable: IEduProfile, ITrackableEntityWritable, IPublishableEntityWritable
    {
        new int EduProgramProfileID { get; set; }

        new int EduProgramID { get; set; }

        new int EduLevelId { get; set; }

        new string ProfileCode { get; set; }

        new string ProfileTitle { get; set; }

        new string Languages { get; set; }

        new bool IsAdopted { get; set; }

        new bool ELearning { get; set; }

        new bool DistanceEducation { get; set; }

        new DateTime? AccreditedToDate { get; set; }

        new DateTime? CommunityAccreditedToDate { get; set; }

        new IEduProgram EduProgram { get; set; }

        new IEduLevel EduLevel { get; set; }

        new ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears { get; set; }

        new ICollection<DocumentInfo> Documents { get; set; }

        new ICollection<EduProgramDivisionInfo> Divisions { get; set; }
    }

    public class EduProfileInfo: IEduProfileWritable
    {
        public int EduProgramProfileID { get; set; }

        public int EduProgramID { get; set; }

        public int EduLevelId { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public string Languages { get; set; }

        public bool IsAdopted { get; set; }

        public bool ELearning { get; set; }

        public bool DistanceEducation { get; set; }

        public DateTime? AccreditedToDate { get; set; }

        public DateTime? CommunityAccreditedToDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserId { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual EduProgramInfo EduProgram { get; set; }

        IEduProgram IEduProfile.EduProgram => EduProgram;

        IEduProgram IEduProfileWritable.EduProgram {
            get { return EduProgram; }
            set { EduProgram = (EduProgramInfo) value; }
        }

        public virtual EduLevelInfo EduLevel { get; set; }

        IEduLevel IEduProfile.EduLevel => EduLevel;

        IEduLevel IEduProfileWritable.EduLevel {
            get { return EduLevel; }
            set { EduLevel = (EduLevelInfo) value; }
        }

        public virtual ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears { get; set; } = new HashSet<EduProgramProfileFormYearInfo> ();

        public virtual ICollection<DocumentInfo> Documents { get; set; } = new HashSet<DocumentInfo> ();

        public virtual ICollection<EduProgramDivisionInfo> Divisions { get; set; } = new HashSet<EduProgramDivisionInfo> ();
    }
}
