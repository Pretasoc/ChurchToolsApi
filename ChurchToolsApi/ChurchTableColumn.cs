using ChurchToolsApi.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchToolsApi
{
    public class ChurchTableColumn
    {
        [JsonProperty("default")]
        public string Default { get;set;}

        [JsonProperty("extra")]
        public string Extra { get; set; }

        [JsonProperty("field")]
        public string Title { get;set;}

        [JsonProperty("key")]
        public string Key { get;set;}

        [JsonProperty("null")]
        [JsonConverter(typeof(YesNoConverter))]
        public bool AllowNull { get;set;}

        [JsonProperty("type")]
        public string Type { get;set;}
    }
}
