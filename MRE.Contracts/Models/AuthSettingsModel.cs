using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Contracts.Models
{
    public class AuthSettingsModel
    {
        public const string AUTH_SETTINGS = "Auth";

        /// <summary>
        /// Secret key for generating the tokens.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Issuer of the token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Open id well know configuration document for microsoft.
        /// </summary>
        public string OpenIdEndpoint { get; set; }
        public string GoogleClientId { get; set; }
    }
}
