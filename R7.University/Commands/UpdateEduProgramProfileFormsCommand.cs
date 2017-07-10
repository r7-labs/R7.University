//
//  UpdateEduProgramProfileFormsCommand.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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
using R7.University.Controls.ViewModels;
using R7.University.Models;

namespace R7.University.Commands
{
    public class UpdateEduProgramProfileFormsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateEduProgramProfileFormsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void UpdateEduProgramProfileForms (IEnumerable<IEditControlViewModel<EduProgramProfileFormInfo>> eppForms, int eduProgramProfileId)
        {
            foreach (var eppForm in eppForms) {
                var eppf = eppForm.CreateModel ();
                switch (eppForm.EditState) {
                case ModelEditState.Added:
                    eppf.EduProgramProfileID = eduProgramProfileId;
                    ModelContext.Add (eppf);
                    break;
                case ModelEditState.Modified:
                    ModelContext.UpdateExternal (eppf);
                    break;
                case ModelEditState.Deleted:
                    ModelContext.RemoveExternal (eppf);
                    break;
                }
            }
        }
    }
}

