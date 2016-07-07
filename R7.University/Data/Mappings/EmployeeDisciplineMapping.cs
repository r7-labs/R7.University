//
//  EmployeeDisciplineMapping.cs
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
    public class EmployeeDisciplineMapping: EntityTypeConfiguration<EmployeeDisciplineInfo>
    {
        public EmployeeDisciplineMapping ()
        {
            HasKey (m => m.EmployeeDisciplineID);
            Property (m => m.EmployeeDisciplineID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
            Property (m => m.EmployeeID).IsRequired ();
            Property (m => m.EduProgramProfileID).IsRequired ();
            Property (m => m.Disciplines).IsOptional ();

            HasRequired (m => m.Employee).WithMany (e => e.Disciplines).HasForeignKey (m => m.EmployeeID).WillCascadeOnDelete (true);
            HasRequired (m => m.EduProgramProfile).WithMany ().HasForeignKey (m => m.EduProgramProfileID).WillCascadeOnDelete (true);
        }
    }
}
