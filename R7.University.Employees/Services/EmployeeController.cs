using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using R7.University.Components;
using R7.University.Employees.Queries;
using R7.University.Models;
using R7.University.Templates;
using R7.University.ViewModels;

// may need the binding redirect in web.config
/*
<dependentAssembly>
    <assemblyIdentity name = "System.Net.Http" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
    <bindingRedirect oldVersion="0.0.0.0-4.2.0.0" newVersion="4.0.0.0" />
</dependentAssembly>
*/

namespace R7.University.Employees.Services
{
    public class EmployeeController: DnnApiController
    {
        IEmployee GetEmployee (int employeeId)
        {
            using (var modelContext = new UniversityModelContext ()) {
                return new EmployeeQuery (modelContext).SingleOrDefault (employeeId);
            }
        }

        WorkbookLiquidTemplateEngine GetEmployeeTemplateEngine (IEmployee employee)
        {
            var employeeBinder = new EmployeeToTemplateBinder (employee, PortalSettings,
                   "~" + UniversityGlobals.INSTALL_PATH + "/R7.University.Employees/App_LocalResources/SharedResources.resx");

            return new WorkbookLiquidTemplateEngine (employeeBinder, new HSSFWorkbookProvider ());
        }

        MemoryStream GetEmployeeExcelStream (IEmployee employee)
        {
            var templateEngine = GetEmployeeTemplateEngine (employee);
            return (MemoryStream) templateEngine.ApplyAndWrite (UniversityTemplateHelper.GetLocalizedEmployeeTemplatePath (), new MemoryStream ());
        }

        string GetEmployeeCsvText (IEmployee employee, IWorkbookSerializer serializer)
        {
            var templateEngine = GetEmployeeTemplateEngine (employee);
            return templateEngine.ApplyAndSerialize (UniversityTemplateHelper.GetLocalizedEmployeeTemplatePath (), serializer).ToString ();
        }

        string GetFileName (IEmployee employee, string extension)
        {
            return UniversityFormatHelper.AbbrName (
                    employee.FirstName, employee.LastName, employee.OtherName)
                    .Replace (".", "").Replace (" ", "_") + extension;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage ExportToExcel (int employeeId)
        {
            try {
                var employee = GetEmployee (employeeId);
                if (employee == null) {
                    return Request.CreateResponse (HttpStatusCode.NotFound);
                }

                var result = Request.CreateResponse (HttpStatusCode.OK);
                var stream = GetEmployeeExcelStream (employee);
                var buffer = stream.ToArray ();
                result.Content = new ByteArrayContent (buffer);
                // TODO: Introduce helper for MIME types?
                result.Content.Headers.ContentType = new MediaTypeHeaderValue ("application/vnd.ms-excel");
                result.Content.Headers.ContentLength = buffer.Length;
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue ("attachment") {
                    FileName = GetFileName (employee, ".xls")
                };

                return result;
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [DnnAuthorize]
        public HttpResponseMessage ExportToCsv (int employeeId)
        {
            try {
                if (!UserInfo.IsSuperUser && !UserInfo.IsInRole ("Administrators")) {
                    return Request.CreateResponse (HttpStatusCode.Forbidden);
                }

                var employee = GetEmployee (employeeId);
                if (employee == null) {
                    return Request.CreateResponse (HttpStatusCode.NotFound);
                }

                var result = Request.CreateResponse (HttpStatusCode.OK);
                var text = GetEmployeeCsvText (employee, new WorkbookToLinearCsvSerializer ());
                result.Content = new StringContent (text, Encoding.UTF8, "text/plain");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue ("text/plain");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue ("attachment") {
                    FileName = GetFileName (employee, ".txt")
                };

                return result;
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
