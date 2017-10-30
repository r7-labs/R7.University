//
//  EditScience.ascx.cs
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

using System;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;

namespace R7.University.Science
{
    public partial class EditScience: UniversityEditPortalModuleBase<EduProgramInfo>
    {
        protected EditScience () : base ("eduprogram_id")
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            var scienceRecordTypes = new FlatQuery<ScienceRecordTypeInfo> (ModelContext).List ();
            formEditScienceRecords.OnInit (this, scienceRecordTypes);
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (EduProgramInfo item)
        {
            formEditScienceRecords.SetData (item.ScienceRecords, item.EduProgramID);

            buttonDelete.Visible = false;
        }

        protected override void BeforeUpdateItem (EduProgramInfo item)
        {
        }

        #region Implemented abstract members of UniverisityEditPortalModuleBase

        protected override void UpdateItem (EduProgramInfo item)
        {
            throw new NotImplementedException ();
        }

        protected override void AddItem (EduProgramInfo item)
        {
            throw new InvalidOperationException ();
        }

        protected override void DeleteItem (EduProgramInfo item)
        {
            throw new InvalidOperationException ();
        }

        #endregion
    }
}
