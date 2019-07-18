using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductDTO
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        private int productId;
        private int shoesId;
        private double price;
        private string color;
        private bool isDeleted;
        public ProductDTO()
        {

        }
        public ProductDTO(int productId, int shoesId, double price, string color, bool isDeleted)
        {
            this.productId = productId;
            this.shoesId = shoesId;
            this.price = price;
            this.color = color;
            this.isDeleted = isDeleted;
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public int ShoesId
        {
            get { return shoesId; }
            set { shoesId = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }

    public class ProductData 
    {
        public List<ProductDTO> GetProductsByShoesID(int shoesID)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "SELECT * FROM Products WHERE shoesID = @shoesID AND isDeleted=0";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@shoesID", shoesID);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            double price;
            ProductDTO dto = null;
            while (dr.Read())
            {
                price = dr.GetDouble(2);
                productID = dr.GetInt32(0);
                color = dr.GetString(3);
                dto = new ProductDTO(productID, shoesID, price, color, false);
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<ProductDTO> GetProductsByCategoryID(int id)
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID and s.categoryID = @catID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@catID", id);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            double price;
            string name;
            ProductDTO dto = null;
            while (dr.Read())
            {
                name = dr.GetString(0);
                price = dr.GetDouble(1);
                productID = dr.GetInt32(2);
                color = dr.GetString(3);
                dto = new ProductDTO(productID, 0, price, color, false);
                dto.Name = name;
                dto.Image = new ProductImageData().GetImageByProductID(productID);
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> list = new List<ProductDTO>();
            string sql = "Select s.name, p.price, p.productID, p.color From Products p, Shoes s Where p.shoesID = s.ShoesID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            string color;
            int productID;
            double price;
            string name;
            ProductDTO dto = null;
            while (dr.Read())
            {
                name = dr.GetString(0);
                price = dr.GetDouble(1);
                productID = dr.GetInt32(2);
                color = dr.GetString(3);
                dto = new ProductDTO(productID, 0, price, color, false);
                dto.Image = new ProductImageData().GetImageByProductID(productID);
                dto.Name = name;
                list.Add(dto);
            }
            cnn.Close();
            return list;
        }

        public bool InsertProducts(List<ProductDTO> productsList)
        {
            DataTable dt = new DataTable();
            dt = ConvertToTable(productsList);

            bool check = false;
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand("Insert Into Products(shoesID, price, color, isDeleted) Values (@ShoesId, @Price, @Color, @IsDeleted)", cnn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.InsertCommand = cmd;
            
            da.InsertCommand.Parameters.Add("@ShoesId", SqlDbType.Int, 0, "shoesId");
            da.InsertCommand.Parameters.Add("@Price", SqlDbType.Float, 0, "price");
            da.InsertCommand.Parameters.Add("@Color", SqlDbType.VarChar, 50, "color");
            da.InsertCommand.Parameters.Add("@IsDeleted", SqlDbType.Bit, 0, "isDeleted");
            //da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            cmd.UpdatedRowSource = UpdateRowSource.None;
            da.UpdateBatchSize = productsList.Count;
            da.Update(dt);
            cnn.Close();
            return check;
        }

        //private DataTable ToDataTable<T>(List<T> collection)
        //{
        //    DataTable dt = new DataTable("DataTable");
        //    Type t = typeof(T);
        //    PropertyInfo[] pia = t.GetProperties();

        //    //Inspect the properties and create the columns in the DataTable
        //    foreach (PropertyInfo pi in pia)
        //    {
        //        Type ColumnType = pi.PropertyType;
        //        if ((ColumnType.IsGenericType))
        //        {
        //            ColumnType = ColumnType.GetGenericArguments()[0];
        //        }
        //        dt.Columns.Add(pi.Name, ColumnType);
        //    }

        //    //Populate the data table
        //    foreach (T item in collection)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr.BeginEdit();
        //        foreach (PropertyInfo pi in pia)
        //        {
        //            if (pi.GetValue(item, null) != null)
        //            {
        //                dr[pi.Name] = pi.GetValue(item, null);
        //            }
        //        }
        //        dr.EndEdit();
        //        dt.Rows.Add(dr);
        //    }
        //    return dt;
        //}

        //private static DataTable ConvertToTable(List<ProductDTO> entities)
        //{
        //    var table = new DataTable(typeof(ProductDTO).Name);

        //    table.Columns.Add("ShoesId", typeof(int));
        //    table.Columns.Add("Price", typeof(float));
        //    table.Columns.Add("Color", typeof(string));
        //    table.Columns.Add("isDeleted", typeof(bool));
        //    foreach (var entity in entities)
        //    {
        //        var row = table.NewRow();
        //        row["ShoesId"] = entity.ShoesId;
        //        row["Price"] = entity.Price;
        //        row["Color"] = entity.Color;
        //        row["isDeleted"] = entity.IsDeleted;
        //        table.Rows.Add(row);
        //    }

        //    return table;
        //}
    }
}
