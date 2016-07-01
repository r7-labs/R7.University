//
//  EmployeeMapping.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EmployeeMapping: EntityTypeConfiguration<EmployeeInfo>
    {
        public EmployeeMapping ()
        {
            HasKey (m => m.EmployeeID);
            Property (m => m.EmployeeID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
            Property (m => m.UserID).IsOptional ();
            Property (m => m.PhotoFileID).IsOptional ();
            Property (m => m.LastName).IsRequired ();
            Property (m => m.FirstName).IsRequired ();
            Property (m => m.OtherName);

            Property (m => m.Phone);
            Property (m => m.CellPhone);
            Property (m => m.Fax);
            Property (m => m.Email);
            Property (m => m.SecondaryEmail);
            Property (m => m.WebSite);
            Property (m => m.WebSiteLabel);
            Property (m => m.Messenger);
            Property (m => m.Biography);   

            Property (m => m.WorkingPlace);
            Property (m => m.WorkingHours);    
            Property (m => m.ExperienceYears).IsOptional ();
            Property (m => m.ExperienceYearsBySpec).IsOptional ();

            Property (m => m.StartDate).IsOptional ();
            Property (m => m.EndDate).IsOptional ();

            Property (m => m.LastModifiedByUserID);
            Property (m => m.LastModifiedOnDate);
            Property (m => m.CreatedByUserID);
            Property (m => m.CreatedOnDate);

            Property (m => m.ShowBarcode);

            Ignore (m => m.OccupiedPositions);
            Ignore (m => m.Achievements);
            Ignore (m => m.Disciplines);

            HasMany (m => m.Positions).WithRequired ().HasForeignKey (x => x.EmployeeID).WillCascadeOnDelete (true);
        }
    }
}
