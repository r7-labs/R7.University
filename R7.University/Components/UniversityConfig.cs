//
//  UniversityConfig.cs
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
