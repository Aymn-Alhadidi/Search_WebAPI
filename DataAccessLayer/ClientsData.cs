using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    //public class ClientDTO
    //{

    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public DateTime created_at { get; set; }

    //    public ClientDTO(string id, string name, DateTime created_at)
    //    {
    //        this.Id = id;
    //        this.Name = name;
    //        this.created_at = created_at;
    //    }

    //}

    public class ClientData
    {
        static string _connectionString = clsDataAccessSetting.ConnectionString;
        public static List<DTOs.ClientDTOs.ClientDTO> GetAllClients()
        {
            var ClientsList = new List<DTOs.ClientDTOs.ClientDTO>();
            string query = "SELECT * FROM Clients;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClientsList.Add(new DTOs.ClientDTOs.ClientDTO
                            (
                                reader.GetString(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("name")),
                                reader.GetDateTime(reader.GetOrdinal("created_at"))

                            ));
                        }
                    }
                }


                return ClientsList;
            }

        }
        public static DTOs.ClientDTOs.ClientDTO GetClientById(string Client)
        {
            string query = "SELECT * FROM Clients WHERE id = @clientID;";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                //command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@clientID", Client.Trim());

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DTOs.ClientDTOs.ClientDTO
                        (
                                reader.GetString(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("name")),
                                reader.GetDateTime(reader.GetOrdinal("created_at"))
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static string AddClient(DTOs.ClientDTOs.ClientDTO ClientDTO)
        {
            string query = @"INSERT INTO Clients (id, name, created_at) VALUES (@id, @name, @created_at);";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                //command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id", ClientDTO.Id);
                command.Parameters.AddWithValue("@name", ClientDTO.Name);
                command.Parameters.AddWithValue("@created_at", ClientDTO.created_at);

                //var outputIdParam = new SqlParameter("@NewClientId", SqlDbType.Int)
                //{
                //    Direction = ParameterDirection.Output
                //};

                //command.Parameters.Add(outputIdParam);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    return "";
                }


                //return Convert.ToString(outputIdParam.Value);
                return ClientDTO.Id;
            }
        }
        public static bool UpdateClient(DTOs.ClientDTOs.ClientDTO ClientDTO)
        {
            int RowAffected = 0;
            string  query = @"UPDATE Clients set 
			name = @name,
			created_at = @created_at
		    WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                //command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id", ClientDTO.Id);
                command.Parameters.AddWithValue("@name", ClientDTO.Name);
                command.Parameters.AddWithValue("@created_at", ClientDTO.created_at);

                try
                {
                    connection.Open();
                    RowAffected = command.ExecuteNonQuery();

                }

                //connection.Open();
                //command.ExecuteNonQuery();

                catch (Exception ex)
                {
                    return false;
                }

                return (RowAffected > 0);

                //return true;

            }



        }
        public static bool DeleteClient(string ClientId)
        {

            string query = "DELETE FROM Clients WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                // Fix 3: Use SqlDbType.UniqueIdentifier for robustness if 'id' is a GUID
                command.Parameters.AddWithValue("@id", ClientId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error during deletion: {ex.Message}");
                    return false;
                }

            }
        }


    }
}
