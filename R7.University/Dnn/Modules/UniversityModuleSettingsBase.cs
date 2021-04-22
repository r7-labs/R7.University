using System;
using System.Web.UI.WebControls;
using R7.Dnn.Extensions.Modules;
using R7.University.Client;
using R7.University.Security;

namespace R7.University.Dnn.Modules
{
    public class UniversityModuleSettingsBase<TSettings>: ModuleSettingsBase<TSettings>
        where TSettings: class, new ()
    {
        #region Controls

        protected Panel panelGeneralSettings;

        #endregion

        #region Properties

        IModuleSecurityContext securityContext;
        protected IModuleSecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo, this)); }
        }

        #endregion

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            ClientResourceHelper.RegisterSelect2 (Page);

            if (panelGeneralSettings != null) {
                panelGeneralSettings.Visible = SecurityContext.CanManageModule ();
            }
        }
    }
}
