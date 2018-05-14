//
//  EduProgramDivisionConfiguration.cs
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
    public class EduProgramDivisionConfiguration: IEntityTypeConfiguration<EduProgramDivisionInfo>
    {
        public void Configure (EntityTypeBuilder<EduProgramDivisionInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EduProgramDivisionInfo> ("University", t => "EduProgramDivisions"));
            entityBuilder.HasKey (m => m.EduProgramDivisionId);
            entityBuilder.Property (m => m.EduProgramDivisionId).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.DivisionId).IsRequired ();
            entityBuilder.Property (m => m.EduProgramId).IsRequired (false);
            entityBuilder.Property (m => m.EduProgramProfileId).IsRequired (false);
            entityBuilder.Property (m => m.DivisionRole).IsRequired (false);
            entityBuilder.HasOne (m => m.Division).WithMany ().HasForeignKey (m => m.DivisionId);
        }
    }
}
