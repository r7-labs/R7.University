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

        public string EditKey { get; protected set; }

        public string EditQueryKey { get; protected set; }

        public LaunchpadPortalModuleBase Module { get; protected set; }

        public GridView Grid { get; protected set; }

        public HyperLink AddButton { get; protected set; }

        #endregion

        protected LaunchpadTableBase (string name)
        {
            Name = name;

            // remove ending "s" and add "_id"
            var baseName = name + "\n";
            EditKey = "edit" + baseName.Replace ("s\n", string.Empty);
            EditQueryKey = baseName.Replace ("s\n", "_id");
        }

        public virtual void Init (LaunchpadPortalModuleBase module, GridView gridView, HyperLink addButton, int pageSize)
        {
            Module = module;
            Grid = gridView;
            Grid.PageSize = pageSize;
            AddButton = addButton;
            AddButton.NavigateUrl = Utils.EditUrl (Module, EditKey);
        }

        public virtual void DataBind (string filter = null)
        {
            Grid.DataSource = GetDataTable (filter);
            Module.Session [Grid.ID] = Grid.DataSource;
            Grid.DataBind ();
        }

        public abstract DataTable GetDataTable (string filter);

        public virtual void SetEditLink (HyperLink link, string id)
        {
            link.NavigateUrl = Utils.EditUrl (Module, EditKey, EditQueryKey, id);
        }

        public virtual ModuleAction GetAction (PortalModuleBase module)
        {
            // ModuleActionCollection is created before OnInit,
            // so need to pass module reference to this method

            return new ModuleAction (
                module.GetNextActionID (),
                // TODO: Action labels require localization
                EditKey.Replace ("Edit", "Add "),
                ModuleActionType.AddContent, 
                "", "",
                Utils.EditUrl (module, EditKey),
                "",
                false, 
                SecurityAccessLevel.Edit,
                true, false
            );
        }
    }
}
