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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace R7.University.Core.Templates
{
    // TODO: Introduce UniversityModelToTemplateBuilder base class - for version binding, etc.
    public abstract class ModelToTemplateBinderBase: IModelToTemplateBinder
    {
        public abstract int Count (string collectionName);

        // public abstract string Eval (string objectName);

        public abstract string Eval (string objectName, string collectionName, int index);

        protected IDictionary<string, Func<string>> Bindings = new Dictionary<string, Func<string>> ();

        protected void AddBinding (Expression<Func<string>> propLambda)
        {
            Bindings.Add (NameOf (propLambda), propLambda.Compile ());
        }

        protected void AddBinding (string objectName, Func<string> func)
        {
            Bindings.Add (objectName, func);
        }

        public virtual string Eval (string objectName)
        {
            if (Bindings.TryGetValue (objectName, out Func<string> func)) {
                return func.Invoke ();
            }

            return null;
        }

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
