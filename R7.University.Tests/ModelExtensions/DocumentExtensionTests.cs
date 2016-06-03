//
// DocumentExtensionsTests.cs
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

