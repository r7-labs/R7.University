//
//  EduProgramMapping.cs
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
    public class EduProgramMapping: EntityTypeConfiguration<EduProgramInfo>
    {
        public EduProgramMapping ()
        {
            HasKey (m => m.EduProgramID);
            Property (m => m.EduProgramID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
            Property (m => m.EduLevelID).IsRequired ();
            Property (m => m.DivisionId).IsOptional ();
            Property (m => m.Code).IsRequired ();
            Property (m => m.Title).IsRequired ();
            Property (m => m.Generation).IsOptional ();
            Property (m => m.HomePage).IsOptional ();

            Property (m => m.StartDate).IsOptional ();
            Property (m => m.EndDate).IsOptional ();

            Property (m => m.LastModifiedByUserID);
            Property (m => m.LastModifiedOnDate);
            Property (m => m.CreatedByUserID);
            Property (m => m.CreatedOnDate);

            HasRequired (m => m.EduLevel).WithMany ().HasForeignKey (m => m.EduLevelID);
            HasOptional (m => m.Division).WithMany ().HasForeignKey (m => m.DivisionId);
            HasMany (m => m.Documents).WithOptional ().HasForeignKey (x => x.EduProgramId);
            HasMany (m => m.EduProgramProfiles).WithRequired (epp => epp.EduProgram).HasForeignKey (epp => epp.EduProgramID);
        }
    }
}
