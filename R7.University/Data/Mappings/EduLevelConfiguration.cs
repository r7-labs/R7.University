//
//  EduLevelConfiguration.cs
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
    public class EduLevelConfiguration: IEntityTypeConfiguration<EduLevelInfo>
    {
        public void Configure (EntityTypeBuilder<EduLevelInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EduLevelInfo> ("University", t => "EduLevels"));
            entityBuilder.HasKey (m => m.EduLevelID);
            entityBuilder.Property (m => m.EduLevelID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.Title).IsRequired ();
            entityBuilder.Property (m => m.ShortTitle).IsRequired (false);
            entityBuilder.Property (m => m.SortIndex).IsRequired ();
            entityBuilder.Property (m => m.ParentEduLevelId);
            entityBuilder.HasOne (m => m.ParentEduLevel).WithMany ().HasForeignKey (m => m.ParentEduLevelId);
        }
    }
}

