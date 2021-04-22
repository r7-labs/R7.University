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
using R7.University.Configuration;
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

        public static int? AddOrUpdateTerm (this IDivision division, IModelContext modelContext)
        {
            var vocabularyName = UniversityConfig.Instance.Vocabularies.OrgStructure;
            var vocabulary = new VocabularyController ().GetVocabularies ().FirstOrDefault (v => v.Name == vocabularyName);
            if (vocabulary == null) {
                Exceptions.LogException (new Exception ($"Could not find the {vocabularyName} vocabulary."));
                return null;
            }

            var termName = GetSafeTermName (division.ShortTitle, division.Title);
            var termCtrl = new TermController ();
            var term = default (Term);

            if (division.DivisionTermID == null) {
                term = termCtrl.GetTermsByVocabulary (vocabulary.VocabularyId).FirstOrDefault (t => t.Name == termName);
                if (term != null) {
                    Exceptions.LogException (new Exception ($"Could not create term {termName} in the {vocabularyName} vocabulary as it already exists."));
                    return null;
                }

                term = new Term (termName, string.Empty, vocabulary.VocabularyId);
            }
            else {
                term = termCtrl.GetTerm (division.DivisionTermID.Value);
                term.Name = termName;
            }

            var parentDivision = division.GetParentDivision (modelContext);
            if (parentDivision != null) {
                term.ParentTermId = parentDivision.DivisionTermID;
            }
            else {
                term.ParentTermId = null;
            }

            try {
                if (division.DivisionTermID == null) {
                    return termCtrl.AddTerm (term);
                }

                termCtrl.UpdateTerm (term);
                return term.TermId;
            }
            catch (Exception ex) {
                Exceptions.LogException (new Exception ($"Error creating/updating {termName} term in the vocabulary {vocabularyName}.", ex));
            }

            return null;
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
