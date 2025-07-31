using System.Data;
using System.Data.SqlClient;

namespace TarifarioBackend.Helpers
{
    public static class SqlHelper
    {
        public static async Task<DataTable> ExecuteQueryAsync(string connectionString, string query, SqlParameter[]? parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string query, SqlParameter[]? parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
