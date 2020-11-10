//
//  EditEduProgramProfileDocuments.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018-2020 Roman M. Yagodin
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
using R7.Dnn.Extensions.Text;
using R7.University.Commands;
using R7.University.Components;
using R7.University.EduProgramProfiles.Queries;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;

namespace R7.University.EduProgramProfiles
{
    public partial class EditEduProgramProfileDocuments: UniversityEditPortalModuleBase<EduProgramProfileInfo>, IActionable
    {
        protected EditEduProgramProfileDocuments () : base ("eduprogramprofile_id")
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            formEditDocuments.OnInit (this, new FlatQuery<DocumentTypeInfo> (ModelContext).List ());
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (EduProgramProfileInfo item)
        {
            if (item != null) {
                return $"{item.FormatTitle ()} : {item.EduLevel.Title}";
            }
            return null;
        }

        protected override void LoadItem (EduProgramProfileInfo item)
        {
            var epp = GetItemWithDependencies (item.EduProgramProfileID);
            base.LoadItem (epp);

            formEditDocuments.SetData (epp.Documents, epp.EduProgramProfileID);
        }

        protected override void BeforeUpdateItem (EduProgramProfileInfo item, bool isNew)
        {
            item.LastModifiedOnDate = DateTime.Now;
            item.LastModifiedByUserId = UserInfo.UserID;
        }

        protected EduProgramProfileInfo GetItemWithDependencies (int itemId)
        {
            return new EduProgramProfileEditQuery (ModelContext).SingleOrDefault (itemId);
        }

        protected override bool CanDeleteItem (EduProgramProfileInfo item) => false;

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EduProgramProfileInfo item) => item.EduProgramProfileID;

        protected override void AddItem (EduProgramProfileInfo item)
        {
            throw new InvalidOperationException ();
        }

        protected override void UpdateItem (EduProgramProfileInfo item)
        {
            ModelContext.Update (item);

            new UpdateDocumentsCommand (ModelContext)
                .Update (formEditDocuments.GetModifiedData (), ModelType.EduProgramProfile, item.EduProgramProfileID, UserId);

            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduProgramProfileInfo item)
        {
            throw new InvalidOperationException ();
        }

        #endregion

        IEduProfile GetEduProgramProfile ()
        {
        	var eppId = ParseHelper.ParseToNullable<int> (Request.QueryString [Key]);
        	return eppId != null ? GetItemWithDependencies (eppId.Value) : null;
        }

        #region IActionable implementation

        public ModuleActionCollection ModuleActions {
            get {
                var actions = new ModuleActionCollection ();
                var epp = GetEduProgramProfile ();
                if (epp != null) {
                    actions.Add (new ModuleAction (GetNextActionID ()) {
                        Title = LocalizeString ("EditEduProgramProfile.Action"),
                        CommandName = ModuleActionType.EditContent,
                        Icon = UniversityIcons.Edit,
                        Secure = SecurityAccessLevel.Edit,
                        Url = EditUrl ("eduprogramprofile_id", epp.EduProgramProfileID.ToString (), "EditEduProgramProfile"),
                        Visible = SecurityContext.IsAdmin
                    });
                }
                return actions;
            }
        }

        #endregion
    }
}
