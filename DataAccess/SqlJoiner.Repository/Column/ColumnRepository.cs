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
        public async Task<IEnumerable<ColumnOL>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                return new List<ColumnOL>();
            });
        }

        public async Task<IEnumerable<ColumnOL>> GetAllForeignKeys()
        {
            try
            {
                var output = new List<ColumnOL>();
                string query = $@"
SELECT 
	kcu.""column_name"" ""ColumnName"", ccu.""table_name"" ""Name"", ccu.""table_schema"" ""SchemaName""
FROM information_schema.table_constraints tc
left JOIN information_schema.key_column_usage kcu 
	ON tc.""constraint_name"" = kcu.""constraint_name""
	AND tc.table_schema = kcu.table_schema
	AND tc.""table_name"" = kcu.""table_name""
left JOIN information_schema.constraint_column_usage ccu
	ON ccu.""constraint_name"" = kcu.""constraint_name""

WHERE tc.""constraint_type"" = 'FOREIGN KEY'
";

                if (Connection.DbConnection != null)
                {
                    var result = await Connection.DbConnection.QueryAsync<ColumnOL, TableOL, SchemaOL, ColumnOL>(query, (column, table, schema) =>
                    {
                        column.Table = table;
                        column.Table.Schema = schema;

                        return column;
                    }, splitOn: "Name,SchemaName");
                    output = result.ToList();
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ColumnOL>> GetByConditionAsync(string condition)
        {
            try
            {
                var output = new List<ColumnOL>();
                string query = $@"
SELECT 
    column_name ""ColumnName"", ordinal_position ""OrdinalPosition"", data_type ""DataType"", table_schema ""SchemaName"", table_name ""Name""
FROM information_schema.columns
WHERE is_updatable = 'YES' {condition}
";

                if (Connection.DbConnection != null)
                {
                    var result = await Connection.DbConnection.QueryAsync<ColumnOL, SchemaOL, TableOL, ColumnOL>(query, (column, schema, table) =>
                    {
                        column.Table = table;
                        column.Table.Schema = schema;

                        return column;
                    }, splitOn: "SchemaName,Name");
                    output = result.ToList();
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
