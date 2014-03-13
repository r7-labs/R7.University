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

namespace R7.University.Employee
{
	public partial class EmployeeController : ControllerBase, IPortable
	{
		#region Public methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Employee.EmployeeController"/> class.
		/// </summary>
		public EmployeeController ()
		{ 

		}

		#endregion

		#region ModuleSearchBase implementaion

		// DOIT: Implement ModuleSearchBase for Employee module

		public override IList<SearchDocument> GetModifiedSearchDocuments(ModuleInfo modInfo, DateTime beginDate)
		{
			var searchDocs = new List<SearchDocument>();

			// DOIT: Implement IPortable for Employee module

			// var sd = new SearchDocument();
			// searchDocs.Add(searchDoc);

			return searchDocs;
		}

		#endregion


		#region IPortable members

		/// <summary>
		/// Exports a module to XML
		/// </summary>
		/// <param name="ModuleID">a module ID</param>
		/// <returns>XML string with module representation</returns>
		public string ExportModule (int moduleId)
		{
			var sb = new StringBuilder ();
			var infos = GetObjects<EmployeeInfo> (moduleId);

			if (infos != null)
			{
				sb.Append ("<Employees>");
				foreach (var info in infos)
				{
					sb.Append ("<Employee>");
					sb.Append ("<content>");
					sb.Append (XmlUtils.XMLEncode (info.LastName));
					sb.Append ("</content>");
					sb.Append ("</Employee>");
				}
				sb.Append ("</Employees>");
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
			var infos = DotNetNuke.Common.Globals.GetContent (Content, "Employees");
		
			foreach (XmlNode info in infos.SelectNodes("Employee"))
			{
				var item = new EmployeeInfo ();

				item.LastName = info.SelectSingleNode ("lastname").InnerText;
				item.CreatedByUserID = UserID;

				Add<EmployeeInfo> (item);
			}
		}

		#endregion
	}
}

