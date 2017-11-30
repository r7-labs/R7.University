//
//  EduVolumeDirectoryViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using System.Linq;
using R7.University.ModelExtensions;

namespace R7.University.EduProgramProfiles.ViewModels
{
    public class EduVolumeDirectoryViewModel
    {
        public bool IsEditable;

        public DateTime Now;

        public EduVolumeDirectoryViewModel WithFilter (bool isEditable, DateTime now)
        {
            IsEditable = isEditable;
            Now = now;
            return this;
        }

        public bool IsEmpty => EduVolumeViewModels.IsNullOrEmpty ();

        IEnumerable<EduVolumeViewModel> _eduVolumeViewModels;
        public IEnumerable<EduVolumeViewModel> EduVolumeViewModels {
            // TODO: Must be ev.EduProgramProfileFormYear.IsPublished()
            // TODO: Implement sorting
            get { return _eduVolumeViewModels.Where (ev => IsEditable || ev.EduProgramProfileFormYear.EduProgramProfile.IsPublished (Now)); }
            set { _eduVolumeViewModels = value; }
        }
    }
}
