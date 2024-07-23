using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace VdwwdBrickLink
{
    internal class Api
    {
        /// <summary>
        /// Make a call to the BrickLink api.
        /// </summary>
        /// <param name="header">The header that must be sent with the api call.</param>
        /// <param name="url">The url of the specific api endpoint.</param>
        /// <param name="json">The update of an order in a json string format.</param>
        /// <returns>VdwwdBrickLink.Classes.OrderRoot</returns>
        internal static async Task<Classes.OrderRoot> callApi(string header, string url, string json = null)
        {
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
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
        }
    }
}
