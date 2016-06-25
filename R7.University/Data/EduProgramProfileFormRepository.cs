//
//  EduProgramProfileFormRepository.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using DotNetNuke.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.University.Components;
using R7.University.Models;
using R7.University.ModelExtensions;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Data
{
    public class EduProgramProfileFormRepository
    {
        #region Singleton implementation

        private static readonly Lazy<EduProgramProfileFormRepository> instance = new Lazy<EduProgramProfileFormRepository> (
            () => new EduProgramProfileFormRepository (UniversityDataProvider.Instance)
        );

        public static EduProgramProfileFormRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        protected Dal2DataProvider DataProvider;

        public EduProgramProfileFormRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public IEnumerable<EduProgramProfileFormInfo> GetEduProgramProfileForms (IEnumerable<IEduProgramProfile> eduProgramProfiles)
        {
            if (!eduProgramProfiles.IsNullOrEmpty ()) {
                return DataProvider.GetObjects<EduProgramProfileFormInfo> (
                    string.Format ("WHERE EduProgramProfileID IN ({0})",
                        TextUtils.FormatList (",", eduProgramProfiles.Select (epp => epp.EduProgramProfileID)))
                );
            }

            return Enumerable.Empty<EduProgramProfileFormInfo> ();
        }

        public void UpdateEduProgramProfileForms (IList<EduProgramProfileFormInfo> eduForms, int eduProgramProfileId)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    var originalEduForms = DataProvider.GetObjects<EduProgramProfileFormInfo> (
                        "WHERE EduProgramProfileID = @0", eduProgramProfileId).ToList ();

                    foreach (var eduForm in eduForms) {
                        if (eduForm.EduProgramProfileFormID <= 0) {
                            eduForm.EduProgramProfileID = eduProgramProfileId;
                            DataProvider.Add<EduProgramProfileFormInfo> (eduForm);
                        }
                        else {
                            DataProvider.Update<EduProgramProfileFormInfo> (eduForm);

                            // objects with same ID could be different!
                            var updatedEduForm = originalEduForms.FirstOrDefault (ef => 
                                ef.EduProgramProfileFormID == eduForm.EduProgramProfileFormID);

                            if (updatedEduForm != null) {
                                // do not delete this object later
                                originalEduForms.Remove (updatedEduForm);
                            }
                        }
                    }

                    // delete remaining items
                    foreach (var eduForm in originalEduForms) {
                        DataProvider.Delete<EduProgramProfileFormInfo> (eduForm);
                    }

                    ctx.Commit ();
                    CacheHelper.RemoveCacheByPrefix ("//r7_University");
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

    }
}

