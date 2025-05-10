using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public static class Orders
    {
        /// <summary>
        /// Get a specific store order with the supplied order ID.
        /// </summary>
        /// <param name="orderID">The BrickLink order ID.</param>
        /// <returns>VdwwdBrickLink.Classes.Order</returns>
        public static async Task<Classes.Order> getOrder(int orderID)
        {
            //create the full url
            string url = $"/orders/{orderID}";

            //call the api
            var result = await Api.callApiOrders(url);

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
        /// Get all the store orders.
        /// </summary>
        /// <returns>List<VdwwdBrickLink.Classes.Order></returns>
        public static async Task<List<Classes.Order>> getOrders()
        {
            return await getOrders(Enums.OrderFilter.NoFilter);
        }


        /// <summary>
        /// Get all the store orders filtered by status.
        /// </summary>
        /// <param name="status">Filter the orders by a status.</param>
        /// <returns>List<VdwwdBrickLink.Classes.Order></returns>
        public static async Task<List<Classes.Order>> getOrders(Enums.OrderFilter status)
        {
            SortedDictionary<string, string> extra_parameters = null;

            //create the full url
            string url = "/orders";

            //is there an filter for the orders
            if (status != Enums.OrderFilter.NoFilter)
            {
                string statusstr = status.ToString().ToLower();

                //add the querystring parameter to the oauth header
                extra_parameters = new SortedDictionary<string, string>
                {
                    { "status", statusstr }
                };

                //add the filter as querystring parameter to the url
                url += "?status=" + statusstr;
            }

            //call the api
            var result = await Api.callApiOrders(url, extra_parameters);

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
        /// Update the payment status of a store order.
        /// </summary>
        /// <param name="orderID">The BrickLink order ID.</param>
        /// <param name="is_payed">Is the order payed or not.</param>
        /// <returns>bool</returns>
        public static async Task<bool> updateOrderPayment(int orderID, bool is_payed)
        {
            //create the full url
            string url = $"/orders/{orderID}/payment_status";

            //create the payment update
            var update = new Classes.OrderStatus
            {
                field = "payment_status",
                value = is_payed ? "Received" : "None"
            };

            //call the api
            var result = await Api.callApiOrders(url, JsonConvert.SerializeObject(update));

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
        /// <returns>bool</returns>
        public static async Task<bool> updateOrderStatus(int orderID, Enums.OrderStatus status)
        {
            //create the full url
            string url = $"/orders/{orderID}/status";

            //create the payment update
            var update = new Classes.OrderStatus
            {
                field = "status",
                value = status.ToString()
            };

            //call the api
            var result = await Api.callApiOrders(url, JsonConvert.SerializeObject(update));

            //if the update was ok then the response is null
            if (result == null)
            {
                return true;
            }

            return false;
        }
    }
}
