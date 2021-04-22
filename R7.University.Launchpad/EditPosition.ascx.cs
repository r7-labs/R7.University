using R7.Dnn.Extensions.Text;
using R7.University.Dnn.Modules;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditPosition : UniversityEditPortalModuleBase<PositionInfo>
    {
        protected EditPosition () : base ("position_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (PositionInfo item)
        {
            return item?.Title;
        }

        protected override void LoadItem (PositionInfo item)
        {
            base.LoadItem (item);

            txtTitle.Text = item.Title;
            txtShortTitle.Text = item.ShortTitle;
            txtWeight.Text = item.Weight.ToString ();
            checkIsTeacher.Checked = item.IsTeacher;
        }

        protected override void BeforeUpdateItem (PositionInfo item, bool isNew)
        {
            item.Title = txtTitle.Text.Trim ();
            item.ShortTitle = txtShortTitle.Text.Trim ();
            item.Weight = ParseHelper.ParseToNullable<int> (txtWeight.Text) ?? 0;
            item.IsTeacher = checkIsTeacher.Checked;
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override int GetItemId (PositionInfo item) => item.PositionID;

        protected override void AddItem (PositionInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (PositionInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (PositionInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
