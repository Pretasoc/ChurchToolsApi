using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchToolsApi
{
    public class ChurchTable
    {
        [JsonProperty("bezeichnung")]
        public string Description { get; set;}

        [JsonProperty("shortname")]
        public string ShortName { get; set; }

        [JsonProperty("desc")]
        public Dictionary<string, ChurchTableColumn> Columns { get;set;}

        [JsonProperty("id")]
        public int Id { get;set;}

        [JsonProperty("sqlorder")]
        public string SqlOrder { get; set; }

        [JsonProperty("tablename")]
        public string Name { get;set;}
    }
}
