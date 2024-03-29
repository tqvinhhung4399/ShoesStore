﻿using System;
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
        public string Username { get; set; }
        public int CartID { get; set; }
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
            string sql = "Insert into Orders(cartID, paymentMethod, total, date, status) values(@cartID, @paymentMethod, @total, @date, @status)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@cartID", order.CartID);
            cmd.Parameters.AddWithValue("@paymentMethod", order.PaymentMethod);
            cmd.Parameters.AddWithValue("@total", order.Total);
            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@status", order.Status);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            return result;
        }

        public List<OrderDTO> GetAllOrdersByUsername(string username)
        {
            List<OrderDTO> list = new List<OrderDTO>();
            string sql = "SELECT o.* FROM Carts c, Orders o WHERE c.cartID = o.cartID AND c.userID = @userID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed){
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@userID", username);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new OrderDTO { OrderId = dr.GetInt32(0), PaymentMethod = dr.GetString(2), Total = (float)dr.GetDouble(3), DateCreated = dr.GetDateTime(4), Status = dr.GetString(5) });
            }
            dr.Close();
            cnn.Close();
            return list;
        }

        public List<OrderDTO> GetAllOrders()
        {
            List<OrderDTO> listOrders = new List<OrderDTO>();
            string sql = "SELECT o.*, userID FROM Carts c, Orders o WHERE c.cartID = o.cartID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed){
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int orderID = (int)dr[0];
                int cartID = (int)dr[1];
                string paymentMethod = (string)dr[2];
                double total = (double)dr[3];
                DateTime date = (DateTime)dr[4];
                string status = (string)dr[5];
                string username = (string)dr[6];
                listOrders.Add(new OrderDTO { OrderId = orderID, CartID = cartID, PaymentMethod = paymentMethod, DateCreated = date, Status = status, Total = (float)total, Username = username });
            }
            cnn.Close();
            return listOrders;
        }

        public bool UpdateOrderStatusByOrderID(int orderID, string status)
        {
            bool result = false;
            string sql = "Update Orders Set status = @status Where orderID = @orderID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@orderID", orderID);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
