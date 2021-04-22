using R7.University.Dnn.Modules;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditYear : UniversityEditPortalModuleBase<YearInfo>
    {
        protected EditYear () : base ("year_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (YearInfo item)
        {
            return item?.Year.ToString ();
        }

        protected override void LoadItem (YearInfo item)
        {
            base.LoadItem (item);

            textYear.Text = item.Year.ToString ();
            checkAdmissionIsOpen.Checked = item.AdmissionIsOpen;
        }

        protected override void BeforeUpdateItem (YearInfo item, bool isNew)
        {
            item.Year = int.Parse (textYear.Text);
            item.AdmissionIsOpen = checkAdmissionIsOpen.Checked;
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override int GetItemId (YearInfo item) => item.YearId;

        protected override void AddItem (YearInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (YearInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (YearInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
