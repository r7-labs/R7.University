//
//  AchievementConfiguration.cs
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
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class AchievementConfiguration: IEntityTypeConfiguration<AchievementInfo>
    {
        public void Configure (EntityTypeBuilder<AchievementInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<AchievementInfo> ("University", t => "Achievements"));
            entityBuilder.HasKey (m => m.AchievementID);
            entityBuilder.Property (m => m.AchievementID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.AchievementTypeId).IsRequired (false);
            entityBuilder.Property (m => m.Title).IsRequired ();
            entityBuilder.Property (m => m.ShortTitle).IsRequired (false);
            entityBuilder.HasOne (m => m.AchievementType).WithMany ().HasForeignKey (m => m.AchievementTypeId);
        }
    }
}

