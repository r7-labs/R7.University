//
//  UniversityDbContextFactory.cs
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
using System.Linq;

namespace R7.University.Data
{
    public abstract class UniversityDbContextFactoryBase
    {
        public abstract IUniversityDbContext Create ();
    }

    public class UniversityDbContextFactory: UniversityDbContextFactoryBase
    {
        #region Singleton implementation

        static readonly Lazy<UniversityDbContextFactory> instance = new Lazy<UniversityDbContextFactory> ();

        public static UniversityDbContextFactory Instance
        {
            get { return instance.Value; }
        }

        #endregion

        public override IUniversityDbContext Create ()
        {
            // return IUniversityDbContext implementation
            return new UniversityDbContext ();
        }

        public void Example ()
        {
            using (var db = UniversityDbContextFactory.Instance.Create ()) {
                db.Set<EmployeeInfo> ().Where (e => e.EmployeeID == 1);
                var x = db.SaveChanges () > 0;
            }
        }
    }
}
