using System;
using System.Linq;
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.University;
using R7.University.ControlExtensions;
using R7.University.ModelExtensions;

namespace R7.University.Launchpad
{
    // available tabs
    public enum EditEduProgramTab { Common, Documents };

	public partial class EditEduProgram : LaunchpadPortalModuleBase
	{
        protected EditEduProgramTab SelectedTab
        {
            get 
            {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget))
                {
                    // check if postback initiator is on Documents tab
                    if (eventTarget.Contains ("$" + formEditDocuments.ID))
                    {
                        ViewState ["SelectedTab"] = EditEduProgramTab.Documents;
                        return EditEduProgramTab.Documents;
                    }
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                if (obj != null) 
                {
                    return (EditEduProgramTab) obj;
                }

                return EditEduProgramTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

		private int? itemId = null;

		#region Overrides

		/// <summary>
		/// Handles Init event for a control.
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);

			// set url for Cancel link
			linkCancel.NavigateUrl = Globals.NavigateURL ();

			// add confirmation dialog to delete button
			buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('" 
                + Localization.GetString ("DeleteItem") + "');");

			// bind education levels
            comboEduLevel.DataSource = LaunchpadController.GetObjects<EduLevelInfo> ();
            comboEduLevel.DataBind ();

            var documentTypes = LaunchpadController.GetObjects<DocumentTypeInfo> ();
            formEditDocuments.OnInit (this, documentTypes);
		}

		/// <summary>
		/// Handles Load event for a control.
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
            	
            if (DotNetNuke.Framework.AJAX.IsInstalled ())
                DotNetNuke.Framework.AJAX.RegisterScriptManager ();
            
			try
			{
                // parse querystring parameters
				itemId = Utils.ParseToNullableInt (Request.QueryString ["eduprogram_id"]);
      
				if (!IsPostBack)
				{
					// load the data into the control the first time we hit this page

					// check we have an item to lookup
					// ALT: if (!Null.IsNull (itemId) 
					if (itemId.HasValue)
					{
						// load the item
						var item = LaunchpadController.Get<EduProgramInfo> (itemId.Value);

						if (item != null)
						{
							textCode.Text = item.Code;
							textTitle.Text = item.Title;
                            textGeneration.Text = item.Generation;
                            dateAccreditedToDate.SelectedDate = item.AccreditedToDate;
                            datetimeStartDate.SelectedDate = item.StartDate;
                            datetimeEndDate.SelectedDate = item.EndDate;
                            Utils.SelectByValue (comboEduLevel, item.EduLevelID.ToString ());

                            auditControl.Bind (item);

                            var documents = LaunchpadController.GetObjects<DocumentInfo> (
                                string.Format ("WHERE ItemID = N'EduProgramID={0}'", item.EduProgramID))
                                .WithDocumentType (LaunchpadController)
                                .ToList ();
                            
                            formEditDocuments.SetData (documents, item.EduProgramID);
						}
						else
							Response.Redirect (Globals.NavigateURL (), true);
					}
					else
					{
                        auditControl.Visible = false;
						buttonDelete.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

        #endregion

        #region Handlers

		/// <summary>
		/// Handles Click event for Update button
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Event args.
		/// </param>
		protected void buttonUpdate_Click (object sender, EventArgs e)
		{
			try
			{
				EduProgramInfo item;

				// determine if we are adding or updating
				// ALT: if (Null.IsNull (itemId))
				if (!itemId.HasValue)
				{
					// add new record
					item = new EduProgramInfo ();
				}
				else
				{
					// update existing record
					item = LaunchpadController.Get<EduProgramInfo> (itemId.Value);
				}

				// fill the object
				item.Code = textCode.Text.Trim ();
				item.Title = textTitle.Text.Trim ();
                item.Generation = textGeneration.Text.Trim ();
                item.AccreditedToDate = dateAccreditedToDate.SelectedDate;
                item.StartDate = datetimeStartDate.SelectedDate;
                item.EndDate = datetimeEndDate.SelectedDate;
                item.EduLevelID = int.Parse (comboEduLevel.SelectedValue);

                if (itemId == null)
                {
                    item.CreatedOnDate = DateTime.Now;
                    item.LastModifiedOnDate = item.CreatedOnDate;
                    item.CreatedByUserID = UserInfo.UserID;
                    item.LastModifiedByUserID = item.CreatedByUserID;
                    LaunchpadController.AddEduProgram (item, formEditDocuments.GetData ());
                }
				else
                {
                    item.LastModifiedOnDate = DateTime.Now;
                    item.LastModifiedByUserID = UserInfo.UserID;

                    // REVIEW: Solve on SqlDataProvider level on upgrage to 2.0.0?
                    if (item.CreatedOnDate == default (DateTime)) 
                    {
                        item.CreatedOnDate = item.LastModifiedOnDate;
                        item.CreatedByUserID = item.LastModifiedByUserID;
                    }

                    LaunchpadController.UpdateEduProgram (item, formEditDocuments.GetData ());
                }

				Utils.SynchronizeModule (this);

				Response.Redirect (Globals.NavigateURL (), true);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
         
		/// <summary>
		/// Handles Click event for Delete button
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Event args.
		/// </param>
		protected void buttonDelete_Click (object sender, EventArgs e)
		{
			try
			{
				if (itemId != null)
				{
                    LaunchpadController.DeleteEduProgram (itemId.Value);
					Response.Redirect (Globals.NavigateURL (), true);
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion
	}
}

