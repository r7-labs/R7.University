//
//  UniversityDataContext.cs
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
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Conventions;
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
        }

        protected override void OnModelCreating (DbModelBuilder modelBuilder)
        {
            // remove trailing '.' from schema name, by ex. "dbo." => "dbo"
            modelBuilder.HasDefaultSchema (Config.GetDataBaseOwner ().TrimEnd ('.'));

            // add mappings
            modelBuilder.Configurations.AddFromAssembly (GetType ().Assembly);

            /*
            modelBuilder.Configurations.Add<EmployeeInfo> (new EmployeeMapping ());
            modelBuilder.Configurations.Add<DivisionInfo> (new DivisionMapping ());
            modelBuilder.Configurations.Add<PositionInfo> (new PositionMapping ());
            modelBuilder.Configurations.Add<OccupiedPositionInfo> (new OccupiedPositionMapping ());
            modelBuilder.Configurations.Add<EmployeeDisciplineInfo> (new EmployeeDisciplineMapping ());
            modelBuilder.Configurations.Add<EduLevelInfo> (new EduLevelMapping ());
            modelBuilder.Configurations.Add<EduProgramInfo> (new EduProgramMapping ());
            modelBuilder.Configurations.Add<EduFormInfo> (new EduFormMapping ());
            modelBuilder.Configurations.Add<EduProgramProfileInfo> (new EduProgramProfileMapping ());
            modelBuilder.Configurations.Add<EduProgramProfileFormInfo> (new EduProgramProfileFormMapping ());
            modelBuilder.Configurations.Add<DocumentInfo> (new DocumentMapping ());
            modelBuilder.Configurations.Add<DocumentTypeInfo> (new DocumentTypeMapping ());
            modelBuilder.Configurations.Add<AchievementInfo> (new AchievementMapping ());
            modelBuilder.Configurations.Add<EmployeeAchievementInfo> (new EmployeeAchievementMapping ());
*/
            // add objectQualifier
            var plurService = new EnglishPluralizationService ();
            modelBuilder.Types ().Configure (entity => 
                entity.ToTable (Config.GetObjectQualifer () 
                    + "University_" + plurService.Pluralize (entity.ClrType.Name.Replace ("Info", string.Empty))));

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #region IUniversityDbContext implementation

        public new IDbSet<TEntity> Set<TEntity> () where TEntity: class
        {
            return base.Set<TEntity> ();
        }

        public void WasModified<TEntity> (TEntity entity) where TEntity: class
        {
            base.Entry<TEntity> (entity).State = EntityState.Modified;
        }

        #endregion
    }
}

