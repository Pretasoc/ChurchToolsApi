using Newtonsoft.Json;
using System.Security;

namespace ChurchToolsApi
{
    internal class TokenLoginParameter
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "directtool")]
        public string ApplicationName { get; set; }

    }
}