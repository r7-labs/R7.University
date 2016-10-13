//
//  DivisionMapping.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class DivisionMapping: EntityTypeConfiguration<DivisionInfo>
    {
        public DivisionMapping ()
        {
            HasKey (m => m.DivisionID);
            Property (m => m.DivisionID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);

            Property (m => m.ParentDivisionID).IsOptional ();
            Property (m => m.DivisionTermID).IsOptional ();
            Property (m => m.HeadPositionID).IsOptional ();

            Property (m => m.Title).IsRequired ();
            Property (m => m.ShortTitle);

            Property (m => m.HomePage);
            Property (m => m.WebSite);
            Property (m => m.WebSiteLabel);

            Property (m => m.Phone);
            Property (m => m.Fax);
            Property (m => m.Email);
            Property (m => m.SecondaryEmail);
            Property (m => m.Address);
            Property (m => m.Location);
            Property (m => m.WorkingHours);
            Property (m => m.DocumentUrl);
            Property (m => m.IsVirtual);
            Property (m => m.IsInformal);

            Property (m => m.StartDate).IsOptional ();
            Property (m => m.EndDate).IsOptional ();

            Property (m => m.LastModifiedByUserID);
            Property (m => m.LastModifiedOnDate);
            Property (m => m.CreatedByUserID);
            Property (m => m.CreatedOnDate);

            Ignore (m => m.Level);
            Ignore (m => m.Path);

            HasMany (m => m.SubDivisions).WithRequired ().HasForeignKey (sd => sd.ParentDivisionID);
        }
    }
}
