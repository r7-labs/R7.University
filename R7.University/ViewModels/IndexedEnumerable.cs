//
// TeacherCollection.cs
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
using System.Collections;
using System.Collections.Generic;

namespace R7.University.ViewModels
{
    public class IndexedEnumerable<T>: IEnumerable<T>
    {
        public IIndexer Indexer { get; protected set; }

        protected IEnumerable<T> Collection;

        public IndexedEnumerable (IIndexer indexer, IEnumerable<T> collection)
        {
            Collection = collection;
            Indexer = indexer;
        }

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator ()
        {
            Indexer.Reset ();
            return Collection.GetEnumerator ();
        }
    
        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator ()
        {
            Indexer.Reset ();
            return Collection.GetEnumerator ();
        }

        #endregion
    }
}

