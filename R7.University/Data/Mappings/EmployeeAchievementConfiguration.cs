//
//  EmployeeAchievementConfiguration.cs
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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R7.Dnn.Extensions.Data;
using R7.Dnn.Extensions.EFCore;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EmployeeAchievementConfiguration: IEntityTypeConfiguration<EmployeeAchievementInfo>
    {
        public void Configure (EntityTypeBuilder<EmployeeAchievementInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EmployeeAchievementInfo> ("University", t => "EmployeeAchievements"));
            entityBuilder.HasKey (m => m.EmployeeAchievementID);
            entityBuilder.Property (m => m.EmployeeAchievementID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.AchievementID).IsRequired (false);
            entityBuilder.Property (m => m.AchievementTypeId).IsRequired (false);
            entityBuilder.Property (m => m.Description).IsRequired (false);
            entityBuilder.Property (m => m.Title).IsRequired (false);
            entityBuilder.Property (m => m.ShortTitle).IsRequired (false);
            entityBuilder.Property (m => m.YearBegin).IsRequired (false);
            entityBuilder.Property (m => m.YearEnd).IsRequired (false);
            entityBuilder.Property (m => m.IsTitle).IsRequired ();
            entityBuilder.Property (m => m.DocumentURL).IsRequired ();
            entityBuilder.Property (m => m.TitleSuffix).IsRequired (false);
            entityBuilder.Property (m => m.Hours).IsRequired (false);
            entityBuilder.Property (m => m.EduLevelId).IsRequired (false);
            //entityBuilder.HasOptional (m => m.Achievement).WithMany ().HasForeignKey (m => m.AchievementID);
            //entityBuilder.HasOptional (m => m.AchievementType).WithMany ().HasForeignKey (m => m.AchievementTypeId);
            //entityBuilder.HasOptional (m => m.EduLevel).WithMany ().HasForeignKey (m => m.EduLevelId);
        }
    }
}

