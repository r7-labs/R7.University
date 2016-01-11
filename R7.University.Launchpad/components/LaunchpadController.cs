using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using DotNetNuke.Collections;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using DotNetNuke.Services.Search.Entities;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class LaunchpadController : UniversityControllerBase
	{
		#region Public methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Launchpad.LaunchpadController"/> class.
		/// </summary>
		public LaunchpadController ()
		{ 
	
		}

		#endregion

		#region ModuleSearchBase implementaion

		public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo modInfo, DateTime beginDate)
		{
			var searchDocs = new List<SearchDocument> ();

			// TODO: Implement GetModifiedSearchDocuments()

			/* var sd = new SearchDocument();
			searchDocs.Add(searchDoc);
			*/
					
			return searchDocs;
		}

		#endregion
    }
}

