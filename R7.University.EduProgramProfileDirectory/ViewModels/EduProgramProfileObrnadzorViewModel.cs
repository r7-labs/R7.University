//
// EduProgramProfileObrnadzorViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.Localization;

namespace R7.University.EduProgramProfileDirectory
{
    public class EduProgramProfileObrnadzorViewModel: IEduProgramProfile
    {
        #region IEduProgramProfile implementation

        public int EduProgramProfileID 
        { 
            get { return Model.EduProgramProfileID; }
            set {}
        }

        public int EduProgramID 
        { 
            get { return Model.EduProgramID; }
            set {}
        }
      
        public string ProfileCode 
        { 
            get { return Model.ProfileCode; }
            set {}
        }

        public string ProfileTitle 
        { 
            get { return Model.ProfileTitle; }
            set {}
        }

        public DateTime? AccreditedToDate 
        { 
            get { return Model.AccreditedToDate; }
            set {}
        }

        public DateTime? CommunityAccreditedToDate 
        { 
            get { return Model.CommunityAccreditedToDate; }
            set { }
        }

        public DateTime? StartDate
        { 
            get { return Model.StartDate; }
            set {}
        }

        public DateTime? EndDate 
        {
            get { return Model.EndDate; }
            set {}
        }

        public EduProgramInfo EduProgram
        {
            get { return Model.EduProgram; }
            set {}
        }
       
        public IList<IEduProgramProfileForm> EduProgramProfileForms
        {
            get { return Model.EduProgramProfileForms; }
            set {}
        }

        #endregion

        public IEduProgramProfile Model { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramProfileObrnadzorViewModel (IEduProgramProfile model, ViewModelContext context, ViewModelIndexer indexer)
        {
            Model = model;
            Context = context;
            Index = indexer.GetNextIndex ();
        }

        protected IEduProgramProfileForm FullTimeForm
        {
            get 
            { 
                return EduProgramProfileForms.FirstOrDefault (eppf => 
                    eppf.EduForm.GetSystemEduForm () == SystemEduForm.FullTime); 
            }
        }

        protected IEduProgramProfileForm PartTimeForm
        {
            get 
            { 
                return EduProgramProfileForms.FirstOrDefault (eppf => 
                    eppf.EduForm.GetSystemEduForm () == SystemEduForm.PartTime); 
            }
        }

        protected IEduProgramProfileForm ExtramuralForm
        {
            get 
            { 
                return EduProgramProfileForms.FirstOrDefault (eppf => 
                    eppf.EduForm.GetSystemEduForm () == SystemEduForm.Extramural); 
            }
        }

        public string TimeToLearnFullTimeString
        {
            get
            { 
                if (FullTimeForm == null) {
                    return string.Empty; 
                }

                return FormatHelper.FormatTimeToLearn (FullTimeForm.TimeToLearn,
                    Localization.GetString ("TimeToLearnFull.Format", Context.LocalResourceFile),
                    Localization.GetString ("TimeToLearnYears.Format", Context.LocalResourceFile),
                    Localization.GetString ("TimeToLearnMonths.Format", Context.LocalResourceFile)
                );
            }
        }

        public string TimeToLearnPartTimeString
        {
            get
            {
                if (PartTimeForm == null) {
                    return string.Empty; 
                }

                return FormatHelper.FormatTimeToLearn (PartTimeForm.TimeToLearn,
                    Localization.GetString ("TimeToLearnFull.Format", Context.LocalResourceFile),
                    Localization.GetString ("TimeToLearnYears.Format", Context.LocalResourceFile),
                    Localization.GetString ("TimeToLearnMonths.Format", Context.LocalResourceFile)
                );
            }
        }

        public string TimeToLearnExtramuralString
        {
            get
            {
                if (ExtramuralForm == null) {
                    return string.Empty; 
                }

                return FormatHelper.FormatTimeToLearn (ExtramuralForm.TimeToLearn,
                    Localization.GetString ("TimeToLearnFull.Format", Context.LocalResourceFile),
                    Localization.GetString ("TimeToLearnYears.Format", Context.LocalResourceFile),
                    Localization.GetString ("TimeToLearnMonths.Format", Context.LocalResourceFile)
                );
            }
        }

        public int Index { get; protected set; }

        public string IndexString
        {
            get { return Index + "."; }
        }

        public string Code
        {
            get { return EduProgram.Code; }
        }

        public string Title
        {
            get { return FormatHelper.FormatEduProgramProfileTitle (EduProgram.Title, ProfileCode, ProfileTitle); }
        }

        public string EduLevelString
        {
            get { return EduProgram.EduLevel.Title; }
        }
    }
}
