//
//  UniversityEditPortalModuleBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2020 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.Modules;
using R7.University.Client;
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

        // TODO: Move to the base library
        // TODO: Vary by page, use DataCache instead?
        protected string SessionSelectedItem {
            get { return (string) Session [$"r7_SelectedItem"]; }
            set { Session [$"r7_SelectedItem"] = value; }
        }

        protected void UpdateSelectedItem (int itemId)
        {
            SessionSelectedItem = $"{typeof (TEntity).Name.Replace ("Info", string.Empty).ToLowerInvariant ()}_{ModuleId}_{itemId}";
        }

        #endregion

        protected UniversityEditPortalModuleBase (string key) : base (key)
        {
        }

        protected UniversityEditPortalModuleBase (string key, ICrudProvider<TEntity> crudProvider) : base (key, crudProvider)
        {
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            ClientResourceHelper.RegisterSelect2 (Page);
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            UpdateSelectedItem (ItemKey ?? 0);
        }

        #region Update control title for edited item

        // TODO: Move to the base library?

        void AdjustControlTitle (string appendix)
        {
            ((CDefault) Page).Title = ((CDefault) Page).Title.Append (appendix, " &gt; ");
        }

        /// <summary>
        /// Gets the context string for the entity (to display in the edit control title).
        /// </summary>
        protected abstract string GetContextString (TEntity item);

        protected override void LoadItem (TEntity item)
        {
            var contextString = GetContextString (item);
            if (contextString != null) {
                AdjustControlTitle (contextString);
            }
        }

        protected override void LoadNewItem ()
        {
            var contextString = GetContextString (null);
            if (contextString != null) {
                AdjustControlTitle (contextString);
            }
        }

        #endregion

        protected abstract int GetItemId (TEntity item);

        protected override TEntity GetItem (int itemKey)
        {
            return ModelContext.Get<TEntity, int> (itemKey);
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

        protected override void AfterUpdateItem (TEntity item, bool isNew)
        {
            base.AfterUpdateItem (item, isNew);

            UpdateSelectedItem (GetItemId (item));
        }
    }
}
