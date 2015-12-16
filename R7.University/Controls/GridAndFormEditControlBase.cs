//
// GridAndFormEditControlBase.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
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
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.University.ControlExtensions;

namespace R7.University.Controls
{
    public abstract class GridAndFormEditControlBase<TModel,TViewModel>: UserControl
        where TViewModel: class, IEditControlViewModel<TModel>, new ()
    {
        #region Controls

        protected GridView GridItems;
        protected HiddenField HiddenViewItemID;
        protected LinkButton ButtonAddItem;
        protected LinkButton ButtonUpdateItem;
        protected LinkButton ButtonCancelEditItem;

        #endregion

        #region Extension points

        protected abstract void OnLoadItem (TViewModel item);

        protected abstract void OnUpdateItem (TViewModel item);

        protected abstract void OnResetForm ();

        protected abstract void OnInitControls ();

        #endregion

        protected void InitControls (GridView gridItems, HiddenField hiddenViewItemId, LinkButton buttonAddItem, 
            LinkButton buttonUpdateItem, LinkButton buttonCancelEditItem)
        {
            GridItems = gridItems;
            HiddenViewItemID = hiddenViewItemId;
            ButtonAddItem = buttonAddItem;
            ButtonUpdateItem = buttonUpdateItem;
            ButtonCancelEditItem = buttonCancelEditItem;
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

        #endregion

        private string localResourceFile;
        public string LocalResourceFile
        {
            get
            {
                if (localResourceFile == null)
                {
                    localResourceFile = DotNetNuke.Web.UI.Utilities.GetLocalResourceFile (this);
                }

                return localResourceFile;
            }
        }

        #region Properties to init

        protected PortalModuleBase Module;

        protected int ItemId
        {
            get 
            { 
                var itemId = ViewState ["itemId"];
                return (itemId != null) ? (int) itemId : 0;
            }
            set { ViewState ["itemId"] = value; }
        }

        #endregion

        #region Set and get data

        public List<TModel> GetData ()
        {
            var items = ViewStateItems;
            if (items != null)
            {
                return items.Select (i => i.ToModel ()).ToList ();
            }

            return new List<TModel> ();
        }

        public void SetData (List<TModel> items, int itemId)
        {
            ItemId = itemId;
            var convertor = new TViewModel ();
            var viewModels = items.Select (i => (TViewModel) convertor.FromModel (i, ViewModelContext)).ToList ();
            ViewStateItems = viewModels;

            GridItems.DataSource = DataTableConstructor.FromIEnumerable (viewModels);
            GridItems.DataBind ();
        }

        #endregion

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this, Module)); }
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
            GridItems.RowDataBound += OnGridItemsRowDataBound;

            // localize gridview columns
            GridItems.LocalizeColumns (LocalResourceFile);
        }

        #endregion

        #region Handlers

        protected void OnUpdateItemCommand (object sender, CommandEventArgs e)
        {
            try
            {
                TViewModel item;

                // get item list from viewstate
                var items = ViewStateItems;
                if (items == null)
                {
                    items = new List<TViewModel> ();
                }

                var command = e.CommandArgument.ToString ();
                if (command == "Add")
                {
                    item = new TViewModel ();
                }
                else
                {
                    // restore ItemID from hidden field
                    var hiddenViewItemId = int.Parse (HiddenViewItemID.Value);
                    item = items.Find (i => i.ViewItemID == hiddenViewItemId);
                }

                OnUpdateItem (item);

                if (command == "Add")
                {
                    item.TargetItemID = ItemId;
                    items.Add (item);
                }

                ResetForm ();

                // refresh viewstate
                ViewStateItems = items;

                // rebind viewmodels to the context
                foreach (var form in items)
                {
                    form.Context = ViewModelContext;
                }

                // bind items to the gridview
                GridItems.DataSource = DataTableConstructor.FromIEnumerable (items);
                GridItems.DataBind ();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnCancelEditItemClick (object sender, EventArgs e)
        {
            try
            {
                ResetForm ();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        void ResetForm ()
        {
            // restore default buttons visibility
            ButtonAddItem.Visible = true;
            ButtonUpdateItem.Visible = false;

            OnResetForm ();
        }

        protected void OnGridItemsRowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hide ViewItemID column, also in header
            e.Row.Cells [1].Visible = false;

            // exclude header
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // find edit and delete linkbuttons
                var linkDelete = e.Row.Cells [0].FindControl ("linkDelete") as LinkButton;
                var linkEdit = e.Row.Cells [0].FindControl ("linkEdit") as LinkButton;

                // set record Id to delete
                linkEdit.CommandArgument = e.Row.Cells [1].Text;
                linkDelete.CommandArgument = e.Row.Cells [1].Text;

                // add confirmation dialog to delete link
                linkDelete.Attributes.Add ("onClick", "javascript:return confirm('" 
                    + Localization.GetString ("DeleteItem") + "');");
            }
        }

        protected void OnEditItemCommand (object sender, CommandEventArgs e)
        {
            try
            {
                var items = ViewStateItems;
                if (items != null)
                {
                    var itemId = e.CommandArgument.ToString ();

                    // find item in the list
                    var item = items.Find (i => i.ViewItemID.ToString () == itemId);
                    if (item != null)
                    {
                        // fill form
                        OnLoadItem (item);

                        // store ViewItemID in the hidden field
                        HiddenViewItemID.Value = item.ViewItemID.ToString ();

                        // show / hide buttons
                        ButtonAddItem.Visible = false;
                        ButtonUpdateItem.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void OnDeleteItemCommand (object sender, CommandEventArgs e)
        {
            try
            {
                var items = ViewStateItems;
                if (items != null)
                {
                    var itemId = e.CommandArgument.ToString ();

                    // find position in a list
                    var itemIndex = items.FindIndex (i => i.ViewItemID.ToString () == itemId);

                    if (itemIndex >= 0)
                    {
                        // remove item
                        items.RemoveAt (itemIndex);

                        // refresh viewstate
                        ViewStateItems = items;

                        // rebind viewmodels to context
                        foreach (var form in items)
                        {
                            form.Context = ViewModelContext;
                        }

                        // bind items to the gridview
                        GridItems.DataSource = DataTableConstructor.FromIEnumerable (items);
                        GridItems.DataBind ();

                        // reset form if we deleting currently edited item
                        if (ButtonUpdateItem.Visible && HiddenViewItemID.Value == itemId)
                        {
                            ResetForm ();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        #endregion
    }
}

