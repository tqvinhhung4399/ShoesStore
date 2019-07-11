using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    class CategoryDTO
    {
        private int categoryId;
        private string name;
        private bool isDeleted;

        public CategoryDTO(int categoryId, string name, bool isDeleted)
        {
            this.categoryId = categoryId;
            this.name = name;
            this.isDeleted = isDeleted;
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
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
