//
//  IModelContext.cs
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
using System.Linq.Expressions;

namespace R7.University.Models
{
    public interface IModelContext: IDisposable
    {
        IQueryable<TEntity> Query<TEntity> () where TEntity: class;

        IQueryable<TEntity> QueryOne<TEntity> (Expression<Func<TEntity,bool>> keySelector) where TEntity: class;

        TEntity Get<TEntity> (object key) where TEntity: class;

        bool Exists<TEntity> (TEntity entity) where TEntity : class;

        void Add<TEntity> (TEntity entity) where TEntity: class;

        void Update<TEntity> (TEntity entity) where TEntity: class;

        void AddOrUpdate<TEntity> (TEntity entity) where TEntity: class;

        void UpdateExternal<TEntity> (TEntity entity) where TEntity: class;

        void Remove<TEntity> (TEntity entity) where TEntity: class;

        bool SaveChanges (bool dispose = false);
    }
}

