using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    internal class Helpers
    {
        /// <summary>
        /// Converts a decimal in string format to an actual decimal object.
        /// </summary>
        /// <param name="value">The decimal in string format.</param>
        /// <returns>System.Decimal</returns>
        internal static decimal parseDecimal(string value)
        {
            //try and convert the string decimal in en-us format because the decimal separator is a dot
            decimal amount = decimal.TryParse(value, NumberStyles.Number, new CultureInfo("en-US"), out amount) ? amount : 0;

            //return the amount
            return amount;
        }


        /// <summary>
        /// Converts a string to the corresponding Enum value. If the conversion failed the default value of the Enum will be returned.
        /// </summary>
        /// <typeparam name="T">The Enum type.</typeparam>
        /// <param name="enumstr">The string value to be converted.</param>
        /// <returns>Enum T</returns>
        internal static T parseEnum<T>(string enumstr) where T : struct, IConvertible
        {
            if (string.IsNullOrEmpty(enumstr))
                return default(T);

            T value = Enum.TryParse<T>(enumstr.Trim().Replace(" ", "_").Replace("-", "_"), true, out value) ? value : default(T);

            return value;
        }


        /// <summary>
        /// Reads a posted Stream and returns it as a list of notifications.
        /// </summary>
        /// <param name="stream">The posted stream from BrickLink.</param>
        /// <returns>List<VdwwdBrickLink.Classes.PushNotification></returns>
        public static List<Classes.PushNotification> readPostedStream(Stream stream)
        {
            string json = "";

            //check the stream
            if (stream == null)
            {
                return null;
            }

            //get the posted data from the stream
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            //there was no data
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            //deserialze the posted data to the bricklink notifications
            var notifications = JsonConvert.DeserializeObject<List<Classes.PushNotification>>(json);

            //return the notifications
            return notifications;
        }


        /// <summary>
        /// Create the header that must be sent along with the api call.
        /// </summary>
        /// <param name="method">The HTTP method of the api call.</param>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="extra_parameters">Optional extra parameters that must the encoded for the request</param>
        /// <returns>string</returns>
        internal static string getOAuthHeader(HttpMethod method, string url, SortedDictionary<string, string> extra_parameters = null)
        {
            //check if all the values are present
            Variables.checkVariables();

            //unique sting and timestamp
            string nonce = Guid.NewGuid().ToString();
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            //create the list with the default oauth parameters
            var parameters = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", Variables.ConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", timestamp },
                { "oauth_token", Variables.TokenValue },
                { "oauth_version", "1.0" }
            };

            //if there are extra parameters then add them
            if (extra_parameters != null)
            {
                foreach (var item in extra_parameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }

            //create the signature base string
            string signature_base = $"{method.ToString()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}")))}";

            //the encoding key
            string key = $"{Uri.EscapeDataString(Variables.ConsumerSecret)}&{Uri.EscapeDataString(Variables.TokenSecret)}";

            //compute the hash
            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(key)))
            {
                string signature = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(signature_base)));
                parameters.Add("oauth_signature", signature);
            }

            //retun the completed header
            return "OAuth " + string.Join(", ", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}=\"{Uri.EscapeDataString(p.Value)}\""));
        }
    }
}
