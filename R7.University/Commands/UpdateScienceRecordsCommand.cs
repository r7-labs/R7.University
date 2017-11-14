//
//  UpdateScienceRecordsCommand.cs
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

using System.Collections.Generic;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Commands
{
    public class UpdateScienceRecordsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateScienceRecordsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void Update (IEnumerable<IEditModel<ScienceRecordInfo>> sRecs, int eduProgramId)
        {
            foreach (var sRec in sRecs) {
                var sr = sRec.CreateModel ();
                switch (sRec.EditState) {
                    case ModelEditState.Added:
                        sr.EduProgramId = eduProgramId;
                        ModelContext.Add (sr);
                        break;
                    case ModelEditState.Modified:
                        ModelContext.UpdateExternal (sr);
                        break;
                    case ModelEditState.Deleted:
                        ModelContext.RemoveExternal (sr);
                        break;
                }
            }
        }
    }
}