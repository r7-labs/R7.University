//
//  UniversityRepositoryBase.cs
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

namespace R7.University.Data
{
    // REVIEW: Make abstract?
    public class UniversityRepository<TEntity> : IDisposable where TEntity : class, new ()
    {
        private readonly IUniversityDbContext db;

        public UniversityRepository (IUniversityDbContext dbContext)
        {
            db = dbContext;
        }

        public virtual TEntity SingleOrDefault (Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) {
                throw new Exception ("Predicate value cannot be null.");
            }

            return db.Set<TEntity> ().SingleOrDefault (predicate);
        }

        public virtual TEntity Get (object key)
        {
            if (key == null) {
                throw new Exception ("Key value cannot be null.");
            }

            try {
                return db.Set<TEntity> ().Find (key);
            }
            catch (Exception ex) {
                // TODO: Log exception
            }
            return null;
        }

        public virtual IQueryable<TEntity> Where (Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) {
                throw new Exception ("Predicate value cannot be null.");
            }
        
            try {
                return db.Set<TEntity> ().Where (predicate);
            }
            catch (Exception ex) {
                // TODO: Log exception
            }
            return null;
        }

        public virtual IQueryable<TEntity> All ()
        {
            try {
                return db.Set<TEntity> ();
            }
            catch (Exception ex) {
                // TODO: Log exception
            }
            return null;
        }

        public virtual bool Save ()
        {
            try {
                return db.SaveChanges () > 0;
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual bool Update (TEntity entity)
        {
            try {
                db.Set<TEntity> ().Attach (entity);
                return db.SaveChanges () > 0;
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual bool Delete (TEntity entity)
        {
            try {
                db.Set<TEntity> ().Remove (entity);
                return db.SaveChanges () > 0;
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }

        }

        #region IDisposable implementation

        public void Dispose ()
        {
            if (db != null) {
                db.Dispose ();
            }
        }

        #endregion
    }
}
