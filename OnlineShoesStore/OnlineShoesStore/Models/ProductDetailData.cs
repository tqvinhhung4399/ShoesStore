﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductDetailDTO
    {
        private int productDetailId;
        private float size;
        private int quantity;
        private int productId;
        private bool isDeleted;

        public ProductDetailDTO()
        {

        }

        public ProductDetailDTO(int productDetailId, float size, int quantity, int productId, bool isDeleted)
        {
            this.productDetailId = productDetailId;
            this.size = size;
            this.quantity = quantity;
            this.productId = productId;
            this.isDeleted = isDeleted;
        }

        public int ProductDetailId
        {
            get { return productDetailId; }
            set { productDetailId = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }

    public class ProductDetailData
    {
        public List<ProductDetailDTO> GetProductDetailsByProductID(int productID)
        {
            List<ProductDetailDTO> result = new List<ProductDetailDTO>();
            string sql = "Select * From ProductDetails Where productID = @productID and Quantity > 0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@productID", productID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int productDetailID = (int)dr[0];
                double size = (double)dr[1];
                int quantity = (int)dr[2];
                bool isDeleted = (bool)dr[4];
                result.Add(new ProductDetailDTO(productDetailID, (float)size, quantity, productID, isDeleted));
            }
            cnn.Close();
            return result;
        }

        public bool UpdateQuantityByProductDTO(List<ProductDetailDTO> listProducts)
        {
            bool check = true;
            string sql = "Update ProductDetails Set quantity = @Quantity Where productID = @Id And size = @Size";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd;
            foreach (ProductDetailDTO detail in listProducts)
            {
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmd.Parameters.AddWithValue("@Id", detail.ProductId);
                cmd.Parameters.AddWithValue("@Size", detail.Size);
                if (cmd.ExecuteNonQuery() <= 0)
                {
                    check = false;
                    break;
                }
            }
            cnn.Close();
            return check;
        }

        public bool AddProductDetailsByProductDTO(List<ProductDetailDTO> listProducts)
        {
            bool check = true;
            string sql = "Insert Into ProductDetails(size, quantity, productID, isDeleted) Values (@Size, @Quantity, @ProductId, @IsDeleted)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd;
            foreach (ProductDetailDTO detail in listProducts)
            {
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@Size", detail.Size);
                cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmd.Parameters.AddWithValue("@ProductId", detail.ProductId);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                if (cmd.ExecuteNonQuery() <= 0)
                {
                    check = false;
                    break;
                }
            }
            cnn.Close();
            return check;
        }

        public List<int> GetAvailableQuantityByProductDetailIDs(List<ProductDetailDTO> listProductDetail)
        {
            List<int> listQuantity = new List<int>();
            string sql = "Select quantity From ProductDetails Where productDetailID = @id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }

            foreach (ProductDetailDTO item in listProductDetail)
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@id", item.ProductDetailId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int quantity = (int)dr[0];
                    listQuantity.Add(quantity);
                }
                dr.Close();
            }
            cnn.Close();
            return listQuantity;
        }

        public List<int> GetAvailableQuantityByProductDetailIDs(List<CartItemDTO> listCartItems)
        {
            List<int> listQuantity = new List<int>();
            string sql = "Select quantity From ProductDetails Where productDetailID = @id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            foreach (CartItemDTO item in listCartItems)
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@id", item.ProductDetailId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int quantity = (int)dr[0];
                    listQuantity.Add(quantity);
                }
                dr.Close();
            }
            cnn.Close();
            return listQuantity;
        }



        //H
        public double GetSizeByProductDetailID(int productDetailID)
        {
            double size = 0;
            string sql = "Select size From ProductDetails Where productDetailID = @id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@id", productDetailID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                size = (double)dr[0];
            }
            cnn.Close();
            return size;
        }

        public void UpdateAvailableProductDetailQuantity(List<CartItemDTO> listCartItems)
        {
            string sql = "Update ProductDetails Set Quantity = @quantity Where ProductDetailID = @productDetailID";
            List<int> listQuantity = GetAvailableQuantityByProductDetailIDs(listCartItems);
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd;
            for (int i = 0; i < listCartItems.Count; i++)
            {
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@quantity", listQuantity[i] - listCartItems[i].Quantity);
                cmd.Parameters.AddWithValue("@productDetailID", listCartItems[i].ProductDetailId);
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }
    }
}
