//
//  EduProgramProfileConfiguration.cs
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
using R7.Dnn.Extensions.EFCore;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EduProgramProfileConfiguration: IEntityTypeConfiguration<EduProgramProfileInfo>
    {
        public void Configure (EntityTypeBuilder<EduProgramProfileInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EduProgramProfileInfo> ("University", t => "EduProgramProfiles"));
            entityBuilder.HasKey (m => m.EduProgramProfileID);
            entityBuilder.Property (m => m.EduProgramProfileID).ValueGeneratedOnAdd ();
            entityBuilder.Property (m => m.EduProgramID).IsRequired ();
            entityBuilder.Property (m => m.EduLevelId).IsRequired ();
            entityBuilder.Property (m => m.ProfileCode).IsRequired (false);
            entityBuilder.Property (m => m.ProfileTitle).IsRequired (false);
            entityBuilder.Property (m => m.Languages).IsRequired (false);
            entityBuilder.Property (m => m.IsAdopted).IsRequired ();
            entityBuilder.Property (m => m.ELearning).IsRequired ();
            entityBuilder.Property (m => m.DistanceEducation).IsRequired ();
            entityBuilder.Property (m => m.AccreditedToDate).IsRequired (false);
            entityBuilder.Property (m => m.CommunityAccreditedToDate).IsRequired (false);
            entityBuilder.Property (m => m.StartDate).IsRequired (false);
            entityBuilder.Property (m => m.EndDate).IsRequired (false);
            entityBuilder.Property (m => m.LastModifiedByUserId).IsRequired ();
            entityBuilder.Property (m => m.LastModifiedOnDate).IsRequired ();
            entityBuilder.Property (m => m.CreatedByUserId).IsRequired ();
            entityBuilder.Property (m => m.CreatedOnDate).IsRequired ();
            entityBuilder.HasOne (m => m.EduProgram).WithMany (ep => ep.EduProgramProfiles).HasForeignKey (m => m.EduProgramID);
            entityBuilder.HasOne (m => m.EduLevel).WithMany ().HasForeignKey (m => m.EduLevelId);
            entityBuilder.HasMany (m => m.Documents).WithOne ().HasForeignKey (d => d.EduProgramProfileId);
            entityBuilder.HasMany (m => m.EduProgramProfileFormYears).WithOne ().HasForeignKey (eppfy => eppfy.EduProgramProfileId);
        }
    }
}
