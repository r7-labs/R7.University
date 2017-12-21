//
//  ContingentMapping.cs
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
    public class ContingentMapping: EntityTypeConfiguration<ContingentInfo>
    {
        public ContingentMapping ()
        {
            ToTable (UniversityMappingHelper.GetTableName<ContingentInfo> (pluralize: false));
            HasKey (m => m.ContingentId);

            Property (m => m.AvgAdmScore).IsOptional ();

            Property (m => m.AdmittedFB).IsOptional ();
            Property (m => m.AdmittedRB).IsOptional ();
            Property (m => m.AdmittedMB).IsOptional ();
            Property (m => m.AdmittedBC).IsOptional ();

            Property (m => m.ActualFB).IsOptional ();
            Property (m => m.ActualRB).IsOptional ();
            Property (m => m.ActualMB).IsOptional ();
            Property (m => m.ActualBC).IsOptional ();

            Property (m => m.VacantFB).IsOptional ();
            Property (m => m.VacantRB).IsOptional ();
            Property (m => m.VacantMB).IsOptional ();
            Property (m => m.VacantBC).IsOptional ();

            Property (m => m.MovedIn).IsOptional ();
            Property (m => m.MovedOut).IsOptional ();
            Property (m => m.Restored).IsOptional ();
            Property (m => m.Expelled).IsOptional ();

            HasRequired (m => m.EduProgramProfileFormYear).WithOptional (x => x.Contingent);
        }
    }
}