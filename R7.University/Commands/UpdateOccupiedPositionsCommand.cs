//
//  UpdateOccupiedPositionsCommand.cs
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
using System.Collections.Generic;
using System.Linq;
using R7.University.Components;
using R7.University.Models;

namespace R7.University.Commands
{
    public class UpdateOccupiedPositionsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateOccupiedPositionsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void UpdateOccupiedPositions (IList<OccupiedPositionInfo> occupiedPositions, int employeeId)
        {
            var originalOccupiedPositions = ModelContext.Query<OccupiedPositionInfo> ()
                .Where (op => op.EmployeeID == employeeId)
                .ToList ();
            
            foreach (var occupiedPosition in occupiedPositions) {
                var originalOccupiedPosition = originalOccupiedPositions.SingleOrDefault (op => op.OccupiedPositionID == occupiedPosition.OccupiedPositionID);
                if (originalOccupiedPosition == null) {
                    occupiedPosition.EmployeeID = employeeId;
                    ModelContext.Add<OccupiedPositionInfo> (occupiedPosition);
                }
                else {
                    CopyCstor.Copy<OccupiedPositionInfo> (occupiedPosition, originalOccupiedPosition);
                    ModelContext.Update<OccupiedPositionInfo> (originalOccupiedPosition);

                    // do not delete this document later
                    originalOccupiedPositions.Remove (originalOccupiedPosition);
                }
            }

            // should delete all remaining edu. forms
            foreach (var originalOccupiedPosition in originalOccupiedPositions) {
                ModelContext.Remove<OccupiedPositionInfo> (originalOccupiedPosition);
            }
        }
    }
}

