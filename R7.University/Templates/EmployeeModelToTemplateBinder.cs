//
//  EmployeeModelToTemplateBinder.cs
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
using R7.University.Core.Templates;
using R7.University.Models;

namespace R7.University.Templates
{
    public class EmployeeToTemplateBinder: IModelToTemplateBinder
    {
        protected readonly IEmployee Model;

        protected readonly IList<OccupiedPositionInfo> Positions;

        protected readonly IList<EmployeeAchievementInfo> Achievements;

        protected readonly IList<EmployeeDisciplineInfo> Disciplines;

        public EmployeeToTemplateBinder (IEmployee model)
        {
            Model = model;

            Positions = new List<OccupiedPositionInfo> (Model.Positions);
            Disciplines = new List<EmployeeDisciplineInfo> (Model.Disciplines);
        }

        public string Evaluate (string objectName)
        {
            // TODO: Add simple bindings via configuration
            if (objectName == NameOf (() => Model.LastName)) {
                return Model.LastName;
            }
            if (objectName == NameOf (() => Model.FirstName)) {
                return Model.FirstName;
            }
            if (objectName == NameOf (() => Model.OtherName)) {
                return Model.OtherName;
            }

            return null;
        }

        public string Evaluate (string objectName, string collectionName, int index)
        {
            if (collectionName == nameof (Positions)) {
                if (objectName == "Title") {
                    return Positions [index].Position.Title;
                }
                if (objectName == "DivisionTitle") {
                    return Positions [index].Division.Title;
                }
                // TODO: Localize values
                if (objectName == "IsPrimary") {
                    return Positions [index].IsPrime ? "да" : "нет";
                }
            }

            if (collectionName == nameof (Disciplines)) {
            }

            if (collectionName == nameof (Achievements)) {
            }

            return null;
        }

        public int Count (string collectionName)
        {
            if (collectionName == nameof (Positions)) {
                return Model.Positions.Count;
            }
            if (collectionName == nameof (Disciplines)) {
                return Model.Disciplines.Count;
            }

            return 0;
        }

        // TODO: Move to the base class?
        string NameOf<T> (Expression<Func<T>> propLambda)
        {
            var expr = propLambda.Body as MemberExpression;
            if (expr == null) {
                throw new ArgumentException ("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }
            return expr.Member.Name;
        }
    }
}
