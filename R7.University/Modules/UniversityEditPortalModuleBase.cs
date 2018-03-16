//
//  UniversityEditPortalModuleBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using R7.Dnn.Extensions.Modules;
using R7.University.Models;
using R7.University.Security;

namespace R7.University.Modules
{
    public abstract class UniversityEditPortalModuleBase<TEntity>: EditPortalModuleBase<TEntity, int>
        where TEntity : class, new()
    {
        #region Contexts

        ModelContextBase modelContext;
        protected ModelContextBase ModelContext {
            get { return modelContext ?? (modelContext = new UniversityModelContext ()); }
        }

        public override void Dispose ()
        {
            if (modelContext != null) {
                modelContext.Dispose ();
            }

            base.Dispose ();
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        #endregion

        #region Session-state properties

        protected string SessionLastItem {
            get { return (string) Session [$"r7_University_LastItem"]; }
            set { Session [$"r7_University_LastItem"] = value; }
        }

        protected void RememberLastItem (int itemId)
        {
            SessionLastItem = $"{typeof (TEntity).Name.ToLowerInvariant ()}_{ModuleId}_{itemId}";
        }

        #endregion

        protected UniversityEditPortalModuleBase (string key) : base (key)
        {
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            RememberLastItem (ItemId ?? 0);
        }

        protected abstract int GetItemId (TEntity item);

        protected override TEntity GetItem (int itemId)
        {
            return ModelContext.Get<TEntity> (itemId);
        }

        protected override bool CanDeleteItem (TEntity item)
        {
            return SecurityContext.CanDelete (item);
        }

        // HACK: Dispose current model context used in load to create new one for update
        protected override void OnButtonUpdateClick (object sender, EventArgs e)
        {
            if (modelContext != null) {
                modelContext.Dispose ();
                modelContext = null;
            }

            base.OnButtonUpdateClick (sender, e);
        }

        protected override void AfterUpdateItem (TEntity item)
        {
            base.AfterUpdateItem (item);

            RememberLastItem (GetItemId (item));
        }
    }
}
