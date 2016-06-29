//
//  OccupiedPositionInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using DotNetNuke.ComponentModel.DataAnnotations;
using R7.University.Models;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace R7.University.Data
{
    // TODO: Add Unique constraint to OccupiedPositions table FK's
    [TableName ("University_OccupiedPositions")]
    [PrimaryKey ("OccupiedPositionID", AutoIncrement = true)]
    public class OccupiedPositionInfo: IOccupiedPosition
    {
        #region IOccupiedPosition implementation

        public int OccupiedPositionID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        [IgnoreColumn]
        public PositionInfo Position { get; set; }

        [IgnoreColumn]
        public DivisionInfo Division { get; set; }

        [IgnoreColumn]
        public EmployeeInfo Employee { get; set; }

        #endregion
    }

    public class OccupiedPositionMapping: EntityTypeConfiguration<OccupiedPositionInfo>
    {
        public OccupiedPositionMapping ()
        {
            HasKey (m => m.OccupiedPositionID);
            Property (m => m.OccupiedPositionID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);

            Property (m => m.PositionID).IsRequired ();
            Property (m => m.DivisionID).IsRequired ();
            Property (m => m.EmployeeID).IsRequired ();

            HasRequired<PositionInfo> (m => m.Position).WithMany ().HasForeignKey (m => m.PositionID).WillCascadeOnDelete (true);
            HasRequired<DivisionInfo> (m => m.Division).WithMany ().HasForeignKey (m => m.DivisionID).WillCascadeOnDelete (true);
            HasRequired<EmployeeInfo> (m => m.Employee).WithMany ().HasForeignKey (m => m.EmployeeID).WillCascadeOnDelete (true);

            Property (m => m.IsPrime);
            Property (m => m.TitleSuffix);
        }
    }
}

