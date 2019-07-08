using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    class CartDTO
    {
        private int cartId;
        private string username;
        private bool isCheckedOut;

        public int CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool IsCheckedOut
        {
            get { return isCheckedOut; }
            set { isCheckedOut = value; }
        }

    }
}
