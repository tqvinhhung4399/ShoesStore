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
        private string connectionString = "Server=.\\SQLEXPRESS;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<CategoryDTO> GetCategories()
        {
            List<CategoryDTO> result = null;
            string sql = "Select * From Categories Where isDeleted = @Deleted";
            SqlConnection cnn = new SqlConnection(connectionString);
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
            SqlConnection cnn = new SqlConnection(connectionString);
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
