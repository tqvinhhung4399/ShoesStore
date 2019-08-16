using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoesStore.Models
{
    public class DataTableDTO
    {
        private int productID;

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        private string name;
        private string color;
        private string brand;
        private double price;
        private bool isDeleted;

        

        private List<string> sizeQuantity;

        public DataTableDTO(string brand, string name, string color, double price, List<string> sizeQuantity, int productID, bool isDeleted)
        {
            this.brand = brand;
            this.name = name;
            this.color = color;
            this.price = price;
            this.sizeQuantity = sizeQuantity;
            this.productID = productID;
            this.isDeleted = isDeleted;
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public List<string> SizeQuantity
        {
            get { return sizeQuantity; }
            set { sizeQuantity = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }


        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }



        public string Color
        {
            get { return color; }
            set { color = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }

    public class DataTableData
    {
        public List<string> GetAllSizeQuantity(int id)
        {
            List<string> sizeQuantity;
            string sql = "SELECT size, quantity FROM ProductDetails p WHERE p.isDeleted = @Deleted AND p.productID = @Id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            sizeQuantity = new List<string>();
            while (dr.Read())
            {
                string sq = "" + dr.GetDouble(0) + " - " + dr.GetInt32(1);
                sizeQuantity.Add(sq);
            }
            dr.Close();
            cnn.Close();
            return sizeQuantity;
        }

        public List<DataTableDTO> GetData()
        {
            List<DataTableDTO> result = null;
            string sql = "SELECT a.brand, a.name, p.color, p.price, p.productID, p.isDeleted " +
                            "FROM(SELECT b.name as brand, s.name, s.shoesID " +
                                    "FROM Shoes s, Brands b " +
                                    "WHERE s.brandID = b.brandID " +
                                    "AND s.isDeleted = 0 " +
                                    "AND b.isDeleted = 0) a, Products p " +
                            "WHERE a.shoesID = p.shoesID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            int proID;
            string name;
            string brand;
            string color;
            double prices;
            bool isDeleted;
            List<string> sizeQuantity;
            result = new List<DataTableDTO>();
            while (dr.Read())
            {
                brand = dr.GetString(0);
                name = dr.GetString(1);
                color = dr.GetString(2);
                prices = dr.GetDouble(3);
                proID = dr.GetInt32(4);
                sizeQuantity = GetAllSizeQuantity(dr.GetInt32(4));
                isDeleted = dr.GetBoolean(5);
                result.Add(new DataTableDTO(brand, name, color, prices, sizeQuantity, proID, isDeleted));
            }
            dr.Close();
            cnn.Close();
            return result;
        }
    }
}