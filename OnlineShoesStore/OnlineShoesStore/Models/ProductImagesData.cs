using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductImagesDTO
    {
        private int productImageId;
        private int productId;
        private string image;

        public ProductImagesDTO(int productImageId, int productId, string image)
        {
            this.productImageId = productImageId;
            this.productId = productId;
            this.image = image;
        }

        public int ProductImageId
        {
            get { return productImageId; }
            set { productImageId = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

    }

    public class ProductImageData
    {
        public List<ProductImagesDTO> GetImagesByProductID(int productID)
        {
            List<ProductImagesDTO> result = new List<ProductImagesDTO>();
            string sql = "Select * From ProductImages Where productID = @productID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@productID", productID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int productImageID = (int)dr[0];
                string image = (string)dr[2];
                result.Add(new ProductImagesDTO(productImageID, productID, image));
            }
            cnn.Close();
            return result;
        }

        public string GetImageByProductID(int productID)
        {
            string image = "";
            string sql = "Select image From ProductImages Where productID = @productID";
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
                image = (string)dr[0];
            }
            cnn.Close();
            return image;
        }

        public bool InsertImagesByProductID(int productID , List<string> images)
        {
            bool check = true;
            string sql = "INSERT INTO ProductImages Values(@productID, @image)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            foreach (string image in images)
            {
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@productID", productID);
                cmd.Parameters.AddWithValue("@image", image);
                if (cmd.ExecuteNonQuery() <= 0)
                {
                    check = false;
                    break;
                }
            }
            cnn.Close();
            return check;
        }
    }
}
