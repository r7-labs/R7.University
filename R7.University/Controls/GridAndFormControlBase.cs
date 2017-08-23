//
//  GridAndFormControlBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ControlExtensions;
using R7.University.Controls.ViewModels;
using R7.University.Models;
using R7.University.Utilities;
using DnnWebUiUtilities = DotNetNuke.Web.UI.Utilities;

namespace R7.University.Controls
{
    public abstract class GridAndFormControlBase<TModel,TViewModel>: UserControl
        where TViewModel: class, IEditControlViewModel<TModel>, new()
    {
        #region Controls

        protected GridView GridItems;

        protected HiddenField HiddenViewItemID;

        protected LinkButton ButtonAddItem;

        protected LinkButton ButtonUpdateItem;

        protected LinkButton ButtonCancelEditItem;

        protected LinkButton ButtonResetForm;

        #endregion

        #region Extension points

        protected abstract void OnLoadItem (TViewModel item);

        protected abstract void OnUpdateItem (TViewModel item);

        /// <summary>
        /// Called when user clicks the cancel edit button.
        /// Override this to fix wierd behavior of controls like DnnUrlControl which loose its state on postback.
        /// </summary>
        /// <param name="item">Item.</param>
        protected virtual void OnCancelEdit (TViewModel item)
        {
        }

        /// <summary>
        /// Called when form initialized and when user clicks the reset form button.
        /// Implementation must contain code to reset form to default values.
        /// </summary>
        protected abstract void OnResetForm ();

        protected abstract void OnInitControls ();

        #endregion

        protected void InitControls (GridView gridItems, HiddenField hiddenViewItemId, LinkButton buttonAddItem, 
                                     LinkButton buttonUpdateItem, LinkButton buttonCancelEditItem, LinkButton buttonResetForm)
        {
            GridItems = gridItems;
            HiddenViewItemID = hiddenViewItemId;
            ButtonAddItem = buttonAddItem;
            ButtonUpdateItem = buttonUpdateItem;
            ButtonCancelEditItem = buttonCancelEditItem;
            ButtonResetForm = buttonResetForm;
        }

        #region Bindable icons

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        protected string DeleteIconUrl
        {
            get { return IconController.IconURL ("Delete"); }
        }

        protected string UndeleteIconUrl
        {
            get { return "~/DesktopModules/MVC/R7.University/R7.University/images/Rollback_16x16_Standard.png"; }
        }

        #endregion

        string _localResourceFile;
        public string LocalResourceFile
        {
            get { return _localResourceFile ?? (_localResourceFile = DnnWebUiUtilities.GetLocalResourceFile (this)); }
        }

        protected string LocalizeString (string value)
        {
            return Localization.GetString (value, LocalResourceFile);
        }

        #region Properties to init

        protected PortalModuleBase Module;

        protected int TargetItemId
        {
            get { 
                var targetItemId = ViewState ["targetItemId"];
                return (targetItemId != null) ? (int) targetItemId : 0;
            }
            set { ViewState ["targetItemId"] = value; }
        }

        protected virtual string TargetItemKey {
            get { return string.Empty; }
        }
   
        #endregion

        #region Set and get data

        [Obsolete]
        public virtual List<TModel> GetData ()
        {
            var items = ViewStateItems;
            if (items != null) {
                return items.Select (i => i.CreateModel ()).ToList ();
            }

            return new List<TModel> ();
        }

        public virtual IEnumerable<TViewModel> GetModifiedData ()
        {
            var items = ViewStateItems;
            if (items != null) {
                // TODO: Remove ToList()?
                return items.Where (i => i.EditState != ModelEditState.Unchanged).ToList ();
            }

            return new List<TViewModel> ();
        }

        public virtual void SetData (IEnumerable<TModel> items, int targetItemId)
        {
            TargetItemId = targetItemId;

            var convertor = new TViewModel ();
            var viewModels = items.Select (i => DebugEnsureCreatedProperly ((TViewModel) convertor.Create (i, ViewModelContext))).ToList ();
            ViewStateItems = viewModels;

            GridItems.DataSource = viewModels;
            GridItems.DataBind ();
        }

        #endregion

        TViewModel DebugEnsureCreatedProperly (TViewModel viewModel)
        {
            Debug.Assert (viewModel != null);
            Debug.Assert (viewModel.Context != null);

            return viewModel;
        }

        ViewModelContext _viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return _viewModelContext ?? (_viewModelContext = new ViewModelContext (this, Module)); }
        }

        protected List<TViewModel> ViewStateItems
        {
            get { return XmlSerializationHelper.Deserialize<List<TViewModel>> (ViewState ["items"]); }
            set { ViewState ["items"] = XmlSerializationHelper.Serialize<List<TViewModel>> (value); }
        }

        #region Overrides

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            OnInitControls ();

            // wireup handlers
            ButtonAddItem.Command += OnUpdateItemCommand;
            ButtonUpdateItem.Command += OnUpdateItemCommand;
            ButtonCancelEditItem.Click += OnCancelEditItemClick;
            ButtonResetForm.Click += OnResetFormClick;
            GridItems.RowDataBound += OnGridItemsRowDataBound;

            // localize gridview columns
            GridItems.LocalizeColumns (LocalResourceFile);

            OnResetForm ();
            SwitchToAddMode ();
        }

        #endregion

        #region Handlers

        protected void OnUpdateItemCommand (object sender, CommandEventArgs e)
        {
            try {
                if (!Module.Page.IsValid) {
                    return;
                }

                TViewModel item;

                // get item list from viewstate
                var items = ViewStateItems;
                if (items == null) {
                    items = new List<TViewModel> ();
                }

                var command = e.CommandArgument.ToString ();
                if (command == "Add") {
                    item = new TViewModel ();
                    item.EditState = ModelEditState.Added;
                }
                else {
                    // restore ItemID from hidden field
                    var hiddenViewItemId = int.Parse (HiddenViewItemID.Value);
                    item = items.Find (i => i.ViewItemID == hiddenViewItemId);

                    if (item.EditState != ModelEditState.Added) {
                        item.EditState = ModelEditState.Modified;
                    }
                }

                OnUpdateItem (item);

                if (command == "Add") {
                    item.SetTargetItemId (TargetItemId, TargetItemKey);
                    items.Add (item);
                }

                SwitchToAddMode ();

                // refresh viewstate
                ViewStateItems = items;

                // rebind viewmodels to the context
                foreach (var form in items) {
                    form.Context = ViewModelContext;
                }

                // bind items to the gridview
                GridItems.DataSource = items;
                GridItems.DataBind ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnCancelEditItemClick (object sender, EventArgs e)
        {
            try {
                var item = ViewStateItems.FirstOrDefault (i => i.ViewItemID.ToString () == HiddenViewItemID.Value);
                if (item != null) {
                    OnCancelEdit (item);
                }

                SwitchToAddMode ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnResetFormClick (object sender, EventArgs e)
        {
            try {
                OnResetForm ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnGridItemsRowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hide ViewItemID column, also in header
            e.Row.Cells [1].Visible = false;

            // exclude header
            if (e.Row.RowType == DataControlRowType.DataRow) {
                // find edit and delete linkbuttons
                var linkDelete = e.Row.Cells [0].FindControl ("linkDelete") as LinkButton;
                var linkUndelete = e.Row.Cells [0].FindControl ("linkUndelete") as LinkButton;
                var linkEdit = e.Row.Cells [0].FindControl ("linkEdit") as LinkButton;
                var labelEditMarker = (Label) e.Row.Cells [0].FindControl ("labelEditMarker");

                // set itemId
                linkEdit.CommandArgument = e.Row.Cells [1].Text;
                linkDelete.CommandArgument = e.Row.Cells [1].Text;
                linkUndelete.CommandArgument = e.Row.Cells [1].Text;

                var item = (IEditControlViewModel<TModel>) e.Row.DataItem;

                if (item.EditState == ModelEditState.Deleted) {
                    linkDelete.Visible = false;
                    linkUndelete.Visible = true;
                }
                else {
                    linkUndelete.Visible = false;
                }

                labelEditMarker.CssClass = "u8y-edit-marker " + item.CssClass;
            }
        }

        protected void OnEditItemCommand (object sender, CommandEventArgs e)
        {
            try {
                var items = ViewStateItems;
                if (items != null) {
                    var itemId = e.CommandArgument.ToString ();

                    // find item in the list
                    var item = items.Find (i => i.ViewItemID.ToString () == itemId);
                    if (item != null) {
                        // fill form
                        OnLoadItem (item);

                        // store ViewItemID in the hidden field
                        HiddenViewItemID.Value = item.ViewItemID.ToString ();

                        SwitchToUpdateMode ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnDeleteItemCommand (object sender, CommandEventArgs e)
        {
            try {
                var items = ViewStateItems;
                if (items != null) {
                    var itemId = e.CommandArgument.ToString ();

                    // find position in a list
                    var itemIndex = items.FindIndex (i => i.ViewItemID.ToString () == itemId);

                    if (itemIndex >= 0) {
                        // remove item
                        var item = items [itemIndex];
                        if (item.EditState != ModelEditState.Added) {
                            item.EditState = ModelEditState.Deleted;    
                        }
                        else {
                            items.RemoveAt (itemIndex);
                        }

                        // refresh viewstate
                        ViewStateItems = items;

                        // rebind viewmodels to context
                        foreach (var form in items) {
                            form.Context = ViewModelContext;
                        }

                        // bind items to the gridview
                        GridItems.DataSource = items;
                        GridItems.DataBind ();

                        // return to Add mode if we deleting currently edited item
                        if (ButtonUpdateItem.Visible && HiddenViewItemID.Value == itemId) {
                            SwitchToAddMode ();
                        }
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnUndeleteItemCommand (object sender, CommandEventArgs e)
        {
            try {
                var items = ViewStateItems;
                if (items != null) {
                    var itemId = e.CommandArgument.ToString ();
                    var item = items.Find (i => i.ViewItemID.ToString () == itemId);
                    if (item != null) {

                        // restore previous edit state
                        item.RestoreEditState ();
                  
                        // refresh viewstate
                        ViewStateItems = items;

                        // rebind viewmodels to context
                        foreach (var form in items) {
                            form.Context = ViewModelContext;
                        }

                        // bind items to the gridview
                        GridItems.DataSource = items;
                        GridItems.DataBind ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        #endregion

        void SwitchToAddMode ()
        {
            ButtonAddItem.Visible = true;
            ButtonCancelEditItem.Visible = false;
            ButtonUpdateItem.Visible = false;
        }

        void SwitchToUpdateMode ()
        {
            ButtonAddItem.Visible = false;
            ButtonCancelEditItem.Visible = true;
            ButtonUpdateItem.Visible = true;
        }
    }
}

