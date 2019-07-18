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

        public ShoesDTO()
        {

        }

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
        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; }
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
        public List<ShoesDTO> FindAll()
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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

        public List<ShoesDTO> GetAllShoes()
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            ShoesDTO dto = null;
            while (dr.Read())
            {
                shoesId = dr.GetInt32(0);
                name = dr.GetString(1);
                categoryId = dr.GetInt32(2);
                brandId = dr.GetInt32(3);
                material = dr.GetString(4);
                des = dr.GetString(5);
                originId = dr.GetInt32(6);
                dto = new ShoesDTO(shoesId, name, categoryId, brandId, material, des, originId, false);
                dto.BrandName = new BrandData().GetBrandNameByID(brandId);
                dto.CategoryName = new CategoryData().GetCategoryNameByID(categoryId);
                dto.OriginName = new OriginData().GetOriginNameByID(originId);
                result.Add(dto);
            }
            cnn.Close();
            return result;
        }

        public List<ShoesDTO> FindByCategory(int categoryId)
        {
            List<ShoesDTO> result = null;
            string sql = "Select * From Shoes Where isDeleted = @Deleted And categoryId = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            string sql = "Select name, categoryID, brandID, material, description, originID, P.price, P.color From Shoes, Products P Where Shoes.isDeleted = @isDeleted and Shoes.shoesID = @shoesID and P.productID = @productID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            cmd.Parameters.AddWithValue("@productID", productID);
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
                double price = (double)dr[6];
                string color = (string)dr[7];
                shoes = new ShoesDTO(shoesID, name, categoryName, brandName, (float)price, null);
                shoes.Color = color;
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
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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

        public bool AddNewShoes(ShoesDTO shoes)
        {
            bool result = false;
            string sql = "Insert into Shoes(name, categoryID, brandID, material, description, originID, isDeleted) values (@name, @categoryID, @brandID, @material, @description, @originID, @isDeleted)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@name", shoes.Name);
            cmd.Parameters.AddWithValue("@categoryID", shoes.CategoryId);
            cmd.Parameters.AddWithValue("@brandID", shoes.BrandId);
            cmd.Parameters.AddWithValue("@material", shoes.Material);
            cmd.Parameters.AddWithValue("@description", shoes.Description);
            cmd.Parameters.AddWithValue("@originID", shoes.OriginId);
            cmd.Parameters.AddWithValue("@isDeleted", false);
            if (cmd.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            cnn.Close();
            return result;
        }


        public int GetNewestShoesId()
        {
            int shoesId = 0;
            string sql = "Select TOP 1 shoesID From Shoes Order By shoesID DESC";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                shoesId = dr.GetInt32(0);
            }
            return shoesId;
        }


        public ShoesDTO GetShoesInformationByShoesID(int shoesID)
        {
            ShoesDTO shoes = null;
            string sql = "SELECT * FROM Shoes WHERE shoesID = @shoesID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int shoesId = (int)dr[0];
                string name = (string)dr[1];
                int categoryID = (int)dr[2];
                string categoryName = new CategoryData().GetCategoryNameByID(categoryID);
                int brandID = (int)dr[3];
                string brandName = new BrandData().GetBrandNameByID(brandID);
                string material = (string)dr[4];
                string description = (string)dr[5];
                int originID = (int)dr[6];
                string originName = new OriginData().GetOriginNameByID(originID);
                shoes = new ShoesDTO
                {
                    ShoesId = shoesId,
                    Name = name,
                    CategoryId = categoryID,
                    CategoryName = categoryName,
                    BrandId = brandID,
                    BrandName = brandName,
                    Material = material,
                    Description = description,
                    OriginId = originID,
                    OriginName = originName
                };
            }
            cnn.Close();
            return shoes;
        }
    }
}
