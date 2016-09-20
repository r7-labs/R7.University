//
//  IEditControlViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using R7.DotNetNuke.Extensions.ViewModels;

namespace R7.University.Controls
{
    public interface IEditControlViewModel<TModel>
    {
        int ViewItemID { get; set; }

        ViewModelContext Context { get; set; }

        TModel ToModel ();

        IEditControlViewModel<TModel> FromModel (TModel model, ViewModelContext context);

        void SetTargetItemId (int targetItemId, string targetItemKey);
    }
}
