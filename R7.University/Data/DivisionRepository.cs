//
//  DivisionRepository.cs
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
using R7.University.Models;

namespace R7.University.Data
{
    public class DivisionRepository
    {
        protected Dal2DataProvider DataProvider;

        public DivisionRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        #region Singleton implementation

        private static readonly Lazy<DivisionRepository> instance = new Lazy<DivisionRepository> (
            () => new DivisionRepository (UniversityDataProvider.Instance)
        );

        public static DivisionRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        public IEnumerable<DivisionInfo> FindDivisions (string searchText, int divisionId)
        {
            // TODO: Remove @includeSubdivision argument from sp
            return DataProvider.GetObjectsFromSp<DivisionInfo> ("{databaseOwner}[{objectQualifier}University_FindDivisions]", 
                searchText, true, divisionId);
        }

        public EmployeeInfo GetHeadEmployee (int divisionId, int? headPositionId)
        {
            if (headPositionId != null) {
                return DataProvider.GetObjectsFromSp<EmployeeInfo> ("{databaseOwner}[{objectQualifier}University_GetHeadEmployee]", 
                    divisionId, headPositionId.Value).FirstOrDefault ();
            }

            return null;
        }

        public IEnumerable<DivisionInfo> GetSubDivisions (int divisionId)
        {
            return DataProvider.GetObjects<DivisionInfo> (CommandType.Text,
                @"SELECT DISTINCT D.*, DH.[Level], DH.[Path] 
                    FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D 
                    INNER JOIN {databaseOwner}[{objectQualifier}University_DivisionsHierarchy] (@0) AS DH
                        ON D.DivisionID = DH.DivisionID
                    ORDER BY DH.[Path], D.Title", divisionId);
        }

        public IEnumerable<DivisionInfo> GetRootDivisions ()
        {
            return DataProvider.GetObjects<DivisionInfo> ("WHERE [ParentDivisionID] IS NULL");
        }

        public IEnumerable<DivisionInfo> GetDivisions (IEnumerable<int> divisionIds)
        {
            if (divisionIds != null && divisionIds.Any ()) {
                return DataProvider.GetObjects<DivisionInfo> (
                    string.Format ("WHERE DivisionID IN ({0})", TextUtils.FormatList (",", divisionIds))
                );
            }

            return Enumerable.Empty<DivisionInfo> ();
        }
    }
}
