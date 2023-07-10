using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MRE.Contracts.Dtos
{
    public class CqrsResponse
    {
        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
