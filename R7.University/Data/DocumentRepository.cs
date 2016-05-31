//
// DocumentRepository.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
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
using DotNetNuke.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.University.Components;
using R7.University.Models;

namespace R7.University.Data
{
    public class DocumentRepository
    {
        #region Singleton implementation

        private static readonly Lazy<DocumentRepository> instance = new Lazy<DocumentRepository> ();

        public static DocumentRepository Instance
        {
            get { return instance.Value; }
        }

        private Dal2DataProvider dataProvider;

        protected Dal2DataProvider DataProvider
        {
            get { return dataProvider ?? (dataProvider = new Dal2DataProvider ()); }
        }

        #endregion

        public IEnumerable<IDocument> GetDocuments_ForItemType (string itemType)
        {
            return DataProvider.GetObjects<DocumentInfo> ("WHERE ItemID LIKE @0", itemType + "=%");
        }

        public IEnumerable<IDocument> GetDocuments (string itemId)
        {
            return DataProvider.GetObjects<DocumentInfo> ("WHERE ItemID = @0", itemId);
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

