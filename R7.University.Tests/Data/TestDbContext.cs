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

namespace R7.University.Tests.Data
{
    public class TestDbContext: IUniversityDbContext
    {
        protected IDbSet<EmployeeInfo> employees;
        protected IDbSet<DivisionInfo> divisions;
        protected IDbSet<OccupiedPositionInfo> occupiedPositions;
        protected IDbSet<PositionInfo> positions;

        public TestDbContext ()
        {
            employees = new TestDbSet<EmployeeInfo> ();
            divisions = new TestDbSet<DivisionInfo> ();
            occupiedPositions = new TestDbSet<OccupiedPositionInfo> ();
            positions = new TestDbSet<PositionInfo> ();
        }

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
            get { return employees; }
            set { employees = value; }
        }

        public IDbSet<DivisionInfo> Divisions
        {
            get { return divisions; }
            set { divisions = value; }
        }

        public IDbSet<OccupiedPositionInfo> OccupiedPositions
        {
            get { return occupiedPositions; }
            set { occupiedPositions = value; }
        }

        public IDbSet<PositionInfo> Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        #endregion

        #region IDisposable implementation

        public void Dispose ()
        {
        }

        #endregion
    }
}

