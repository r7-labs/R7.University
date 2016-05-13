//
// UniversityConfig.cs
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
using System.IO;
using System.Collections.Concurrent;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace R7.University.Components
{
    public static class UniversityConfig
    {
        #region Singleton implementation

        private static readonly ConcurrentDictionary<int,Lazy<UniversityPortalConfig>> portalConfigs = 
            new ConcurrentDictionary<int,Lazy<UniversityPortalConfig>> ();

        public static UniversityPortalConfig Instance
        {
            get { return GetInstance (PortalSettings.Current.PortalId); }
        }

        public static UniversityPortalConfig GetInstance (int portalId)
        {
            var lazyPortalConfig = portalConfigs.GetOrAdd (portalId, newKey => 
                new Lazy<UniversityPortalConfig> (() => {

                    var portalSettings = new PortalSettings (portalId);
                    var portalConfigFile = Path.Combine (portalSettings.HomeDirectoryMapPath, "R7.University.yml");

                    // ensure portal config file exists
                    if (!File.Exists (portalConfigFile)) {
                        File.Copy (Path.Combine (
                            Globals.ApplicationMapPath,
                            "DesktopModules\\R7.University\\R7.University\\R7.University.yml"), 
                            portalConfigFile);
                    }

                    using (var configReader = new StringReader (File.ReadAllText (portalConfigFile))) {
                        var deserializer = new Deserializer (namingConvention: new HyphenatedNamingConvention ());
                        return deserializer.Deserialize<UniversityPortalConfig> (configReader);
                    }
                }
                ));

            return lazyPortalConfig.Value;
        }

        #endregion
    }
}
