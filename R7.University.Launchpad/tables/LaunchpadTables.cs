//
// LaunchpadTables.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2015 
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
using System.Linq;
using System.Collections.Generic;

namespace R7.University.Launchpad
{
    public class LaunchpadTables
	{
        public List<LaunchpadTableBase> Tables = new List<LaunchpadTableBase>() {
            // add new tables contructors here!
            new PositionsTable (),
            new DivisionsTable (),
            new EmployeesTable (),
            new AchievementsTable (),
            new EduLevelsTable (),
            new EduProgramsTable (),
            new EduProgramProfilesTable (),
            new DocumentTypesTable ()
        };

        public List<string> TableNames 
        {
            get { return Tables.Select (t => t.Name).ToList (); }
        }

        public Dictionary<string, LaunchpadTableBase> GridsDictionary;

        public Dictionary<string, LaunchpadTableBase> NamesDictionary;
       
        public LaunchpadTables ()
        {
            NamesDictionary = new Dictionary<string, LaunchpadTableBase> ();
            foreach (var table in Tables)
                NamesDictionary.Add (table.Name, table);
        }

        public void InitGridsDictionary ()
        {
            GridsDictionary = new Dictionary<string, LaunchpadTableBase> ();
            foreach (var table in Tables)
            {
                if (table.Grid == null)
                    throw new InvalidOperationException ("Null GridView reference. Note that LaunchpadTables.InitGridsDictionary() method should be called after LaunchpadTableBase.Init()!");

                GridsDictionary.Add (table.Grid.ID, table);
            }
        }
    }
}
