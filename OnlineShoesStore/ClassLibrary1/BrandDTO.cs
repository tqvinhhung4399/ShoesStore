using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    class BrandDTO
    {
        private int brandId;
        private string name;
        private bool isDeleted;

        public BrandDTO(int brandId, string name, bool isDeleted)
        {
            this.brandId = brandId;
            this.name = name;
            this.isDeleted = isDeleted;
        }

        public int BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

    }
}
