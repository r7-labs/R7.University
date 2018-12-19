//
//  TaxonomyExtensions.cs
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
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.Models;
using R7.University.Components;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class TaxonomyExtensions
    {
        public static string GetSafeTermName  (string shortTitle, string title)
        {
            var termName = UniversityModelHelper.HasUniqueShortTitle (shortTitle, title) ? shortTitle : title;
            return Regex.Replace (Regex.Replace (termName, @"[^(\w\-)]", " ").Trim (), @"\s+", " ");
        }

        public static int? AddTerm (this IDivision division, IModelContext modelContext)
        {
            var vocabularyName = UniversityConfig.Instance.Vocabularies.OrgStructure;
            var vocabulary = new VocabularyController ().GetVocabularies ().FirstOrDefault (v => v.Name == vocabularyName);
            if (vocabulary != null) {
                var termName = GetSafeTermName (division.ShortTitle, division.Title);
                try {
                    var term = new Term (termName, string.Empty, vocabulary.VocabularyId);
                    var parentDivision = division.GetParentDivision (modelContext);
                    if (parentDivision != null) {
                        term.ParentTermId = parentDivision.DivisionTermID;
                    }
                    return new TermController ().AddTerm (term);
                }
                catch (Exception ex) {
                    Exceptions.LogException (new Exception ($"Error adding {termName} term to {vocabularyName} vocabulary.", ex));
                }
            }
            else {
                Exceptions.LogException (new Exception ($"Could not find vocabulary with name {vocabularyName}."));
            }

            return null;
        }

        public static void UpdateTerm (this IDivision division, IModelContext modelContext)
        {
            var termName = GetSafeTermName (division.ShortTitle, division.Title);
            try {
                var parentDivision = division.GetParentDivision (modelContext);
                if (division.DivisionTermID != null) {
                    var termCtrl = new TermController ();
                    var term = termCtrl.GetTerm (division.DivisionTermID.Value);
                    if (term != null) {
                        term.Name = termName;
                        if (parentDivision != null) {
                            term.ParentTermId = parentDivision.DivisionTermID;
                        }
                        termCtrl.UpdateTerm (term);
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.LogException (new Exception ($"Error updating {termName} term.", ex));
            }
        }

        public static void DeleteTerm (int termId)
        {
            var termCtrl = new TermController ();
            var term = termCtrl.GetTerm (termId);
            if (term != null) {
                termCtrl.DeleteTerm (term);
            }
        }
    }
}
