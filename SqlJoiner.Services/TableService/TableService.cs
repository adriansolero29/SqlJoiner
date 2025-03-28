using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Services.TableService
{
    public class TableService : ITableService
    {
        private readonly ITableRepository tableRepository;
        public TableService(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public async Task<IEnumerable<TableOL>> CheckColumnIfTable(ColumnOL col)
        {
            return await tableRepository.CheckIfForeignKey(col);
        }

        public async Task<IEnumerable<TableOL>> GetBySchema(SchemaOL schema)
        {
            string condition = $@"AND table_schema = '{schema.SchemaName}'";
            return await tableRepository.GetByConditionAsync(condition);
        }
    }
}
