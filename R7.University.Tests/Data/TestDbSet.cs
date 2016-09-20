//
//  TestDbSet.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace R7.University.Tests.Data
{
    public class TestDbSet<TEntity>: IDbSet<TEntity> where TEntity: class
    {
        private HashSet<TEntity> entities;

        private IQueryable query;

        public TestDbSet ()
        {
            entities = new HashSet<TEntity> ();
            query = entities.AsQueryable ();
        }

        public TEntity Add (TEntity entity)
        {
            entities.Add (entity);
            return entity;
        }

        public TEntity Find (params object[] keyValues)
        {
            throw new NotImplementedException ();
        }

        public TEntity Remove (TEntity entity)
        {
            entities.Remove (entity);
            return entity;
        }

        public TEntity Attach (TEntity entity)
        {
            throw new NotImplementedException ();
        }

        public TEntity Create ()
        {
            throw new NotImplementedException ();
        }

        public TDerivedEntity Create<TDerivedEntity> () where TDerivedEntity : class, TEntity
        {
            throw new NotImplementedException ();
        }

        public System.Collections.ObjectModel.ObservableCollection<TEntity> Local
        {
            get { throw new NotImplementedException (); }
        }

        public IEnumerator<TEntity> GetEnumerator ()
        {
            return entities.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return entities.GetEnumerator ();
        }

        public Expression Expression
        {
            get { return query.Expression; }
        }

        public Type ElementType
        {
            get { return query.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return query.Provider; }
        }
    }
}

