using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlJoiner.Presenter
{
    public class SqlJoinerMainPresenter
    {
        private readonly IJoinerMainView joinerMainView;
        private readonly ISchemaService schemaService;
        private readonly ITableService tableService;
        private readonly IColumnService columnService;

        public SqlJoinerMainPresenter(IJoinerMainView joinerMainView, ISchemaService schemaService, ITableService tableService, IColumnService columnService)
        {
            this.joinerMainView = joinerMainView ?? throw new ArgumentNullException(nameof(joinerMainView));
            this.schemaService = schemaService ?? throw new ArgumentNullException(nameof(schemaService));
            this.tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
            this.columnService = columnService ?? throw new ArgumentNullException(nameof(columnService));

            joinerMainView.Init += JoinerMainView_Init;
            joinerMainView.GenerateSql += JoinerMainView_GenerateSql;
            joinerMainView.SelectedSchemaValueChanged += JoinerMainView_SelectedSchemaValueChanged;
        }

        private async void JoinerMainView_SelectedSchemaValueChanged(object? sender, EventArgs e)
        {
            await loadTablesBySchema();
        }

        private void JoinerMainView_GenerateSql(object? sender, EventArgs e)
        {
            var a = joinerMainView.SelectedSchema;
        }

        private async void JoinerMainView_Init(object? sender, EventArgs e)
        {
            await loadFullDatabaseInformation();
        }

        private async Task loadFullDatabaseInformation()
        {
            var output = new List<CustomDatabaseEntityModelOL>();

            var schemaList = await schemaService.GetAll();
            foreach (var schema in schemaList)
            {
                var tableList = await tableService.GetBySchema(schema);
                var list = new List<CustomDataTableEntityOL>();
                foreach (var table in tableList)
                {
                    var column = await columnService.GetByTable(table);
                    list.Add(new CustomDataTableEntityOL
                    {
                        TableInformation = table,
                        ColumnList = (List<ColumnOL>)column
                    });
                }

                output.Add(new CustomDatabaseEntityModelOL
                {
                    SchemaInfo = schema,
                    RecurseTableInformation = list
                });

                Console.WriteLine("Details loaded for " + schema.SchemaName);
            }

            Console.WriteLine("Finished");

            joinerMainView.FullDatabaseEntityModel = new List<CustomDatabaseEntityModelOL>();
            joinerMainView?.FullDatabaseEntityModel?.Clear();
            joinerMainView?.FullDatabaseEntityModel?.AddRange(output);
        }

        private async Task loadTablesBySchema()
        {
            if (joinerMainView.SelectedSchema == null)
                return;

            var result = await tableService.GetBySchema(joinerMainView.SelectedSchema);

            joinerMainView.TableList = new List<TableOL>();
            joinerMainView.TableList?.Clear();
            joinerMainView.TableList?.AddRange(result);
        }

        public async Task loadSchemaList()
        {
            var result = await schemaService.GetAll();

            joinerMainView.SchemaList = new List<SchemaOL>();
            joinerMainView?.SchemaList?.Clear();
            joinerMainView?.SchemaList?.AddRange(result);
        }
    }
}
