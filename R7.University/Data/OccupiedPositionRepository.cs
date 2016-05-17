//
// OccupiedPositionRepository.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Data
{
    public class OccupiedPositionRepository
    {
        #region Singleton implementation

        private static readonly Lazy<OccupiedPositionRepository> instance = new Lazy<OccupiedPositionRepository> ();

        public static OccupiedPositionRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        private Dal2DataProvider dataProvider;

        public Dal2DataProvider DataProvider
        {
            get { return dataProvider ?? (dataProvider = new Dal2DataProvider ()); }
        }

        public IEnumerable<OccupiedPositionInfoEx> GetOccupiedPositions (int employeeId)
        {
            return DataProvider.GetObjects<OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0", employeeId)
                .OrderByDescending (opx => opx.IsPrime)
                .ThenByDescending (opx => opx.PositionWeight);
        }

        public IEnumerable<OccupiedPositionInfoEx> GetOccupiedPositions_ForEmployees (IEnumerable<int> employeeIds, int divisionId)
        {
            var strEmployeeIds = TextUtils.FormatList (", ", employeeIds);

            // TODO: Use {databaseOwner} and {objectQualifier} 
            // current division positions go first, then checks IsPrime, then PositionWeight
            // add "AND [DivisionID] = @1" to display employee positions only from current division
            return DataProvider.GetObjects<OccupiedPositionInfoEx> (string.Format ("WHERE [EmployeeID] IN ({0}) "
                + "ORDER BY (CASE WHEN [DivisionID]={1} THEN 0 ELSE 1 END), [IsPrime] DESC, [PositionWeight] DESC", 
                    strEmployeeIds, divisionId)
            );
        }
    }
}

