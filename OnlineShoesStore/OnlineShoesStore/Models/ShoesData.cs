using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OnlineShoesStore.Models
{
    public class ShoesDTO
    {
        private int shoesId;
        private string name;
        private int categoryId;
        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        
        private int brandId;
        private string brandName;
        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }
        private float price;

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }


        private string material;
        private string description;
        private int originId;
        private string originName;
        public string OriginName
        {
            get { return originName; }
            set { originName = value; }
        }
        
        private bool isDeleted;

        public ShoesDTO(int shoesId, string name, int categoryId, int brandId, string material, string description, int originId, bool isDeleted)
        {
            this.shoesId = shoesId;
            this.name = name;
            this.categoryId = categoryId;
            this.brandId = brandId;
            this.material = material;
            this.description = description;
            this.originId = originId;
            this.isDeleted = isDeleted;
        }

        public ShoesDTO(int shoesID, string name, string categoryName, string brandName, float price, string image)
        {
            this.shoesId = shoesID;
            this.name = name;
            this.categoryName = categoryName;
            this.brandName = brandName;
            this.price = price;
            this.image = image;
        }

        public int ShoesId
        {
            get { return shoesId; }
            set { shoesId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public int BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        public string Material
        {
            get { return material; }
            set { material = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int OriginId
        {
            get { return originId; }
            set { originId = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }

    public class ShoesData
    {
        private string connectionString = "Server=.\\SQLEXPRESS;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<ShoesDTO> FindAll()
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            SqlDataReader dr = cmd.ExecuteReader();
            int shoesId, categoryId, brandId, originId;
            string name, material, des;
            result = new List<ShoesDTO>();
            while (dr.Read())
            {
                shoesId = dr.GetInt32(0);
                name = dr.GetString(1);
                categoryId = dr.GetInt32(2);
                brandId = dr.GetInt32(3);
                material = dr.GetString(4);
                des = dr.GetString(5);
                originId = dr.GetInt32(6);
                result.Add(new ShoesDTO(shoesId, name, categoryId, brandId, material, des, originId, false));
            }
            cnn.Close();
            return result;
        }

        public List<ShoesDTO> FindByCategory(int categoryId)
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted And categoryId = @Id";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@Id", categoryId);
            SqlDataReader dr = cmd.ExecuteReader();
            int shoesId, brandId, originId;
            string name, material, des;
            result = new List<ShoesDTO>();
            while (dr.Read())
            {
                shoesId = dr.GetInt32(0);
                name = dr.GetString(1);
                brandId = dr.GetInt32(3);
                material = dr.GetString(4);
                des = dr.GetString(5);
                originId = dr.GetInt32(6);
                result.Add(new ShoesDTO(shoesId, name, categoryId, brandId, material, des, originId, false));
            }
            cnn.Close();
            return result;
        }

        public List<ShoesDTO> FindByBrand(int brandId)
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted And brandId = @Id";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@Id", brandId);
            SqlDataReader dr = cmd.ExecuteReader();
            int shoesId, categoryId, originId;
            string name, material, des;
            result = new List<ShoesDTO>();
            while (dr.Read())
            {
                shoesId = dr.GetInt32(0);
                name = dr.GetString(1);
                categoryId = dr.GetInt32(2);
                material = dr.GetString(4);
                des = dr.GetString(5);
                originId = dr.GetInt32(6);
                result.Add(new ShoesDTO(shoesId, name, categoryId, brandId, material, des, originId, false));
            }
            cnn.Close();
            return result;
        }

        public List<ShoesDTO> FindByOrigin(int originId)
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted And originId = @Id";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@Id", originId);
            SqlDataReader dr = cmd.ExecuteReader();
            int shoesId, categoryId, brandId;
            string name, material, des;
            result = new List<ShoesDTO>();
            while (dr.Read())
            {
                shoesId = dr.GetInt32(0);
                name = dr.GetString(1);
                categoryId = dr.GetInt32(2);
                brandId = dr.GetInt32(3);
                material = dr.GetString(4);
                des = dr.GetString(5);
                result.Add(new ShoesDTO(shoesId, name, categoryId, originId, material, des, originId, false));
            }
            cnn.Close();
            return result;
        }

        public List<ShoesDTO> FindByName(string search) //tim kiem giay theo ten, hien thi o trang search: name, category, brand, price, image
        {
            List<ShoesDTO> result = null;
            string sql = "Select shoeID, name, categoryID, brandID From Shoes Where isDeleted = @Deleted And name like @name";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@name", "%" + search + "%");
            SqlDataReader dr = cmd.ExecuteReader();
            result = new List<ShoesDTO>();
            while (dr.Read())
            {
                int shoesID = dr.GetInt32(0);
                string name = dr.GetString(1);
                int categoryID = dr.GetInt32(2);
                string categoryName = new CategoryData().GetCategoryNameByID(categoryID);
                int brandID = dr.GetInt32(3);
                string brandName = new BrandData().GetBrandNameByID(brandID);
                float price = GetPriceByShoesID(shoesID);
                string image = GetImageByShoesID(shoesID);
                result.Add(new ShoesDTO(shoesID, name, categoryName, brandName, price, image));
            }
            cnn.Close();
            return result;
        }

        private int GetShoesIDByProductID(int productID)
        {
            int shoesID = 0;
            string sql = "Select ShoesID From Products Where productID = @productID";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@productID", productID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                shoesID = (int)dr[0];
            }
            cnn.Close();
            return shoesID;
        }

        public ShoesDTO GetShoesDetailByProductID(int productID)
        {
            int shoesID = GetShoesIDByProductID(productID);
            ShoesDTO shoes = null;
            string sql = "Select name, categoryID, brandID, material, description, originID From Shoes Where isDeleted = @isDeleted and shoesID = @shoesID";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string name = (string)dr[0];
                int categoryID = (int)dr[1];
                string categoryName = new CategoryData().GetCategoryNameByID(categoryID);
                int brandID = (int)dr[2];
                string brandName = new BrandData().GetBrandNameByID(brandID);
                string material = (string)dr[3];
                string description = (string)dr[4];
                int originID = (int)dr[5];
                string originName = new OriginData().GetOriginNameByID(originID);
                shoes = new ShoesDTO(shoesID, name, categoryName, brandName, 0, null);
                shoes.Material = material;
                shoes.Description = description;
                shoes.OriginName = originName;
            }
            cnn.Close();
            return shoes;
        }

        public float GetPriceByShoesID(int shoesID)
        {
            float price = 0;
            string sql = "Select MIN(P.price) From Products P, Shoes S Where P.shoesID = S.shoesID and S.shoesID = @shoesID";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                price = (float)dr[0];
            }
            cnn.Close();
            return price;
        }

        public string GetImageByShoesID(int shoesID)
        {
            string image = "";
            string sql = "Select top 1 PI.image " +
                        "From ProductImages PI, (Select top 1 P.productID From Products P, Shoes S Where P.shoesID = S.shoesID and S.shoesID = @shoesID) PP " +
                        "Where PI.productID = PP.productID";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                image = (string)dr[0];
            }
            cnn.Close();
            return image;
        }

    }
}
