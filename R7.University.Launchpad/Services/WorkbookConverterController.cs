using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;

namespace R7.University.Launchpad.Services
{
    public class WorkbookConverterController: DnnApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Ping ()
        {
            try {
                return Request.CreateResponse (HttpStatusCode.OK, new {pingLabel = "Pong!"});
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
