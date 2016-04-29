//
// WorkingHours.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using R7.University.ControlExtensions;

namespace R7.University.SharedLogic
{
    public static class WorkingHoursLogic
    {
        public static void Init (PortalModuleBase module, DropDownList comboWorkingHours)
        {
            // fill working hours terms
            var termCtrl = new TermController ();
            var workingHours = termCtrl.GetTermsByVocabulary ("University_WorkingHours").ToList ();
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

        public static string Update (DropDownList comboWorkingHours, string workingHours, bool addToVocabulary)
        {
            workingHours = workingHours.Trim ();
            var workingHoursNonEmpty = !string.IsNullOrWhiteSpace (workingHours);

            if (comboWorkingHours.SelectedIndex <= 0 || workingHoursNonEmpty) {
                // REVIEW: Shouldn't we try to add term after updating main item?
                if (addToVocabulary && workingHoursNonEmpty) {
                    // try add new term to University_WorkingHours vocabulary
                    var vocCtrl = new VocabularyController ();
                    var voc = vocCtrl.GetVocabularies ().SingleOrDefault (v => v.Name == "University_WorkingHours");
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
