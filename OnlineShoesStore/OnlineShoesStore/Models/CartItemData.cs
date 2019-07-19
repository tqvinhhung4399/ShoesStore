using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class CartItemDTO
    {
        private int cartId;
        private int productDetailId;
        private int quantity;
        public CartItemDTO()
        {

        }
        public CartItemDTO(int cartId, int productDetailId, int quantity)
        {
            this.cartId = cartId;
            this.productDetailId = productDetailId;
            this.quantity = quantity;
        }

        public int CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public int ProductDetailId
        {
            get { return productDetailId; }
            set { productDetailId = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

    }

    public class CartItemData
    {
        //HÀM NÀY QUAN TRỌNG, CHECKOUT GIỎ HÀNG PHẢI GỌI RA CHECK
        //public bool CheckValidCartItems(List<CartItemDTO> listCartItems)
        //{
        //    bool result = false;
        //    string sql = "";
        //    SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
        //    if (cnn.State == ConnectionState.Closed)
        //    {
        //        cnn.Open();
        //    }
        //    SqlCommand cmd;
        //    foreach (CartItemDTO item in listCartItems)
        //    {
        //        cmd = new SqlCommand(sql, cnn);
        //        //kiểm tra nếu như ko valid thì remove item đó ra khỏi list
        //        //result = false làm cờ để trả lại trang cart, true thì dẫn qua check out

        //    }
        //}

        

        //load tất cả sản phẩm trong giỏ hàng
        //public List<CartItemDTO> GetCartItemsByCartID(int cartID)
        //{
        //    List<CartItemDTO> listCartItems = new List<CartItemDTO>();

        //}

        //thêm sản phẩm mới vào giỏ hàng
        public bool InsertNewItemToCart(CartItemDTO item)
        {
            bool result = false;
            string sql = "Insert into CartItems values(@cartID, @productDetailID, @quantity)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@cartID", item.CartId);
            cmd.Parameters.AddWithValue("@productDetailID", item.ProductDetailId);
            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            cnn.Close();
            return result;
        }

        
    }
}
