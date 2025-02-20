using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Interfaces.Service
{
    public interface ISchemaService
    {
        Task<IEnumerable<SchemaOL>> GetAll();
        Task<IEnumerable<SchemaOL>> GetByName(SchemaOL schemaName);
    }
}
