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
    public class Helpers
    {
        /// <summary>
        /// Converts a decimal in string format to an actual decimal object.
        /// </summary>
        /// <param name="value">The decimal in string format.</param>
        /// <returns>System.Decimal</returns>
        public static decimal parseDecimal(string value)
        {
            //try and convert the string decimal in en-us format because the decimal separator is a dot
            decimal amount = decimal.TryParse(value, NumberStyles.Number, new CultureInfo("en-US"), out amount) ? amount : 0;

            //return the amount
            return amount;
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
        internal static string getOauthHeader(HttpMethod method, string url, List<Classes.OAuthRequestParameter> extra_parameters = null)
        {
            //check if all the values are present
            Variables.checkVariables();

            //unique sting and timestamp
            string signature;
            string nonce = Guid.NewGuid().ToString();
            string timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString();

            //create the list with the parameters
            var parameters = new List<Classes.OAuthRequestParameter>
            {
                new Classes.OAuthRequestParameter("oauth_consumer_key", Variables.ConsumerKey),
                new Classes.OAuthRequestParameter("oauth_nonce", nonce),
                new Classes.OAuthRequestParameter("oauth_signature_method", "HMAC-SHA1"),
                new Classes.OAuthRequestParameter("oauth_timestamp", timestamp),
                new Classes.OAuthRequestParameter("oauth_token", Variables.TokenValue),
                new Classes.OAuthRequestParameter("oauth_version", "1.0")
            };

            //if there are extra parameters then combine them
            if (extra_parameters != null)
            {
                parameters = parameters.Concat(extra_parameters).ToList();
            }

            //normalize the parameters for the signature
            string parameters_normalized = string.Join(Uri.EscapeDataString("&"), parameters.Select(x => x.KeyValueUrl));

            //create the signature base string
            string signature_base = $"{method.ToString().ToUpper()}&{Uri.EscapeDataString(url)}&{parameters_normalized}";

            //the encoding key
            string key = $"{Uri.EscapeDataString(Variables.ConsumerSecret)}&{Uri.EscapeDataString(Variables.TokenSecret)}";

            //the encoding type
            var encoding = new ASCIIEncoding();

            //compute the hash
            using (var SHA1 = new HMACSHA1(encoding.GetBytes(key)))
            {
                signature = Convert.ToBase64String(SHA1.ComputeHash(encoding.GetBytes(signature_base)));
            }

            //retun the completed header
            return $"OAuth oauth_signature=\"{Uri.EscapeDataString(signature)}\",{string.Join(",", parameters.Select(x => x.KeyValueHeader))}";
        }
    }
}
