using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductDetailDTO
    {
        private int productDetailId;
        private float size;
        private int quantity;
        private int productId;
        private bool isDeleted;

        public ProductDetailDTO(int productDetailId, float size, int quantity, int productId, bool isDeleted)
        {
            this.productDetailId = productDetailId;
            this.size = size;
            this.quantity = quantity;
            this.productId = productId;
            this.isDeleted = isDeleted;
        }

        public int ProductDetailId
        {
            get { return productDetailId; }
            set { productDetailId = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }
}
