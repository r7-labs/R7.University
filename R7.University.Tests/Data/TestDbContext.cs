//
//  TestDbContext.cs
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
using R7.University.Data;
using System.Data.Entity;

namespace R7.University.Tests
{
    public class TestDbContext: IUniversityDbContext
    {
        #region IUniversityDbContext implementation

        public IDbSet<TEntity> Set<TEntity> () where TEntity : class, new()
        {
            throw new NotImplementedException ();
        }

        public int SaveChanges ()
        {
            return 0;
        }

        public IDbSet<EmployeeInfo> Employees
        {
            get { throw new NotImplementedException (); }
            set { throw new NotImplementedException (); }
        }

        public IDbSet<DivisionInfo> Divisions
        {
            get { throw new NotImplementedException (); }
            set { throw new NotImplementedException (); }
        }

        public IDbSet<OccupiedPositionInfo> OccupiedPositions
        {
            get { throw new NotImplementedException (); }
            set { throw new NotImplementedException (); }
        }

        public IDbSet<PositionInfo> Positions
        {
            get { throw new NotImplementedException (); }
            set { throw new NotImplementedException (); }
        }

        #endregion

        #region IDisposable implementation

        public void Dispose ()
        {
        }

        #endregions
    }
}

