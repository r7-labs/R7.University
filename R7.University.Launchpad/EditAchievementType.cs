using R7.University.Dnn.Modules;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditAchievementType: UniversityEditPortalModuleBase<AchievementTypeInfo>
    {
        protected EditAchievementType () : base ("achievementtype_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (AchievementTypeInfo item)
        {
            // TODO: No resx strings in project. Instead of copy them here, it's better to implement solution-level localization
            return item?.Localize (LocalResourceFile);
        }

        protected override void LoadItem (AchievementTypeInfo item)
        {
            base.LoadItem (item);

            textType.Text = item.Type;
            textDescription.Text = item.Description;
            checkIsSystem.Checked = item.IsSystem;

            // disable textType for system types
            textType.Enabled = !item.IsSystem;
        }

        protected override void BeforeUpdateItem (AchievementTypeInfo item, bool isNew)
        {
            // don't update Type for system types,
            // also don't update IsSystem value at all

            if (!item.IsSystem) {
                item.Type = textType.Text.Trim ();
            }

            item.Description = textDescription.Text.Trim ();
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (AchievementTypeInfo item) => item.AchievementTypeId;

        protected override void AddItem (AchievementTypeInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (AchievementTypeInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (AchievementTypeInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
