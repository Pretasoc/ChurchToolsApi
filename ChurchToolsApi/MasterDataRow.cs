﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChurchToolsApi
{
    class MasterDataRow
    {
        [JsonProperty("id")]
        public int Id { get;set;}

        [JsonProperty("table")]
        public string Table { get;set;}

        [JsonIgnore]
        public Dictionary<string, string> Columns { get;} = new Dictionary<string, string>();

        [JsonExtensionData()]
        private Dictionary<string, JToken> _additionalData { get;set;}

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            int i = 0;
            while(_additionalData.TryGetValue($"col{i}", out JToken column))
            {
                var value = _additionalData[$"val{i}"];
                Columns.Add(column.ToObject<string>(), value.ToObject<string>());
                i++;
            }
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            int i = 0;
            _additionalData = new Dictionary<string, JToken>();
            foreach (var item in Columns)
            {
                _additionalData.Add($"col{i}", item.Key);
                _additionalData.Add($"val{i}", item.Value);
                i++;
            }
        }
    }
}
