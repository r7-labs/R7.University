//
//  EduProgramProfileFormMapping.cs
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

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using R7.University.Models;

namespace R7.University.Data.Mappings
{
    public class EduProgramProfileFormMapping: EntityTypeConfiguration<EduProgramProfileFormInfo>
    {
        public EduProgramProfileFormMapping ()
        {
            HasKey (m => m.EduProgramProfileFormID);
            Property (m => m.EduProgramProfileFormID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
            Property (m => m.EduProgramProfileID).IsRequired ();
            Property (m => m.EduFormID).IsRequired ();
            Property (m => m.TimeToLearn).IsRequired ();
            Property (m => m.TimeToLearnUnit).IsRequired ();
            Property (m => m.IsAdmissive).IsRequired ();

            HasRequired (m => m.EduForm).WithMany ().HasForeignKey (m => m.EduFormID);
        }
    }
}

