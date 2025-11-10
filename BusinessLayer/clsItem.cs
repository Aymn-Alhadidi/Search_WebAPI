using DataAccessLayer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsItem
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public DTOs.ItemDTOs.ItemDTO ItemDTO
        {
            get { return (new DTOs.ItemDTOs.ItemDTO(this.ID, this.Name, this.created_at)); }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime created_at { get; set; }

        public clsItem(DTOs.ItemDTOs.ItemDTO CDTO, enMode cMode = enMode.AddNew)

        {
            this.ID = CDTO.Id;
            this.Name = CDTO.Name;
            this.created_at = CDTO.created_at;

            Mode = cMode;
        }


        public static List<DTOs.ItemDTOs.ItemDTO> GetAllItems()
        {
            return ItemsData.GetAllItems();
        }

        public static clsItem Find(int ID)
        {

            DTOs.ItemDTOs.ItemDTO SDTO = ItemsData.GetItemById(ID);

            if (SDTO != null)
            {
                return new clsItem(SDTO, enMode.Update);
            }

            else
                return null;
        }

        private bool _AddNewItem()
        {
            //call DataAccess Layer 

            this.ID = ItemsData.AddItem(ItemDTO);

            return (this.ID != -1);
        }

        private bool _UpdateItem()
        {
            return ItemsData.UpdateItem(ItemDTO);
        }

        public static bool DeleteItem(int ID)
        {
            return ItemsData.DeleteItem(ID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewItem())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateItem();

            }

            return false;
        }
    }
}
