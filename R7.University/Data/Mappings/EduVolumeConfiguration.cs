//
//  EduVolumeConfiguration.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
    public class EduVolumeConfiguration: IEntityTypeConfiguration<EduVolumeInfo>
    {
        public void Configure (EntityTypeBuilder<EduVolumeInfo> entityBuilder)
        {
            entityBuilder.ToTable (DnnTableMappingHelper.GetTableName<EduVolumeInfo> ("University", t => "EduVolume"));
            entityBuilder.HasKey (m => m.EduVolumeId);
            entityBuilder.Property (m => m.TimeToLearnHours).IsRequired ();
            entityBuilder.Property (m => m.TimeToLearnMonths).IsRequired ();
            entityBuilder.Property (m => m.Year1Cu).IsRequired (false);
            entityBuilder.Property (m => m.Year2Cu).IsRequired (false);
            entityBuilder.Property (m => m.Year3Cu).IsRequired (false);
            entityBuilder.Property (m => m.Year4Cu).IsRequired (false);
            entityBuilder.Property (m => m.Year5Cu).IsRequired (false);
            entityBuilder.Property (m => m.Year6Cu).IsRequired (false);
            entityBuilder.Property (m => m.PracticeType1Cu).IsRequired (false);
            entityBuilder.Property (m => m.PracticeType2Cu).IsRequired (false);
            entityBuilder.Property (m => m.PracticeType3Cu).IsRequired (false);
            entityBuilder.HasOne (m => m.EduProgramProfileFormYear).WithOne (eppfy => eppfy.EduVolume).HasForeignKey<EduVolumeInfo> (ev => ev.EduVolumeId);
        }
    }
}