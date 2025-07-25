using SqlJoiner.Helpers;
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
            joinerMainView.SelectSchema += JoinerMainView_SelectedSchemaValueChanged;
            joinerMainView.SelectTable += JoinerMainView_SelectedTableValueChanged;
            joinerMainView.SelectColumn += JoinerMainView_SelectedColumnValueChanged;
            joinerMainView.LoadTableBySelectedSchema += JoinerMainView_LoadTableBySelectedSchema;
            joinerMainView.LoadColumnFromSelectedTable += JoinerMainView_LoadColumnFromSelectedTable;
            joinerMainView.LoadForeignKeyColumnsBySelectedForeignKey += JoinerMainView_LoadForeignKeyColumnsBySelectedForeignKey;

            joinerMainView.GenerateQuery += SqlJoinerMainPresenter_GenerateQuery;
        }

        public List<SchemaOL> MainSchemaList { get; set; } = new List<SchemaOL>();
        public List<TableOL> MainTableList { get; set; } = new List<TableOL>();
        public List<ColumnOL> MainColumnList { get; set; } = new List<ColumnOL>();
        public List<ColumnOL> MainForeignKeyColumnsList { get; set; } = new List<ColumnOL>();

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

        private void JoinerMainView_LoadTableBySelectedSchema(object? sender, EventArgs e)
        {
            if (MainTableList != null)
            {
                if (joinerMainView?.CurrentSelectedSchema != null)
                    joinerMainView.CurrentSelectedSchema.TempId = GuidGenerator.Generate() + "-" + joinerMainView?.CurrentSelectedSchema?.SchemaName;

                var result = MainTableList.Where(x => x.Schema?.SchemaName == joinerMainView?.CurrentSelectedSchema?.SchemaName);
                result.ToList().ForEach(x => x.TempHeadId = joinerMainView?.CurrentSelectedSchema?.TempId);
                joinerMainView?.TableList?.Clear();
                joinerMainView?.TableList?.AddRange(result);
            }
        }

        private void JoinerMainView_LoadColumnFromSelectedTable(object? sender, EventArgs e)
        {
            if (MainColumnList != null)
            {
                if (joinerMainView?.CurrentSelectedTable != null)
                    joinerMainView.CurrentSelectedTable.TempId = GuidGenerator.Generate() + "-" + joinerMainView?.CurrentSelectedTable?.Name;

                var result = MainColumnList.Where(x => x.Table?.Name == joinerMainView?.CurrentSelectedTable?.Name);
                var output = new List<ColumnOL>();
                foreach (var item in result)
                {
                    var isForeignKey = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == item.ColumnName);

                    if (isForeignKey != null) item.IsForeignKey = true;

                    output.Add(item);
                }

                output.ForEach(x => x.HeadTempId = joinerMainView?.CurrentSelectedTable?.TempId);
                joinerMainView?.ColumnList?.Clear();
                joinerMainView?.ColumnList?.AddRange(output.OrderBy(x => x.OrdinalPosition));
            }
        }

        private void JoinerMainView_LoadForeignKeyColumnsBySelectedForeignKey(object? sender, EventArgs e)
        {
            if (MainForeignKeyColumnsList != null)
            {
                if (joinerMainView?.CurrentSelectedColumn?.ColumnInfo != null)
                {
                    if (joinerMainView.CurrentSelectedColumn.ColumnInfo.IsForeignKey)
                        JoinerMainView_SelectedColumnValueChanged(sender, e);

                    joinerMainView.CurrentSelectedColumn.ColumnInfo.TempId = GuidGenerator.Generate() + "-" + joinerMainView?.CurrentSelectedColumn?.ColumnInfo?.ColumnName;
                }

                var result = MainForeignKeyColumnsList.Where(x => x.ColumnName == joinerMainView?.CurrentSelectedColumn?.ColumnInfo?.ColumnName);
                var table = MainTableList.FirstOrDefault(x => x.Name == result.FirstOrDefault()?.Table?.Name);
                var foreignKeysColumns = MainColumnList.Where(x => x.Table?.Name == table?.Name);

                var output = new List<ColumnOL>();
                foreach (var item in foreignKeysColumns)
                {
                    var isForeignKey = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == item.ColumnName);
                    if (isForeignKey != null) item.IsForeignKey = true;
                    output.Add(item);
                }

                output.ForEach(x => x.HeadTempId = joinerMainView?.CurrentSelectedColumn?.ColumnInfo?.TempId);
                joinerMainView?.ForeignKeyColumnsList?.Clear();
                joinerMainView?.ForeignKeyColumnsList?.AddRange(output.OrderBy(x => x.OrdinalPosition));
            }
        }

        public List<SchemaOL> SelectedSchemaList { get; set; } = new List<SchemaOL>();
        public List<TableOL> SelectedTableList { get; set; } = new List<TableOL>();
        public List<CustomRecurseColumnOL> SelectedColumnList { get; set; } = new List<CustomRecurseColumnOL>();

        private void JoinerMainView_SelectedSchemaValueChanged(object? sender, EventArgs e)
        {
            if (joinerMainView?.CurrentSelectedSchema != null)
                SelectedSchemaList.Add(joinerMainView.CurrentSelectedSchema);
        }

        private void JoinerMainView_SelectedTableValueChanged(object? sender, EventArgs e)
        {
            if (joinerMainView?.CurrentSelectedTable != null)
                SelectedTableList.Add(joinerMainView.CurrentSelectedTable);
        }

        private void JoinerMainView_SelectedColumnValueChanged(object? sender, EventArgs e)
        {
            if (joinerMainView?.CurrentSelectedColumn != null)
            {
                if (joinerMainView.CurrentSelectedColumn.ColumnInfo == null) return;
                if (joinerMainView.CurrentSelectedColumn.ColumnInfo.IsForeignKey)
                {
                    SelectedColumnList.Add(new CustomRecurseColumnOL()
                    {
                        ColumnInfo = joinerMainView.CurrentSelectedColumn.ColumnInfo,
                        ReferencedTable = joinerMainView.CurrentSelectedColumn.ColumnInfo.Table,
                        FieldFromReferencedTable = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == joinerMainView.CurrentSelectedColumn.ColumnInfo.ColumnName)?.FieldFromReferencedTable,
                        FieldFromSourceTable = joinerMainView.CurrentSelectedColumn.ColumnInfo.ColumnName,
                        ReferencedColumn = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == joinerMainView.CurrentSelectedColumn.ColumnInfo.ColumnName)
                    });
                }
                else
                {
                    SelectedColumnList.Add(new CustomRecurseColumnOL()
                    {
                        ColumnInfo = joinerMainView.CurrentSelectedColumn.ColumnInfo,
                        ReferencedTable = SelectedColumnList.FirstOrDefault(x => x.ColumnInfo?.TempId == joinerMainView.CurrentSelectedColumn.ColumnInfo.HeadTempId)?.ReferencedColumn?.Table
                    });
                }
            }
        }

        private void SqlJoinerMainPresenter_GenerateQuery(object? sender, EventArgs e)
        {
         
        }
    }
}
