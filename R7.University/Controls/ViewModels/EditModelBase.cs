//
//  EditModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls.ViewModels
{
    public abstract class EditModelBase<TModel>: IEditModel<TModel>
    {
        public int ViewItemID { get; set; } = ViewNumerator.GetNextItemID ();

        [JsonIgnore]
        public ViewModelContext Context { get; set; }

        [JsonConverter (typeof (StringEnumConverter))]
        public ModelEditState PrevEditState { get; set; }

        protected ModelEditState _editState;

        [JsonConverter (typeof (StringEnumConverter))]
        public ModelEditState EditState {
            get { return _editState; }
            set { PrevEditState = _editState; _editState = value; }
        }

        public virtual bool IsPublished => true;

        public abstract TModel CreateModel ();

        public abstract IEditModel<TModel> Create (TModel model, ViewModelContext context);

        public abstract void SetTargetItemId (int targetItemId, string targetItemKey);
    }
}
