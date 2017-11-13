//
//  ResourcesTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using System.IO;
using System.Resources;
using Xunit;

namespace R7.University.Tests.Resources
{
    public class ResourcesTests
    {
        [Fact]
        public void ResourcesAreValidTest ()
        {
            var resourceFiles = Directory.GetFiles (Path.Combine ("..", "..", ".."), "*.resx", SearchOption.AllDirectories);
            foreach (var resourceFile in resourceFiles) {
                var ex = Record.Exception (() => ReadResourceFile (resourceFile));
                Assert.Null (ex);
            }
        }

        void ReadResourceFile (string resourceFile)
        {
            try {
                using (var resxReader = new ResXResourceReader (resourceFile)) {
                    resxReader.UseResXDataNodes = true;
                    var resxEnumerator = resxReader.GetMetadataEnumerator ();
                    while (resxEnumerator.MoveNext ()) {
                        var resxNode = (ResXDataNode) resxEnumerator.Entry.Value;
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception ($"Resource File Not Valid: {resourceFile}", ex);
            }
        }
    }
}
