//
//  TestDataContext.cs
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
using R7.Dnn.Extensions.Data;
using R7.Dnn.Extensions.Models;

namespace R7.University.Tests.Data
{
    public class TestDataContext: IDataContext
    {
        private IDictionary<Type,object> dictEntities = new Dictionary<Type,object> ();

        #region IUniversityDbContext implementation

        public IDataSet<TEntity> GetDataSet<TEntity> () where TEntity : class
        {
            if (dictEntities.ContainsKey (typeof (TEntity))) {
                return (TestDataSet<TEntity>) dictEntities [typeof (TEntity)];
            }

            var dbSet = new TestDataSet<TEntity> ();
            dictEntities [typeof (TEntity)] = dbSet;
            return dbSet;
        }

        public void WasModified<TEntity> (TEntity entity) where TEntity : class
        {
            throw new NotImplementedException ();
        }

        public void WasRemoved<TEntity> (TEntity entity) where TEntity : class
        {
            throw new NotImplementedException ();
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity> (string queryName, params KeyValuePair<string,object> [] parameters) where TEntity : class
        {
            throw new NotImplementedException ();
        }

        public int SaveChanges ()
        {
            return 0;
        }

        public ITransaction BeginTransaction ()
        {
            throw new NotImplementedException ();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose ()
        {
        }

        #endregion
    }
}

