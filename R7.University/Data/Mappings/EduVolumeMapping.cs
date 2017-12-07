//
//  EduVolumeMapping.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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

using System.Data.Entity.ModelConfiguration;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EduVolumeMapping: EntityTypeConfiguration<EduVolumeInfo>
    {
        public EduVolumeMapping ()
        {
            ToTable (UniversityMappingHelper.GetTableName<EduVolumeInfo> (pluralize: false));
            HasKey (m => m.EduVolumeId);

            Property (m => m.TimeToLearnHours).IsRequired ();
            Property (m => m.TimeToLearnMonths).IsRequired ();

            Property (m => m.Year1Cu).IsOptional ();
            Property (m => m.Year2Cu).IsOptional ();
            Property (m => m.Year3Cu).IsOptional ();
            Property (m => m.Year4Cu).IsOptional ();
            Property (m => m.Year5Cu).IsOptional ();
            Property (m => m.Year6Cu).IsOptional ();

            Property (m => m.PracticeType1Cu).IsOptional ();
            Property (m => m.PracticeType2Cu).IsOptional ();
            Property (m => m.PracticeType3Cu).IsOptional ();

            HasRequired (m => m.EduProgramProfileFormYear).WithOptional (x => x.EduVolume);
        }
    }
}