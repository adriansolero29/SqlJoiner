using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
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
            joinerMainView.SelectedSchemaValueChanged += JoinerMainView_SelectedSchemaValueChanged;
            joinerMainView.SelectedTableValueChanged += JoinerMainView_SelectedTableValueChanged;
            joinerMainView.SelectedColumnValueChanged += JoinerMainView_SelectedColumnValueChanged;
            joinerMainView.GenerateQuery += SqlJoinerMainPresenter_GenerateQuery;
        }

        public List<SchemaOL> MainSchemaList { get; set; } = new List<SchemaOL>();
        public List<TableOL> MainTableList { get; set; } = new List<TableOL>();
        public List<ColumnOL> MainColumnList { get; set; } = new List<ColumnOL>();
        public List<ColumnOL> MainForeignKeyColumnsList { get; set; } = new List<ColumnOL>();

        public Dictionary<SchemaOL, Dictionary<TableOL, List<ColumnOL>>> SelectedModel { get; set; } = new Dictionary<SchemaOL, Dictionary<TableOL, List<ColumnOL>>>();

        #region Private Methods

        private async Task loadSchemas()
        {
            var result = await schemaService.GetAll();
            MainSchemaList?.Clear();
            MainSchemaList?.AddRange(result);

            joinerMainView.SchemaList = new List<SchemaOL>();
            joinerMainView.SchemaList?.Clear();
            joinerMainView.SchemaList?.AddRange(MainSchemaList ?? new List<SchemaOL>());
        }

        private async Task loadTables()
        {
            var result = await tableService.GetAll();
            MainTableList?.Clear();
            MainTableList?.AddRange(result);

            joinerMainView.TableList = new List<TableOL>();
            joinerMainView.TableList?.Clear();
            joinerMainView.TableList?.AddRange(result);
        }

        private async Task loadColumns()
        {
            var result = await columnService.GetAll();
            MainColumnList?.Clear();
            MainColumnList?.AddRange(result);
        }

        private async Task loadForeignKeyColumns()
        {
            var result = await columnService.GetForeignKeyColumns();
            MainForeignKeyColumnsList?.Clear();
            MainForeignKeyColumnsList?.AddRange(result.DistinctBy(x => x.ColumnName));
        }

        #endregion

        private async void JoinerMainView_Init(object? sender, EventArgs e)
        {
            Debug.WriteLine("Loading all columns...");

            await loadSchemas();
            await loadTables();
            await loadColumns();
            await loadForeignKeyColumns();

            Debug.WriteLine("Done loading all columns!");
        }

        private void JoinerMainView_SelectedSchemaValueChanged(object? sender, EventArgs e)
        {
            if (MainTableList != null)
            {
                var result = MainTableList.Where(x => x.Schema?.SchemaName == joinerMainView?.SelectedSchema?.SchemaName);

                joinerMainView?.TableList?.Clear();
                joinerMainView?.TableList?.AddRange(result);

                SelectedModel.TryAdd(joinerMainView?.SelectedSchema ?? new SchemaOL(), new Dictionary<TableOL, List<ColumnOL>>());
            }
        }

        private void JoinerMainView_SelectedTableValueChanged(object? sender, EventArgs e)
        {
            if (MainColumnList != null)
            {
                var result = MainColumnList.Where(x => x.Table?.Name == joinerMainView?.SelectedTable?.Name);

                var output = new List<ColumnOL>();
                foreach (var item in result)
                {
                    var isForeignKey = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == item.ColumnName);

                    if (isForeignKey != null) item.IsForeignKey = true;

                    output.Add(item);
                }

                joinerMainView?.ColumnList?.Clear();
                joinerMainView?.ColumnList?.AddRange(output.OrderBy(x => x.OrdinalPosition));

                var selectedSchemaDict = SelectedModel.FirstOrDefault(x => x.Key.SchemaName == joinerMainView?.SelectedSchema?.SchemaName);
                selectedSchemaDict.Value.TryAdd(joinerMainView?.SelectedTable ?? new TableOL(), new List<ColumnOL>());
            }
        }

        private void JoinerMainView_SelectedColumnValueChanged(object? sender, EventArgs e)
        {
            if (MainForeignKeyColumnsList != null)
            {
                var result = MainForeignKeyColumnsList.Where(x => x.ColumnName == joinerMainView?.SelectedColumn?.ColumnName);
                var table = MainTableList.Where(x => x.Name == result.FirstOrDefault()?.Table?.Name).FirstOrDefault();
                var foreignKeysColumns = MainColumnList.Where(x => x.Table?.Name == table?.Name);

                var output = new List<ColumnOL>();
                foreach (var item in foreignKeysColumns)
                {
                    var isForeignKey = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == item.ColumnName);

                    if (isForeignKey != null) item.IsForeignKey = true;

                    output.Add(item);
                }

                joinerMainView?.ForeignKeyColumnsList?.Clear();
                joinerMainView?.ForeignKeyColumnsList?.AddRange(output.OrderBy(x => x.OrdinalPosition));

                var selectedSchemaDict = SelectedModel.FirstOrDefault(x => x.Key.SchemaName == joinerMainView?.SelectedSchema?.SchemaName);
                var selectedTableDict = selectedSchemaDict.Value;
                var a = selectedSchemaDict.Value.FirstOrDefault(x => x.Key.Name == joinerMainView?.SelectedColumn?.Table?.Name);
                a.Value.Add(joinerMainView?.SelectedColumn ?? new ColumnOL());
            }
        }

        private void SqlJoinerMainPresenter_GenerateQuery(object? sender, EventArgs e)
        {
        }
    }
}
