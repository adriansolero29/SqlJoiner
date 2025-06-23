using Npgsql;
using SqlJoiner.Interfaces.DataAccess;
using System.Text;

namespace SqlJoiner.DataAccess
{
    public sealed class PostgresDataConnectionInitializer : IDataConnectionInitializer
    {
        public void CloseConnection()
        {
            if (Connection.DbConnection?.State != System.Data.ConnectionState.Connecting)
            {
                Connection.DbConnection?.Close();
                Connection.DbConnection?.Dispose();
            }
        }

        public void InitializeConnectionAsync()
        {
            try
            {
                NpgsqlConnectionStringBuilder cnStringBuilder = new NpgsqlConnectionStringBuilder(Connection.ConnectionString);
                Connection.DbConnection = new NpgsqlConnection(cnStringBuilder.ConnectionString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void OpenConnectionAsync()
        {
            try
            {
                if (Connection.DbConnection == null)
                    throw new ArgumentNullException("DbConnection is null");

                if (Connection.DbConnection.State == System.Data.ConnectionState.Closed)
                    Connection.DbConnection.Open();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
