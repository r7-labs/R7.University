//
//  ScienceConfiguration.cs
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
    public class ScienceConfiguration:  IEntityTypeConfiguration<ScienceInfo>
    {
        public void Configure (EntityTypeBuilder<ScienceInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<ScienceInfo> ("University", t => "Science"));
            entityBuilder.HasKey (m => m.ScienceId);
            entityBuilder.Property (m => m.Directions).IsRequired (false);
            entityBuilder.Property (m => m.Base).IsRequired (false);
            entityBuilder.Property (m => m.Scientists).IsRequired (false);
            entityBuilder.Property (m => m.Students).IsRequired (false);
            entityBuilder.Property (m => m.Monographs).IsRequired (false);
            entityBuilder.Property (m => m.Articles).IsRequired (false);
            entityBuilder.Property (m => m.ArticlesForeign).IsRequired (false);
            entityBuilder.Property (m => m.Patents).IsRequired (false);
            entityBuilder.Property (m => m.PatentsForeign).IsRequired (false);
            entityBuilder.Property (m => m.Certificates).IsRequired (false);
            entityBuilder.Property (m => m.CertificatesForeign).IsRequired (false);
            entityBuilder.Property (m => m.FinancingByScientist).HasColumnType ("decimal(18,3)").IsRequired (false);
            //entityBuilder.HasOne (m => m.EduProgram).WithOne (ep => ep.Science).HasForeignKey<ScienceInfo> (m => m.ScienceId);
        }
    }
}
