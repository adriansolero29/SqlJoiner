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

namespace SqlJoiner.Repository.Column
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly IDataConnectionInitializer dataConnectionInitializer;

        public ColumnRepository(IDataConnectionInitializer dataConnectionInitializer)
        {
            this.dataConnectionInitializer = dataConnectionInitializer;
        }

        public Task<IEnumerable<ColumnOL>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ColumnOL>> GetByConditionAsync(string condition)
        {
            try
            {
                var output = new List<ColumnOL>();
                string query = $@"
SELECT 
    column_name ""ColumnName"", data_type ""DataType"", table_schema ""SchemaName"", table_name ""Name""
FROM information_schema.columns
WHERE is_updatable = 'YES' {condition}
";

                dataConnectionInitializer.InitializeConnectionAsync();
                if (Connection.DbConnection != null)
                {
                    using (Connection.DbConnection)
                    {
                        dataConnectionInitializer.OpenConnectionAsync();

                        var result = await Connection.DbConnection.QueryAsync<ColumnOL, TableOL, SchemaOL, ColumnOL>(query, (column, table, schema) =>
                        {
                            column.Table = table;
                            column.Table.Schema = schema;

                            return column;
                        }, splitOn: "SchemaName,Name");
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

        public Task<ColumnOL> GetFirstAsync(string condition)
        {
            throw new NotImplementedException();
        }
    }
}
