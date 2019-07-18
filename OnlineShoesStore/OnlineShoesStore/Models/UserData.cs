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
        private DateTime dob;
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
            this.dob = dateOfBirth;
            this.address = address;
            this.tel = tel;
            this.isDeleted = isDeleted;
            this.role = role;
        }

        public UserDTO()
        {

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

        public DateTime Dob
        {
            get { return dob; }
            set { dob = value; }
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
        //private readonly string connectionString = "Server=.;Database=ShoesStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        // may cai nay anh test thu, khong quan trong nhung cung dung xoa nha
        // public IConfiguration Configuration { get; }
        // public UserData(IConfiguration configuration)
        // {
        //    Configuration = configuration;
        // }

        public bool RegisterUser(UserDTO user)
        {

            // string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            bool result;
            string sql = "Insert into Users values (@username, @password, @role, @fullname, " +
                "@gender, @dob, @address, @tel, @isDeleted)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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
            cmd.Parameters.AddWithValue("@dob", user.Dob);
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
            string sql = "Select fullname, gender, dob, address, tel, role From Users Where UserID = @username and Password = @password and isDeleted = @false";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
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

        public List<UserDTO> LoadUsers()
        {
            List<UserDTO> result = new List<UserDTO>();
            string sql = "Select userID, fullname, gender, dob, address, tel, isDeleted, role From Users";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read()) {
                string username = (string)dr[0];
                string fullname = (string)dr[1];
                string gender = (string)dr[2];
                DateTime dob = (DateTime)dr[3];
                string address = (string)dr[4];
                string tel = (string)dr[5];
                bool isDeleted = (bool)dr[6];
                string role = (string)dr[7];
                UserDTO user = new UserDTO(username,null,fullname,gender,dob,address,tel,isDeleted,role);
                result.Add(user);
            }
            cnn.Close();
            return result;
        }

        public bool BanUserByUsername(string username)
        {
            bool result = false;
            string sql = "Update Users Set isDeleted = @isDeleted Where userID = @userID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@isDeleted", true);
            cmd.Parameters.AddWithValue("@userID", username);
            result = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return result;
        }

        public bool UnbanUserByUsername(string username)
        {
            bool result = false;
            string sql = "Update Users Set isDeleted = @isDeleted Where userID = @userID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@isDeleted", false);
            cmd.Parameters.AddWithValue("@userID", username);
            result = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return result;
        }

        public bool ChangePassword(string username, string password)
        {
            bool result = false;
            string sql = "Update Users Set password = @Password Where userID = @userID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@userID", username);
            result = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return result;
        }

        public bool ChangeInfo(UserDTO dto)
        {
            bool result = false;
            string sql = "Update Users Set fullname=@Fullname, gender=@Gender, dob=@Dob, address=@Address, tel=@Tel Where userID = @userID";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@Fullname", dto.Fullname);
            cmd.Parameters.AddWithValue("@Gender", dto.Gender);
            cmd.Parameters.AddWithValue("@Dob", dto.Dob);
            cmd.Parameters.AddWithValue("@Address", dto.Address);
            cmd.Parameters.AddWithValue("@Tel", dto.Tel);
            cmd.Parameters.AddWithValue("@userID", dto.Username);
            result = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return result;
        }

        public UserDTO GetUserInfoByUserID(string username)
        {
            UserDTO dto = null;
            string sql = "SELECT fullname, gender, dob, address, tel FROM Users WHERE userID=@username";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@username", username);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dto = new UserDTO
                {
                    Fullname = dr.GetString(0),
                    Gender = dr.GetString(1),
                    Dob = dr.GetDateTime(2),
                    Address = dr.GetString(3),
                    Tel = dr.GetString(4),
                    Username = username
                };
            }
            cnn.Close();
            return dto;
        }
    }
}
