//
//  EduProgramProfileViewModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using R7.University.Models;

namespace R7.University.ViewModels
{
    public abstract class EduProfileViewModelBase: IEduProfile
    {
        public IEduProfile EduProgramProfile { get; protected set; }

        protected EduProfileViewModelBase (IEduProfile model)
        {
            EduProgramProfile = model;
        }

        #region IEduProgramProfile implementation

        public int EduProgramProfileID => EduProgramProfile.EduProgramProfileID;

        public int EduProgramID => EduProgramProfile.EduProgramID;

        public int EduLevelId => EduProgramProfile.EduLevelId;

        public string ProfileCode => EduProgramProfile.ProfileCode;

        public string ProfileTitle => EduProgramProfile.ProfileTitle;

        public string Languages => EduProgramProfile.Languages;

        public bool IsAdopted => EduProgramProfile.IsAdopted;

        public bool ELearning => EduProgramProfile.ELearning;

        public bool DistanceEducation => EduProgramProfile.DistanceEducation;

        public DateTime? AccreditedToDate => EduProgramProfile.AccreditedToDate;

        public DateTime? CommunityAccreditedToDate => EduProgramProfile.CommunityAccreditedToDate;

        public DateTime? StartDate => EduProgramProfile.StartDate;

        public DateTime? EndDate => EduProgramProfile.EndDate;

        public int LastModifiedByUserId => EduProgramProfile.LastModifiedByUserId;

        public DateTime LastModifiedOnDate => EduProgramProfile.LastModifiedOnDate;

        public int CreatedByUserId => EduProgramProfile.CreatedByUserId;

        public DateTime CreatedOnDate => EduProgramProfile.CreatedOnDate;

        public IEduProgram EduProgram => EduProgramProfile.EduProgram;

        public IEduLevel EduLevel => EduProgramProfile.EduLevel;

        public ICollection<EduProgramProfileFormYearInfo> EduProgramProfileFormYears => EduProgramProfile.EduProgramProfileFormYears;

        public ICollection<DocumentInfo> Documents => EduProgramProfile.Documents;

        public ICollection<EduProgramDivisionInfo> Divisions => EduProgramProfile.Divisions;

        #endregion
    }
}

