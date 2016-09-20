//
//  FlatQuery.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using R7.University.Models;

namespace R7.University.Queries
{
    public class FlatQuery<TEntity>: QueryBase where TEntity: class
    {
        public FlatQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<TEntity> List ()
        {
            return ModelContext.Query<TEntity> ().ToList ();
        }

        public IList<TEntity> ListWhere (Expression<Func<TEntity,bool>> predicate)
        {
            return ModelContext.Query<TEntity> ().Where (predicate).ToList ();
        }

        public IList<TEntity> ListOrderBy<TKey> (Expression<Func<TEntity,TKey>> keySelector)
        {
            return ModelContext.Query<TEntity> ().OrderBy (keySelector).ToList ();
        }

        public IList<TEntity> ListWhereOrderBy<TKey> (Expression<Func<TEntity,bool>> predicate, Expression<Func<TEntity,TKey>> keySelector)
        {
            return ModelContext.Query<TEntity> ().Where (predicate).OrderBy (keySelector).ToList ();
        }
    }
}

