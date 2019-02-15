//
//  EditPosition.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2018 Roman M. Yagodin
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

using R7.University.Models;
using R7.University.Modules;

namespace R7.University.Launchpad
{
    public partial class EditYear : UniversityEditPortalModuleBase<YearInfo>
    {
        protected EditYear () : base ("year_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (YearInfo item)
        {
            textYear.Text = item.Year.ToString ();
            checkAdmissionIsOpen.Checked = item.AdmissionIsOpen;
        }

        protected override void BeforeUpdateItem (YearInfo item, bool isNew)
        {
            item.Year = int.Parse (textYear.Text);
            item.AdmissionIsOpen = checkAdmissionIsOpen.Checked;
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override int GetItemId (YearInfo item) => item.YearId;

        protected override void AddItem (YearInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (YearInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (YearInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}
