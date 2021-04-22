using R7.University.Dnn.Modules;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditEduForm: UniversityEditPortalModuleBase<EduFormInfo>
    {
        protected EditEduForm () : base ("eduform_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (EduFormInfo item)
        {
            return item?.Title;
        }

        protected override void LoadItem (EduFormInfo item)
        {
            base.LoadItem (item);

            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            textSortIndex.Text = item.SortIndex.ToString ();
            checkIsSystem.Checked = item.IsSystem;

            // disable fields for system items
            textTitle.Enabled = !item.IsSystem;
        }

        protected override void BeforeUpdateItem (EduFormInfo item, bool isNew)
        {
            // don't update fields for system items,
            // also don't update IsSystem value at all
            if (!item.IsSystem) {
                item.Title = textTitle.Text.Trim ();
            }

            item.ShortTitle = textShortTitle.Text.Trim ();
            item.SortIndex = int.Parse (textSortIndex.Text);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EduFormInfo item) => item.EduFormID;

        protected override void AddItem (EduFormInfo item)
        {
            ModelContext.Add<EduFormInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (EduFormInfo item)
        {
            ModelContext.Update<EduFormInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduFormInfo item)
        {
            ModelContext.Remove<EduFormInfo> (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
