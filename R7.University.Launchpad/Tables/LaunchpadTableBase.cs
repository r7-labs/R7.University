//
//  LaunchpadTableBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015 Roman M. Yagodin
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
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public abstract class LaunchpadTableBase
    {
        #region Properties

        public string Name { get; protected set; }

        public string ResourceKey { get; protected set; }

        public string EditControlKey { get; protected set; }

        public string EditQueryKey { get; protected set; }

        public GridView Grid { get; protected set; }

        public virtual bool IsEditable
        { 
            get { return true; }
        }

        public Type EntityType { get; protected set; }

        #endregion

        protected LaunchpadTableBase (string name, Type entityType)
        {
            Name = name.ToLowerInvariant ();
            ResourceKey = name + ".Text";
            EntityType = entityType;

            // remove ending "s" and add "_id"
            var baseName = name + "\n";
            EditControlKey = "edit" + baseName.Replace ("s\n", string.Empty);
            EditQueryKey = baseName.Replace ("s\n", "_id");
        }

        public virtual void Init (PortalModuleBase module, GridView gridView, int pageSize)
        {
            Grid = gridView;
            Grid.PageSize = pageSize;
        }

        public virtual void DataBind (PortalModuleBase module, UniversityModelContext modelContext, string search = null)
        {
            Grid.DataSource = GetDataTable (module, modelContext, search);
            module.Session [Grid.ID] = Grid.DataSource;
            Grid.DataBind ();
        }

        public abstract DataTable GetDataTable (PortalModuleBase module, UniversityModelContext modelContext, string search);

        public virtual string GetAddUrl (PortalModuleBase module)
        {
            return module.EditUrl (EditControlKey);
        }

        public virtual string GetEditUrl (PortalModuleBase module, string id)
        {
            return module.EditUrl (EditQueryKey, id, EditControlKey);
        }

        public virtual ModuleAction GetAction (PortalModuleBase module)
        {
            // ModuleActionCollection is created before OnInit,
            // so need to pass module reference to this method

            return new ModuleAction (
                module.GetNextActionID (),
                // TODO: Action labels require localization
                EditControlKey.Replace ("edit", "Add "),
                ModuleActionType.AddContent,
                "",
                IconController.IconURL ("Add"),
                module.EditUrl (EditControlKey),
                "",
                false, 
                SecurityAccessLevel.Edit,
                true, false
            );
        }
    }
}
