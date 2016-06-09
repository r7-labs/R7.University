//
//  OccupiedPositionRepository.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

        protected Dal2DataProvider DataProvider
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

