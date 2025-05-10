using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public partial class Classes
    {
        public class CatalogItemRoot
        {
            public Meta meta { get; set; }

            [JsonConverter(typeof(SingleOrArrayConverter<CatalogItem>))]
            public List<CatalogItem> data { get; set; }
        }


        public class CatalogItem
        {
            public string no { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string image_url { get; set; }
            public string thumbnail_url { get; set; }
            public decimal weight { get; set; }
            public decimal dim_x { get; set; }
            public decimal dim_y { get; set; }
            public decimal dim_z { get; set; }
            public int year_released { get; set; }
            public bool is_obsolete { get; set; }
            public int category_id { get; set; }

            //made an enum for the type string property (not part of the bricklink specifications)
            [JsonIgnore]
            public Enums.CatalogItemType itemType
            {
                get
                {
                    return Helpers.parseEnum<Enums.CatalogItemType>(type);
                }
            }
        }


        public class CatalogItemColor
        {
            public int color_id { get; set; }
            public int quantity { get; set; }
        }


        public class CatalogPriceRoot
        {
            public Meta meta { get; set; }

            [JsonConverter(typeof(SingleOrArrayConverter<CatalogPrice>))]
            public List<CatalogPrice> data { get; set; }
        }


        public class CatalogPrice
        {
            public CatalogPriceItem item { get; set; }
            public string new_or_used { get; set; }
            public string currency_code { get; set; }
            public string min_price { get; set; }
            public string max_price { get; set; }
            public string avg_price { get; set; }
            public string qty_avg_price { get; set; }
            public int unit_quantity { get; set; }
            public int total_quantity { get; set; }
            public List<CatalogPriceDetail> price_detail { get; set; }
        }


        public class CatalogPriceItem
        {
            public string no { get; set; }
            public string type { get; set; }

            //made an enum for the type string property (not part of the bricklink specifications)
            [JsonIgnore]
            public Enums.CatalogItemType itemType
            {
                get
                {
                    return Helpers.parseEnum<Enums.CatalogItemType>(type);
                }
            }
        }


        public class CatalogPriceDetail
        {
            public int quantity { get; set; }
            public bool shipping_available { get; set; }
            public decimal unit_price { get; set; }
            public string seller_country_code { get; set; }
            public string buyer_country_code { get; set; }
            public DateTime date_ordered { get; set; }
        }
    }
}
