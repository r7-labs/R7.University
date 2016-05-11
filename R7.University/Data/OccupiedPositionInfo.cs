//
// OccupiedPositionInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University.Data
{
    // TODO: Add Unique constraint to OccupiedPositions table FK's

    // More attributes for class:
    // Set caching for table: [Cacheable("R7.University_OccupiedPositions", CacheItemPriority.Default, 20)]
    // Explicit mapping declaration: [DeclareColumns]
    // More attributes for class properties:
    // Custom column name: [ColumnName("OccupiedPositionID")]
    // Explicit include column: [IncludeColumn]
    // Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
    [TableName ("University_OccupiedPositions")]
    [PrimaryKey ("OccupiedPositionID", AutoIncrement = true)]
    public class OccupiedPositionInfo
    {
        #region Fields

        #endregion

        /// <summary>
        /// Empty default cstor
        /// </summary>
        public OccupiedPositionInfo ()
        {
        }

	
        #region Properties

        public int OccupiedPositionID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        #endregion
    }
}

