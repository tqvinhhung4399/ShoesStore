using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DTOs
{
    public class ShoesDTO
    {
        private int shoesId;
        private string name;
        private int categoryId;
        private int brandId;
        private string material;
        private string description;
        private int originId;
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
    }
}
