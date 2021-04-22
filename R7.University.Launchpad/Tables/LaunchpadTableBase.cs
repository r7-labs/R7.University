using System;
using System.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.University.Components;
using R7.University.Dnn;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public abstract class LaunchpadTableBase
    {
        #region Properties

        public string Name { get; protected set; }

        public string ResourceKey { get; protected set; }

        public string EditControlKey { get; protected set; }

        public string EditQueryKey { get; protected set; }

        public GridView Grid { get; protected set; }

        public virtual bool IsEditable
        {
            get { return true; }
        }

        public Type EntityType { get; protected set; }

        #endregion

        protected LaunchpadTableBase (Type entityType)
        {
            var name = entityType.Name.Replace ("Info", string.Empty);
            var namePlural = name + "s";

            Name = namePlural.ToLowerInvariant ();
            ResourceKey = namePlural + ".Text";
            EntityType = entityType;

            EditControlKey = "edit" + name;
            EditQueryKey = name + "_id";
        }

        public virtual void Init (PortalModuleBase module, GridView gridView, int pageSize)
        {
            Grid = gridView;
            Grid.PageSize = pageSize;
        }

        public virtual void DataBind (PortalModuleBase module, UniversityModelContext modelContext, string search = null)
        {
            Grid.DataSource = GetDataTable (module, modelContext, search);
            module.Session [Grid.ID] = Grid.DataSource;
            Grid.DataBind ();
        }

        public abstract DataTable GetDataTable (PortalModuleBase module, UniversityModelContext modelContext, string search);

        public virtual string GetAddUrl (PortalModuleBase module)
        {
            return module.EditUrl (EditControlKey);
        }

        public virtual string GetEditUrl (PortalModuleBase module, string id)
        {
            return module.EditUrl (EditQueryKey, id, EditControlKey);
        }

        public virtual ModuleAction GetAction (PortalModuleBase module)
        {
            // ModuleActionCollection is created before OnInit,
            // so need to pass module reference to this method

            return new ModuleAction (
                module.GetNextActionID (),
                // TODO: Action labels require localization
                EditControlKey.Replace ("edit", "Add "),
                ModuleActionType.AddContent,
                "",
                UniversityIcons.Add,
                module.EditUrl (EditControlKey),
                "",
                false,
                SecurityAccessLevel.Edit,
                true, false
            );
        }
    }
}
