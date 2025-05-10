using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace VdwwdBrickLink
{
    internal class Api
    {
        /// <summary>
        /// Make a call to the BrickLink api to get an order.
        /// </summary>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <returns>VdwwdBrickLink.Classes.OrderRoot</returns>
        internal static async Task<Classes.OrderRoot> callApiOrders(string url)
        {
            return await callApiOrders(HttpMethod.Get, url, null, null);
        }


        /// <summary>
        /// Make a call to the BrickLink api to get an order.
        /// </summary>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="extra_parameters">Extra querystring url parameters to be encoded in the Oauth Header.</param>
        /// <returns>VdwwdBrickLink.Classes.OrderRoot</returns>
        internal static async Task<Classes.OrderRoot> callApiOrders(string url, SortedDictionary<string, string> extra_parameters)
        {
            return await callApiOrders(HttpMethod.Get, url, null, extra_parameters);
        }


        /// <summary>
        /// Make a call to the BrickLink api to update an order.
        /// </summary>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="json">The update of an order in a json string format.</param>
        /// <returns>VdwwdBrickLink.Classes.OrderRoot</returns>
        internal static async Task<Classes.OrderRoot> callApiOrders(string url, string json)
        {
            return await callApiOrders(HttpMethod.Put, url, json, null);
        }


        /// <summary>
        /// Make a call to the BrickLink api to get or update an order.
        /// </summary>
        /// <param name="method">Must the api call be a GET or PUT command.</param>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="json">The update of an order in a json string format.</param>
        /// <param name="extra_parameters">Extra querystring url parameters to be encoded in the Oauth Header.</param>
        /// <returns>VdwwdBrickLink.Classes.OrderRoot</returns>
        private static async Task<Classes.OrderRoot> callApiOrders(HttpMethod method, string url, string json = null, SortedDictionary<string, string> extra_parameters = null)
        {
            //call the api
            string result = await callBricklinkApi(method, url, json, extra_parameters);

            //deserialize the response
            var data = JsonConvert.DeserializeObject<Classes.OrderRoot>(result);

            //if the response status 204 (no content) then return null. if 204 is returned means the update of an order was succesful
            if (data.meta.code == (int)HttpStatusCode.NoContent)
            {
                return null;
            }
            //if the response status is no ok throw an error
            else if (data.meta.code != (int)HttpStatusCode.OK)
            {
                throw new Exception(string.Format(Variables.GlobalErrorMsg, $"{data.meta.message}\r\n{data.meta.description}"));
            }

            //return the order root
            return data;
        }


        /// <summary>
        /// Make a call to the BrickLink api to get a string of data to be serialized.
        /// </summary>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <returns>string</returns>
        internal static async Task<string> callApi(string url)
        {
            return await callBricklinkApi(HttpMethod.Get, url, null, null);
        }


        /// <summary>
        /// Make a call to the BrickLink api to get a string of data to be serialized.
        /// </summary>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="extra_parameters">Extra querystring url parameters to be encoded in the Oauth Header.</param>
        /// <returns>string</returns>
        internal static async Task<string> callApi(string url, SortedDictionary<string, string> extra_parameters)
        {
            return await callBricklinkApi(HttpMethod.Get, url, null, extra_parameters);
        }


        /// <summary>
        /// Make a call to the BrickLink api.
        /// </summary>
        /// <param name="method">Must the api call be a GET or PUT command.</param>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="json">The update of an order in a json string format.</param>
        /// <param name="extra_parameters">Extra querystring url parameters to be encoded in the Oauth Header.</param>
        /// <returns>string</returns>
        private static async Task<string> callBricklinkApi(HttpMethod method, string url, string json, SortedDictionary<string, string> extra_parameters)
        {
            HttpResponseMessage response;

            //create the full url
            url = Variables.ApiUrl + url;

            using (var client = new HttpClient())
            {
                //create the header string
                string header = Helpers.getOAuthHeader(method, url.Split('?')[0], extra_parameters);

                //add the authorization header
                client.DefaultRequestHeaders.Add("Authorization", header);

                //make the call, if the json string is not empty then the call is an update order
                if (!string.IsNullOrEmpty(json))
                {
                    //create content for the request
                    var content = new StringContent(json);

                    //add the json content-type header, bricklink does not accept the header with the charset parameter
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    //put the request
                    response = await client.PutAsync(url, content);
                }
                else
                {
                    //get the request
                    response = await client.GetAsync(url);
                }

                //read the response as string
                string result = await response.Content.ReadAsStringAsync();

                //if there is content then throw error
                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception(string.Format(Variables.GlobalErrorMsg, "The API response is null or empty"));
                }

                //return the received json
                return result;
            }
        }
    }
}
