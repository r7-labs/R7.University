using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class EditEmployee
	{
		// NOTE: Do not use asp:Button for dnnSecondaryActions - this is not applied

		protected ModuleAuditControl ctlAudit;
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;

		protected LabelControl labelPhoto;
		protected DnnFilePickerUploader pickerPhoto;
		protected LabelControl labelUser;
		protected UrlControl urlUser;
		protected LabelControl labelLastName;
		protected TextBox textLastName;
		protected LabelControl labelFirstName;
		protected TextBox textFirstName;
		protected LabelControl labelOtherName;
		protected TextBox textOtherName;
		protected LabelControl labelAcademicDegree;
		protected TextBox textAcademicDegree;
		protected LabelControl labelAcademicTitle;
		protected TextBox textAcademicTitle;
		protected LabelControl labelNamePrefix;
		protected TextBox textNamePrefix;
		protected LabelControl labelPhone;
		protected TextBox textPhone;
		protected LabelControl labelCellPhone;
		protected TextBox textCellPhone;
		protected LabelControl labelFax;
		protected TextBox textFax;
		protected LabelControl labelEmail;
		protected TextBox textEmail;
		protected LabelControl labelSecondaryEmail;
		protected TextBox textSecondaryEmail;
		protected LabelControl labelWebSite;
		protected TextBox textWebSite;
		protected LabelControl labelMessenger;
		protected TextBox textMessenger;
		protected LabelControl labelWorkingHours;
		protected TextBox textWorkingHours;
		protected LabelControl labelWorkingPlace;
		protected TextBox textWorkingPlace;
		protected LabelControl labelExperienceYears;
		protected TextBox textExperienceYears;
		protected LabelControl labelExperienceYearsBySpec;
		protected TextBox textExperienceYearsBySpec;
		protected LabelControl labelBiography;
		protected TextEditor textBiography;
		protected LabelControl labelIsPublished;
		protected CheckBox checkIsPublished;
		// protected LabelControl labelIsDeleted;
		// protected CheckBox checkIsDeleted;

		protected LinkButton buttonUserLookup;
		protected CheckBox checkIncludeDeletedUsers;
		protected TextBox textUserLookup;
		protected Label labelUserNames;
		protected LabelControl labelUserLookup;
		protected DnnComboBox comboUsers;

		protected GridView gridOccupiedPositions;
		protected LabelControl labelPositions;
		protected LabelControl labelDivisions;
		protected LabelControl labelIsPrime;
		protected DnnComboBox comboPositions;
		protected DnnComboBox comboDivisions;
		protected LinkButton buttonAddOccupiedPosition;
		protected CheckBox checkIsPrime;
	}
}
