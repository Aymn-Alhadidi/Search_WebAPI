using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsClient
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public DTOs.ClientDTOs.ClientDTO clientDTO
        {
            get { return (new DTOs.ClientDTOs.ClientDTO(this.ID, this.Name, this.created_at)); }
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime created_at { get; set; }

        public clsClient(DTOs.ClientDTOs.ClientDTO CDTO, enMode cMode = enMode.AddNew)

        {
            this.ID = CDTO.Id;
            this.Name = CDTO.Name;
            this.created_at = CDTO.created_at;

            Mode = cMode;
        }


        public static List<DTOs.ClientDTOs.ClientDTO> GetAllClients()
        {
            return ClientData.GetAllClients();
        }

        public static clsClient Find(string ID)
        {

            DTOs.ClientDTOs.ClientDTO SDTO = ClientData.GetClientById(ID);

            if (SDTO != null)
            {
                return new clsClient(SDTO, enMode.Update);
            }

            else
                return null;
        }

        private bool _AddNewClient()
        {
            //call DataAccess Layer 

            this.ID = ClientData.AddClient(clientDTO);

            return (!string.IsNullOrEmpty(this.ID));
        }

        private bool _UpdateClient()
        {
            return ClientData.UpdateClient(clientDTO);
        }

        public static bool DeleteClient(string ID)
        {
            return ClientData.DeleteClient(ID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewClient())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateClient();

            }

            return false;
        }
    }
}
