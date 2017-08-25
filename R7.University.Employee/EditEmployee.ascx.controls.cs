using System.Web.UI.WebControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University.Controls;

namespace R7.University.Employee
{
    public partial class EditEmployee
	{
		protected ModuleAuditControl ctlAudit;
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;
		protected DnnFilePickerUploader pickerPhoto;
        protected LinkButton buttonPhotoLookup;
		protected DnnUrlControl urlUser;
		protected TextBox textLastName;
		protected TextBox textFirstName;
		protected TextBox textOtherName;
		protected TextBox textPhone;
		protected TextBox textCellPhone;
		protected TextBox textFax;
		protected TextBox textEmail;
		protected TextBox textSecondaryEmail;
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
        protected GridView gridOccupiedPositions;
		protected DropDownList comboPositions;
		protected TextBox textPositionTitleSuffix;
        protected DivisionSelector divisionSelector;
		protected CheckBox checkIsPrime;
		protected LinkButton buttonAddPosition;
		protected LinkButton buttonUpdatePosition;
		protected LinkButton buttonCancelEditPosition;
		protected HiddenField hiddenOccupiedPositionItemID;
        protected EditAchievements formEditAchievements;
        protected EditDisciplines formEditDisciplines;
	}
}
