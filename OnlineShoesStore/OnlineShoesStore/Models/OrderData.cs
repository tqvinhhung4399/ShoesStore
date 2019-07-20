using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class OrderDTO
    {
        private int orderId;
        private string paymentMethod;
        private float total;
        private DateTime dateCreated;
        private string status;

        public int cartID { get; set; }
        public OrderDTO()
        {

        }
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

    public class OrderData
    {
        public bool InsertNewOrder(OrderDTO order)
        {
            bool result = false;
            string sql = "Insert into Orders values(@cartID, @paymentMethod, @total, @date, @status)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@cartID", order.cartID);
            cmd.Parameters.AddWithValue("@paymentMethod", order.PaymentMethod);
            cmd.Parameters.AddWithValue("@total", order.Total);
            cmd.Parameters.AddWithValue("@date", order.DateCreated);
            cmd.Parameters.AddWithValue("@status", order.Status);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
