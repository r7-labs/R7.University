//
//  UniversityModelContext.cs
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
using R7.University.Components;
using R7.University.Data;

namespace R7.University.Models
{
    public class UniversityModelContext: ModelContextBase
    {
        public UniversityModelContext (IDataContext dataContext): base (dataContext)
        {
        }

        public UniversityModelContext ()
        {
        }

        #region ModelContextBase implementation

        public override IDataContext CreateDataContext ()
        {
            return UniversityDataContextFactory.Instance.Create ();
        }

        public override bool SaveChanges (bool dispose = true)
        {
            var result = base.SaveChanges (dispose);

            // drop cache on final call
            if (dispose) {
                CacheHelper.RemoveCacheByPrefix ("//r7_University");
            }

            return result;
        }

        #endregion
    }
}
