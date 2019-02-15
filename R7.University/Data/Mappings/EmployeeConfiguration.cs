//
//  EmployeeConfiguration.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2019 Roman M. Yagodin
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
    public class EmployeeConfiguration: IEntityTypeConfiguration<EmployeeInfo>
    {
        public void Configure (EntityTypeBuilder<EmployeeInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EmployeeInfo> ("University", t => "Employees"));
            entityBuilder.HasKey (m => m.EmployeeID);
            entityBuilder.Property (m => m.EmployeeID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.UserID).IsRequired (false);
            entityBuilder.Property (m => m.PhotoFileID).IsRequired (false);
            entityBuilder.Property (m => m.AltPhotoFileId).IsRequired (false);
            entityBuilder.Property (m => m.LastName).IsRequired ();
            entityBuilder.Property (m => m.FirstName).IsRequired ();
            entityBuilder.Property (m => m.OtherName);
            entityBuilder.Property (m => m.Phone);
            entityBuilder.Property (m => m.CellPhone);
            entityBuilder.Property (m => m.Fax);
            entityBuilder.Property (m => m.Email);
            entityBuilder.Property (m => m.SecondaryEmail);
            entityBuilder.Property (m => m.WebSite);
            entityBuilder.Property (m => m.WebSiteLabel);
            entityBuilder.Property (m => m.Messenger);
            entityBuilder.Property (m => m.Biography);   
            entityBuilder.Property (m => m.WorkingPlace);
            entityBuilder.Property (m => m.WorkingHours);    
            entityBuilder.Property (m => m.ExperienceYears).IsRequired (false);
            entityBuilder.Property (m => m.ExperienceYearsBySpec).IsRequired (false);
            entityBuilder.Property (m => m.StartDate).IsRequired (false);
            entityBuilder.Property (m => m.EndDate).IsRequired (false);
            entityBuilder.Property (m => m.LastModifiedByUserId);
            entityBuilder.Property (m => m.LastModifiedOnDate);
            entityBuilder.Property (m => m.CreatedByUserId);
            entityBuilder.Property (m => m.CreatedOnDate);
            entityBuilder.Property (m => m.ShowBarcode);
            entityBuilder.Property (m => m.ScienceIndexAuthorId);
            entityBuilder.HasMany (m => m.Positions).WithOne ().HasForeignKey (x => x.EmployeeID);
            entityBuilder.HasMany (m => m.Disciplines).WithOne (ed => ed.Employee).HasForeignKey (x => x.EmployeeID);
            entityBuilder.HasMany (m => m.Achievements).WithOne ().HasForeignKey (x => x.EmployeeID);
        }
    }
}
