using System;

namespace DTOs
{
    public class ShoesDTO
    {
        private int shoesId;
        private string name;
        private int categoryId;
        private int brandId;
        private string material;
        private string description;
        private int originId;
        private bool isDeleted;

        public ShoesDTO(int shoesId, string name, int categoryId, int brandId, string material, string description, int originId, bool isDeleted)
        {
            this.shoesId = shoesId;
            this.name = name;
            this.categoryId = categoryId;
            this.brandId = brandId;
            this.material = material;
            this.description = description;
            this.originId = originId;
            this.isDeleted = isDeleted;
        }

        public int ShoesId
        {
            get { return shoesId; }
            set { shoesId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public int BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        public string Material
        {
            get { return material; }
            set { material = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int OriginId
        {
            get { return originId; }
            set { originId = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }
}
