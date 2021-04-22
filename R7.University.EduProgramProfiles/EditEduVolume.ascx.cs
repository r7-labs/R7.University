using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.Dnn.Extensions.Text;
using R7.University.Commands;
using R7.University.Components;
using R7.University.Dnn;
using R7.University.EduProgramProfiles.Models;
using R7.University.EduProgramProfiles.Modules;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.EduProgramProfiles
{
    public partial class EditEduVolume : EduProgramProfileFormYearEditModuleBase<EduVolumeInfo>, IActionable
    {
        public enum EditEduVolumeTab
        {
            Common,
            Years,
            Practices
        }

        #region Properties

        protected EditEduVolumeTab SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditEduVolumeTab) obj : GetDefaultTab ();
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        EditEduVolumeTab GetDefaultTab ()
        {
            if (Settings != null) {
                if (Settings.Mode == EduVolumeDirectoryMode.EduVolume) {
                    return EditEduVolumeTab.Years;
                }

                if (Settings.Mode == EduVolumeDirectoryMode.Practices) {
                    return EditEduVolumeTab.Practices;
                }
            }

            return EditEduVolumeTab.Common;
        }

        EduVolumeDirectorySettings _settings;
        protected new EduVolumeDirectorySettings Settings =>
            _settings ?? (_settings = ModuleConfiguration.ModuleDefinition.DefinitionName == ModuleDefinitions.EduVolumeDirectory
                          ? new EduVolumeDirectorySettingsRepository ().GetSettings (ModuleConfiguration)
                          : null);

        #endregion

        protected EditEduVolume () : base ("eduvolume_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            BindTabs ();
        }

        protected override string GetContextString (EduVolumeInfo item)
        {
            var eppfy = item?.EduProgramProfileFormYear ?? GetEduProgramProfileFormYear ();
            return eppfy.FormatTitle (GetLastYear (), LocalResourceFile);
        }

        private void BindTabs ()
        {
            var showAllTabs = SecurityContext.IsAdmin;

            // TODO: Introduce separate edit form for base edu. volume
            tabCommon.Visible = showAllTabs || ModuleConfiguration.ModuleDefinition.DefinitionName == ModuleDefinitions.EduProgramProfileDirectory;
            pnlCommon.Visible = tabCommon.Visible;

            tabYears.Visible = showAllTabs || (Settings != null && Settings.Mode == EduVolumeDirectoryMode.EduVolume);
            pnlYears.Visible = tabYears.Visible;

            tabPractices.Visible = showAllTabs || (Settings != null && Settings.Mode == EduVolumeDirectoryMode.Practices);
            pnlPractices.Visible = tabPractices.Visible;
        }

        protected override void LoadItem (EduVolumeInfo item)
        {
            var ev = GetItemWithDependencies (ItemKey.Value);
            base.LoadItem (ev);

            textTimeToLearnHours.Text = ev.TimeToLearnHours.ToString ();
            textTimeToLearnYears.Text = (ev.TimeToLearnMonths / 12).ToString ();
            textTimeToLearnMonths.Text = (ev.TimeToLearnMonths % 12).ToString ();

            textYear1Cu.Text = ev.Year1Cu.ToString ();
            textYear2Cu.Text = ev.Year2Cu.ToString ();
            textYear3Cu.Text = ev.Year3Cu.ToString ();
            textYear4Cu.Text = ev.Year4Cu.ToString ();
            textYear5Cu.Text = ev.Year5Cu.ToString ();
            textYear6Cu.Text = ev.Year6Cu.ToString ();

            textPracticeType1Cu.Text = ev.PracticeType1Cu.ToString ();
            textPracticeType2Cu.Text = ev.PracticeType2Cu.ToString ();
            textPracticeType3Cu.Text = ev.PracticeType3Cu.ToString ();
        }

        protected override void BeforeUpdateItem (EduVolumeInfo item, bool isNew)
        {
            var updateAllTabs = SecurityContext.IsAdmin;

            if (updateAllTabs || ModuleConfiguration.ModuleDefinition.DefinitionName == ModuleDefinitions.EduProgramProfileDirectory) {
                item.TimeToLearnHours = int.Parse (textTimeToLearnHours.Text);
                item.TimeToLearnMonths =
                    int.Parse (textTimeToLearnYears.Text) * 12 + int.Parse (textTimeToLearnMonths.Text);
            }

            if (updateAllTabs || (Settings != null && Settings.Mode == EduVolumeDirectoryMode.EduVolume)) {
                item.Year1Cu = ParseHelper.ParseToNullable<int> (textYear1Cu.Text);
                item.Year2Cu = ParseHelper.ParseToNullable<int> (textYear2Cu.Text);
                item.Year3Cu = ParseHelper.ParseToNullable<int> (textYear3Cu.Text);
                item.Year4Cu = ParseHelper.ParseToNullable<int> (textYear4Cu.Text);
                item.Year5Cu = ParseHelper.ParseToNullable<int> (textYear5Cu.Text);
                item.Year6Cu = ParseHelper.ParseToNullable<int> (textYear6Cu.Text);
            }

            if (updateAllTabs || (Settings != null && Settings.Mode == EduVolumeDirectoryMode.Practices)) {
                item.PracticeType1Cu = ParseHelper.ParseToNullable<int> (textPracticeType1Cu.Text);
                item.PracticeType2Cu = ParseHelper.ParseToNullable<int> (textPracticeType2Cu.Text);
                item.PracticeType3Cu = ParseHelper.ParseToNullable<int> (textPracticeType3Cu.Text);
            }
        }

        protected EduVolumeInfo GetItemWithDependencies (int itemId)
        {
            return ModelContext.Get<EduVolumeInfo,int> (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EduVolumeInfo item) => item.EduVolumeId;

        protected override void AddItem (EduVolumeInfo item)
        {
            var eduVolumeId = ParseHelper.ParseToNullable<int> (Request.QueryString ["eduprogramprofileformyear_id"]);
            if (eduVolumeId != null) {
                item.EduVolumeId = eduVolumeId.Value;
            }

            new AddCommand<EduVolumeInfo> (ModelContext, SecurityContext).Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (EduVolumeInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduVolumeInfo item)
        {
            new DeleteCommand<EduVolumeInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();
                var eppfy = GetEduProgramProfileFormYear ();
                if (eppfy != null) {
                    actions.Add (new ModuleAction (GetNextActionID ()) {
                        Title = LocalizeString ("EditEduProgramProfile.Action"),
                        CommandName = ModuleActionType.EditContent,
                        Icon = UniversityIcons.Edit,
                        Secure = SecurityAccessLevel.Edit,
                        Url = EditUrl ("eduprofile_id", eppfy.EduProgramProfileId.ToString (), "EditEduProgramProfile"),
                        Visible = SecurityContext.IsAdmin
                    });
                }
                return actions;
            }
        }

        #endregion
    }
}
