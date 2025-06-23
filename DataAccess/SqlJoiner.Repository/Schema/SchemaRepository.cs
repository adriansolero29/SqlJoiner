using Dapper;
using SqlJoiner.DataAccess;
using SqlJoiner.Interfaces.DataAccess;
using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Models;

namespace SqlJoiner.Repository.Schema
{
    public class SchemaRepository : ISchemaRepository
    {
        public async Task<IEnumerable<SchemaOL>> GetAllAsync()
        {
            try
            {
                var output = new List<SchemaOL>();
                string query = $@"
SELECT 
    catalog_name ""CatalogName"", schema_name ""SchemaName"", schema_owner ""SchemaOwner"" 
FROM information_schema.schemata
WHERE SCHEMA_NAME <> 'information_schema' AND NOT SCHEMA_NAME ILIKE 'pg_%' AND SCHEMA_NAME <> 'public'";

                if (Connection.DbConnection != null)
                {
                    var result = await Connection.DbConnection.QueryAsync<SchemaOL>(query);
                    output = result.ToList();
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SchemaOL>> GetByConditionAsync(string condition)
        {
            try
            {
                var output = new List<SchemaOL>();
                string query = $@"
SELECT catalog_name ""CatalogName"", schema_name ""SchemaName"", schema_owner ""SchemaOwner"" 
FROM information_schema.schemata
{condition}
";

                if (string.IsNullOrEmpty(condition)) throw new ArgumentNullException("condition is empty or null");

                if (Connection.DbConnection != null)
                {
                    var result = await Connection.DbConnection.QueryAsync<SchemaOL>(query);
                    output = result.ToList();
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SchemaOL> GetFirstAsync(string condition)
        {
            var output = await GetByConditionAsync(condition);
            return output.FirstOrDefault() ?? new SchemaOL();
        }
    }
}
