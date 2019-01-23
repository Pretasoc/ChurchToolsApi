using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ChurchToolsApi
{
    internal class PasswordLoginParameter
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get;set;}

        [JsonProperty(PropertyName = "password")]
        public string Password { get;set;}

        [JsonProperty(PropertyName = "directtool")]
        public string ApplicationName { get;set;}
    }
}
