//
//  OccupiedPositionMapping.cs
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
    public class OccupiedPositionMapping: EntityTypeConfiguration<OccupiedPositionInfo>
    {
        public OccupiedPositionMapping ()
        {
            HasKey (m => m.OccupiedPositionID);
            Property (m => m.OccupiedPositionID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);

            Property (m => m.PositionID).IsRequired ();
            Property (m => m.DivisionID).IsRequired ();
            Property (m => m.EmployeeID).IsRequired ();

            HasRequired<PositionInfo> (m => m.Position).WithMany ().HasForeignKey (m => m.PositionID);
            HasRequired<DivisionInfo> (m => m.Division).WithMany ().HasForeignKey (m => m.DivisionID);
            HasRequired<EmployeeInfo> (m => m.Employee).WithMany ().HasForeignKey (m => m.EmployeeID);

            Property (m => m.IsPrime);
            Property (m => m.TitleSuffix);
        }
    }
}
