using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Icons;
using R7.University;
using R7.University.Extensions;

namespace R7.University.Launchpad
{
	public partial class EditEduProgram : LaunchpadPortalModuleBase
	{
        #region Types

        public enum EditEduProgramTab { Common, Documents };

        #endregion

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

        protected EditEduProgramTab SelectedTab
        {
            get 
            {
                // get postback initiator
                var eventTarget = Request.Form ["__EVENTTARGET"];

                // document URL control is on Documents tab
                if (!string.IsNullOrEmpty (eventTarget) && eventTarget.Contains ("$" + urlDocumentUrl.ID +"$"))
                {
                    ViewState ["SelectedTab"] = EditEduProgramTab.Documents;
                    return EditEduProgramTab.Documents;
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditEduProgramTab) obj : EditEduProgramTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

		// ALT: private int itemId = Null.NullInteger;
		private int? itemId = null;

        #region Documents & document types

        protected void RememberDocumentTypes (IEnumerable<DocumentTypeInfo> documentTypes)
        {
            ViewState ["documentTypes"] = documentTypes.ToList ();
        }

        protected string GetDocumentType (int? documentTypeId)
        {
            if (documentTypeId != null)
            {
                var documentTypes = (List<DocumentTypeInfo>) ViewState ["documentTypes"];
                return documentTypes.Single (dt => dt.DocumentTypeID == documentTypeId.Value).Type;
            }

            return string.Empty;
        }

        private List<DocumentInfo> GetDocuments ()
        {
            if (ViewStateDocuments != null)
            {
                return ViewStateDocuments.Select (dvm => dvm.NewDocumentInfo ()).ToList ();
            }

            return new List<DocumentInfo> ();
        }

        protected List<DocumentViewModel> ViewStateDocuments
        {
            get { return XmlSerializationHelper.Deserialize<List<DocumentViewModel>> (ViewState ["documents"]); }
            set { ViewState ["documents"] = XmlSerializationHelper.Serialize<List<DocumentViewModel>> (value); }
        }

        #endregion

		#region Handlers

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
			buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");

			// bind education levels
            comboEduLevel.DataSource = LaunchpadController.GetObjects<EduLevelInfo> ();
            comboEduLevel.DataBind ();

            // bind document types
            var documentTypes = LaunchpadController.GetObjects<DocumentTypeInfo> ();
            RememberDocumentTypes (documentTypes);

            comboDocumentType.DataSource = DocumentTypeViewModel.GetBindableList (documentTypes, LocalResourceFile, true);
            comboDocumentType.DataBind ();
           
            gridDocuments.LocalizeColumns (LocalResourceFile);
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

                            var documents = LaunchpadController.GetObjects<DocumentInfoEx> (string.Format (
                                "WHERE ItemID = N'EduProgramID={0}'", item.EduProgramID)).Select (d => new DocumentViewModel (d, ViewModelContext)).ToList ();

                            ViewStateDocuments = documents;
                            gridDocuments.DataSource = DataTableConstructor.FromIEnumerable (documents);
                            gridDocuments.DataBind ();
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
                    LaunchpadController.AddEduProgram (item, GetDocuments ());
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

                    LaunchpadController.UpdateEduProgram (item, GetDocuments ());
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
       
        #region Documents

        protected void buttonAddDocument_Command (object sender, CommandEventArgs e)
        {
            try
            {
                SelectedTab = EditEduProgramTab.Documents;

                DocumentViewModel document;

                // get documents list from viewstate
                var documents = ViewStateDocuments;

                // creating new list, if none
                if (documents == null)
                    documents = new List<DocumentViewModel> ();

                var command = e.CommandArgument.ToString ();
                if (command == "Add")
                {
                    document = new DocumentViewModel ();
                }
                else
                {
                    // restore ItemID from hidden field
                    var hiddenItemID = int.Parse (hiddenDocumentItemID.Value);
                    document = documents.Find (d => d.ViewItemID == hiddenItemID);
                }

                document.Title = textDocumentTitle.Text.Trim ();
                document.DocumentTypeID = Utils.ParseToNullableInt (comboDocumentType.SelectedValue);
                document.Type = GetDocumentType (document.DocumentTypeID);
                document.SortIndex = int.Parse (textDocumentSortIndex.Text);
                document.StartDate = datetimeDocumentStartDate.SelectedDate;
                document.EndDate = datetimeDocumentEndDate.SelectedDate;
                document.Url = urlDocumentUrl.Url;

                if (command == "Add")
                {
                    document.ItemID = "EduProgramID=" + itemId.Value;
                    documents.Add (document);
                }

                ResetEditDocumentForm ();

                // refresh viewstate
                ViewStateDocuments = documents;

                // bind items to the gridview
                DocumentViewModel.BindToView (documents, ViewModelContext);
                gridDocuments.DataSource = DataTableConstructor.FromIEnumerable (documents);
                gridDocuments.DataBind ();
                
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkEditDocument_Command (object sender, CommandEventArgs e)
        {
            try
            {
                SelectedTab = EditEduProgramTab.Documents;

                var documents = ViewStateDocuments;
                if (documents != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find document in a list
                    var document = documents.Find (d => d.ViewItemID.ToString () == itemID);

                    if (document != null)
                    {
                        // fill form
                        Utils.SelectByValue (comboDocumentType, document.DocumentTypeID);
                        textDocumentTitle.Text = document.Title;
                        textDocumentSortIndex.Text = document.SortIndex.ToString ();
                        datetimeDocumentStartDate.SelectedDate = document.StartDate;
                        datetimeDocumentEndDate.SelectedDate = document.EndDate;
                        urlDocumentUrl.Url = document.Url;

                        // store ItemID in the hidden field
                        hiddenDocumentItemID.Value = document.ViewItemID.ToString ();

                        // show / hide buttons
                        buttonAddDocument.Visible = false;
                        buttonUpdateDocument.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkDeleteDocument_Command (object sender, CommandEventArgs e)
        {
            try
            {
                SelectedTab = EditEduProgramTab.Documents;

                var documents = ViewStateDocuments;
                if (documents != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find position in a list
                    var documentIndex = documents.FindIndex (d => d.ViewItemID.ToString () == itemID);

                    if (documentIndex >= 0)
                    {
                        // remove item
                        documents.RemoveAt (documentIndex);

                        // refresh viewstate
                        ViewStateDocuments = documents;

                        // bind to the gridview
                        DocumentViewModel.BindToView (documents, ViewModelContext);
                        gridDocuments.DataSource = DataTableConstructor.FromIEnumerable (documents);
                        gridDocuments.DataBind ();

                        // reset form if we deleting currently edited item
                        if (buttonUpdateDocument.Visible && hiddenDocumentItemID.Value == itemID)
                            ResetEditDocumentForm ();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonCancelEditDocument_Click (object sender, EventArgs e)
        {
            try
            {
                SelectedTab = EditEduProgramTab.Documents;

                ResetEditDocumentForm ();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        void ResetEditDocumentForm ()
        {
            // restore default buttons visibility
            buttonAddDocument.Visible = true;
            buttonUpdateDocument.Visible = false;

            comboDocumentType.SelectedIndex = 0;
            textDocumentTitle.Text = string.Empty;
            textDocumentSortIndex.Text = "0";
            datetimeDocumentStartDate.SelectedDate = null;
            datetimeDocumentEndDate.SelectedDate = null;
            urlDocumentUrl.UrlType = "N";
        }

        protected void gridDocuments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hide ViewItemID column, also in header
            e.Row.Cells [1].Visible = false;

            // exclude header
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // find edit and delete linkbuttons
                var linkDelete = e.Row.Cells [0].FindControl ("linkDelete") as LinkButton;
                var linkEdit = e.Row.Cells [0].FindControl ("linkEdit") as LinkButton;

                // set recordId to delete
                linkEdit.CommandArgument = e.Row.Cells [1].Text;
                linkDelete.CommandArgument = e.Row.Cells [1].Text;

                // add confirmation dialog to delete link
                linkDelete.Attributes.Add ("onClick", "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");
            }
        }
        #endregion
	}
}

