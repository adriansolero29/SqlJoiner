using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Models;

namespace SqlJoiner.Services.SchemaService
{
    public class SchemaService : ISchemaService
    {
        private readonly ISchemaRepository schemaRepository;
        public SchemaService(ISchemaRepository schemaRepository)
        {
            this.schemaRepository = schemaRepository;
        }

        public async Task<IEnumerable<SchemaOL>> GetAll()
        {
            return await schemaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<SchemaOL>> GetByName(SchemaOL schemaName)
        {
            return await schemaRepository.GetByConditionAsync($@"WHERE schema_name ILIKE '%{schemaName}%'");
        }
    }
}
