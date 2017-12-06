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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.Dnn.Extensions.Utilities;
using R7.University.Commands;
using R7.University.Components;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.Science.Queries;

namespace R7.University.Science
{
    public partial class EditScience: UniversityEditPortalModuleBase<EduProgramInfo>, IActionable
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

        protected override bool CanDeleteItem (EduProgramInfo item)
        {
            return false;
        }

        #region UniversityEditPortalModuleBase implementation

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (EduProgramInfo item)
        {
            formEditScienceRecords.SetData (item.ScienceRecords, item.EduProgramID);
        }

        protected override void BeforeUpdateItem (EduProgramInfo item)
        {
        }

        protected override EduProgramInfo GetItemWithDependencies (int itemId)
        {
            return new EduProgramScienceQuery (ModelContext).SingleOrDefault (itemId);
        }

        protected override void UpdateItem (EduProgramInfo item)
        {
            var scienceRecords = formEditScienceRecords.GetModifiedData ();
            new UpdateScienceRecordsCommand (ModelContext).Update (scienceRecords, item.EduProgramID);
            ModelContext.SaveChanges (true);
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

        #region IActionable implementation

        public ModuleActionCollection ModuleActions {
            get {
                var actions = new ModuleActionCollection ();
                var itemId = TypeUtils.ParseToNullable<int> (Request.QueryString [Key]);
                if (itemId != null) {
                    actions.Add (new ModuleAction (GetNextActionID ()) {
                        Title = LocalizeString ("EditEduProgram.Action"),
                        CommandName = ModuleActionType.EditContent,
                        Icon = UniversityIcons.Edit,
                        Secure = SecurityAccessLevel.Edit,
                        Url = EditUrl ("eduprogram_id", itemId.ToString (), "EditEduProgram"),
                        Visible = SecurityContext.IsAdmin
                    });
                }
                return actions;
            }
        }

        #endregion
    }
}
