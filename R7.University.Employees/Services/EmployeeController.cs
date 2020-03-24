//
//  EmployeeController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2020 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using R7.University.Core.Templates;
using R7.University.Models;
using R7.University.Templates;
using R7.University.ViewModels;

/* Need to add binding redirect to web.config (via manifest?)
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
            // TODO: Get employee
            var employee = new EmployeeInfo ();
            employee.LastName = "Иванов";
            employee.FirstName = "Иван";
            employee.OtherName = "Иванович";

            return employee;
        }

        MemoryStream GetEmployeeStream (IEmployee employee)
        {
            var employeeBinder = new EmployeeToTemplateBinder (employee, PortalSettings,
                    "~/DesktopModules/MVC/R7.University/R7.University.Employees/App_LocalResources/SharedResources.resx");
            var templateEngine = new XSSFLiquidTemplateEngine (employeeBinder);

            var stream = new MemoryStream ();

            // TODO: Support for localized templates
            var templatePath = Globals.ApplicationMapPath + "/DesktopModules/MVC/R7.University/R7.University/assets/templates/employee_template.xlsx";

            templateEngine.ApplyAndWrite (templatePath, stream);

            return stream;
        }
               
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Export (int employeeId)
        {
            try {
                var employee = GetEmployee (employeeId);
                if (employee == null) {
                    return Request.CreateResponse (HttpStatusCode.NotFound);
                }

                var stream = GetEmployeeStream (employee);

                // stream.GetBuffer() returns bigger buffer, than data!
                var buffer = stream.ToArray ();

                var result = Request.CreateResponse (HttpStatusCode.OK);
                result.Content = new ByteArrayContent (buffer);
                // TODO: Introduce MimeTypes!
                result.Content.Headers.ContentType = new MediaTypeHeaderValue ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentLength = buffer.Length;
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue ("attachment") {
                    FileName = UniversityFormatHelper.AbbrName (
                        employee.FirstName,
                        employee.LastName,
                        employee.OtherName).Replace (".", "").Replace (" ", "_") + ".xlsx"
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
