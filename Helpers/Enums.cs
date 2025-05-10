using System;

namespace VdwwdBrickLink
{
    public class Enums
    {
        public enum EventType
        {
            None,
            Feedback,
            Message,
            Order,
            OrderStatusUpdate
        }


        public enum OrderFilter
        {
            NoFilter,
            Completed,
            Pending,
            Purged
        }


        public enum OrderStatus
        {
            Pending,
            Completed,
            Packed,
            Paid,
            Processing,
            Ready,
            Shipped,
            Updated
        }


        public enum CatalogItemType
        {
            None,
            Minifig,
            Part,
            Set,
            Book,
            Gear,
            Catalog,
            Instruction,
            Unsorted_lot,
            Original_box
        }


        internal enum CatalogItemAction
        {
            Item,
            Colors,
            Image,
            Price
        }


        public enum CatalogItemPriceType
        {
            New,
            Used
        }


        public enum ColorType
        {
            Solid,
            Chrome,
            Glitter,
            Metallic,
            Milky,
            Modulex,
            Pearl,
            Satin,
            Speckle,
            Transparent
        }
    }
}
