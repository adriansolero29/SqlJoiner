using SqlJoiner.Interfaces.DataAccess;
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
        private readonly IDataConnectionInitializer dataConnectionInitializer;
        private readonly IColumnRepository columnRepository;
        private readonly ITableService tableService;

        public ColumnService(IDataConnectionInitializer dataConnectionInitializer, IColumnRepository columnRepository, ITableService tableService)
        {
            this.dataConnectionInitializer = dataConnectionInitializer;
            this.columnRepository = columnRepository;
            this.tableService = tableService;
        }

        public async Task<IEnumerable<ColumnOL>> GetAll()
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await columnRepository.GetByConditionAsync(string.Empty);

            dataConnectionInitializer.CloseConnection();
            return result;
        }

        public async Task<IEnumerable<ColumnOL>> GetByTable(TableOL table)
        {
            string condition = $@" AND table_name = '{table.Name}'";
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await columnRepository.GetByConditionAsync(condition);

            foreach (var item in result)
                item.IsForeignKey = (await tableService.CheckColumnIfTable(item)).Count() > 0 ? true : false;

            dataConnectionInitializer.CloseConnection();
            return result;
        }

        public async Task<IEnumerable<ColumnOL>> GetForeignKeyColumns()
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await columnRepository.GetAllForeignKeys();

            dataConnectionInitializer.CloseConnection();
            return result;
        }
    }
}
 