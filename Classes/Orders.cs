using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VdwwdBrickLink
{
    public partial class Classes
    {
        public class OrderRoot
        {
            public Meta meta { get; set; }

            [JsonConverter(typeof(SingleOrArrayConverter<Order>))]
            public List<Order> data { get; set; }
        }


        public class Order
        {
            public int order_id { get; set; }
            public DateTime date_ordered { get; set; }
            public DateTime date_status_changed { get; set; }
            public string seller_name { get; set; }
            public string store_name { get; set; }
            public string buyer_name { get; set; }
            public string buyer_email { get; set; }
            public bool require_insurance { get; set; }
            public string status { get; set; }
            public bool is_invoiced { get; set; }
            public string remarks { get; set; }
            public int total_count { get; set; }
            public int unique_count { get; set; }
            public string total_weight { get; set; }
            public int buyer_order_count { get; set; }
            public bool is_filed { get; set; }
            public bool drive_thru_sent { get; set; }
            public Payment payment { get; set; }
            public Shipping shipping { get; set; }
            public Cost cost { get; set; }
            public Cost disp_cost { get; set; }
        }


        public class Payment
        {
            public string method { get; set; }
            public string currency_code { get; set; }
            public DateTime date_paid { get; set; }
            public string status { get; set; }
        }


        public class Shipping
        {
            public int method_id { get; set; }
            public string method { get; set; }
            public Address address { get; set; }
            public DateTime date_shipped { get; set; }
        }


        public class Cost
        {
            public string currency_code { get; set; }
            public string subtotal { get; set; }
            public string grand_total { get; set; }
            public string etc1 { get; set; }
            public string etc2 { get; set; }
            public string insurance { get; set; }
            public string shipping { get; set; }
            public string credit { get; set; }
            public string coupon { get; set; }
            public string vat_rate { get; set; }
            public string vat_amount { get; set; }

            //the amount values are strings, so to use decimal they need to be converted
            public decimal SubTotal
            {
                get
                {
                    return Helpers.parseDecimal(subtotal);
                }
            }
            public decimal GrandTotal
            {
                get
                {
                    return Helpers.parseDecimal(grand_total);
                }
            }
            public decimal Shipping
            {
                get
                {
                    return Helpers.parseDecimal(shipping);
                }
            }
        }


        public class Address
        {
            public Name name { get; set; }
            public string full { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string country_code { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postal_code { get; set; }
            public string phone_number { get; set; }
        }


        public class Name
        {
            public string full { get; set; }
            public string first { get; set; }
            public string last { get; set; }
        }


        public class OrderStatus
        {
            public string field { get; set; }
            public string value { get; set; }
        }
    }
}
