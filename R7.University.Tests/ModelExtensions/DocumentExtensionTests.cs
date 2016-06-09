//
//  DocumentExtensionTests.cs
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
using R7.University.Data;
using R7.University.ModelExtensions;
using R7.University.Models;
using Xunit;

namespace R7.University.Tests.ModelExtensions
{
    public class DocumentExtensionsTests
    {
        [Fact]
        public void WithDocumentType_Test ()
        {
            const string typeName = "TypeName";

            var documentTypes = new List<IDocumentType> { 
                new DocumentTypeInfo {
                    DocumentTypeID = 1,
                    Type = typeName
                }
            };

            var documents = new List<IDocument> { 
                new DocumentInfo {
                    DocumentTypeID = 1
                },
                new DocumentInfo {
                    DocumentTypeID = 3
                }
            };

            var documentsWithType = documents.WithDocumentType (documentTypes);

            // document type exist, DocumentType property should contain object reference
            Assert.Equal (typeName, documentsWithType.ElementAt (0).DocumentType.Type);

            // document type doesn't exist, there shouldn't be second document object in the collection
            Assert.Equal (1, documentsWithType.Count ());
        }
    }
}

