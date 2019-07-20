using System;
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
        public string GetProductNameByProductID(int productID)
        {
            string name = "";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            string sql = "SELECT s.name FROM Products p, Shoes s WHERE p.shoesID = s.shoesID AND p.productID=@productID";
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("productID", productID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                name = dr.GetString(0);
            }
            dr.Close();
            cnn.Close();
            return name;
        }
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
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID and s.categoryID = @catID AND s.isDeleted=0 AND p.isDeleted=0";
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
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID AND s.isDeleted=0 AND p.isDeleted=0";
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



        //ublic ProductDTO GetProductByProductID(int productID)
        public bool UpdateListProducstById(List<ProductDTO> listProducts)
        {
            bool check = true;
            string sql = "Update Products Set price = @Price, color = @Color Where productID = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            foreach (ProductDTO product in listProducts)
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Color", product.Color);
                cmd.Parameters.AddWithValue("@Id", product.ProductId);
                if (cmd.ExecuteNonQuery() <= 0)
                {
                    check = false;
                    break;
                }
            }
            cnn.Close();
            return check;
        }

        public ProductDTO GetProductByProductID(int productID)
        {
            ProductDTO product = null;
            string sql = "Select * From Products Where productID = @productID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@productID", productID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int shoesID = (int)dr[1];
                double price = (double)dr[2];
                string color = (string)dr[3];
                product = new ProductDTO {ShoesId = shoesID, Price = price, Color = color, ProductId = productID };
            }
            cnn.Close();
            return product;
        }

        public ProductDTO GetProductByProductDetailID(int productDetailID)
        {
            ProductDTO product = null;
            string sql = "Select P.shoesID, P.price, P.color, P.productID From Products P, ProductDetails PD Where P.productID = PD.productID and PD.productDetailID = @productDetailID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@productDetailID", productDetailID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int shoesID = (int)dr[0];
                double price = (double)dr[1];
                string color = (string)dr[2];
                //H
                int productID = (int)dr[3];
                product = new ProductDTO { ShoesId = shoesID, Price = price, Color = color, ProductId = productID};
            }
            cnn.Close();
            return product;
        }

        public bool RemoveProduct(int productId)
        {
            bool check = false;
            string sql = "Update Products Set isDeleted = @IsDeleted Where productID = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@IsDeleted", true);
            cmd.Parameters.AddWithValue("@Id", productId);
            check = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return check;
        }

        public bool RestoreProduct(int productId)
        {
            bool check = false;
            string sql = "Update Products Set isDeleted = @IsDeleted Where productID = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@IsDeleted", false);
            cmd.Parameters.AddWithValue("@Id", productId);
            check = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return check;
        }

        public List<ProductDTO> GetProductsByFilters(string categoryID, string brandID, string priceRange)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID " +
                "and s.categoryID like @categoryID and s.brandID like @brandID AND p.price >= @lowPrice and p.price <= @highPrice AND s.isDeleted=0 AND p.isDeleted=0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@categoryID", categoryID);
            cmd.Parameters.AddWithValue("@brandID", brandID);
            int lowPrice = 0;
            int highPrice = 1000000;
            switch(priceRange)
            {
                case "0":
                    break;
                case "1":
                    highPrice = 100;
                    break;
                case "2":
                    lowPrice = 100;
                    highPrice = 200;
                    break;
                case "3":
                    lowPrice = 200;
                    break;
            }
            cmd.Parameters.AddWithValue("@lowPrice", lowPrice);
            cmd.Parameters.AddWithValue("@highPrice", highPrice);
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
    }
}
