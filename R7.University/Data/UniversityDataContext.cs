//
//  UniversityDataContext.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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

using Microsoft.EntityFrameworkCore;
using R7.Dnn.Extensions.Data;
using R7.Dnn.Extensions.EFCore;
using R7.University.Data.Mappings;

namespace R7.University.Data
{
    public class UniversityDataContext: EFCoreDnnDataContextBase, IDataContext
    {
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder);

            ApplyConfiguration (new YearConfiguration (), modelBuilder);
            ApplyConfiguration (new DocumentConfiguration (), modelBuilder);
            ApplyConfiguration (new DocumentTypeConfiguration (), modelBuilder);
            ApplyConfiguration (new EduProgramDivisionConfiguration (), modelBuilder);
            ApplyConfiguration (new EduLevelConfiguration (), modelBuilder);
            ApplyConfiguration (new EduFormConfiguration (), modelBuilder);
            ApplyConfiguration (new PositionConfiguration (), modelBuilder);
            ApplyConfiguration (new AchievementConfiguration (), modelBuilder);
            ApplyConfiguration (new AchievementTypeConfiguration (), modelBuilder);
            ApplyConfiguration (new DivisionConfiguration (), modelBuilder);
            ApplyConfiguration (new EmployeeConfiguration (), modelBuilder);
            ApplyConfiguration (new EmployeeAchievementConfiguration (), modelBuilder);
            ApplyConfiguration (new EmployeeDisciplineConfiguration (), modelBuilder);
            ApplyConfiguration (new OccupiedPositionConfiguration (), modelBuilder);
            ApplyConfiguration (new EduProgramConfiguration (), modelBuilder);
            ApplyConfiguration (new ScienceConfiguration (), modelBuilder);
            ApplyConfiguration (new EduProfileConfiguration (), modelBuilder);
            ApplyConfiguration (new EduProgramProfileFormConfiguration (), modelBuilder);
            ApplyConfiguration (new EduProgramProfileFormYearConfiguration (), modelBuilder);
            ApplyConfiguration (new EduVolumeConfiguration (), modelBuilder);
            ApplyConfiguration (new ContingentConfiguration (), modelBuilder);
        }

        protected void ApplyConfiguration<TEntity> (IEntityTypeConfiguration<TEntity> configuration, ModelBuilder modelBuilder) where TEntity: class
        {
            configuration.Configure (modelBuilder.Entity<TEntity> ());
        }
    }
}

