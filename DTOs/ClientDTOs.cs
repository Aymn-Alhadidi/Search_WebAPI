namespace DTOs
{
    public class ClientDTOs
    {
        public class ClientDTO
        {

            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime created_at { get; set; }

            public ClientDTO(string id, string name, DateTime created_at)
            {
                this.Id = id;
                this.Name = name;
                this.created_at = created_at;
            }

        }
    }
}
