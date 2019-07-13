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
        private string connectionString = "Server=.\\SQLEXPRESS;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<BrandDTO> GetBrands()
        {
            List<BrandDTO> result = null;
            string sql = "Select * From Brands Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(connectionString);
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
    }
}

