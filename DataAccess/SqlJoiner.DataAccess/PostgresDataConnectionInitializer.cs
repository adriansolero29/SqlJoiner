using Npgsql;
using SqlJoiner.Interfaces.DataAccess;
using System.Text;

namespace SqlJoiner.DataAccess
{
    public class PostgresDataConnectionInitializer : IDataConnectionInitializer
    {
        public void CloseConnection()
        {
            Connection.DbConnection?.Close();
            Connection.DbConnection?.Dispose();
        }

        public async Task InitializeConnectionAsync()
        {
            await Task.Run(() =>
            {
                NpgsqlConnectionStringBuilder cnStringBuilder = new NpgsqlConnectionStringBuilder(Connection.ConnectionString);

                if (Connection.DbConnection == null)
                    Connection.DbConnection = new NpgsqlConnection(cnStringBuilder.ConnectionString);
            });
        }

        public async Task OpenConnectionAsync()
        {
            await Task.Run(() =>
            {
                if (Connection.DbConnection == null)
                    throw new ArgumentNullException("DbConnection is null");

                if (Connection.DbConnection.State != System.Data.ConnectionState.Open)
                    Connection.DbConnection.Open();
            });
        }
    }
}
