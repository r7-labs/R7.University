//
//  UniversityDataContext.cs
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

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetNuke.Common.Utilities;

namespace R7.University.Data
{
    public class UniversityDataContext : DbContext, IDataContext
    {
        static UniversityDataContext ()
        {
            // do not use migrations
            Database.SetInitializer<UniversityDataContext> (null);
        }

        public UniversityDataContext (): base ("name=SiteSqlServer")
        {
            // don't autodetect entity changes
            Configuration.AutoDetectChangesEnabled = false; 

            // don't use lazy loading
            Configuration.LazyLoadingEnabled = false;

            // we don't autotodetect changes nor use lazy loading, so disable proxies
            Configuration.ProxyCreationEnabled = false;

            // don't validate entities
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating (DbModelBuilder modelBuilder)
        {
            // remove trailing '.' from schema name, by ex. "dbo." => "dbo"
            modelBuilder.HasDefaultSchema (Config.GetDataBaseOwner ().TrimEnd ('.'));

            // add mappings
            modelBuilder.Configurations.AddFromAssembly (GetType ().Assembly);

            // table names should be configured in mappings
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #region IUniversityDataContext implementation

        public new IDbSet<TEntity> Set<TEntity> () where TEntity: class
        {
            return base.Set<TEntity> ();
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity> (string queryName, params KeyValuePair<string,object> [] parameters) 
            where TEntity: class
        {
            var sqlParameters = new SqlParameter [parameters.Length];
            var strParameters = new StringBuilder ();
            var first = true;

            for (var i = 0; i < parameters.Length; i++) {
                sqlParameters [i] = new SqlParameter (parameters [i].Key, parameters [i].Value);

                if (first) {
                    strParameters.AppendFormat (" @{0}", parameters [i].Key);
                    first = false;
                }
                else {
                    strParameters.AppendFormat (", @{0}", parameters [i].Key);    
                }
            }

            // databaseOwner is set by modelBuilder.HasDefaultSchema
            queryName = queryName.Replace ("{objectQualifier}", Config.GetObjectQualifer ());

            return Database.SqlQuery<TEntity> (queryName + strParameters, sqlParameters).ToList ();
        }

        public void WasModified<TEntity> (TEntity entity) where TEntity: class
        {
            Entry (entity).State = EntityState.Modified;
        }

        public void WasRemoved<TEntity> (TEntity entity) where TEntity: class
        {
            Entry (entity).State = EntityState.Deleted;
        }

        #endregion
    }
}

