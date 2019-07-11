using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    class UserDTO
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
        private int i;
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
}
