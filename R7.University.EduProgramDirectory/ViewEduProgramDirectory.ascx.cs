using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Security;
using DotNetNuke.R7;
using R7.University;
using R7.University.Extensions;

namespace R7.University.EduProgramDirectory
{
    public partial class ViewEduProgramDirectory 
        : ExtendedPortalModuleBase<EduProgramDirectoryController, EduProgramDirectorySettings>, IActionable
	{
		#region Properties

		protected string EditIconUrl
		{
			get { return IconController.IconURL ("Edit"); }
		}

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get
            { 
                if (viewModelContext == null)
                    viewModelContext = new ViewModelContext (this);

                return viewModelContext;
            }
        }

		#endregion

		#region Handlers

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            gridEduStandards.LocalizeColumns (LocalResourceFile);
        }

		/// <summary>
		/// Handles Load event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			
			try
			{
                if (!IsPostBack)
				{
                    // REVIEW: Order / group by edu level first?
                    var eduPrograms = Controller.GetEduPrograms (IsEditable)
                        .OrderBy (ep => ep.Code)
                        .WithDocuments (Controller)
                        .WithEduLevel (Controller)
                        .Select (ep => new EduProgramStandardObrnadzorViewModel (ep, ViewModelContext));
                    
                    gridEduStandards.DataSource = eduPrograms;
                    gridEduStandards.DataBind ();
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

		#region IActionable implementation

		public ModuleActionCollection ModuleActions
		{
			get
			{
				// create a new action to add an item, 
                // this will be added to the controls dropdown menu
				var actions = new ModuleActionCollection ();
				actions.Add (
					GetNextActionID (), 
					LocalizeString ("AddEduProgram.Action"),
					ModuleActionType.AddContent, 
					"", "", 
					EditUrl ("EditEduProgram"),
					false, 
					SecurityAccessLevel.Edit,
					true, 
					false
				);
			
				return actions;
			}
		}

		#endregion

        protected void gridEduStandards_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var eduProgram = (EduProgramInfo) e.Row.DataItem;

                if (IsEditable)
                {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("eduprogram_id", eduProgram.EduProgramID.ToString (), "EditEduProgram");
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                if (!eduProgram.IsPublished)
                {
                    e.Row.CssClass = "not-published";
                }
            }
        }
	}
}

