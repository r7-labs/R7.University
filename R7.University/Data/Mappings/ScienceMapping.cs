//
//  ScienceMapping.cs
//
//  Author:
//       redhound <>
//
//  Copyright (c) 2017 redhound
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
    public class ScienceMapping:  EntityTypeConfiguration<ScienceInfo>
    {
        public ScienceMapping ()
        {
            ToTable (UniversityMappingHelper.GetTableName<ScienceInfo> (pluralize: false));

            HasKey (m => m.ScienceId);

            Property (m => m.Directions).IsOptional ();
            Property (m => m.Base).IsOptional ();
            Property (m => m.Scientists).IsOptional ();
            Property (m => m.Students).IsOptional ();
            Property (m => m.Monographs).IsOptional ();
            Property (m => m.Articles).IsOptional ();
            Property (m => m.ArticlesForeign).IsOptional ();
            Property (m => m.Patents).IsOptional ();
            Property (m => m.PatentsForeign).IsOptional ();
            Property (m => m.Certificates).IsOptional ();
            Property (m => m.CertificatesForeign).IsOptional ();
            Property (m => m.FinancingByScientist).IsOptional ();

            HasRequired (m => m.EduProgram).WithOptional (x => x.Science);
        }
    }
}
