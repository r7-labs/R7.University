//
//  WorkingHoursLogic.cs
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

using System;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using R7.University.Components;
using R7.University.Configuration;
using R7.University.ControlExtensions;

namespace R7.University.SharedLogic
{
    public static class WorkingHoursLogic
    {
        public static void Init (PortalModuleBase module, DropDownList comboWorkingHours)
        {
            // fill working hours terms
            var termCtrl = new TermController ();
            var workingHours = termCtrl.GetTermsByVocabulary (UniversityConfig.Instance.Vocabularies.WorkingHours).ToList ();
            workingHours.Insert (0, new Term (Localization.GetString ("NotSelected.Text", module.LocalResourceFile)));
            comboWorkingHours.DataSource = workingHours;
            comboWorkingHours.DataBind ();
        }

        public static void Load (DropDownList comboWorkingHours, TextBox textWorkingHours, string workingHours)
        {
            // search for working hours text in a combo
            comboWorkingHours.SelectByText (workingHours, StringComparison.CurrentCultureIgnoreCase);
            if (comboWorkingHours.SelectedIndex <= 0)
                textWorkingHours.Text = workingHours;
        }

        // TODO: Need separate methods to get and update
        public static string Update (DropDownList comboWorkingHours, string workingHours, bool addToVocabulary)
        {
            workingHours = workingHours.Trim ();
            var workingHoursNonEmpty = !string.IsNullOrWhiteSpace (workingHours);

            if (comboWorkingHours.SelectedIndex <= 0 || workingHoursNonEmpty) {
                // TODO: Shouldn't we try to add term after updating main item?
                if (addToVocabulary && workingHoursNonEmpty) {
                    // try add new term to working hours vocabulary
                    var vocCtrl = new VocabularyController ();
                    var voc = vocCtrl.GetVocabularies ().SingleOrDefault (v => v.Name == UniversityConfig.Instance.Vocabularies.WorkingHours);
                    if (voc != null) {
                        var termCtrl = new TermController ();
                        termCtrl.AddTerm (new Term (workingHours, "", voc.VocabularyId));
                        vocCtrl.ClearVocabularyCache ();
                    }
                }

                return workingHours;
            }

            // else: get working hours from a combo
            return comboWorkingHours.SelectedItem.Text;
        }
    }
}
