using System;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public partial class Classes
    {
        public class PushNotification
        {
            public string event_type { get; set; }
            public int resource_id { get; set; }
            public DateTime timestamp { get; set; }

            //made an enum for the event_type string property (not part of the bricklink specifications)
            [JsonIgnore]
            public Enums.EventType eventType
            {
                get
                {
                    return Helpers.parseEnum<Enums.EventType>(event_type);
                }
            }
        }


        public class Meta
        {
            public string description { get; set; }
            public string message { get; set; }
            public int code { get; set; }
        }
    }
}
