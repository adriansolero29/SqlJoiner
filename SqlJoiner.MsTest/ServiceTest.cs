using Moq;
using SqlJoiner.DataAccess;
using SqlJoiner.Interfaces.Repository;
using SqlJoiner.Interfaces.Service;
using SqlJoiner.Models;
using SqlJoiner.Repository.Column;
using SqlJoiner.Repository.Schema;
using SqlJoiner.Repository.Table;
using SqlJoiner.Services.ColumnService;
using SqlJoiner.Services.SchemaService;
using SqlJoiner.Services.TableService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.MsTest
{
    [TestClass]
    public class ServiceTest
    {
        private readonly ISchemaService schemaService;
        private readonly ITableService tableService;
        private readonly IColumnService columnService;

        public ServiceTest()
        {
        }

        [TestMethod]
        public async Task GetAllSchema()
        {
            var s = await schemaService.GetAll();
        }

        [TestMethod]
        public async Task GetTablesBySchema()
        {
            SchemaOL schema = new SchemaOL { SchemaName = "IMTE" };
            var s = await tableService.GetBySchema(schema);
        }

        [TestMethod]
        public async Task GetColumnsByTable()
        {
            TableOL table = new TableOL
            { 
                Schema = new SchemaOL { SchemaName = "Purchasing" },
                Name = "SubCon"
            };
            var a = await columnService.GetByTable(table);
        }
    }
}
