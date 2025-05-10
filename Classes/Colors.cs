using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public partial class Classes
    {
        public class ColorRoot
        {
            public Meta meta { get; set; }

            [JsonConverter(typeof(SingleOrArrayConverter<Color>))]
            public List<Color> data { get; set; }
        }


        public class Color
        {
            public int color_id { get; set; }
            public string color_name { get; set; }
            public string color_code { get; set; }
            public string color_type { get; set; }

            [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
            internal int? quantity { get; set; }

            //made an enum for the type string property (not part of the bricklink specifications)
            [JsonIgnore]
            public Enums.ColorType colorType
            {
                get
                {
                    return Helpers.parseEnum<Enums.ColorType>(color_type);
                }
            }
        }
    }
}
