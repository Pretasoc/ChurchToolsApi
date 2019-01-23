using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchToolsApi
{
    internal class SettingsParameter
    {
        [JsonProperty("sub")]
        public string Key { get;set;}

        [JsonProperty("val")]
        public object Value { get; set; }
    }
}
