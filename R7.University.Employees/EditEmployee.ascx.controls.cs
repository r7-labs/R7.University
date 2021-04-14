using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University.Controls;

namespace R7.University.Employees
{
    public partial class EditEmployee
	{
		protected ModuleAuditControl ctlAudit;
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;
		protected DnnFilePickerUploader pickerPhoto;
        protected DnnFilePickerUploader pickerAltPhoto;
        protected DnnUrlControl urlUser;
		protected TextBox textLastName;
		protected TextBox textFirstName;
		protected TextBox textOtherName;
		protected TextBox textPhone;
		protected TextBox textCellPhone;
		protected TextBox textFax;
		protected TextBox textEmail;
        protected RegularExpressionValidator valEmail;
		protected TextBox textSecondaryEmail;
        protected RegularExpressionValidator valSecondaryEmail;
		protected TextBox textWebSite;
		protected TextBox textWebSiteLabel;
		protected TextBox textMessenger;
		protected TextBox textWorkingHours;
		protected CheckBox checkAddToVocabulary;
		protected DropDownList comboWorkingHours;
		protected TextBox textWorkingPlace;
		protected TextBox textExperienceYears;
		protected TextBox textExperienceYearsBySpec;
		protected TextEditor textBiography;
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
		protected LinkButton buttonUserLookup;
		protected CheckBox checkIncludeDeletedUsers;
		protected TextBox textUserLookup;
		protected Label labelUserNames;
		protected DropDownList comboUsers;
        protected CheckBox checkShowBarcode;
        protected TextBox txtScienceIndexAuthorId;
        protected EditAchievements formEditAchievements;
        protected EditDisciplines formEditDisciplines;
        protected EditPositions formEditPositions;
	}
}
