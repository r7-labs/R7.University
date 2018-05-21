//
//  OccupiedPositionConfiguration.cs
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
    public class OccupiedPositionConfiguration: IEntityTypeConfiguration<OccupiedPositionInfo>
    {
        public void Configure (EntityTypeBuilder<OccupiedPositionInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<OccupiedPositionInfo> ("University", t => "OccupiedPositions"));
            entityBuilder.HasKey (m => m.OccupiedPositionID);
            entityBuilder.Property (m => m.OccupiedPositionID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.PositionID).IsRequired ();
            entityBuilder.Property (m => m.DivisionID).IsRequired ();
            entityBuilder.Property (m => m.EmployeeID).IsRequired ();
            entityBuilder.HasOne (m => m.Position).WithMany ().HasForeignKey (m => m.PositionID);
            entityBuilder.HasOne (m => m.Division).WithMany (d => d.OccupiedPositions).HasForeignKey (m => m.DivisionID);
            entityBuilder.HasOne (m => m.Employee).WithMany (e => e.Positions).HasForeignKey (m => m.EmployeeID);
            entityBuilder.Property (m => m.IsPrime);
            entityBuilder.Property (m => m.TitleSuffix);
        }
    }
}
