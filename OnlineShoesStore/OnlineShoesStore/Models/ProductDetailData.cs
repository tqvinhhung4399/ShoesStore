using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductDetailDTO
    {
        private int productDetailId;
        private float size;
        private int quantity;
        private int productId;
        private bool isDeleted;

        public ProductDetailDTO(int productDetailId, float size, int quantity, int productId, bool isDeleted)
        {
            this.productDetailId = productDetailId;
            this.size = size;
            this.quantity = quantity;
            this.productId = productId;
            this.isDeleted = isDeleted;
        }

        public int ProductDetailId
        {
            get { return productDetailId; }
            set { productDetailId = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }

    public class ProductDetailData
    {
        private readonly string connectionString = "Server=.;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<ProductDetailDTO> GetProductDetailsByProductID(int productID)
        {
            List<ProductDetailDTO> result = new List<ProductDetailDTO>();
            string sql = "Select * From ProductDetails Where productID = @productID";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int productDetailID = (int)dr[0];
                double size = (double)dr[1];
                int quantity = (int)dr[2];
                bool isDeleted = (bool)dr[4];
                result.Add(new ProductDetailDTO(productDetailID, (float)size, quantity, productID, isDeleted));
            }
            cnn.Close();
            return result;
        }
    }
}
