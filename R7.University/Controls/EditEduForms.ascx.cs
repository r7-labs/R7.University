//
// EditDocuments.ascx.cs
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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Localization;
using R7.University.ControlExtensions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;
using System.Runtime.InteropServices;

namespace R7.University.Controls
{
    public partial class EditEduForms: UserControl
    {
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

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this, Module)); }
        }

        protected List<EduProgramProfileFormViewModel> ViewStateEduProgramProfileForms
        {
            get { return XmlSerializationHelper.Deserialize<List<EduProgramProfileFormViewModel>> (ViewState ["eduProgramProfileForms"]); }
            set { ViewState ["eduProgramProfileForms"] = XmlSerializationHelper.Serialize<List<EduProgramProfileFormViewModel>> (value); }
        }

        public void OnInit (PortalModuleBase module, IEnumerable<EduFormInfo> eduForms)
        {
            Module = module;

            SetEduForms (eduForms);

            comboEduForm.DataSource = EduFormViewModel.GetBindableList (eduForms, ViewModelContext, true);
            comboEduForm.DataBind ();

            gridEduForms.LocalizeColumns (LocalResourceFile);
        }

        public void SetEduProgramProfileForms (int itemId, IEnumerable<EduProgramProfileFormInfo> eppWithEduForms)
        { 
            ItemId = itemId;

            var eppFormViewModels = eppWithEduForms.Select (ef => 
                new EduProgramProfileFormViewModel (ef, ViewModelContext)).ToList ();
            
            ViewStateEduProgramProfileForms = eppFormViewModels;

            gridEduForms.DataSource = DataTableConstructor.FromIEnumerable (eppFormViewModels);
            gridEduForms.DataBind ();
        }

        public IList<EduProgramProfileFormInfo> GetEduProgramProfileForms ()
        {
            var eppForms = ViewStateEduProgramProfileForms;
            if (eppForms != null)
            {
                return eppForms.Select (ef => ef.NewEduProgramProfileFormInfo ()).ToList ();
            }

            return new List<EduProgramProfileFormInfo> ();    
        }
       
        protected void SetEduForms (IEnumerable<EduFormInfo> eduForms)
        {
            ViewState ["eduForms"] = eduForms.ToList ();
        }

        protected EduFormInfo GetEduForm (int? eduFormId)
        {
            if (eduFormId != null)
            {
                var eduForms = (List<EduFormInfo>) ViewState ["eduForms"];
                return eduForms.Single (ef => ef.EduFormID == eduFormId.Value);
            }

            return new EduFormInfo {
                EduFormID = Null.NullInteger,
                Title = string.Empty
            };
        }

        #region Handlers

        protected void buttonAddEduForm_Command (object sender, CommandEventArgs e)
        {
            try
            {
                EduProgramProfileFormViewModel eppForm;

                // get item list from viewstate
                var eppForms = ViewStateEduProgramProfileForms;

                // creating new list, if none
                if (eppForms == null)
                    eppForms = new List<EduProgramProfileFormViewModel> ();

                var command = e.CommandArgument.ToString ();
                if (command == "Add")
                {
                    eppForm = new EduProgramProfileFormViewModel ();
                }
                else
                {
                    // restore ItemID from hidden field
                    var hiddenItemID = int.Parse (hiddenEduFormItemID.Value);
                    eppForm = eppForms.Find (d => d.ViewItemID == hiddenItemID);
                }

                eppForm.EduFormID = comboEduForm.SelectedIndex;
                eppForm.IsAdmissive = checkIsAdmissive.Checked;
                eppForm.TimeToLearn = int.Parse (textTimeToLearnYears.Text) * 12 + int.Parse (textTimeToLearnMonths.Text);

                if (command == "Add")
                {
                    eppForm.EduProgramProfileID = ItemId;
                    eppForms.Add (eppForm);
                }

                ResetEditEduFormForm ();

                // refresh viewstate
                ViewStateEduProgramProfileForms = eppForms;

                // bind items to the gridview
                EduProgramProfileFormViewModel.BindToView (eppForms, ViewModelContext);

                gridEduForms.DataSource = DataTableConstructor.FromIEnumerable (eppForms);
                gridEduForms.DataBind ();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void linkEditEduForm_Command (object sender, CommandEventArgs e)
        {
            try
            {
                var eppForms = ViewStateEduProgramProfileForms;
                if (eppForms != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find edu. form in the list
                    var eppForm = eppForms.Find (ef => ef.ViewItemID.ToString () == itemID);

                    if (eppForm != null)
                    {
                        // fill form
                        comboEduForm.SelectByValue (eppForm.EduFormID);
                        checkIsAdmissive.Checked = eppForm.IsAdmissive;
                        textTimeToLearnYears.Text = (eppForm.TimeToLearn / 12).ToString ();
                        textTimeToLearnMonths.Text = (eppForm.TimeToLearn % 12).ToString ();

                        // store ViewItemID in the hidden field
                        hiddenEduFormItemID.Value = eppForm.ViewItemID.ToString ();

                        // show / hide buttons
                        buttonAddEduForm.Visible = false;
                        buttonUpdateEduForm.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void linkDeleteEduForm_Command (object sender, CommandEventArgs e)
        {
            try
            {
                var eppForms = ViewStateEduProgramProfileForms;
                if (eppForms != null)
                {
                    var itemId = e.CommandArgument.ToString ();

                    // find position in a list
                    var eppFormIndex = eppForms.FindIndex (ef => ef.ViewItemID.ToString () == itemId);

                    if (eppFormIndex >= 0)
                    {
                        // remove item
                        eppForms.RemoveAt (eppFormIndex);

                        // refresh viewstate
                        ViewStateEduProgramProfileForms = eppForms;

                        // bind to the gridview
                        EduProgramProfileFormViewModel.BindToView (eppForms, ViewModelContext);
                        gridEduForms.DataSource = DataTableConstructor.FromIEnumerable (eppForms);
                        gridEduForms.DataBind ();

                        // reset form if we deleting currently edited item
                        if (buttonUpdateEduForm.Visible && hiddenEduFormItemID.Value == itemId)
                        {
                            ResetEditEduFormForm ();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        protected void buttonCancelEditEduForm_Click (object sender, EventArgs e)
        {
            try
            {
                ResetEditEduFormForm ();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (Module, ex);
            }
        }

        void ResetEditEduFormForm ()
        {
            // restore default buttons visibility
            buttonAddEduForm.Visible = true;
            buttonUpdateEduForm.Visible = false;

            comboEduForm.SelectedIndex = 0;
            textTimeToLearnYears.Text = "0";
            textTimeToLearnMonths.Text = "0";
            checkIsAdmissive.Checked = false;
        }

        protected void gridEduForms_RowDataBound (object sender, GridViewRowEventArgs e)
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

        #endregion
    }
}
