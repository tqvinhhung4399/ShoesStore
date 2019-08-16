using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class BrandDTO
    {
        private int brandId;
        private string name;
        private bool isDeleted;

        public BrandDTO()
        {

        }

        public BrandDTO(int brandId, string name, bool isDeleted)
        {
            this.brandId = brandId;
            this.name = name;
            this.isDeleted = isDeleted;
        }

        public int BrandId
        {
            get { return brandId; }
            set { brandId = value; }
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

    public class BrandData
    {
        public List<BrandDTO> GetBrands()
        {
            List<BrandDTO> result = null;
            string sql = "Select * From Brands Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            SqlDataReader dr = cmd.ExecuteReader();
            int brandId;
            string name;
            result = new List<BrandDTO>();
            while (dr.Read())
            {
                brandId = dr.GetInt32(0);
                name = dr.GetString(1);
                result.Add(new BrandDTO(brandId, name, false));
            }
            cnn.Close();
            return result;
        }

        public string GetBrandNameByID(int id) {
            string brandName = "";
            string sql = "Select name From Brands Where brandID = @brandID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@brandID", id);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read()) {
                brandName = (string)dr[0];
            }
            cnn.Close();
            return brandName;
        }

        public bool AddNewBrand(string name)
        {
            bool check = false;
            string sql = "Insert Into Brands(name, isDeleted) Values(@Name, @IsDeleted)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@IsDeleted", false);
            check = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return check;
        }
        
        public bool UpdateBrand(BrandDTO brand)
        {
            bool check = false;
            string sql = "Update Brands Set name = @Name Where brandID = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Name", brand.Name);
            cmd.Parameters.AddWithValue("@Id", brand.BrandId);
            check = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return check;
        }
    }
}

