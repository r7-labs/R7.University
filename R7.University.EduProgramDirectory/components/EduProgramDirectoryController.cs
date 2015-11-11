using System;
using System.Data;
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

namespace R7.University.EduProgramDirectory
{
    public partial class EduProgramDirectoryController : UniversityControllerBase, IPortable
	{
		#region Public methods

		/// <summary>
		/// Initializes a new instance of the <see cref="EduProgramDirectory.EduProgramDirectoryController"/> class.
		/// </summary>
		public EduProgramDirectoryController ()
		{ 

		}

		#endregion

		#region ModuleSearchBase implementaion

		public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo modInfo, DateTime beginDate)
		{
			var searchDocs = new List<SearchDocument> ();
			
            return searchDocs;
		}

		#endregion


		#region IPortable members

		// TODO: Implement IPortable for EduProgramDirectory module

		/// <summary>
		/// Exports a module to XML
		/// </summary>
		/// <param name="ModuleID">a module ID</param>
		/// <returns>XML string with module representation</returns>
		public string ExportModule (int moduleId)
		{
			var sb = new StringBuilder ();
			var infos = GetObjects<EmployeeInfo> (moduleId);

            if (infos.Any ())
			{
				sb.Append ("<EduProgramDirectorys>");
				foreach (var info in infos)
				{
					sb.Append ("<EduProgramDirectory>");
					sb.Append ("<content>");
					sb.Append (XmlUtils.XMLEncode (info.FullName));
					sb.Append ("</content>");
					sb.Append ("</EduProgramDirectory>");
				}
				sb.Append ("</EduProgramDirectorys>");
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
			var infos = DotNetNuke.Common.Globals.GetContent (Content, "EduProgramDirectorys");
		
			foreach (XmlNode info in infos.SelectNodes("Employee"))
			{
				var item = new EmployeeInfo ();
				item.FirstName = info.SelectSingleNode ("firstname").InnerText;
				item.CreatedByUserID = UserID;

				Add<EmployeeInfo> (item);
			}
		}

		#endregion
	}
}

