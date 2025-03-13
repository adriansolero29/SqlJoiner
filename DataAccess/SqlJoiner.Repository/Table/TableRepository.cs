using Dapper;
using SqlJoiner.DataAccess;
using SqlJoiner.Interfaces.DataAccess;
using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Repository.Table
{
    public class TableRepository : ITableRepository
    {
        private readonly IDataConnectionInitializer dataConnectionInitializer;

        public TableRepository(IDataConnectionInitializer dataConnectionInitializer)
        {
            this.dataConnectionInitializer = dataConnectionInitializer;
        }

        public async Task<IEnumerable<TableOL>> GetAllAsync()
        {
            try
            {
                var output = new List<TableOL>();
                string query = $@"
SELECT 
    table_name ""Name"", table_catalog ""SchemaName""
FROM information_schema.tables
";

                dataConnectionInitializer.InitializeConnectionAsync();
                dataConnectionInitializer.OpenConnectionAsync();

                if (Connection.DbConnection != null)
                {
                    using (Connection.DbConnection)
                    {
                        var result = await Connection.DbConnection.QueryAsync<TableOL>(query);
                        output = result.ToList();
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TableOL>> GetByConditionAsync(string condition)
        {
            try
            {
                var output = new List<TableOL>();
                string query = $@"
SELECT 
    table_name ""Name"", table_schema ""SchemaName""
FROM information_schema.tables
WHERE is_insertable_into = 'YES' AND table_type = 'BASE TABLE' {condition}
";

                dataConnectionInitializer.InitializeConnectionAsync();
                if (Connection.DbConnection != null)
                {
                    using (Connection.DbConnection)
                    {
                        Connection.DbConnection.Open();

                        var result = await Connection.DbConnection.QueryAsync<TableOL, SchemaOL, TableOL>(query, (table, schema) =>
                        {
                            table.Schema = schema;

                            return table;
                        }, splitOn: "SchemaName");
                        output = result.ToList();
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<TableOL> GetFirstAsync(string condition)
        {
            throw new NotImplementedException();
        }
    }
}
