using Dapper;
using SqlJoiner.DataAccess;
using SqlJoiner.Interfaces.Repository;
using System.Text.Json;
using System.Threading.Tasks;

namespace SqlJoiner.Repository.Query
{
    public class QueryRepository : IQueryRepository
    {
        public async Task<string> RawSqlToJson(string query)
        {
			try
			{
                string output = "";
                if (Connection.DbConnection != null)
                {
                    var result = await Connection.DbConnection.QueryAsync(query);
                    output = JsonSerializer.Serialize(result);
                }

                return output;
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
			}
        }
    }
}
