using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MRE.Contracts.Models
{
    public class ErrorResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
}
