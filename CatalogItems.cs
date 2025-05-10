using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public static class CatalogItems
    {
        /// <summary>
        /// Get a catalog item with a type and catalog ID.
        /// </summary>
        /// <param name="type">The enum representation of a catalog type.</param>
        /// <param name="catalogID">The catalog ID of an item.</param>
        /// <returns>VdwwdBrickLink.Classes.CatalogItem</returns>
        public static async Task<Classes.CatalogItem> GetItem(Enums.CatalogItemType type, string catalogID)
        {
            //get the data
            object data = await GetCatalogItem(Enums.CatalogItemAction.Item, type, Enums.CatalogItemPriceType.New, catalogID, 0);

            if (data == null)
            {
                return null;
            }

            //convert object to correct data type
            var item = (List<Classes.CatalogItem>)data;

            //return the data
            return item[0];
        }


        /// <summary>
        /// Get all the known colors of a catalog item.
        /// </summary>
        /// <param name="type">The enum representation of a catalog type.</param>
        /// <param name="catalogID">The catalog ID of an item.</param>
        /// <returns>VdwwdBrickLink.Classes.CatalogItemColor</returns>
        public static async Task<List<Classes.CatalogItemColor>> GetItemColors(Enums.CatalogItemType type, string catalogID)
        {
            //get the data
            object data = await GetCatalogItem(Enums.CatalogItemAction.Colors, type, Enums.CatalogItemPriceType.New, catalogID, 0);

            if (data == null)
            {
                return null;
            }

            //convert object to correct data type
            var items = (List<Classes.Color>)data;

            //return the data
            return items.Select(x => new Classes.CatalogItemColor()
            {
                color_id = x.color_id,
                quantity = (int)x.quantity,
            }).ToList();
        }


        /// <summary>
        /// Get a catalog item image url with a type and catalog ID.
        /// </summary>
        /// <param name="type">The enum representation of a catalog type.</param>
        /// <param name="catalogID">The catalog ID of an item.</param>
        /// <returns>string</returns>
        public static async Task<string> GetItemImageUrl(Enums.CatalogItemType type, string catalogID)
        {
            //get the data
            return await GetItemImageUrl(type, catalogID, Variables.ItemColorDefault);
        }


        /// <summary>
        /// Get a catalog item image url with a type and catalog ID in a specific color
        /// </summary>
        /// <param name="type">The enum representation of a catalog type.</param>
        /// <param name="catalogID">The catalog ID of an item.</param>
        /// <param name="colorID">The color ID of an item.</param>
        /// <returns>string</returns>
        public static async Task<string> GetItemImageUrl(Enums.CatalogItemType type, string catalogID, int colorID)
        {
            //check the color range
            if (colorID < Variables.ItemColorDefault || colorID > Variables.ItemColorMaxValue)
            {
                colorID = Variables.ItemColorDefault;
            }

            //get the data
            object data = await GetCatalogItem(Enums.CatalogItemAction.Image, type, Enums.CatalogItemPriceType.New, catalogID, colorID);

            if (data == null)
            {
                return null;
            }

            //convert object to correct data type
            var item = (List<Classes.CatalogItem>)data;

            //return the data
            if (!string.IsNullOrEmpty(item[0].thumbnail_url))
            {
                return "https:" + item[0].thumbnail_url;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get the price details of a catalog item from the last 6 months sales.
        /// </summary>
        /// <param name="type">The enum representation of a catalog type.</param>
        /// <param name="price_type">The enum representation of a price type.</param>
        /// <param name="catalogID">The catalog ID of an item.</param>
        /// <param name="colorID">The color ID of an item.</param>
        /// <returns>VdwwdBrickLink.Classes.CatalogPrice</returns>
        public static async Task<Classes.CatalogPrice> GetPriceGuide(Enums.CatalogItemType type, Enums.CatalogItemPriceType price_type, string catalogID, int colorID)
        {
            //check the color range
            if (colorID < Variables.ItemColorDefault || colorID > Variables.ItemColorMaxValue)
            {
                colorID = Variables.ItemColorDefault;
            }

            //get the data
            object data = await GetCatalogItem(Enums.CatalogItemAction.Price, type, price_type, catalogID, colorID);

            if (data == null)
            {
                return null;
            }

            //convert object to correct data type
            var item = (List<Classes.CatalogPrice>)data;

            //return the data
            return item[0];
        }


        /// <summary>
        /// Get a catalog item from BrickLink.
        /// </summary>
        /// <param name="action">Which type of catalog item data.</param>
        /// <param name="type">The enum representation of a catalog type.</param>
        /// <param name="price_type">The enum representation of a price type.</param>
        /// <param name="catalogID">The catalog ID of an item.</param>
        /// <param name="colorID">The color ID of an item.</param>
        /// <returns></returns>
        private static async Task<object> GetCatalogItem(Enums.CatalogItemAction action, Enums.CatalogItemType type, Enums.CatalogItemPriceType price_type, string catalogID, int colorID)
        {
            SortedDictionary<string, string> extra_parameters = null;

            //create the correct url
            string url = $"/items/{type.ToString().ToLower()}/{catalogID}";

            if (action == Enums.CatalogItemAction.Colors)
            {
                url += "/colors";
            }
            else if (action == Enums.CatalogItemAction.Image)
            {
                url += "/images/" + colorID;
            }
            else if (action == Enums.CatalogItemAction.Price)
            {
                //add the querystring parameter to the oauth header
                extra_parameters = new SortedDictionary<string, string>
                {
                    { "new_or_used", price_type.ToString().Substring(0, 1) },
                    { "guide_type", "sold" },
                    { "color_id", colorID.ToString() }
                };

                //add the parameters to the url
                url += "/price?" + string.Join("&", extra_parameters.Select(p => $"{p.Key}={p.Value}"));
            }

            //call the api
            var result = await Api.callApi(url, extra_parameters);

            //deserialize the response
            if (action == Enums.CatalogItemAction.Price)
            {
                //deserialize the response
                var data = JsonConvert.DeserializeObject<Classes.CatalogPriceRoot>(result);

                //if the result is not null and contains an item then return the colors
                if (data != null && data.data != null && data.data.Any())
                {
                    return data.data;
                }
            }
            else if (action == Enums.CatalogItemAction.Colors)
            {
                //deserialize the response
                var data = JsonConvert.DeserializeObject<Classes.ColorRoot>(result);

                //if the result is not null and contains an item then return the colors
                if (data != null && data.data != null && data.data.Any())
                {
                    return data.data;
                }
            }
            else
            {
                //deserialize the response
                var data = JsonConvert.DeserializeObject<Classes.CatalogItemRoot>(result);

                //if the result is not null and contains an item then return the colors
                if (data != null && data.data != null && data.data.Any())
                {
                    return data.data;
                }
            }

            return null;
        }
    }
}
