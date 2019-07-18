﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductDTO
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        private int productId;
        private int shoesId;
        private double price;
        private string color;
        private bool isDeleted;
        public ProductDTO()
        {

        }
        public ProductDTO(int productId, int shoesId, double price, string color, bool isDeleted)
        {
            this.productId = productId;
            this.shoesId = shoesId;
            this.price = price;
            this.color = color;
            this.isDeleted = isDeleted;
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public int ShoesId
        {
            get { return shoesId; }
            set { shoesId = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }

    public class ProductData
    {
        public List<ProductDTO> GetProductsByShoesID(int shoesID)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "SELECT * FROM Products WHERE shoesID = @shoesID AND isDeleted=0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            double price;
            ProductDTO dto = null;
            while (dr.Read())
            {
                price = dr.GetDouble(2);
                productID = dr.GetInt32(0);
                color = dr.GetString(3);
                dto = new ProductDTO(productID, shoesID, price, color, false);
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<ProductDTO> GetProductsByCategoryID(int id)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID and s.categoryID = @catID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@catID", id);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            double price;
            string name;
            ProductDTO dto = null;
            while (dr.Read())
            {
                name = dr.GetString(0);
                price = dr.GetDouble(1);
                productID = dr.GetInt32(2);
                color = dr.GetString(3);
                dto = new ProductDTO(productID, 0, price, color, false);
                dto.Name = name;
                dto.Image = new ProductImageData().GetImageByProductID(productID);
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            double price;
            string name;
            ProductDTO dto = null;
            while (dr.Read())
            {
                name = dr.GetString(0);
                price = dr.GetDouble(1);
                productID = dr.GetInt32(2);
                color = dr.GetString(3);
                dto = new ProductDTO(productID, 0, price, color, false);
                dto.Image = new ProductImageData().GetImageByProductID(productID);
                dto.Name = name;
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public bool InsertProducts(List<ProductDTO> productsList)
        {
            bool check = true;
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            string sql = "Insert Into Products(shoesID, price, color, isDeleted) Values (@ShoesId, @Price, @Color, @IsDeleted)";
            SqlCommand cmd;
            foreach (ProductDTO product in productsList)
            {
                cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@ShoesId", product.ShoesId);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Color", product.Color);
                cmd.Parameters.AddWithValue("@IsDeleted", product.IsDeleted);
                if (cmd.ExecuteNonQuery() <= 0)
                {
                    check = false;
                    break;
                }
            }            
            cnn.Close();
            return check;
        }

        public bool UpdateProductById(ProductDTO product)
        {
            bool check = false;
            string sql = "Update Products Set price = @Price, color = @Color Where productID = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Color", product.Color);
            cmd.Parameters.AddWithValue("@Id", product.ProductId);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            check = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return check;
        }
    }
}
