using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public static class Orders
    {
        /// <summary>
        /// Get a specific order with the supplied order ID.
        /// </summary>
        /// <param name="orderID">The BrickLink order ID.</param>
        /// <returns>VdwwdBrickLink.Classes.Order</returns>
        public static async Task<Classes.Order> getOrder(int orderID)
        {
            //create the full url
            string url = $"{Variables.ApiUrl}/orders/{orderID}";

            //get the header for the request
            string header = Helpers.getOauthHeader(HttpMethod.Get, url);

            //call the api
            var result = await Api.callApi(header, url);

            //if the result is not null and contains an item then return the order
            if (result != null && result.data != null && result.data.Any())
            {
                return result.data[0];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get all the orders.
        /// </summary>
        /// <param name="status">Optional filter the order by a status.</param>
        /// <returns>VdwwdBrickLink.Classes.Order</returns>
        public static async Task<List<Classes.Order>> getOrders(Enums.OrderFilter status = Enums.OrderFilter.NoFilter)
        {
            string header = "";

            //create the full url
            string url = $"{Variables.ApiUrl}/orders";

            //is there an filter for the orders
            if (status == Enums.OrderFilter.NoFilter)
            {
                //get the header for the request
                header = Helpers.getOauthHeader(HttpMethod.Get, url);
            }
            else
            {
                //add the querystring parameter to the oauth header
                var extra_parameter = new List<Classes.OAuthRequestParameter>()
                {
                    new Classes.OAuthRequestParameter("status", status.ToString().ToLower())
                };

                //get the header for the request
                header = Helpers.getOauthHeader(HttpMethod.Get, url, extra_parameter);

                //add the filter as querystring parameter to the url
                url += "?status=" + status.ToString().ToLower();
            }

            //call the api
            var result = await Api.callApi(header, url);

            //if the result is not null then return the list of orders
            if (result != null && result.data != null)
            {
                return result.data;
            }
            else
            {
                //return an empty list
                return new List<Classes.Order>();
            }
        }


        /// <summary>
        /// Update the payment status of an order.
        /// </summary>
        /// <param name="orderID">The BrickLink order ID.</param>
        /// <param name="is_payed">Is the order payed or not.</param>
        /// <returns>VdwwdBrickLink.Classes.OrderRoot</returns>
        public static async Task<bool> updateOrderPayment(int orderID, bool is_payed)
        {
            //create the full url
            string url = $"{Variables.ApiUrl}/orders/{orderID}/payment_status";

            //get the header for the request
            string header = Helpers.getOauthHeader(HttpMethod.Put, url);

            //create the payment update
            var update = new Classes.OrderStatus
            {
                field = "payment_status",
                value = is_payed ? "Received" : "None"
            };

            //call the api
            var result = await Api.callApi(header, url, JsonConvert.SerializeObject(update));

            //if the update was ok then the response is null
            if (result == null)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Update the status of an order.
        /// </summary>
        /// <param name="orderID">The BrickLink order ID.</param>
        /// <param name="status">The new status of the order.</param>
        /// <returns></returns>
        public static async Task<bool> updateOrderStatus(int orderID, Enums.OrderStatus status)
        {
            //create the full url
            string url = $"{Variables.ApiUrl}/orders/{orderID}/status";

            //get the header for the request
            string header = Helpers.getOauthHeader(HttpMethod.Put, url);

            //create the payment update
            var update = new Classes.OrderStatus
            {
                field = "status",
                value = status.ToString()
            };

            //call the api
            var result = await Api.callApi(header, url, JsonConvert.SerializeObject(update));

            //if the update was ok then the response is null
            if (result == null)
            {
                return true;
            }

            return false;
        }
    }
}
