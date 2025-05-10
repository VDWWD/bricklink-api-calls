using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace VdwwdBrickLink
{
    //the converter is needed because the data property in the order root can either be an array or a single item
    internal class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(List<T>);
        }

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            else if (token.Type == JTokenType.Null)
            {
                return null;
            }

            return new List<T> {
                token.ToObject<T>()
            };
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
