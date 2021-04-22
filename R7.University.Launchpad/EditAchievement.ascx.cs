using System;
using System.Linq;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.University.Dnn.Modules;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public partial class EditAchievement : UniversityEditPortalModuleBase<AchievementInfo>
    {
        protected EditAchievement () : base ("achievement_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            comboAchievementType.DataSource = new FlatQuery<AchievementTypeInfo> (ModelContext).List ()
                .Select (at => new { Value = at.AchievementTypeId, Text = at.Localize (LocalResourceFile) });

            comboAchievementType.DataBind ();
            comboAchievementType.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        protected override string GetContextString (AchievementInfo item)
        {
            return item?.Title;
        }

        protected override void LoadItem (AchievementInfo item)
        {
            base.LoadItem (item);

            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            comboAchievementType.SelectByValue (item.AchievementTypeId);
        }

        protected override void BeforeUpdateItem (AchievementInfo item, bool isNew)
        {
            item.Title = textTitle.Text.Trim ();
            item.ShortTitle = textShortTitle.Text.Trim ();
            item.AchievementTypeId = ParseHelper.ParseToNullable<int> (comboAchievementType.SelectedValue, true);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (AchievementInfo item) => item.AchievementID;

        protected override void AddItem (AchievementInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (AchievementInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (AchievementInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}

