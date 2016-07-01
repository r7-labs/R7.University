//
//  DocumentRepository.cs
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

namespace R7.University.Data
{
    public class DocumentRepository
    {
        protected Dal2DataProvider DataProvider;

        public DocumentRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        #region Singleton implementation

        private static readonly Lazy<DocumentRepository> instance = new Lazy<DocumentRepository> (
            () => new DocumentRepository (UniversityDataProvider.Instance)
        );

        public static DocumentRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        public IEnumerable<DocumentInfo> GetDocuments_ForItemType (string itemType)
        {
            return DataProvider.GetObjects<DocumentInfo> (string.Format ("WHERE ItemID LIKE N'{0}=%'", itemType));
        }

        public IEnumerable<DocumentInfo> GetDocuments (string itemId)
        {
            return DataProvider.GetObjects<DocumentInfo> ("WHERE ItemID = @0", itemId);
        }

        public IEnumerable<DocumentInfo> FindDocuments (string search)
        {
            return DataProvider.FindObjects<DocumentInfo> (
                @"WHERE CONCAT([ItemID], ' ', [Title], ' ', [Url]) LIKE N'%{0}%'", search, false);
        }

        public void UpdateDocuments (IList<DocumentInfo> documents, string itemKey, int itemId)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    var originalDocuments = GetDocuments (string.Format ("{0}={1}", itemKey, itemId)).ToList ();

                    foreach (var document in documents) {
                        if (document.DocumentID <= 0) {
                            document.ItemID = itemKey + "=" + itemId;
                            DataProvider.Add<DocumentInfo> (document);
                        }
                        else {
                            DataProvider.Update<DocumentInfo> (document);

                            // documents with same ID could be different objects!
                            var updatedDocument = originalDocuments.FirstOrDefault (d => d.DocumentID == document.DocumentID);
                            if (updatedDocument != null) {
                                // do not delete this document later
                                originalDocuments.Remove (updatedDocument);
                            }
                        }
                    }

                    // delete remaining documents
                    foreach (var document in originalDocuments) {
                        DataProvider.Delete<DocumentInfo> (document.DocumentID);
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

