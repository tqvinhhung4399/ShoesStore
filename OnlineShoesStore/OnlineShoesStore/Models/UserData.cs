using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace OnlineShoesStore.Models
{
    public class UserDTO
    {
        private string username;
        private string password;
        private string fullname;
        private string gender;
        private DateTime dateOfBirth;
        private string address;
        private string tel;
        private bool isDeleted;
        private string role;

        public UserDTO(string username, string password, string fullname, string gender, DateTime dateOfBirth, string address, string tel, bool isDeleted, string role)
        {
            this.username = username;
            this.password = password;
            this.fullname = fullname;
            this.gender = gender;
            this.dateOfBirth = dateOfBirth;
            this.address = address;
            this.tel = tel;
            this.isDeleted = isDeleted;
            this.role = role;
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

    }

    public class UserData
    {
        //dat connection string o day xai` tam nha
        private string connectionString = "Server=.\\SQLEXPRESS;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        // may cai nay anh test thu, khong quan trong nhung cung dung xoa nha
        //public IConfiguration Configuration { get; }
        //public UserData(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public bool RegisterUser(UserDTO user)
        {

            //string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            bool result;
            string sql = "Insert into Users values (@username, @password, @role, @fullname, " +
                "@gender, @dob, @address, @tel, @isDeleted)";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@role", user.Role);
            cmd.Parameters.AddWithValue("@fullname", user.Fullname);
            cmd.Parameters.AddWithValue("@gender", user.Gender);
            cmd.Parameters.AddWithValue("@dob", user.DateOfBirth);
            cmd.Parameters.AddWithValue("@address", user.Address);
            cmd.Parameters.AddWithValue("@tel", user.Tel);
            cmd.Parameters.AddWithValue("@isDeleted", user.IsDeleted);
            result = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return result;
        }
        
        public UserDTO CheckLogin(string username, string password)
        {
            UserDTO user = null;
            string sql = "Select fullname, gender, dob, address, tel, role From Users Where UserID like @username and Password like @password and isDeleted = @false";
            SqlConnection cnn = new SqlConnection(connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@false", false);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string fullname = (string)dr[0];
                string gender = (string)dr[1];
                DateTime dob = (DateTime)dr[2];
                string address = (string)dr[3];
                string tel = (string)dr[4];
                string role = (string)dr[5];
                user = new UserDTO(username, password = null, fullname, gender, dob, address, tel, false, role);
            }
            cnn.Close();
            return user;
        }
    }
}
