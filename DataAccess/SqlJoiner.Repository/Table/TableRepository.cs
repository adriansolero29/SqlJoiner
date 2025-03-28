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

        public async Task<IEnumerable<TableOL>> CheckIfForeignKey(ColumnOL col)
        {
            try
            {
                var output = new List<TableOL>();
                string query = $@"
SELECT
    tc.table_schema, 
    tc.constraint_name, 
    tc.table_name, 
    kcu.column_name, 
    ccu.table_schema AS foreign_table_schema,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name 
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
    AND tc.table_schema = kcu.table_schema
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
    AND kcu.column_name = '{col.ColumnName}'
    AND tc.table_schema = '{col.Table?.Schema?.SchemaName}'
    AND tc.table_name = '{col.Table?.Name}';
";

                dataConnectionInitializer.InitializeConnectionAsync();
                dataConnectionInitializer.OpenConnectionAsync();

                if (Connection.DbConnection != null)
                {
                    using (Connection.DbConnection)
                    {
                        var result = await Connection.DbConnection.QueryAsync(query);
                        if (result != null)
                        {
                            var foreignSchema = result?.FirstOrDefault()?.foreign_table_schema;
                            var foreignTable = result?.FirstOrDefault()?.foreign_table_name;

                            var table = await GetByConditionAsync($"AND c.schema_name = '{foreignSchema}' AND t.table_name = '{foreignTable}'");

                            output = table.ToList();
                        }
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TableOL>> GetAllAsync()
        {
            try
            {
                var output = new List<TableOL>();
                string query = $@"
SELECT 
    t.table_name ""Name"",
    c.schema_name ""SchemaName""
FROM information_schema.tables t
JOIN information_schema.schemata c ON c.schema_name = t.table_schema
WHERE t.table_type = 'BASE TABLE'
";

                dataConnectionInitializer.InitializeConnectionAsync();
                dataConnectionInitializer.OpenConnectionAsync();

                if (Connection.DbConnection != null)
                {
                    using (Connection.DbConnection)
                    {
                        var result = await Connection.DbConnection.QueryAsync<TableOL, SchemaOL, TableOL>(query, (table, schema) =>
                        {
                            table.Schema = schema;

                            return table;
                        },splitOn: "schema_name");
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
    t.table_name ""Name"", 
    c.schema_name ""SchemaName""
FROM information_schema.tables t
JOIN information_schema.schemata c ON c.schema_name = t.table_schema
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
