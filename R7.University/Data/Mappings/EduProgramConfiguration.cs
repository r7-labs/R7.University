//
//  EduProgramConfiguration.cs
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
    public class EduProgramConfiguration: IEntityTypeConfiguration<EduProgramInfo>
    {
        public void Configure (EntityTypeBuilder<EduProgramInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EduProgramInfo> ("University", t => "EduPrograms"));
            entityBuilder.HasKey (m => m.EduProgramID);
            entityBuilder.Property (m => m.EduProgramID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.EduLevelID).IsRequired ();
            entityBuilder.Property (m => m.Code).IsRequired ();
            entityBuilder.Property (m => m.Title).IsRequired ();
            entityBuilder.Property (m => m.Generation).IsRequired (false);
            entityBuilder.Property (m => m.HomePage).IsRequired (false);
            entityBuilder.Property (m => m.StartDate).IsRequired (false);
            entityBuilder.Property (m => m.EndDate).IsRequired (false);
            entityBuilder.Property (m => m.LastModifiedByUserId);
            entityBuilder.Property (m => m.LastModifiedOnDate);
            entityBuilder.Property (m => m.CreatedByUserId);
            entityBuilder.Property (m => m.CreatedOnDate);
            entityBuilder.HasOne (m => m.EduLevel).WithMany ().HasForeignKey (m => m.EduLevelID);
            entityBuilder.HasOne (m => m.Science).WithOne (ep => ep.EduProgram).HasForeignKey<ScienceInfo> (s => s.ScienceId);
            entityBuilder.HasMany (m => m.Documents).WithOne ().HasForeignKey (d => d.EduProgramId);
            entityBuilder.HasMany (m => m.EduProgramProfiles).WithOne (epp => epp.EduProgram).HasForeignKey (epp => epp.EduProgramID);
        }
    }
}
