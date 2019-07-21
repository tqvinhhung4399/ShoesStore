using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ContactDTO
    {

        private string fullname;
        private string email;
        private string message;
        private int contactID;

        public int ContactID
        {
            get { return contactID; }
            set { contactID = value; }
        }


        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }

    }
    public class ContactData
    {
        public bool InsertNewContact(ContactDTO dto)
        {
            bool result;
            string sql = "INSERT INTO Contacts(name, email, message) VALUES(@name, @email, @message)";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@name", dto.Fullname);
            cmd.Parameters.AddWithValue("@email", dto.Email);
            cmd.Parameters.AddWithValue("@message", dto.Message);
            result = cmd.ExecuteNonQuery() > 0;
            cnn.Close();
            return result;
        }

        public List<ContactDTO> GetAllData()
        {
            List<ContactDTO> list = new List<ContactDTO>();
            string sql = "SELECT * FROM Contacts";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if(cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new ContactDTO { ContactID = dr.GetInt32(0), Fullname = dr.GetString(1), Email = dr.GetString(2), Message = dr.GetString(3) });
            }
            dr.Close();
            cnn.Close();
            return list;
        }

        public string GetMessageByContactID(int id)
        {
            string message = "";
            string sql = "SELECT message FROM Contacts WHERE contactID = @id";
            SqlConnection cnn = new SqlConnection(Consts.Consts.connectionString);
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
            }
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                message = dr.GetString(0);
            }
            dr.Close();
            cnn.Close();
            return message;
        }
    }
}
