using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class SearchDTOs
    {
        public class PopularItems
        {

            public int item_id { get; set; }
            public string item_name { get; set; }
            public int selection_count { get; set; }

            public PopularItems(int id, string name, int created_at)
            {
                this.item_id = id;
                this.item_name = name;
                this.selection_count = created_at;
            }

        }

        public class SearchItem
        {

            public string id { get; set; }
            public string client_id { get; set; }
            public string keyword { get; set; }
            public int item_id { get; set; }
            public string item_name { get; set; }
            public DateTime searched_at { get; set; }

            public SearchItem(string id, string client_id, string keyword, int item_id, string item_name, DateTime searched_at)
            {
                this.id = id;
                this.client_id = client_id;
                this.keyword = keyword;
                this.item_id = item_id;
                this.item_name = item_name;
                this.searched_at = searched_at;
            }

        }
    }
}
