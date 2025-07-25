using SqlJoiner.Helpers;
using SqlJoiner.Interfaces.DataAccess;
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
        private readonly IDataConnectionInitializer dataConnectionInitializer;
        private readonly ITableRepository tableRepository;
        public TableService(IDataConnectionInitializer dataConnectionInitializer, ITableRepository tableRepository)
        {
            this.dataConnectionInitializer = dataConnectionInitializer;
            this.tableRepository = tableRepository;
        }

        public async Task<IEnumerable<TableOL>> CheckColumnIfTable(ColumnOL col)
        {
            var result = await tableRepository.CheckIfForeignKey(col);

            return result;
        }

        public async Task<IEnumerable<TableOL>> GetAll()
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            var result = await tableRepository.GetAllAsync();

            dataConnectionInitializer.CloseConnection();
            return result;
        }

        public async Task<IEnumerable<TableOL>> GetBySchema(SchemaOL schema)
        {
            dataConnectionInitializer.InitializeConnectionAsync();
            dataConnectionInitializer.OpenConnectionAsync();

            string condition = $@"AND table_schema = '{schema.SchemaName}'";
            var result = await tableRepository.GetByConditionAsync(condition);

            dataConnectionInitializer.CloseConnection();
            return result;
        }
    }
}
