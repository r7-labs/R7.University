//
//  UniversityDbRepository.cs
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
    // TODO: Write errors to DNN eventlog
    // REVIEW: Extract IDbRepository interface?
    // REVIEW: Make abstract?

    /// <summary>
    /// Implements generic repository pattern
    /// </summary>
    public class UniversityDbRepository : IDisposable
    {
        // REVIEW: Use factory for repository, not db context?
        private IUniversityDbContext _context;
        protected IUniversityDbContext Context
        {
            get { return _context ?? (_context = UniversityDbContextFactory.Instance.Create ()); }
        }

        public UniversityDbRepository ()
        {
        }

        public UniversityDbRepository (IUniversityDbContext dbContext)
        {
            _context = dbContext;
        }

        #region Repository methods

        public virtual TEntity SingleOrDefault<TEntity> (Expression<Func<TEntity, bool>> predicate) where TEntity: class
        {
            if (predicate == null) {
                throw new ArgumentException ("Predicate value cannot be null.");
            }

            try {
                return Context.Set<TEntity> ().SingleOrDefault (predicate);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual TEntity Get<TEntity> (object key) where TEntity: class
        {
            if (key == null) {
                throw new ArgumentException ("Key value cannot be null.");
            }

            try {
                return Context.Set<TEntity> ().Find (key);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual IQueryable<TEntity> Where<TEntity> (Expression<Func<TEntity, bool>> predicate) where TEntity: class
        {
            if (predicate == null) {
                throw new ArgumentException ("Predicate value cannot be null.");
            }
        
            try {
                return Context.Set<TEntity> ().Where (predicate);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual IQueryable<TEntity> GetAll<TEntity> () where TEntity: class
        {
            try {
                return Context.Set<TEntity> ();
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual bool SaveChanges ()
        {
            try {
                return Context.SaveChanges () > 0;
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual void Add<TEntity> (TEntity entity) where TEntity: class
        {
            try {
                Context.Set<TEntity> ().Add (entity);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual void Update<TEntity> (TEntity entity) where TEntity: class
        {
            try {
                Context.WasModified (entity);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual void AddOrUpdate<TEntity> (TEntity entity) where TEntity: class
        {
            try {
                if (!Exists (entity)) {
                    // add
                    Context.Set<TEntity> ().Add (entity);
                } 
                else {
                    // update
                    Context.WasModified (entity);
                }
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        // TODO: Test this
        public virtual void UpdateExternal<TEntity> (TEntity entity) where TEntity: class
        {
            try {
                Context.Set<TEntity> ().Attach (entity);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        public virtual void Remove<TEntity> (TEntity entity) where TEntity: class
        {
            try {
                Context.Set<TEntity> ().Remove (entity);
            }
            catch (Exception ex) {
                // TODO: Log exception
                throw;
            }
        }

        #endregion

        #region Private methods

        private bool Exists<TEntity> (TEntity entity) where TEntity : class
        {
            return Context.Set<TEntity> ().Local.Any (e => e == entity);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose ()
        {
            if (_context != null) {
                _context.Dispose ();
            }
        }

        #endregion
    }
}
