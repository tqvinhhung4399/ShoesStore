using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class CartItemDTO
    {
        private int cartId;
        private int productDetailId;
        private int quantity;

        public CartItemDTO(int cartId, int productDetailId, int quantity)
        {
            this.cartId = cartId;
            this.productDetailId = productDetailId;
            this.quantity = quantity;
        }

        public int CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public int ProductDetailId
        {
            get { return productDetailId; }
            set { productDetailId = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

    }
}
