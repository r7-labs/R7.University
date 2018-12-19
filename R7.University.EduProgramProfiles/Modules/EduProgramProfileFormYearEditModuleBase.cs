//
//  EduProgramProfileFormYearEditModuleBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Utilities;
using R7.University.EduProgramProfiles.Queries;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;

namespace R7.University.EduProgramProfiles.Modules
{
    public abstract class EduProgramProfileFormYearEditModuleBase<T> : UniversityEditPortalModuleBase<T> where T : class, new()
    {
        protected EduProgramProfileFormYearEditModuleBase (string key) : base (key)
        {
        }

        protected int? GetEduProgramProfileFormYearId () =>
            ParseHelper.ParseToNullable<int> (Request.QueryString [Key])
                     ?? ParseHelper.ParseToNullable<int> (Request.QueryString ["eduprogramprofileformyear_id"]);

        protected IEduProgramProfileFormYear GetEduProgramProfileFormYear ()
        {
            var eppfyId = GetEduProgramProfileFormYearId ();
            return eppfyId != null ? new EduProgramProfileFormYearEditQuery (ModelContext).SingleOrDefault (eppfyId.Value) : null;
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            var eppfy = GetEduProgramProfileFormYear ();
            var lastYear = new FlatQuery<YearInfo> (ModelContext).List ().LastYear ();
            if (eppfy != null) {
                ((CDefault) Page).Title = ((CDefault) Page).Title.Append (eppfy.FormatTitle (lastYear, LocalResourceFile), " &gt; ");
            }
        }
    }
}
