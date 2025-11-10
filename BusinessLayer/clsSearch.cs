using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.clsClient;
using static DTOs.ClientDTOs;

namespace BusinessLayer
{
    public class clsSearch
    {

        public DTOs.SearchDTOs.SearchItem SearchItem
        {
            get { return (new DTOs.SearchDTOs.SearchItem(this.id, this.client_id, this.keyword, this.item_id, this.item_name, this.searched_at)); }
        }

        public string id { get; set; }
        public string client_id { get; set; }
        public string keyword { get; set; }
        public int item_id { get; set; }
        public string item_name { get; set; }
        public DateTime searched_at { get; set; }

        public clsSearch(DTOs.SearchDTOs.SearchItem SDTO)

        {
            this.id = SDTO.id;
            this.client_id = SDTO.client_id;
            this.keyword = SDTO.keyword;
            this.item_id = SDTO.item_id;
            this.item_name = SDTO.item_name;
            this.searched_at = SDTO.searched_at;
        }

        public static List<DTOs.SearchDTOs.PopularItems> GetPopularItems(string client_id)
        {
            return SearchData.GetPopularItems(client_id);
        }

        public bool AddNewClient()
        {
            //call DataAccess Layer 

            this.id = SearchData.AddSearchRecord(SearchItem);

            return (!string.IsNullOrEmpty(this.id));
        }
    }
}
