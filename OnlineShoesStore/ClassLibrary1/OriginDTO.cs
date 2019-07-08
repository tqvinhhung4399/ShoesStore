using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    class OriginDTO
    {
        private int originId;
        private string name;
        private bool isDeleted;

        public OriginDTO(int originId, string name, bool isDeleted)
        {
            this.originId = originId;
            this.name = name;
            this.isDeleted = isDeleted;
        }

        public int OriginId
        {
            get { return originId; }
            set { originId = value; }
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
