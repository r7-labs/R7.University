//
//  ContingentDirectoryViewModel.cs
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
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.EduProgramProfiles.ViewModels
{
    public class ContingentDirectoryViewModel
    {
        public bool IsEditable;

        public DateTime Now;

        public ContingentDirectoryViewModel WithFilter (bool isEditable, DateTime now)
        {
            IsEditable = isEditable;
            Now = now;
            return this;
        }

        public bool IsEmpty => ContingentViewModels.IsNullOrEmpty ();

        public bool IsConfigured => Settings.Mode != null;

        public ContingentDirectorySettings Settings;

        public IYear LastYear;

        IEnumerable<ContingentViewModel> _contingentViewModels;
        public IEnumerable<ContingentViewModel> ContingentViewModels {
            get { return _contingentViewModels.Where (ev => IsEditable || ev.IsPublished (Now)); }
            set { _contingentViewModels = value; }
        }

        public string ItemProp {
            get {
                switch (Settings.Mode) {
                    case ContingentDirectoryMode.Actual: return "eduChislen";
                    case ContingentDirectoryMode.Admission: return "eduPriem";
                    case ContingentDirectoryMode.Movement: return "eduPerevod";
                    case ContingentDirectoryMode.Vacant: return "vacant";
                }

                return string.Empty;
            }
        }
    }
}
