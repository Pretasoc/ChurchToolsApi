using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchToolsApi.Converter
{
    class YesNoConverter : JsonConverter<bool>
    {
        public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if(string.Equals("yes", value, StringComparison.InvariantCultureIgnoreCase))
                return true;
            else if(string.Equals("no", value, StringComparison.InvariantCultureIgnoreCase))
                return false;

            throw new InvalidOperationException();
        }

        public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
        {
            writer.WriteValue(value ? "yes" : "no");
        }
    }
}
