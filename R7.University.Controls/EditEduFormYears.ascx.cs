using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Utilities;
using R7.University.Controls.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditEduFormYears:
        GridAndFormControlBase<EduProgramProfileFormYearInfo,EduProgramProfileFormYearEditModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<EduFormInfo> eduForms, IEnumerable<YearInfo> years)
        {
            Module = module;

            var eduFormViewModels = EduFormViewModel.GetBindableList (eduForms, ViewModelContext, false);

            ViewState ["eduForms"] = Json.Serialize (eduFormViewModels.ToList ());
            ViewState ["years"] = Json.Serialize (years.ToList ());

            radioEduForm.DataSource = eduFormViewModels;
            radioEduForm.DataBind ();
            radioEduForm.SelectedIndex = 0;

            var lastYear = GetLastYear ();
            comboYear.DataSource = years.Select (y => new {
                y.YearId,
                y.Year,
                YearWithCourse = y.FormatWithCourse (lastYear)
            }).OrderByDescending (y => y.Year);

            comboYear.DataBind ();
            comboYear.InsertDefaultItem ("-");
        }

        protected EduFormViewModel GetEduForm (int eduFormId)
        {
            var eduForms = Json.Deserialize<List<EduFormViewModel>> ((string) ViewState ["eduForms"]);
            return eduForms.Single (ef => ef.EduFormID == eduFormId);
        }

        protected IYear GetYear (int? yearId)
        {
            if (yearId != null) {
                var years = Json.Deserialize<List<YearInfo>> ((string) ViewState ["years"]);
                return years.Single (y => y.YearId == yearId);
            }

            return null;
        }

        protected IYear GetLastYear ()
        {
            var years = Json.Deserialize<List<YearInfo>> ((string) ViewState ["years"]);
            return years.LastYear ();
        }

        protected override EduProgramProfileFormYearEditModel CreateViewModel (
            EduProgramProfileFormYearInfo model, EduProgramProfileFormYearEditModel convertor)
        {
            return (EduProgramProfileFormYearEditModel) convertor.Create (model, ViewModelContext, GetLastYear ());
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (EduProgramProfileFormYearEditModel item)
        {
            comboYear.SelectByValue (item.YearId);
            radioEduForm.SelectByValue (item.EduFormId);

            datetimeStartDate.SelectedDate = item.StartDate;
            datetimeEndDate.SelectedDate = item.EndDate;

            hiddenEduFormID.Value = item.EduFormId.ToString ();
        }

        protected override void OnUpdateItem (EduProgramProfileFormYearEditModel item)
        {
            item.YearId = ParseHelper.ParseToNullable<int> (comboYear.SelectedValue, true);
            item.YearString = GetYear (item.YearId).FormatWithCourse (GetLastYear ());

            item.EduFormId = int.Parse (radioEduForm.SelectedValue);
            item.EduFormViewModel = GetEduForm (item.EduFormId);

            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;
        }

        protected override void OnResetForm ()
        {
            comboYear.SelectedIndex = 0;
            radioEduForm.SelectedIndex = 0;
            datetimeStartDate.SelectedDate = null;
            datetimeEndDate.SelectedDate = null;
        }

        IEnumerable<EduProgramProfileFormYearEditModel> SortItems (IEnumerable<EduProgramProfileFormYearEditModel> items)
        {
            return items.OrderBy (eppfy => eppfy.EduFormViewModel.SortIndex)
                        .ThenByDescending (eppfy => eppfy.YearString);
        }

        protected override void BindItems (IEnumerable<EduProgramProfileFormYearEditModel> items)
        {
            base.BindItems (SortItems (items));

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion
    }
}
