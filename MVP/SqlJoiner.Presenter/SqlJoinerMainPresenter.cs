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

        public SqlJoinerMainPresenter(IJoinerMainView joinerMainView, ISchemaService schemaService, ITableService tableService)
        {
            this.joinerMainView = joinerMainView ?? throw new ArgumentNullException(nameof(joinerMainView));
            this.schemaService = schemaService ?? throw new ArgumentNullException(nameof(schemaService));
            this.tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));

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
            await loadSchemaList();
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
