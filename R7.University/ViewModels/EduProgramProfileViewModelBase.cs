//
//  EduProgramProfileViewModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
    public abstract class EduProgramProfileViewModelBase: IEduProgramProfile
    {
        public IEduProgramProfile Model { get; protected set; }

        protected EduProgramProfileViewModelBase (IEduProgramProfile model)
        {
            Model = model;
        }

        #region IEduProgramProfile implementation

        public int EduProgramProfileID
        {
            get { return Model.EduProgramProfileID; }
            set { throw new InvalidOperationException (); }
        }

        public int EduProgramID
        {
            get { return Model.EduProgramID; }
            set { throw new InvalidOperationException (); }
        }

        public int EduLevelId
        {
            get { return Model.EduLevelId; }
            set { throw new InvalidOperationException (); }
        }

        public int? DivisionId
        {
            get { return Model.EduLevelId; }
            set { throw new InvalidOperationException (); }
        }

        public string ProfileCode
        {
            get { return Model.ProfileCode; }
            set { throw new InvalidOperationException (); }
        }

        public string ProfileTitle
        {
            get { return Model.ProfileTitle; }
            set { throw new InvalidOperationException (); }
        }

        public string Languages
        {
            get { return Model.Languages; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime? AccreditedToDate
        {
            get { return Model.AccreditedToDate; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime? CommunityAccreditedToDate
        {
            get { return Model.CommunityAccreditedToDate; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime? StartDate
        {
            get { return Model.StartDate; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime? EndDate
        {
            get { return Model.EndDate; }
            set { throw new InvalidOperationException (); }
        }

        public int LastModifiedByUserID
        {
            get { return Model.LastModifiedByUserID; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime LastModifiedOnDate
        {
            get { return Model.LastModifiedOnDate; }
            set { throw new InvalidOperationException (); }
        }

        public int CreatedByUserID
        {
            get { return Model.CreatedByUserID; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime CreatedOnDate
        {
            get { return Model.CreatedOnDate; }
            set { throw new InvalidOperationException (); }
        }

        public EduProgramInfo EduProgram
        {
            get { return Model.EduProgram; }
            set { throw new InvalidOperationException (); }
        }

        public EduLevelInfo EduLevel
        {
            get { return Model.EduLevel; }
            set { throw new InvalidOperationException (); }
        }

        public DivisionInfo Division
        {
            get { return Model.Division; }
            set { throw new InvalidOperationException (); }
        }

        public ICollection<EduProgramProfileFormInfo> EduProgramProfileForms
        {
            get { return Model.EduProgramProfileForms; }
            set { throw new InvalidOperationException (); }
        }

        public ICollection<DocumentInfo> Documents
        {
            get { return Model.Documents; }
            set { throw new InvalidOperationException (); }
        }

        #endregion
    }
}

