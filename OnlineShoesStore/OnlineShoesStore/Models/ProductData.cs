using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        string connectionString = "Server=.\\SQLEXPRESS;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<ProductDTO> GetProductsByCategoryID(int id)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select p.name, p.price, p.productID, p.color From Product P, Shoes S Where P.shoesID = S.ShoesID and S.categoryID = @catID";
            SqlConnection cnn = new SqlConnection(connectionString);
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
                dto.Image = new ProductImageData().GetImageByProductID(productID);
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select p.name, p.price, p.productID, p.color From Product P, Shoes S Where P.shoesID = S.ShoesID";
            SqlConnection cnn = new SqlConnection(connectionString);
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
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }
    }
}
