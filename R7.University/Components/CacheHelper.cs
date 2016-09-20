//
//  CacheHelper.cs
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
using System.Collections.ObjectModel;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Cache;

namespace R7.University.Components
{
    public static class CacheHelper
    {
        public static bool Exists (string key)
        {
            return DataCache.GetCache (key) != null;
        }

        public static void Set<T> (T toSet, string key, int seconds)
        {
            DataCache.SetCache (key, toSet, TimeSpan.FromSeconds (seconds)); 
        }

        public static T Get<T> (string key)
        {
            return (T) DataCache.GetCache (key);
        }

        public static T TryGet<T> (string key, T defValue)
        {
            var obj = DataCache.GetCache (key);
            if (obj != null)
                return (T) obj;

            return defValue;
        }

        /// <summary>
        /// Remove all cache keys with specified prefix
        /// </summary>
        /// <param name="cacheKeyPrefix">Cache key prefix.</param>
        public static void RemoveCacheByPrefix (string cacheKeyPrefix)
        {
            // get all cache keys with s
            var cacheKeys = new Collection<string> ();
            var cacheEnumerator = CachingProvider.Instance ().GetEnumerator ();

            while (cacheEnumerator.MoveNext ()) {
                var cacheKey = cacheEnumerator.Key.ToString ();
                if (cacheKey.StartsWith ("DNN_" + cacheKeyPrefix, StringComparison.InvariantCultureIgnoreCase)) {
                    cacheKeys.Add (cacheKey);
                }
            }

            foreach (var cacheKey in cacheKeys) {
                // Substring (4) removes DNN_ prefix 
                DataCache.RemoveCache (cacheKey.Substring (4));
            }
        }
    }
}
