using System;

namespace VdwwdBrickLink
{
    public class Variables
    {
        public static string ApiUrl { get; private set; } = "https://api.bricklink.com/api/store/v1";
        public static string ApiHelpUrl { get; private set; } = "https://www.bricklink.com/v3/api.page";
        internal static string ConsumerKey { get; set; }
        internal static string ConsumerSecret { get; set; }
        internal static string TokenValue { get; set; }
        internal static string TokenSecret { get; set; }
        internal static string GlobalErrorMsg { get; set; } = "VdwwdBrickLink Error: {0}.";
        internal static int ItemColorDefault { get; set; } = 1; //white
        internal static int ItemColorMaxValue { get; set; } = 260; //current highest color is 255 pearl brown


        /// <summary>
        /// Set the needed BrickLink variables. Note that the TokenValue and TokenSecret are specific per IP address.
        /// </summary>
        /// <param name="consumerKey">The ConsumerKey.</param>
        /// <param name="consumerSecret">The ConsumerSecret.</param>
        /// <param name="tokenValue">The TokenValue.</param>
        /// <param name="tokenSecret">The TokenSecret.</param>
        public static void setVariables(string consumerKey, string consumerSecret, string tokenValue, string tokenSecret)
        {
            setVariables(consumerKey, consumerSecret, tokenValue, tokenSecret, null);
        }


        /// <summary>
        /// Set the needed BrickLink variables. Note that the TokenValue and TokenSecret are specific per IP address.
        /// </summary>
        /// <param name="consumerKey">The ConsumerKey.</param>
        /// <param name="consumerSecret">The ConsumerSecret.</param>
        /// <param name="tokenValue">The TokenValue.</param>
        /// <param name="tokenSecret">The TokenSecret.</param>
        /// <param name="apiUrl">The optional url of the api endpoint. If not specified will use the default url.</param>
        public static void setVariables(string consumerKey, string consumerSecret, string tokenValue, string tokenSecret, string apiUrl)
        {
            //set the variables
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            TokenValue = tokenValue;
            TokenSecret = tokenSecret;

            //if the api url was provided then override the default one
            if (!string.IsNullOrEmpty(apiUrl))
            {
                ApiUrl = apiUrl;
            }

            //check if all the values are present
            checkVariables();
        }


        /// <summary>
        /// Checks if all the required variables are present and throws an error if they are not.
        /// </summary>
        internal static void checkVariables()
        {
            string error = string.Format(GlobalErrorMsg, "{0} is null or empty");

            //check if the required variables are present
            if (string.IsNullOrEmpty(ApiUrl))
            {
                throw new Exception(string.Format(error, "ApiUrl"));
            }
            if (string.IsNullOrEmpty(ConsumerKey))
            {
                throw new Exception(string.Format(error, "ConsumerKey"));
            }
            if (string.IsNullOrEmpty(ConsumerSecret))
            {
                throw new Exception(string.Format(error, "ConsumerSecret"));
            }
            if (string.IsNullOrEmpty(TokenValue))
            {
                throw new Exception(string.Format(error, "TokenValue"));
            }
            if (string.IsNullOrEmpty(TokenSecret))
            {
                throw new Exception(string.Format(error, "TokenSecret"));
            }
        }
    }
}
