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
        private string name;
        private string color;
        private string brand;
        private double price;
        private List<string> sizeQuantity;

        public DataTableDTO(string brand, string name, string color, double price, List<string> sizeQuantity)
        {
            this.brand = brand;
            this.name = name;
            this.color = color;
            this.price = price;
            this.sizeQuantity = sizeQuantity;
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
            return sizeQuantity;
        }

        public List<DataTableDTO> GetData()
        {
            List<DataTableDTO> result = null;
            string sql = "SELECT a.brand, a.name, p.color, p.price, p.productID " +
                            "FROM(SELECT b.name as brand, s.name, s.shoesID " +
                                    "FROM Shoes s, Brands b " +
                                    "WHERE s.brandID = b.brandID " +
                                    "AND s.isDeleted = 0 " +
                                    "AND b.isDeleted = 0) a, Products p " +
                            "WHERE a.shoesID = p.shoesID " +
                                "AND p.isDeleted = 0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            string name;
            string brand;
            string color;
            double prices;
            List<string> sizeQuantity;
            result = new List<DataTableDTO>();
            while (dr.Read())
            {
                brand = dr.GetString(0);
                name = dr.GetString(1);
                color = dr.GetString(2);
                prices = dr.GetDouble(3);
                sizeQuantity = GetAllSizeQuantity(dr.GetInt32(4));
                result.Add(new DataTableDTO(brand, name, color, prices, sizeQuantity));
            }
            cnn.Close();
            return result;
        }
    }
}