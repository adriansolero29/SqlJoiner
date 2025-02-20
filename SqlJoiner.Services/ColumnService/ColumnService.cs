using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Services.ColumnService
{
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository columnRepository;

        public ColumnService(IColumnRepository columnRepository)
        {
            this.columnRepository = columnRepository;
        }

        public async Task<IEnumerable<ColumnOL>> GetByTable(TableOL table)
        {
            string condition = $@" AND table_schema = '{table.Schema?.SchemaName}' AND table_name = '{table.Name}'";
            return await columnRepository.GetByConditionAsync(condition);
        }
    }
}
 