//
//  UpdateEduProgramProfileFormsCommand.cs
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
using R7.University.Components;
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

        public void UpdateEduProgramProfileForms (IList<EduProgramProfileFormInfo> eduProgramProfileForms, int eduProgramProfileId)
        {
            var originalEduProgramProfileForms = ModelContext.Query<EduProgramProfileFormInfo> ()
                .Where (eppf => eppf.EduProgramProfileID == eduProgramProfileId)
                .ToList ();
            
            foreach (var eppf in eduProgramProfileForms) {
                var oeppf = originalEduProgramProfileForms.SingleOrDefault (_eppf => _eppf.EduProgramProfileFormID == eppf.EduProgramProfileFormID);
                if (oeppf == null) {
                    eppf.EduProgramProfileID = eduProgramProfileId;
                    ModelContext.Add<EduProgramProfileFormInfo> (eppf);
                }
                else {
                    CopyCstor.Copy<EduProgramProfileFormInfo> (eppf, oeppf);
                    ModelContext.Update<EduProgramProfileFormInfo> (oeppf);

                    // do not delete this document later
                    originalEduProgramProfileForms.Remove (oeppf);
                }
            }

            // should delete all remaining edu. forms
            foreach (var oeppf in originalEduProgramProfileForms) {
                ModelContext.Remove<EduProgramProfileFormInfo> (oeppf);
            }
        }
    }
}

