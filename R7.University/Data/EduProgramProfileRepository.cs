//
//  EduProgramProfileRepository.cs
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
using System.Data;
using System.Linq;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.ModelExtensions;

namespace R7.University.Data
{
    public class EduProgramProfileRepository
    {
        #region Singleton implementation

        private static readonly Lazy<EduProgramProfileRepository> instance = new Lazy<EduProgramProfileRepository> (
            () => new EduProgramProfileRepository (UniversityDataProvider.Instance)
        );

        public static EduProgramProfileRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        protected Dal2DataProvider DataProvider;

        public EduProgramProfileRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public EduProgramProfileInfo Get (int eduProgramProfileId)
        {
            return DataProvider.Get<EduProgramProfileInfo> (eduProgramProfileId)
                .WithEduProgram ();
        }

        public IEnumerable<EduProgramProfileInfo> GetEduProgramProfiles_ByEduLevel (int eduLevelId)
        {
            return DataProvider.GetObjects<EduProgramProfileInfo> (
                    "WHERE EduLevelID = @0", eduLevelId)
                    .WithEduProgram ()
                    // TODO: Move sorting ouside extension method
                    .OrderBy (epp => epp.EduProgram.Code)
                    .ThenBy (epp => epp.ProfileCode)
                    .ThenBy (epp => epp.ProfileTitle);
        }

        public IEnumerable<EduProgramProfileInfo> GetEduProgramProfiles_ByEduLevels (IEnumerable<int> eduLevelIds)
        {
            if (eduLevelIds.Any ()) {
                return DataProvider.GetObjects<EduProgramProfileInfo> (
                    "WHERE EduLevelID IN (" + TextUtils.FormatList (",", eduLevelIds) + ")")
                    .WithEduProgram ();
            }

            return Enumerable.Empty<EduProgramProfileInfo> ();
        }

        public IEnumerable<EduProgramProfileInfo> FindEduProgramProfiles (string search)
        {
            return DataProvider.GetObjects<EduProgramProfileInfo> (CommandType.Text,
                @"SELECT EPP.* FROM {databaseOwner}[{objectQualifier}University_EduProgramProfiles] AS EPP
                    INNER JOIN {databaseOwner}[{objectQualifier}University_EduPrograms] AS EP 
                        ON EPP.EduProgramID = EP.EduProgramID
                    WHERE CONCAT(EPP.ProfileCode, ' ', EPP.ProfileTitle, ' ', EP.Code, ' ', EP.Title) 
                        LIKE N'%" + search + "%'")
                .WithEduProgram ();
        }
    }
}
