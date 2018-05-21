//
//  EduProgramProfileFormYearConfiguration.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
    public class EduProgramProfileFormYearConfiguration: IEntityTypeConfiguration<EduProgramProfileFormYearInfo>
    {
        public void Configure (EntityTypeBuilder<EduProgramProfileFormYearInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EduProgramProfileFormYearInfo> ("University", t => "EduProgramProfileFormYears"));
            entityBuilder.HasKey (m => m.EduProgramProfileFormYearId);
            entityBuilder.Property (m => m.EduProgramProfileFormYearId).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.EduProgramProfileId).IsRequired ();
            entityBuilder.Property (m => m.EduFormId).IsRequired ();
            entityBuilder.Property (m => m.YearId).IsRequired (false);
            entityBuilder.Property (m => m.StartDate).IsRequired (false);
            entityBuilder.Property (m => m.EndDate).IsRequired (false);
            entityBuilder.HasOne (m => m.EduProgramProfile).WithMany (x => x.EduProgramProfileFormYears).HasForeignKey (m => m.EduProgramProfileId);
            entityBuilder.HasOne (m => m.EduForm).WithMany ().HasForeignKey (m => m.EduFormId);
            entityBuilder.HasOne (m => m.Year).WithMany ().HasForeignKey (m => m.YearId);
            entityBuilder.HasOne (m => m.EduVolume).WithOne (x => x.EduProgramProfileFormYear);
            entityBuilder.HasOne (m => m.Contingent).WithOne (x => x.EduProgramProfileFormYear);
        }
    }
}

