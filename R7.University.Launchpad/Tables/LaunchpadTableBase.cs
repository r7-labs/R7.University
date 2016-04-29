//
// LaunchpadTableBase.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;
using System.Data;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;

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

        #endregion

        protected LaunchpadTableBase (string name)
        {
            Name = name.ToLowerInvariant ();
            ResourceKey = name + ".Text";

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

        public virtual void DataBind (PortalModuleBase module, string search = null)
        {
            Grid.DataSource = GetDataTable (module, search);
            module.Session [Grid.ID] = Grid.DataSource;
            Grid.DataBind ();
        }

        public abstract DataTable GetDataTable (PortalModuleBase module, string search);

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
                EditControlKey.Replace ("Edit", "Add "),
                ModuleActionType.AddContent, 
                "", "",
                module.EditUrl (EditControlKey),
                "",
                false, 
                SecurityAccessLevel.Edit,
                true, false
            );
        }
    }
}
