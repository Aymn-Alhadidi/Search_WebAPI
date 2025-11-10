using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SearchData
    {

        static string _connectionString = clsDataAccessSetting.ConnectionString;
        public static List<DTOs.SearchDTOs.PopularItems> GetPopularItems(string client_id)
        {
            var PopularItemsList = new List<DTOs.SearchDTOs.PopularItems>();
            string query = "SELECT TOP 20 item_id, item_name, COUNT(*) as selection_count FROM Searches WHERE client_id = @id AND item_id IS NOT NULL AND searched_at >= DATEADD(day, -30, GETDATE()) GROUP BY item_id, item_name ORDER BY selection_count DESC";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", client_id);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PopularItemsList.Add(new DTOs.SearchDTOs.PopularItems
                            (
                                reader.GetInt32(reader.GetOrdinal("item_id")),
                                reader.GetString(reader.GetOrdinal("item_name")),
                                reader.GetInt32(reader.GetOrdinal("selection_count"))

                            ));
                        }
                    }
                }


                return PopularItemsList;
            }

        }
        public static string AddSearchRecord(DTOs.SearchDTOs.SearchItem SearchDTO)
        {
            string query = @"INSERT INTO Searches (id, client_id, keyword, item_id, item_name, searched_at)
                        VALUES (@id, @client_id, @keyword, @item_id, @item_name, @searched_at);
                        SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("id", SearchDTO.id);
                command.Parameters.AddWithValue("@client_id", SearchDTO.client_id);
                command.Parameters.AddWithValue("@keyword", SearchDTO.keyword);
                command.Parameters.AddWithValue("@searched_at", SearchDTO.searched_at);

                if (SearchDTO.item_id != null && SearchDTO.item_id != 0)
                    command.Parameters.AddWithValue("@item_id", SearchDTO.item_id);
                else
                    command.Parameters.AddWithValue("@item_id", System.DBNull.Value);

                if (SearchDTO.item_name != null && SearchDTO.item_name != "string")
                    command.Parameters.AddWithValue("@item_name", SearchDTO.item_name);
                else
                    command.Parameters.AddWithValue("@item_name", System.DBNull.Value);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    return "";
                }

                return SearchDTO.id;
            }
        }


        //-------------------------------------------

    }

    //public class SearchArchiveService : BackgroundService
    //{
        //    // Placeholder for connection string retrieval based on previous context
        //    private readonly string _connectionString = clsDataAccessSetting.ConnectionString;

        //    // Define the interval for the job
        //    private readonly TimeSpan _executionInterval = TimeSpan.FromHours(24);

        //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //    {
        //        // Initial delay to let the rest of the application start up
        //        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        //        while (!stoppingToken.IsCancellationRequested)
        //        {
        //            Console.WriteLine($"Archival job started at: {DateTimeOffset.Now}");

        //            try
        //            {
        //                await ArchiveOldSearchesAsync();
        //                Console.WriteLine($"Archival job finished successfully at: {DateTimeOffset.Now}");
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log the error without blocking the main application flow
        //                Console.WriteLine($"Archival Job FAILED: {ex.Message}");
        //                // In a real application, use ILogger here.
        //            }

        //            // Wait for the defined interval before running again
        //            await Task.Delay(_executionInterval, stoppingToken);
        //        }
        //    }

        //    private async Task ArchiveOldSearchesAsync()
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            using (var command = new SqlCommand("ArchiveOldSearches", connection))
        //            {
        //                // Specify that we are running a Stored Procedure
        //                command.CommandType = CommandType.StoredProcedure;

        //                // Set a timeout in case the archiving takes a long time (e.g., 5 minutes)
        //                command.CommandTimeout = 300;

        //                await connection.OpenAsync();
        //                await command.ExecuteNonQueryAsync();
        //            }
        //        }
        //    }
        //}
    }
