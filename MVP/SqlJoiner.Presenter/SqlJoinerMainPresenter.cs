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
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqlJoiner.Presenter
{
    public class SqlJoinerMainPresenter
    {
        private readonly IJoinerMainView joinerMainView;
        private readonly ISchemaService schemaService;
        private readonly ITableService tableService;
        private readonly IColumnService columnService;
        private readonly IQueryService queryService;

        public SqlJoinerMainPresenter(IJoinerMainView joinerMainView, ISchemaService schemaService, ITableService tableService, IColumnService columnService, IQueryService queryService)
        {
            this.joinerMainView = joinerMainView ?? throw new ArgumentNullException(nameof(joinerMainView));
            this.schemaService = schemaService ?? throw new ArgumentNullException(nameof(schemaService));
            this.tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
            this.columnService = columnService ?? throw new ArgumentNullException(nameof(columnService));
            this.queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));

            joinerMainView.Init += JoinerMainView_Init;
            joinerMainView.SelectSchema += JoinerMainView_SelectedSchemaValueChanged;
            joinerMainView.SelectTable += JoinerMainView_SelectedTableValueChanged;
            joinerMainView.SelectColumn += JoinerMainView_SelectedColumnValueChanged;
            joinerMainView.UnSelectSchema += JoinerMainView_UnSelectSchema;
            joinerMainView.UnSelectTable += JoinerMainView_UnSelectTable;
            joinerMainView.UnSelectColumn += JoinerMainView_UnSelectColumn;

            joinerMainView.LoadTableBySelectedSchema += JoinerMainView_LoadTableBySelectedSchema;
            joinerMainView.LoadColumnFromSelectedTable += JoinerMainView_LoadColumnFromSelectedTable;
            joinerMainView.LoadForeignKeyColumnsBySelectedForeignKey += JoinerMainView_LoadForeignKeyColumnsBySelectedForeignKey;

            joinerMainView.GenerateQuery += SqlJoinerMainPresenter_GenerateQuery;
            joinerMainView.Reset += JoinerMainView_Reset;
            joinerMainView.RunQuery += JoinerMainView_RunQuery;
        }

        private async void JoinerMainView_RunQuery(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(joinerMainView.GeneratedQuery)) return;
            joinerMainView.JsonOutputFromQuery = await queryService.RawQueryToJson(joinerMainView.GeneratedQuery);
        }

        private void JoinerMainView_UnSelectColumn(object? sender, EventArgs e)
        {
            if (SelectedColumnList.Count == 0) return;

            var toDeleteCol = SelectedColumnList.FirstOrDefault(x => x.ColumnInfo?.TempId == joinerMainView?.CurrentSelectedColumn?.ColumnInfo?.TempId);
            SelectedColumnList.Remove(toDeleteCol ?? new CustomRecurseColumnOL());
        }

        private void JoinerMainView_UnSelectTable(object? sender, EventArgs e)
        {
            if (SelectedTableList.Count == 0) return;

            SelectedTableList.Remove(joinerMainView?.CurrentSelectedTable ?? new TableOL());
        }

        private void JoinerMainView_UnSelectSchema(object? sender, EventArgs e)
        {
            if (SelectedSchemaList.Count == 0) return;

            SelectedSchemaList.Remove(joinerMainView?.CurrentSelectedSchema ?? new SchemaOL());
        }

        private void JoinerMainView_Reset(object? sender, EventArgs e)
        {
            MainSchemaList.Clear();
            MainTableList.Clear();
            MainColumnList.Clear();
            MainForeignKeyColumnsList.Clear();

            SelectedColumnList.Clear();
            SelectedSchemaList.Clear();
            SelectedTableList.Clear();

            if (joinerMainView == null) return;

            joinerMainView.GeneratedQuery = string.Empty;
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
                joinerMainView?.TableList?.AddRange(result.OrderBy(x => x.Name));
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

                    item.TempId = GuidGenerator.Generate() + item.ColumnName;
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
                    joinerMainView.CurrentSelectedColumn.ColumnInfo.TempId = GuidGenerator.Generate() + "-" + joinerMainView?.CurrentSelectedColumn?.ColumnInfo?.ColumnName;

                    if (joinerMainView?.CurrentSelectedColumn.ColumnInfo.IsForeignKey == true)
                        JoinerMainView_SelectedColumnValueChanged(sender, e);
                }

                var result = MainForeignKeyColumnsList.Where(x => x.ColumnName == joinerMainView?.CurrentSelectedColumn?.ColumnInfo?.ColumnName);
                var table = MainTableList.FirstOrDefault(x => x.Name == result.FirstOrDefault()?.Table?.Name);
                var foreignKeysColumns = MainColumnList.Where(x => x.Table?.Name == table?.Name);

                var output = new List<ColumnOL>();
                foreach (var item in foreignKeysColumns)
                {
                    var isForeignKey = MainForeignKeyColumnsList.FirstOrDefault(x => x.ColumnName == item.ColumnName);
                    if (isForeignKey != null) item.IsForeignKey = true;

                    item.TempId = GuidGenerator.Generate() + item.ColumnName;
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
            Dictionary<string, string> alias = new Dictionary<string, string>();

            int counter = 1;
            SelectedTableList.ForEach(x =>
            {
                string toBeAlias = $"{x?.Name?.ToLower().Substring(0, (x.Name.Length >= 4 ? 4 : x.Name.Length))}";
                var isExisting = alias.Any(x => x.Value == toBeAlias);
                if (isExisting)
                {
                    toBeAlias += counter;
                    counter += 1;
                }

                alias.Add(x?.TempId ?? "", toBeAlias);
            });

            int counter2 = 1;
            SelectedColumnList.Where(x => x.ColumnInfo?.IsForeignKey == true).ToList().ForEach(x =>
            {
                string toBeAlias = $"{x?.ReferencedColumn?.Table?.Name?.ToLower()?.Substring(0, (x.ReferencedColumn.Table.Name.Length >= 4 ? 4 : x.ReferencedColumn.Table.Name.Length))}";
                var isExisting = alias.Any(x => x.Value == toBeAlias);
                if (isExisting)
                {
                    toBeAlias += counter2;
                    counter2 += 1;
                }

                alias.Add(x?.ColumnInfo?.TempId ?? "", toBeAlias);
            });

            string cols = $@"";
            string joins = $@"";

            foreach (var joinCol in SelectedColumnList.Where(x => x.ColumnInfo?.IsForeignKey == false))
            {
                if (joinCol?.ColumnInfo == null) return;
                string? toBeAlias = "";
                alias.TryGetValue(joinCol.ColumnInfo.HeadTempId ?? "", out toBeAlias);

                cols += $@"     {toBeAlias}.""{joinCol?.ColumnInfo?.ColumnName}"", 
";
            }

            var headTable = alias.FirstOrDefault();
            var headTableInfo = SelectedTableList?.FirstOrDefault(x => x.TempId == headTable.Key);
            joins = $@"FROM ""{headTableInfo?.Schema?.SchemaName}"".""{headTableInfo?.Name}"" {headTable.Value}
";
            foreach (var item in alias)
            {
                if (item.Key != headTable.Key)
                {
                    var foreignKeyInfo = SelectedColumnList.FirstOrDefault(x => x?.ColumnInfo?.TempId == item.Key);
                    var foreignKeyColumnReferencedTable = alias.GetValueOrDefault(foreignKeyInfo?.ColumnInfo?.HeadTempId);
                    joins += $@"LEFT JOIN ""{foreignKeyInfo?.ReferencedColumn?.Table?.Schema?.SchemaName}"".""{foreignKeyInfo?.ReferencedColumn?.Table?.Name}"" {item.Value} ON {item.Value}.""{foreignKeyInfo.FieldFromReferencedTable}"" = {foreignKeyColumnReferencedTable}.""{foreignKeyInfo.FieldFromSourceTable}""
";
                }
            }

            joinerMainView.GeneratedQuery = "SELECT \n" + cols.Remove(cols.Length - 4) + "\n\n" + joins;
        }
    }
}
