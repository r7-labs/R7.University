//
//  IndexedEnumerable.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

