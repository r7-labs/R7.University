//
//  ModelToTemplateBinderBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2020 Roman M. Yagodin
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
using System.Linq.Expressions;

namespace R7.University.Core.Templates
{
    public abstract class ModelToTemplateBinderBase: IModelToTemplateBinder
    {
        public abstract int Count (string collectionName);

        public abstract string Eval (string objectName);

        public abstract string Eval (string objectName, string collectionName, int index);

        protected string NameOf<T> (Expression<Func<T>> propLambda)
        {
            var expr = propLambda.Body as MemberExpression;
            if (expr == null) {
                throw new ArgumentException ("You must pass a lambda of the form: \"() => Class.Property\" or \"() => object.Property\"");
            }
            return expr.Member.Name;
        }
    }
}
