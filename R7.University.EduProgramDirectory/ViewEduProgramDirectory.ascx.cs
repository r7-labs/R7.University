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
using DotNetNuke.R7;
using R7.University;

namespace R7.University.EduProgramDirectory
{
    public partial class ViewEduProgramDirectory : ExtendedPortalModuleBase<EduProgramDirectoryController, EduProgramDirectorySettings>, IActionable
	{
		#region Properties

		protected string EditIconUrl
		{
			get { return IconController.IconURL ("Edit"); }
		}

		#endregion

		#region Handlers

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
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

		#region IActionable implementation

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				// create a new action to add an item, this will be added 
				// to the controls dropdown menu
				var actions = new ModuleActionCollection ();
				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("AddEduProgram.Action", this.LocalResourceFile),
					ModuleActionType.AddContent, 
					"", 
					"", 
					Utils.EditUrl (this, "EditEduProgram"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
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
                    linkEdit.NavigateUrl = Utils.EditUrl (this, "EditEduProgram", "eduprogram_id", eduProgram.EduProgramID.ToString ());
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }
            }
        }
	}
}

