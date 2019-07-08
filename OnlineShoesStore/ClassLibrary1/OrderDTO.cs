using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    class OrderDTO
    {
        private int orderId;
        private string paymentMethod;
        private float total;
        private DateTime dateCreated;
        private string status;

        public OrderDTO(int orderId, string method, float total, DateTime dateCreated, string status)
        {
            this.orderId = orderId;
            this.paymentMethod = method;
            this.total = total;
            this.dateCreated = dateCreated;
            this.status = status;
        }

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        public string PaymentMethod
        {
            get { return paymentMethod; }
            set { paymentMethod = value; }
        }

        public float Total
        {
            get { return total; }
            set { total = value; }
        }

        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

    }
}
