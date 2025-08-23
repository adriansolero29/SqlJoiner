using SqlJoiner.Interfaces.DataAccess;
using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Services.QueryService
{
    public class QueryService : IQueryService
    {
        private readonly IDataConnectionInitializer dataConnectionInitializer;
        private readonly IQueryRepository queryRepository;

        public QueryService(IDataConnectionInitializer dataConnectionInitializer, IQueryRepository queryRepository)
        {
            this.dataConnectionInitializer = dataConnectionInitializer;
            this.queryRepository = queryRepository;
        }

        public async Task<string> RawQueryToJson(string query)
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await queryRepository.RawSqlToJson(query);

            dataConnectionInitializer.CloseConnection();
            return result;
        }
    }
}
