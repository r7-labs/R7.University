//
//  DivisionSelector.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;
using DotNetNuke.Web.UI.WebControls;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ControlExtensions;
using R7.University.Models;

namespace R7.University.Controls
{
    public enum DivisionSelectionMode { List = 0, Tree = 1 }

    public partial class DivisionSelector: UserControl
    {
        #region Control properties

        public DivisionSelectionMode DefaultMode { get; set; }

        public IEnumerable<DivisionInfo> DataSource { get; set; }

        public int? DivisionId
        {
            get {
                if (IsCurrentMode (DivisionSelectionMode.List)) {
                    return TypeUtils.ParseToNullable<int> (comboDivision.SelectedValue);
                }
                return TypeUtils.ParseToNullable<int> (treeDivision.SelectedValue);
            }
            set {
                if (IsCurrentMode (DivisionSelectionMode.List)) {
                    comboDivision.SelectByValue (value != null ? value.Value : Null.NullInteger);
                }
                else {
                    treeDivision.CollapseAllNodes ();
                    treeDivision.SelectAndExpandByValue (value != null ? value.Value.ToString () : Null.NullInteger.ToString ());
                }
            }
        }

        public string DivisionTitle
        {
            get {
                if (IsCurrentMode (DivisionSelectionMode.List)) {
                    return comboDivision.SelectedItem.Text;
                }
                return treeDivision.SelectedNode != null ? treeDivision.SelectedNode.Text : string.Empty;
            }
        }

        #endregion

        ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this, this.FindParentOfType<IModuleControl> ())); }
        }

        protected bool IsCurrentMode (DivisionSelectionMode mode)
        {
            return radioSelectionMode.SelectedIndex == (int) mode;
        }

        public override void DataBind ()
        {
            var notSelectedText = Localization.GetString ("NotSelected.Text", ViewModelContext.LocalResourceFile);

            comboDivision.DataSource = DataSource;
            comboDivision.DataBind ();
            comboDivision.InsertDefaultItem (notSelectedText);

            treeDivision.DataSource = DataSource;
            treeDivision.DataBind ();
            treeDivision.Nodes.Insert (0, new DnnTreeNode { Value = Null.NullInteger.ToString (), Text = notSelectedText });
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            radioSelectionMode.DataSource = EnumViewModel<DivisionSelectionMode>.GetValues (ViewModelContext, false);
            radioSelectionMode.DataBind ();
            radioSelectionMode.SelectedIndex = (int) DefaultMode;

            comboDivision.Visible = DefaultMode == DivisionSelectionMode.List;
            treeDivision.Visible = DefaultMode == DivisionSelectionMode.Tree;
        }

        protected void radioSelectionMode_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (IsCurrentMode (DivisionSelectionMode.List)) {
                comboDivision.Visible = true;
                comboDivision.SelectByValue (treeDivision.SelectedValue);
                treeDivision.Visible = false;
            }
            else {
                treeDivision.Visible = true;
                treeDivision.SelectAndExpandByValue (comboDivision.SelectedValue);
                comboDivision.Visible = false;
            }
        }
    }
}
