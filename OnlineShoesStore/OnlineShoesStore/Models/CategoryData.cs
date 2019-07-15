using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class CategoryDTO
    {
        private int categoryId;
        private string name;
        private bool isDeleted;

        public CategoryDTO(int categoryId, string name, bool isDeleted)
        {
            this.categoryId = categoryId;
            this.name = name;
            this.isDeleted = isDeleted;
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }

    public class CategoryData
    {        
        public List<ProductDTO> GetProductsByCategoryID(int id)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "SELECT s.name, p.price, p.productID, i.image, p.color " +
                "FROM Products p, Shoes s, ProductImages i " +
                "WHERE p.shoesID = s.shoesID " +
                "AND p.productID = i.productID " +
                "AND s.categoryID = @catID " +
                "AND p.isDeleted=0 " +
                "AND s.isDeleted=0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@catID", id);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            string image;
            double price;
            string name;
            ProductDTO dto = null;
            while (dr.Read())
            {
                name = dr.GetString(0);
                price = dr.GetDouble(1);
                productID = dr.GetInt32(2);
                image = dr.GetString(3);
                color = dr.GetString(4);
                dto = new ProductDTO(productID, 0, price, color, false);
                dto.Name = name;
                dto.Image = image;
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "SELECT s.name, p.price, p.productID, i.image, p.color " +
                "FROM Products p, Shoes s, ProductImages i " +
                "WHERE p.shoesID = s.shoesID " +
                "AND p.productID = i.productID " +
                "AND p.isDeleted=0 " +
                "AND s.isDeleted=0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            string image;
            double price;
            string name;
            ProductDTO dto = null;
            while (dr.Read())
            {
                name = dr.GetString(0);
                price = dr.GetDouble(1);
                productID = dr.GetInt32(2);
                image = dr.GetString(3);
                color = dr.GetString(4);
                dto = new ProductDTO(productID, 0, price, color, false);
                dto.Name = name;
                dto.Image = image;
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<CategoryDTO> GetCategories()
        {
            List<CategoryDTO> result = null;
            string sql = "Select * From Categories Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            SqlDataReader dr = cmd.ExecuteReader();
            int categoryId;
            string name;
            result = new List<CategoryDTO>();
            while (dr.Read())
            {
                categoryId = dr.GetInt32(0);
                name = dr.GetString(1);
                result.Add(new CategoryDTO(categoryId, name, false));
            }
            cnn.Close();
            return result;
        }

        public string GetCategoryNameByID(int id) {
            string categoryID = "";
            string sql = "Select name From Categories Where categoryID = @categoryID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@categoryID", id);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read()) {
                categoryID = (string)dr[0];
            }
            cnn.Close();
            return categoryID;
        }
    }
}
