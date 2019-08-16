using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class CartDTO
    {
        private int cartId;
        private string username;
        private bool isCheckedOut;

        public int CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool IsCheckedOut
        {
            get { return isCheckedOut; }
            set { isCheckedOut = value; }
        }

    }

    public class CartData
    {
        //kiểm tra xem đã tồn tại Cart cho user đó chưa, nếu có thì trả ra cartID, chưa thì trả ra 0
        private int CheckCartIsExisted(string username) 
        {
            int cartID = 0;
            string sql = "Select * From Carts Where UserID = @userID and isCheckedOut = @isCheckedOut";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@userID", username);
            cmd.Parameters.AddWithValue("@isCheckedOut", false);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cartID = (int)dr[0];
            }
            dr.Close();
            cnn.Close();
            return cartID;
        }

        //lấy cartID theo user, nếu có rồi thì lấy còn chưa có thì tạo mới
        public int GetCartIDByUsername(string username)
        {
            int cartID;
            if ((cartID = CheckCartIsExisted(username)) == 0)
            {
                string sql = "Insert into Carts(userID, isCheckedOut) values(@userID, @isCheckedOut)";
                SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@userID", username);
                cmd.Parameters.AddWithValue("@isCheckedOut", false);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    cartID = CheckCartIsExisted(username);
                }
                cnn.Close();
            }
            return cartID;
        }

        //lúc check out thì update thuộc tính isDeleted của bảng Carts
        //đồng thời tạo mới record bên bảng Order
        public bool CheckOutCartByCartID(int cartID)
        {
            bool result = false;
            string sql = "Update Carts Set isCheckedOut = @isCheckedOut Where CartID = @cartID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@isCheckedOut", true);
            cmd.Parameters.AddWithValue("@cartID", cartID);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            cnn.Close();
            return result;
        }
    }
}
