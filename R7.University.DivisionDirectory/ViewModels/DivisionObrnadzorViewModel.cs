//
// DivisionObrnadzorViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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
using DotNetNuke.ExtensionPoints;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace R7.University.DivisionDirectory
{
    public class DivisionObrnadzorViewModel: DivisionInfo
    {
        #region Properties
            
        protected ViewModelContext Context { get; set; }

        public string Order { get; protected set; }

        public string TitleLink { get; set; }

        public string HeadEmployee { get; set; }

        public string WebSiteLink { get; set; }

        public string EmailLink { get; set; }

        public string DocumentUrlLink { get; set; }

        #endregion

        public DivisionObrnadzorViewModel (DivisionInfo division, ViewModelContext context)
        {
            CopyCstor.Copy<DivisionInfo> (division, this);
            Context = context;
        }

        /// <summary>
        /// Calculates the hierarchical order of divisions.
        /// </summary>
        /// <param name="divisions">Divisions. Must be sorted properly.</param>
        public static void CalculateOrder (IEnumerable<DivisionObrnadzorViewModel> divisions)
        {
            const string separator = ".";
            var orderCounter = 1;
            var orderStack = new List<int> ();
           
            DivisionObrnadzorViewModel prevDivision = null;

            foreach (var division in divisions)
            {
                if (prevDivision != null)
                {
                    // moving on same level
                    if (division.ParentDivisionID == prevDivision.ParentDivisionID)
                    {
                        orderCounter++;
                    }
                    // moving down
                    else if (division.ParentDivisionID == prevDivision.DivisionID)
                    {
                        orderStack.Add (orderCounter);
                        orderCounter = 1;
                    }
                    // moving up
                    else
                    {
                        orderCounter = orderStack [orderStack.Count - 1];
                        orderStack.RemoveAt (orderStack.Count - 1);
                        orderCounter++;
                    }
                }

                // format order value
                if (orderStack.Count == 0)
                {
                    division.Order = orderCounter + separator;
                }
                else
                {
                    division.Order = Utils.FormatList (separator, orderStack) + separator + orderCounter + separator;
                }

                prevDivision = division;
            }
        }
    }
}

