//
//  EduProgramProfileMapping.cs
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
    public class EduProgramProfileMapping: EntityTypeConfiguration<EduProgramProfileInfo>
    {
        public EduProgramProfileMapping ()
        {
            ToTable (UniversityMappingHelper.GetTableName<EduProgramProfileInfo> ());
            HasKey (m => m.EduProgramProfileID);
            Property (m => m.EduProgramProfileID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
            Property (m => m.EduProgramID).IsRequired ();
            Property (m => m.EduLevelId).IsRequired ();
            Property (m => m.ProfileCode).IsOptional ();
            Property (m => m.ProfileTitle).IsOptional ();
            Property (m => m.Languages).IsOptional ();
            Property (m => m.IsAdopted).IsRequired ();
            Property (m => m.ELearning).IsRequired ();
            Property (m => m.DistanceEducation).IsRequired ();
            Property (m => m.AccreditedToDate).IsOptional ();
            Property (m => m.CommunityAccreditedToDate).IsOptional ();

            Property (m => m.StartDate).IsOptional ();
            Property (m => m.EndDate).IsOptional ();

            Property (m => m.LastModifiedByUserID);
            Property (m => m.LastModifiedOnDate);
            Property (m => m.CreatedByUserID);
            Property (m => m.CreatedOnDate);

            HasRequired (m => m.EduProgram).WithMany ().HasForeignKey (m => m.EduProgramID);
            HasRequired (m => m.EduLevel).WithMany ().HasForeignKey (m => m.EduLevelId);
            HasMany (m => m.Documents).WithOptional ().HasForeignKey (d => d.EduProgramProfileId);
            HasMany (m => m.EduProgramProfileFormYears).WithRequired ().HasForeignKey (eppfy => eppfy.EduProgramProfileId);
        }
    }
}
