//
//  ContingentConfiguration.cs
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
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class ContingentConfiguration: IEntityTypeConfiguration<ContingentInfo>
    {
        public void Configure (EntityTypeBuilder<ContingentInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<ContingentInfo> ("University", t => "Contingent"));
            entityBuilder.HasKey (m => m.ContingentId);

            entityBuilder.Property (m => m.AvgAdmScore).HasColumnType ("decimal(18,3)").IsRequired (false);

            entityBuilder.Property (m => m.AdmittedFB).IsRequired (false);
            entityBuilder.Property (m => m.AdmittedRB).IsRequired (false);
            entityBuilder.Property (m => m.AdmittedMB).IsRequired (false);
            entityBuilder.Property (m => m.AdmittedBC).IsRequired (false);

            entityBuilder.Property (m => m.ActualFB).IsRequired (false);
            entityBuilder.Property (m => m.ActualRB).IsRequired (false);
            entityBuilder.Property (m => m.ActualMB).IsRequired (false);
            entityBuilder.Property (m => m.ActualBC).IsRequired (false);

            entityBuilder.Property (m => m.VacantFB).IsRequired (false);
            entityBuilder.Property (m => m.VacantRB).IsRequired (false);
            entityBuilder.Property (m => m.VacantMB).IsRequired (false);
            entityBuilder.Property (m => m.VacantBC).IsRequired (false);

            entityBuilder.Property (m => m.MovedIn).IsRequired (false);
            entityBuilder.Property (m => m.MovedOut).IsRequired (false);
            entityBuilder.Property (m => m.Restored).IsRequired (false);
            entityBuilder.Property (m => m.Expelled).IsRequired (false);

            entityBuilder.HasOne (m => m.EduProgramProfileFormYear).WithOne (x => x.Contingent).HasForeignKey<ContingentInfo> (m => m.ContingentId);
        }
    }
}