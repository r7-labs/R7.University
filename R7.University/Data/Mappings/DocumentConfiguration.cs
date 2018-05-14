//
//  DocumentConfiguration.cs
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
    public class DocumentConfiguration: IEntityTypeConfiguration<DocumentInfo>
    {
        public void Configure (EntityTypeBuilder<DocumentInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<DocumentInfo> ("University", t => "Documents"));
            entityBuilder.HasKey (m => m.DocumentID);
            entityBuilder.Property (m => m.DocumentID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.DocumentTypeID).IsRequired ();
            entityBuilder.Property (m => m.EduProgramId).IsRequired (false);
            entityBuilder.Property (m => m.EduProgramProfileId).IsRequired (false);
            entityBuilder.Property (m => m.Title).IsRequired (false);
            entityBuilder.Property (m => m.Group).IsRequired (false);
            entityBuilder.Property (m => m.Url).IsRequired (false);
            entityBuilder.Property (m => m.SortIndex).IsRequired ();
            entityBuilder.Property (m => m.StartDate).IsRequired (false);
            entityBuilder.Property (m => m.EndDate).IsRequired (false);
            entityBuilder.Property (m => m.CreatedOnDate).IsRequired ();
            entityBuilder.Property (m => m.LastModifiedOnDate).IsRequired ();
            entityBuilder.Property (m => m.CreatedByUserId).IsRequired ();
            entityBuilder.Property (m => m.LastModifiedByUserId).IsRequired ();
            entityBuilder.HasOne (m => m.DocumentType).WithMany ().HasForeignKey (m => m.DocumentTypeID);
        }
    }
}
