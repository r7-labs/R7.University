using System;
using System.Collections.Generic;
using R7.University.Models;

namespace R7.University.ViewModels
{
    public abstract class EduProfileViewModelBase: IEduProfile
    {
        public IEduProfile EduProfile { get; protected set; }

        protected EduProfileViewModelBase (IEduProfile model)
        {
            EduProfile = model;
        }

        #region IEduProgramProfile implementation

        public int EduProgramProfileID => EduProfile.EduProgramProfileID;

        public int EduProgramID => EduProfile.EduProgramID;

        public int EduLevelId => EduProfile.EduLevelId;

        public string ProfileCode => EduProfile.ProfileCode;

        public string ProfileTitle => EduProfile.ProfileTitle;

        public string Languages => EduProfile.Languages;

        public bool IsAdopted => EduProfile.IsAdopted;

        public bool ELearning => EduProfile.ELearning;

        public bool DistanceEducation => EduProfile.DistanceEducation;

        public DateTime? AccreditedToDate => EduProfile.AccreditedToDate;

        public DateTime? CommunityAccreditedToDate => EduProfile.CommunityAccreditedToDate;

        public DateTime? StartDate => EduProfile.StartDate;

        public DateTime? EndDate => EduProfile.EndDate;

        public int LastModifiedByUserId => EduProfile.LastModifiedByUserId;

        public DateTime LastModifiedOnDate => EduProfile.LastModifiedOnDate;

        public int CreatedByUserId => EduProfile.CreatedByUserId;

        public DateTime CreatedOnDate => EduProfile.CreatedOnDate;

        public IEduProgram EduProgram => EduProfile.EduProgram;

        public IEduLevel EduLevel => EduProfile.EduLevel;

        public ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears => EduProfile.EduProgramProfileFormYears;

        public ICollection<DocumentInfo> Documents => EduProfile.Documents;

        public ICollection<EduProgramDivisionInfo> Divisions => EduProfile.Divisions;

        #endregion
    }
}

