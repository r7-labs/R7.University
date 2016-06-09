//
//  LaunchpadTables.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2015 Roman M. Yagodin
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
using System.Linq;
using System.Collections.Generic;

namespace R7.University.Launchpad
{
    public class LaunchpadTables
    {
        public List<LaunchpadTableBase> Tables = new List<LaunchpadTableBase> ()
        {
            // add new tables contructors here!
            new PositionsTable (),
            new DivisionsTable (),
            new EmployeesTable (),
            new AchievementsTable (),
            new EduLevelsTable (),
            new EduProgramsTable (),
            new EduProgramProfilesTable (),
            new DocumentTypesTable (),
            new DocumentsTable (),
            new EduFormsTable ()
        };

        public List<string> TableNames
        {
            get { return Tables.Select (t => t.Name).ToList (); }
        }

        protected Dictionary<string, LaunchpadTableBase> GridsDictionary;

        protected Dictionary<string, LaunchpadTableBase> NamesDictionary;

        public LaunchpadTables ()
        {
            NamesDictionary = new Dictionary<string, LaunchpadTableBase> ();
            foreach (var table in Tables)
                NamesDictionary.Add (table.Name, table);
        }

        public void InitGridsDictionary ()
        {
            GridsDictionary = new Dictionary<string, LaunchpadTableBase> ();
            foreach (var table in Tables) {
                if (table.Grid == null)
                    throw new InvalidOperationException ("Null GridView reference. Note that LaunchpadTables.InitGridsDictionary() method should be called after LaunchpadTableBase.Init()!");

                GridsDictionary.Add (table.Grid.ID, table);
            }
        }

        public LaunchpadTableBase GetByName (string name)
        {
            return NamesDictionary [name];
        }

        public LaunchpadTableBase GetByGridId (string gridId)
        {
            return GridsDictionary [gridId];
        }
    }
}
