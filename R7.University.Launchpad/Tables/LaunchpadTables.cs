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
            new AchievementTypesTable (),
            new EduLevelsTable (),
            new EduProgramsTable (),
            new EduProfilesTable (),
            new DocumentTypesTable (),
            new EduFormsTable (),
            new YearsTable ()
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
            return NamesDictionary.TryGetValue(name, out var table) ? table : null;
        }

        public LaunchpadTableBase GetByGridId (string gridId)
        {
            return GridsDictionary.TryGetValue (gridId, out var table) ? table : null;
        }
    }
}
