//
//  AddCommand.cs
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

using System;
using R7.University.Models;
using R7.University.Security;

namespace R7.University.Commands
{
    public class AddCommand<TEntity> : ISecureCommand
        where TEntity : class, ITrackableEntityWritable
    {
        public IModelContext ModelContext { get; set; }

        public ISecurityContext SecurityContext { get; set; }

        public AddCommand (IModelContext modelContext, ISecurityContext securityContext)
        {
            ModelContext = modelContext;
            SecurityContext = securityContext;
        }

        public void Add (TEntity entity)
        {
            Add (entity, DateTime.Now);
        }

        public void Add (TEntity entity, DateTime dateTime)
        {
            if (SecurityContext.CanAdd (typeof (TEntity))) {

                entity.CreatedByUserID = entity.LastModifiedByUserID = SecurityContext.UserId;
                entity.CreatedOnDate = entity.LastModifiedOnDate = dateTime;

                ModelContext.Add (entity);
            }
        }
    }
}