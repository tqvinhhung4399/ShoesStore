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
        public float Price { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
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
        //HÀM NÀY QUAN TRỌNG, XEM GIỎ HÀNG VÀ CHECKOUT GIỎ HÀNG PHẢI GỌI RA CHECK
        public bool CheckValidCartItems(List<CartItemDTO> listCartItems)
        {
            if (CheckCartItemsIsDeleted(listCartItems) && UpdateQuantityInvalidCartItems(listCartItems))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private bool CheckCartItemsIsDeleted(List<CartItemDTO> listCartItems)
        {
            bool result = true;
            string sql = "Select S.ShoesID " +
                        "From Shoes S, " +
                                        "(Select shoesID " +
                                        "From Products P," +
                                                            "(Select PD.productID " +
                                                            "From ProductDetails PD, CartItem CI " +
                                                            "Where PD.productDetailID = CI.productDetailID and CI.productDetailID = @id) TMP " +
                                        "Where p.productID = TMP.productID and P.isDeleted = 0) TMP " +
                        "Where S.isDeleted = 0 and S.shoesID = TMP.shoesID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            foreach (CartItemDTO item in listCartItems)
            {
                cmd.Parameters.AddWithValue("@id", item.ProductDetailId);
                if (!cmd.ExecuteReader().Read())
                {
                    listCartItems.Remove(item);
                    result = false;
                }
            }
            cnn.Close();
            return result;
        }

        private bool UpdateQuantityInvalidCartItems(List<CartItemDTO> listCartItems)
        {
            bool result = true;
            List<int> listQuantities = new ProductDetailData().GetAvailableQuantityByProductDetailIDs(listCartItems);
            for(int i = 0; i < listCartItems.Count; i++)
            {
                if (listCartItems[0].Quantity > listQuantities[0])
                {
                    listCartItems[0].Quantity = listQuantities[0];
                    result = false;
                }
            }
            UpdateCartItemsQuantity(listCartItems);
            return result;
        }

        private void UpdateCartItemsQuantity(List<CartItemDTO> listCartItems)
        {
            string sql = "Update CartItem Set Quantity = @quantity Where cartID = @cartID and productDetailID = @productDetailID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            foreach (CartItemDTO item in listCartItems)
            {
                cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@cartID", item.CartId);
                cmd.Parameters.AddWithValue("@productDetailID", item.ProductDetailId);
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }

        

        //load tất cả sản phẩm trong giỏ hàng
        public List<CartItemDTO> GetCartItemsByCartID(int cartID)
        {
            List<CartItemDTO> listCartItems = new List<CartItemDTO>();
            string sql = "Select * From CartItem Where cartID = @cartID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@cartID", cartID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int productDetailID = (int)dr[1];
                int quantity = (int)dr[2];
                ProductDTO product = new ProductData().GetProductByProductDetailID(productDetailID);
                double price = product.Price;
                string color = product.Color;
                string name = new ShoesData().GetShoesDetailByProductID(product.ProductId).Name;
                CartItemDTO dto = new CartItemDTO { ProductDetailId = productDetailID, Quantity = quantity, Price = (float)price, Color = color, Name = name };
                listCartItems.Add(dto);
            }
            cnn.Close();
            return listCartItems;
        }

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

        public bool AlreadyExistedInCart(int cartID, int productDetailID)
        {
            bool result = false;
            string sql = "SELECT * FROM CartItem WHERE cartID=@cartID AND productDetailID=@productDetailID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@cartID", cartID);
            cmd.Parameters.AddWithValue("@productDetailID", productDetailID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = true;
            }
            cnn.Close();
            return result;
        }

    }
}
