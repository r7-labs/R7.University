using System;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.University.Dnn.Modules;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public partial class EditEduLevel: UniversityEditPortalModuleBase<EduLevelInfo>
    {
        protected EditEduLevel () : base ("edulevel_id")
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            comboParentEduLevel.DataSource = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboParentEduLevel.DataBind ();
            comboParentEduLevel.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetContextString (EduLevelInfo item)
        {
            return item?.Title;
        }

        protected override void LoadItem (EduLevelInfo item)
        {
            base.LoadItem (item);

            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            textSortIndex.Text = item.SortIndex.ToString ();
            comboParentEduLevel.SelectByValue (item.ParentEduLevelId);
        }

        protected override void BeforeUpdateItem (EduLevelInfo item, bool isNew)
        {
            item.Title = textTitle.Text.Trim ();
            item.ShortTitle = textShortTitle.Text.Trim ();
            item.SortIndex = ParseHelper.ParseToNullable<int> (textSortIndex.Text) ?? 0;
            item.ParentEduLevelId = ParseHelper.ParseToNullable<int> (comboParentEduLevel.SelectedValue, true);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EduLevelInfo item) => item.EduLevelID;

        protected override void AddItem (EduLevelInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (EduLevelInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduLevelInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}

