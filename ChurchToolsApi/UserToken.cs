using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ChurchToolsApi
{
    struct UserToken
    {
        [JsonProperty("id")]
        public int Id { get;set;}

        [JsonProperty("token")]
        public string Token { get;set;}
    }
}
