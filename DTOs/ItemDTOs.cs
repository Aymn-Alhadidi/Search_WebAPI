using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ItemDTOs
    {
        public class ItemDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime created_at { get; set; }

            public ItemDTO(int id, string name, DateTime created_at)
            {
                this.Id = id;
                this.Name = name;
                this.created_at = created_at;
            }

        }
    }
}
