//
//  DocumentSelector.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;
using R7.University.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using System.Web.UI.WebControls;
using R7.University.Queries;

namespace R7.University.Controls
{
    public enum DivisionSelectionMode { List, Tree }

    public partial class DocumentSelector: UserControl
    {
        #region Control properties

        public PortalModuleBase Module { get; set; }

        public IEnumerable<DivisionInfo> Divisions { get; set; }

        #endregion

        protected List<TreeNode> GetDivisionNodes (IEnumerable<DivisionInfo> divisions)
        {
            var nodes = new List<TreeNode> ();
            foreach (var division in divisions) {
                nodes.Add (new TreeNode (division.Title, division.DivisionID.ToString ()));
            }

            return nodes;
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            radioSelectionMode.DataSource = EnumViewModel<DivisionSelectionMode>.GetValues (new ViewModelContext (this, Module), false);
            radioSelectionMode.DataBind ();

            treeDivision.DataSource = GetDivisionNodes (Divisions);
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
        }
    }
}
