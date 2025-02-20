using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Interfaces.Service
{
    public interface ITableService
    {
        Task<IEnumerable<TableOL>> GetBySchema(SchemaOL schema);
    }
}
