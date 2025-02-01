using SqlJoiner.DataAccess;

namespace SqlJoiner.MsTest
{
    [TestClass]
    public sealed class TestConnection
    {
        [TestMethod]
        public async Task NpgsqlConnection()
        {
            PostgresDataConnectionInitializer p = new PostgresDataConnectionInitializer();

            await p.InitializeConnectionAsync();
            var a = Connection.DbConnection;

            await p.OpenConnectionAsync();
            var b = Connection.DbConnection;

            p.CloseConnection();
            var c = Connection.DbConnection;
        }
    }
}
