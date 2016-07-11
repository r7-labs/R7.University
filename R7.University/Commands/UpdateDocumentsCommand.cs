//
//  UpdateDocumentsCommand.cs
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
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Commands
{
    public class UpdateDocumentsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateDocumentsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void UpdateDocuments (IList<DocumentInfo> documents, string model, int itemId)
        {
            var originalDocuments = default (IList<DocumentInfo>);

            if (model == "EduProgram") {
                originalDocuments = ModelContext.Query<DocumentInfo> ()
                    .Where (d => d.EduProgramId == itemId)
                    .ToList ();
            }
            else if (model == "EduProgramProfile") {
                originalDocuments = ModelContext.Query<DocumentInfo> ()
                    .Where (d => d.EduProgramProfileId == itemId)
                    .ToList ();
            }
            else {
                throw new ArgumentException ("Wrong model argument.");
            }

            foreach (var document in documents) {
                var originalDocument = originalDocuments.SingleOrDefault (d => d.DocumentID == document.DocumentID);
                if (originalDocument == null) {
                    document.SetModelId (model, itemId);
                    ModelContext.Add<DocumentInfo> (document);
                }
                else {
                    ModelContext.Update<DocumentInfo> (originalDocument);

                    // do not delete this document later
                    originalDocuments.Remove (originalDocument);
                }
            }

            // should delete all remaining documents
            foreach (var originalDocument in originalDocuments) {
                ModelContext.Remove<DocumentInfo> (originalDocument);
            }
        }
    }
}

