//
//  DocumentTypeConfiguration.cs
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
    public class DocumentTypeConfiguration: IEntityTypeConfiguration<DocumentTypeInfo>
    {
        public void Configure (EntityTypeBuilder<DocumentTypeInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<DocumentTypeInfo> ("University", t => "DocumentTypes"));
            entityBuilder.HasKey (m => m.DocumentTypeID);
            entityBuilder.Property (m => m.DocumentTypeID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.Type).IsRequired ();
            entityBuilder.Property (m => m.Description).IsRequired (false);
            entityBuilder.Property (m => m.IsSystem).IsRequired ();
            entityBuilder.Property (m => m.FilenameFormat).IsRequired (false);
        }
    }
}
