//
//  EmployeeAchievementMapping.cs
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
    public class EmployeeAchievementMapping: EntityTypeConfiguration<EmployeeAchievementInfo>
    {
        public EmployeeAchievementMapping ()
        {
            HasKey (m => m.EmployeeAchievementID);
            Property (m => m.EmployeeAchievementID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
            Property (m => m.AchievementID).IsOptional ();
            Property (m => m.Description).IsOptional ();
            Property (m => m.Title).IsOptional ();
            Property (m => m.ShortTitle).IsOptional ();
            Property (m => m.YearBegin).IsOptional ();
            Property (m => m.YearEnd).IsOptional ();
            Property (m => m.IsTitle).IsRequired ();
            Property (m => m.DocumentURL).IsRequired ();
            Property (m => m.TitleSuffix).IsOptional ();
            Property (m => m.AchievementTypeString).HasColumnName ("AchievementType").IsRequired ();
            Ignore (m => m.AchievementType);

            HasOptional (m => m.Achievement).WithMany ().HasForeignKey (m => m.AchievementID);
        }
    }
}

