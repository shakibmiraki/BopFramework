using System;
using Newtonsoft.Json;

namespace  Bop.Core
{
    public class CardStatusConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;

            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return false;
            }

            if (value.ToString().ToLower() == "active")
            {
                return true;
            }

            if (value.ToString().ToLower() == "deactive")
            {
                return false;
            }

            if (value.ToString().ToLower() == "done")
            {
                return true;
            }

            if (value.ToString().ToLower() == "fail")
            {
                return false;
            }

            return false;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(string) || objectType == typeof(bool))
            {
                return true;
            }
            return false;
        }
    }
}
