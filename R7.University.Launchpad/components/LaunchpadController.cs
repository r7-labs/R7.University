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
	public partial class LaunchpadController : UniversityControllerBase, IPortable
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

		#region IPortable implementation

		/// <summary>
		/// Exports a module to XML
		/// </summary>
		/// <param name="ModuleID">a module ID</param>
		/// <returns>XML string with module representation</returns>
		public string ExportModule (int moduleId)
		{
			var sb = new StringBuilder ();
			var infos = GetObjects<LaunchpadInfo> (moduleId);

            if (infos.Any ())
			{
				sb.Append ("<Launchpads>");
				foreach (var info in infos)
				{
					sb.Append ("<Launchpad>");
					sb.Append ("<content>");
					sb.Append (XmlUtils.XMLEncode (info.Content));
					sb.Append ("</content>");
					sb.Append ("</Launchpad>");
				}
				sb.Append ("</Launchpads>");
			}
			
			return sb.ToString ();
		}

		/// <summary>
		/// Imports a module from an XML
		/// </summary>
		/// <param name="ModuleID"></param>
		/// <param name="Content"></param>
		/// <param name="Version"></param>
		/// <param name="UserID"></param>
		public void ImportModule (int ModuleID, string Content, string Version, int UserID)
		{
			var infos = DotNetNuke.Common.Globals.GetContent (Content, "Launchpads");
		
			foreach (XmlNode info in infos.SelectNodes("Launchpad"))
			{
				var item = new LaunchpadInfo ();
				item.ModuleID = ModuleID;
				item.Content = info.SelectSingleNode ("content").InnerText;
				item.CreatedByUser = UserID;

				Add<LaunchpadInfo> (item);
			}
		}

		#endregion
	}
}

