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
    }
}
