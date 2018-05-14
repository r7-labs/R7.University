//
//  DivisionConfiguration.cs
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
    public class DivisionConfiguration: IEntityTypeConfiguration<DivisionInfo>
    {
        public void Configure (EntityTypeBuilder<DivisionInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<DivisionInfo> ("University", t => "Divisions"));
            entityBuilder.HasKey (m => m.DivisionID);
            entityBuilder.Property (m => m.DivisionID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.ParentDivisionID).IsRequired (false);
            entityBuilder.Property (m => m.DivisionTermID).IsRequired (false);
            entityBuilder.Property (m => m.HeadPositionID).IsRequired (false);
            entityBuilder.Property (m => m.Title).IsRequired ();
            entityBuilder.Property (m => m.ShortTitle);
            entityBuilder.Property (m => m.HomePage);
            entityBuilder.Property (m => m.WebSite);
            entityBuilder.Property (m => m.WebSiteLabel);
            entityBuilder.Property (m => m.Phone);
            entityBuilder.Property (m => m.Fax);
            entityBuilder.Property (m => m.Email);
            entityBuilder.Property (m => m.SecondaryEmail);
            entityBuilder.Property (m => m.Address);
            entityBuilder.Property (m => m.Location);
            entityBuilder.Property (m => m.WorkingHours);
            entityBuilder.Property (m => m.DocumentUrl);
            entityBuilder.Property (m => m.IsSingleEntity);
            entityBuilder.Property (m => m.IsInformal);
            entityBuilder.Property (m => m.IsGoverning);
            entityBuilder.Property (m => m.StartDate).IsRequired (false);
            entityBuilder.Property (m => m.EndDate).IsRequired (false);
            entityBuilder.Property (m => m.LastModifiedByUserId);
            entityBuilder.Property (m => m.LastModifiedOnDate);
            entityBuilder.Property (m => m.CreatedByUserId);
            entityBuilder.Property (m => m.CreatedOnDate);
            entityBuilder.Ignore (m => m.Level);
            entityBuilder.Ignore (m => m.Path);
            entityBuilder.HasMany (m => m.SubDivisions).WithOne ().HasForeignKey (sd => sd.ParentDivisionID);
            entityBuilder.HasMany (m => m.OccupiedPositions).WithOne ().HasForeignKey (op => op.DivisionID);
        }
    }
}
