//
//  UniversityDataRepository.cs
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
    // TODO: Write errors to DNN eventlog
    // REVIEW: Extract IDbRepository interface?
    // REVIEW: Make abstract?

    /// <summary>
    /// Implements generic repository pattern
    /// </summary>
    public class UniversityDataRepository : IDataRepository
    {
        private bool _disposed = false;

        // REVIEW: Use factory for repository, not db context?
        private IDataContext _context;
        protected IDataContext Context
        {
            get {
                if (!_disposed) {
                    return _context ?? (_context = UniversityDataContextFactory.Instance.Create ());
                }

                throw new InvalidOperationException ("Cannot use repository after it was disposed.");
            }
        }

        public UniversityDataRepository ()
        {
        }

        public UniversityDataRepository (IDataContext dbContext)
        {
            _context = dbContext;
        }

        #region IDataRepository implementation

        public virtual IQueryable<TEntity> Query<TEntity> () where TEntity: class
        {
            return Context.Set<TEntity> ();
        }

        /*
        public virtual IQueryable<TEntity> Query<TEntity> (Expression<Func<TEntity,object>> keySelector, object key) where TEntity: class
        {
            if (keySelector == null || key == null) {
                throw new ArgumentException ("KeySelector and key values cannot be null.");
            }

            return Context.Set<TEntity> ().Where (e => keySelector (e) == key);
        }*/

        public virtual TEntity Get<TEntity> (object key) where TEntity: class
        {
            if (key == null) {
                throw new ArgumentException ("Key value cannot be null.");
            }

            return Context.Set<TEntity> ().Find (key);
        }

        public virtual void Add<TEntity> (TEntity entity) where TEntity: class
        {
            Context.Set<TEntity> ().Add (entity);
        }

        public virtual void Update<TEntity> (TEntity entity) where TEntity: class
        {
            Context.WasModified (entity);
        }

        public virtual void AddOrUpdate<TEntity> (TEntity entity) where TEntity: class
        {
            if (!Exists (entity)) {
                // add
                Context.Set<TEntity> ().Add (entity);
            } 
            else {
                // update
                Context.WasModified (entity);
            }
        }

        // TODO: Test this
        public virtual void UpdateExternal<TEntity> (TEntity entity) where TEntity: class
        {
            Context.Set<TEntity> ().Attach (entity);
        }

        public virtual void Remove<TEntity> (TEntity entity) where TEntity: class
        {
            Context.Set<TEntity> ().Remove (entity);
        }

        public bool Exists<TEntity> (TEntity entity) where TEntity : class
        {
            return Context.Set<TEntity> ().Local.Any (e => e == entity);
        }

        public virtual bool SaveChanges (bool dispose = false)
        {
            var result = Context.SaveChanges () > 0;

            if (dispose) {
                Dispose ();
            }

            return result;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose ()
        {
            _disposed = true;
            if (_context != null) {
                _context.Dispose ();
            }
        }

        #endregion
    }
}
