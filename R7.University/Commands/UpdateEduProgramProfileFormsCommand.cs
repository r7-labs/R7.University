//
//  UpdateEduFormsCommand.cs
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
using R7.University.Models;

namespace R7.University.Commands
{
    public class UpdateEduFormsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateEduFormsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void UpdateEduForms (IList<EduProgramProfileFormInfo> eduForms, int eduProgramProfileId)
        {
            var originalEduForms = ModelContext.Query<EduProgramProfileFormInfo> ()
                .Where (eppf => eppf.EduProgramProfileID == eduProgramProfileId)
                .ToList ();
            
            foreach (var eduForm in eduForms) {
                var originalEduForm = originalEduForms.SingleOrDefault (eppf => eppf.EduProgramProfileFormID == eduForm.EduProgramProfileFormID);
                if (originalEduForm == null) {
                    eduForm.EduProgramProfileID = eduProgramProfileId;
                    ModelContext.Add<EduProgramProfileFormInfo> (eduForm);
                }
                else {
                    ModelContext.Update<EduProgramProfileFormInfo> (originalEduForm);

                    // do not delete this document later
                    originalEduForms.Remove (originalEduForm);
                }
            }

            // should delete all remaining edu. forms
            foreach (var originalEduForm in originalEduForms) {
                ModelContext.Remove<EduProgramProfileFormInfo> (originalEduForm);
            }
        }
    }
}

