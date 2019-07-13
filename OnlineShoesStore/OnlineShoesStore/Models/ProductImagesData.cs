using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShoesStore.Models
{
    public class ProductImagesDTO
    {
        private int productImageId;
        private int productId;
        private string image;

        public ProductImagesDTO(int productImageId, int productId, string image)
        {
            this.productImageId = productImageId;
            this.productId = productId;
            this.image = image;
        }

        public int ProductImageId
        {
            get { return productImageId; }
            set { productImageId = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

    }
}
