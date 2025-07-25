using SqlJoiner.Helpers;
using SqlJoiner.Interfaces.DataAccess;
using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Models;
using System.ComponentModel.DataAnnotations;

namespace SqlJoiner.Services.SchemaService
{
    public class SchemaService : ISchemaService
    {
        private readonly IDataConnectionInitializer dataConnectionInitializer;
        private readonly ISchemaRepository schemaRepository;
        public SchemaService(IDataConnectionInitializer dataConnectionInitializer, ISchemaRepository schemaRepository)
        {
            this.dataConnectionInitializer = dataConnectionInitializer;
            this.schemaRepository = schemaRepository;
        }

        public async Task<IEnumerable<SchemaOL>> GetAll()
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await schemaRepository.GetAllAsync();

            dataConnectionInitializer.CloseConnection();
            return result;
        }

        public async Task<IEnumerable<SchemaOL>> GetByName(SchemaOL schemaName)
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await schemaRepository.GetByConditionAsync($@"WHERE schema_name ILIKE '%{schemaName}%'");

            dataConnectionInitializer.CloseConnection();
            return result;
        }
    }
}
