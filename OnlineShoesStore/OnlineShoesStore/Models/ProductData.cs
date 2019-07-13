using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShoesStore.Models
{
    class ProductDTO
    {
        private int productId;
        private int shoesId;
        private float price;
        private string color;
        private bool isDeleted;

        public ProductDTO(int productId, int shoesId, float price, string color, bool isDeleted)
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

        public float Price
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
}
