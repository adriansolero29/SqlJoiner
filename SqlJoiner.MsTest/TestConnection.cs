using SqlJoiner.DataAccess;
using SqlJoiner.Repository.MainRepository;

namespace SqlJoiner.MsTest
{
    [TestClass]
    public sealed class TestConnection
    {
        [TestMethod]
        public async Task NpgsqlConnection()
        {
            PostgresDataConnectionInitializer p = new PostgresDataConnectionInitializer();

            p.InitializeConnectionAsync();
            var a = Connection.DbConnection;

            p.OpenConnectionAsync();
            var b = Connection.DbConnection;

            p.CloseConnection();
            var c = Connection.DbConnection;
        }
    }

    [TestClass]
    public sealed class TestDataAccess
    {
        [TestMethod]
        public async Task GetData()
        {
            DataProcessor p = new DataProcessor(new PostgresDataConnectionInitializer());
            await p.Get($@"SELECT table_name FROM information_schema.""columns"" WHERE table_schema = 'DocumentControl'");
        }
    }
}
