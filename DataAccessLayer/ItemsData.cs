using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
    public class ItemsData
    {

        static string _connectionString = clsDataAccessSetting.ConnectionString;
        public static List<DTOs.ItemDTOs.ItemDTO> GetAllItems()
        {
            var ClientsList = new List<DTOs.ItemDTOs.ItemDTO>();
            string query = "SELECT * FROM Items;";

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
                            ClientsList.Add(new DTOs.ItemDTOs.ItemDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("name")),
                                reader.GetDateTime(reader.GetOrdinal("created_at"))

                            ));
                        }
                    }
                }


                return ClientsList;
            }

        }
        public static DTOs.ItemDTOs.ItemDTO GetItemById(int Item)
        {
            string query = "SELECT * FROM Items WHERE id = @Item_Id;";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                //command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Item_Id", Item);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DTOs.ItemDTOs.ItemDTO
                        (
                                reader.GetInt32(reader.GetOrdinal("id")),
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

        /*
               public static int AddNewItem(string name, DateTime created_at)
        {

            int ContactID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"INSERT INTO Items (name, created_at)
                        VALUES (@name, @created_at);
                        SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);


            Command.Parameters.AddWithValue("@name", name);

Command.Parameters.AddWithValue("@created_at", created_at);



            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    ContactID = insertedID;
                }

            }


            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex);
            }

            finally
            {
                Connection.Close();
            }

            return ContactID;
        }
         */



        public static int AddItem(DTOs.ItemDTOs.ItemDTO ItemDTO)
        {
            string query = @"INSERT INTO Items (name, created_at)
                        VALUES (@name, @created_at);
                        SELECT SCOPE_IDENTITY();";

            int ContactID = -1;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {

                //command.Parameters.AddWithValue("@id", ItemDTO.Id);
                command.Parameters.AddWithValue("@name", ItemDTO.Name);
                command.Parameters.AddWithValue("@created_at", ItemDTO.created_at);


                try
                {
                    connection.Open();

                    object Result = command.ExecuteScalar();
                    if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                    {
                        ContactID = insertedID;
                    }

                }

                catch (Exception ex)
                {
                    return -1;
                }


                //return Convert.ToString(outputIdParam.Value);
                return ContactID;
            }
        }
        public static bool UpdateItem(DTOs.ItemDTOs.ItemDTO ItemDTO)
        {
            int RowAffected = 0;
            string query = @"UPDATE Items set 
			name = @name,
			created_at = @created_at
		    WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                //command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id", ItemDTO.Id);
                command.Parameters.AddWithValue("@name", ItemDTO.Name);
                command.Parameters.AddWithValue("@created_at", ItemDTO.created_at);
                //return true;

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

            }

        }
        public static bool DeleteItem(int ItemID)
        {

            string query = "DELETE FROM Items WHERE id = @id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                // Fix 3: Use SqlDbType.UniqueIdentifier for robustness if 'id' is a GUID
                command.Parameters.AddWithValue("@id", ItemID);

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
