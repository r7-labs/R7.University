using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using R7.University.Core.Templates;
using R7.University.Launchpad.ViewModels;

namespace R7.University.Launchpad.Services
{
    public class WorkbookConverterController: DnnApiController
    {
        [HttpPost]
        [DnnAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Upload ()
        {
            try {
                var provider = new MultipartMemoryStreamProvider ();
                await Request.Content.ReadAsMultipartAsync (provider);

                var results = new List<WorkbookConverterUploadResult> ();
                foreach (var contents in provider.Contents) {
                    var fileBytes = await contents.ReadAsByteArrayAsync ();

                    // create temp file and store uploaded data
                    var tempFilePath = Path.GetTempFileName ();
                    File.WriteAllBytes (tempFilePath, fileBytes);

                    // extract file name and file contents
                    var fileNameParam = contents.Headers.ContentDisposition.Parameters
                        .FirstOrDefault (p => p.Name.ToLower () == "filename");

                    var fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim ('"');

                    results.Add (new WorkbookConverterUploadResult {
                        FileName = fileName,
                        TempFileName = Path.GetFileName (tempFilePath)
                    });
                }

                return Request.CreateResponse (HttpStatusCode.OK, results);
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [DnnAuthorize]
        public HttpResponseMessage Convert (string fileName, string tempFileName, string format)
        {
            try {
                var result = default (HttpResponseMessage);

               if (string.Equals (format, "CSV", StringComparison.OrdinalIgnoreCase)) {
                    result = Request.CreateResponse (HttpStatusCode.OK);
                    var text = GetWorkbookText (Path.Combine (Path.GetTempPath (), tempFileName));
                    result.Content = new StringContent (text, Encoding.UTF8, "text/plain");
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue ("text/plain");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue ("attachment") {
                        FileName = Path.GetFileNameWithoutExtension (fileName) + ".txt"
                    };
                }
                else {
                    result = Request.CreateResponse (HttpStatusCode.BadRequest);
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
            return workbookManager.SerializeWorkbook (tempFilePath, WorkbookSerializationFormat.LinearCSV);
        }
    }
}
