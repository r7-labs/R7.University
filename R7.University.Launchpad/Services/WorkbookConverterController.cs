using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using R7.University.Core.Templates;
using R7.University.Launchpad.ViewModels;

namespace R7.University.Launchpad.Services
{
    public class WorkbookConverterController: DnnApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Ping ()
        {
            try {
                return Request.CreateResponse (HttpStatusCode.OK, new { pingLabel = "Pong!" });
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[SupportedModules ("R7.University.Launchpad")]
        //[DnnModuleAuthorize (AccessLevel = SecurityAccessLevel.Admin)]
        public async Task<HttpResponseMessage> Upload ()
        {
            try {
                var provider = new MultipartMemoryStreamProvider ();
                await Request.Content.ReadAsMultipartAsync (provider);

                var fileBytes = await provider.Contents [0].ReadAsByteArrayAsync ();

                // create temp file and store uploaded data
                var tempFilePath = Path.GetTempFileName ();
                File.WriteAllBytes (tempFilePath, fileBytes);

                // extract file name and file contents
                var fileNameParam = provider.Contents [0].Headers.ContentDisposition.Parameters
                    .FirstOrDefault (p => p.Name.ToLower () == "filename");

                var fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim ('"');

                return Request.CreateResponse (HttpStatusCode.OK,
                    new WorkbookConverterUploadResult {
                        FileName = fileName,
                        TempFilePath = tempFilePath
                    });
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Convert (string fileName, string tempFilePath, string format)
        {
            try {
                var result = default (HttpResponseMessage);

               if (string.Equals (format, "CSV", StringComparison.OrdinalIgnoreCase)) {
                    result = Request.CreateResponse (HttpStatusCode.OK);
                    var text = GetWorkbookText (tempFilePath);
                    result.Content = new StringContent (text, Encoding.UTF8, "text/plain");
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue ("text/plain");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue ("attachment") {
                        FileName = Path.GetFileNameWithoutExtension (fileName) + ".txt"
                    };
                }
                else {
                    result = Request.CreateErrorResponse (HttpStatusCode.BadRequest, "The format argument is required!");
                }

                return result;
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }

        string GetWorkbookText (string tempFilePath)
        {
            var workbookManager = new WorkbookManager ();
            return workbookManager.SerializeWorkbook (tempFilePath, WorkbookSerializationFormat.CSV);
        }
    }
}
