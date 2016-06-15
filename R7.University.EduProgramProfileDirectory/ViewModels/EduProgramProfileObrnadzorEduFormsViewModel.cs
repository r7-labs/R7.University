//
//  EduProgramProfileObrnadzorEduFormsViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ViewModels;
using R7.University.Models;

namespace R7.University.EduProgramProfileDirectory.ViewModels
{
    public class EduProgramProfileObrnadzorEduFormsViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramProfileDirectoryEduFormsViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }

        public ViewModelIndexer Indexer { get; protected set; }

        public EduProgramProfileObrnadzorEduFormsViewModel (
            IEduProgramProfile model,
            EduProgramProfileDirectoryEduFormsViewModel rootViewModel,
            ViewModelIndexer indexer): base (model)
        {
            RootViewModel = rootViewModel;
            Indexer = indexer;
        }

        protected IEduProgramProfileForm FullTimeForm
        {
            get { 
                return EduProgramProfileForms.FirstOrDefault (eppf => 
                    eppf.EduForm.GetSystemEduForm () == SystemEduForm.FullTime); 
            }
        }

        protected IEduProgramProfileForm PartTimeForm
        {
            get { 
                return EduProgramProfileForms.FirstOrDefault (eppf => 
                    eppf.EduForm.GetSystemEduForm () == SystemEduForm.PartTime); 
            }
        }

        protected IEduProgramProfileForm ExtramuralForm
        {
            get { 
                return EduProgramProfileForms.FirstOrDefault (eppf => 
                    eppf.EduForm.GetSystemEduForm () == SystemEduForm.Extramural); 
            }
        }

        protected string TimeToLearnApplyMarkup (string eduFormResourceKey, string timeToLearn)
        {
            return "<span class=\"hidden\" itemprop=\"EduForm\">"
            + Localization.GetString (eduFormResourceKey, Context.LocalResourceFile)
            + "</span>" + "<span itemprop=\"LearningTerm\">" + timeToLearn + "</span>";
        }

        public string TimeToLearnFullTimeString
        {
            get { 
                if (FullTimeForm == null) {
                    return string.Empty; 
                }

                return TimeToLearnApplyMarkup ("TimeToLearnFullTime.Column",
                    FormatHelper.FormatTimeToLearn (FullTimeForm.TimeToLearn,
                        "TimeToLearnYears.Format", "TimeToLearnMonths.Format", Context.LocalResourceFile)
                );
            }
        }

        public string TimeToLearnPartTimeString
        {
            get {
                if (PartTimeForm == null) {
                    return string.Empty; 
                }

                return TimeToLearnApplyMarkup ("TimeToLearnPartTime.Column",
                    FormatHelper.FormatTimeToLearn (PartTimeForm.TimeToLearn,
                        "TimeToLearnYears.Format", "TimeToLearnMonths.Format", Context.LocalResourceFile)
                );
            }
        }

        public string TimeToLearnExtramuralString
        {
            get {
                if (ExtramuralForm == null) {
                    return string.Empty; 
                }

                return TimeToLearnApplyMarkup ("TimeToLearnExtramural.Column",
                    FormatHelper.FormatTimeToLearn (ExtramuralForm.TimeToLearn,
                        "TimeToLearnYears.Format", "TimeToLearnMonths.Format", Context.LocalResourceFile)
                );
            }
        }

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string Code
        {
            get { return "<span itemprop=\"EduCode\">" + EduProgram.Code + "</span>"; }
        }

        public string Title
        {
            get { return FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle); }
        }

        public string EduLevelString
        {
            get { return "<span itemprop=\"EduLevel\">" + EduProgram.EduLevel.Title + "</span>"; }
        }

        public string AccreditedToDateString
        {
            get { 
                if (AccreditedToDate != null) {
                    return "<span itemprop=\"DateEnd\">" + AccreditedToDate.Value.ToShortDateString () + "</span>";
                }

                return string.Empty;
            }
        }
    }
}
