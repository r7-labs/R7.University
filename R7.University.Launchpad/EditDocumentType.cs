using R7.University.Dnn.Modules;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class EditDocumentType: UniversityEditPortalModuleBase<DocumentTypeInfo>
    {
        protected EditDocumentType () : base ("documenttype_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (DocumentTypeInfo item)
        {
            // TODO: No resx strings in project. Instead of copy them here, it's better to implement solution-level localization
            return item?.Localize (LocalResourceFile);
        }

        protected override void LoadItem (DocumentTypeInfo item)
        {
            base.LoadItem (item);

            textType.Text = item.Type;
            textDescription.Text = item.Description;
            checkIsSystem.Checked = item.IsSystem;
            textFilenameFormat.Text = item.FilenameFormat;

            // disable textType for system types
            textType.Enabled = !item.IsSystem;
        }

        protected override void BeforeUpdateItem (DocumentTypeInfo item, bool isNew)
        {
            // don't update Type for system types,
            // also don't update IsSystem value at all

            if (!item.IsSystem) {
                item.Type = textType.Text.Trim ();
            }

            item.Description = textDescription.Text.Trim ();
            item.FilenameFormat = textFilenameFormat.Text.Trim ();
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (DocumentTypeInfo item) => item.DocumentTypeID;

        protected override void AddItem (DocumentTypeInfo item)
        {
            ModelContext.Add<DocumentTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (DocumentTypeInfo item)
        {
            ModelContext.Update<DocumentTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (DocumentTypeInfo item)
        {
            ModelContext.Remove<DocumentTypeInfo> (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
