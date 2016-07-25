//
//  EduProgramViewModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using R7.University.Models;
using System.Collections.Generic;

namespace R7.University.ViewModels
{
    public abstract class EduProgramViewModelBase: IEduProgram
    {
        public IEduProgram Model { get; protected set; }

        protected EduProgramViewModelBase (IEduProgram model)
        {
            Model = model;
        }

        #region IEduProgram implementation

        public int EduProgramID
        {
            get { return Model.EduProgramID; }
            set { throw new InvalidOperationException (); }
        }

        public int EduLevelID
        {
            get { return Model.EduLevelID; }
            set { throw new InvalidOperationException (); }
        }

        public int? DivisionId
        {
            get { return Model.DivisionId; }
            set { throw new InvalidOperationException (); }
        }

        public string Code
        {
            get { return Model.Code; }
            set { throw new InvalidOperationException (); }
        }

        public string Title
        {
            get { return Model.Title; }
            set { throw new InvalidOperationException (); }
        }

        public string Generation
        {
            get { return Model.Generation; }
            set { throw new InvalidOperationException (); }
        }

        public string HomePage
        {
            get { return Model.HomePage; }
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

        public ICollection<DocumentInfo> Documents
        {
            get { return Model.Documents; }
            set { throw new InvalidOperationException (); }
        }

        public ICollection<EduProgramProfileInfo> EduProgramProfiles
        {
            get { return Model.EduProgramProfiles; }
            set { throw new InvalidOperationException (); }
        }

        #endregion
    }
}
