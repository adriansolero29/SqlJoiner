using SqlJoiner.Interfaces.Service;
using SqlJoiner.Interfaces.View;
using SqlJoiner.Models;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
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
            joinerMainView.GenerateSql += JoinerMainView_GenerateSql;
            joinerMainView.SelectedSchemaValueChanged += JoinerMainView_SelectedSchemaValueChanged;
            joinerMainView.SelectedTableChanged += JoinerMainView_SelectedTableChanged;
            joinerMainView.SelectedColumnChanged += JoinerMainView_SelectedColumnChanged;
            joinerMainView.ColumnSelection += JoinerMainView_ColumnSelection;
            joinerMainView.ColumnRemoveSelection += JoinerMainView_ColumnRemoveSelection;
            joinerMainView.TableSelection += JoinerMainView_TableSelection;
            joinerMainView.TableRemoveSelection += JoinerMainView_TableRemoveSelection;

        }

        #region Private Methods

        public async Task<bool> checkIfColumnIsForeignKeyTable(ColumnOL obj)
        {
            var result = await tableService.CheckColumnIfTable(obj);
            return result.Count() > 0 ? true : false;
        }

        private async Task loadTablesBySchema()
        {
            var result = await tableService.GetBySchema(joinerMainView.SelectedSchema ?? new SchemaOL());

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

        public async Task loadColumnsByTable()
        {
            var result = await columnService.GetByTable(joinerMainView.SelectedTable ?? new TableOL());

            joinerMainView.ColumnList = new List<ColumnOL>();
            joinerMainView.ColumnList?.Clear();
            joinerMainView.ColumnList?.AddRange(result);
        }

        private void generateQuery()
        {
            if (joinerMainView.SelectedTables?.Count > 0)
            {
                string forColumn = "";
                string tableJoining = "";

                joinerMainView.SqlJoinedResult = "";
                foreach (var item in joinerMainView.SelectedTables)
                {
                    forColumn = "";
                    foreach (var columns in joinerMainView.SelectedColumns?.Where(x => x.Table?.Name == item.Name).DistinctBy(x => x.ColumnName) ?? new List<ColumnOL>())
                    {
                        forColumn += $@"""{columns.ColumnName}"", ";
                    }

                    tableJoining += $@"""{item.Schema?.SchemaName}"".""{item?.Name}""";
                }

                joinerMainView.SqlJoinedResult = $"""

                        SELECT 
                        {forColumn}
                        FROM {tableJoining};

                        """;
            }
        }

        #endregion

        private void JoinerMainView_TableRemoveSelection(object? sender, TableOL e)
        {
            joinerMainView.SelectedTables?.Remove(e);
            generateQuery();
        }

        private void JoinerMainView_TableSelection(object? sender, TableOL e)
        {
            joinerMainView.SelectedTables?.Add(e);
            //generateQuery();
        }

        private void JoinerMainView_ColumnRemoveSelection(object? sender, ColumnOL e)
        {
            joinerMainView.SelectedColumns?.Remove(e);
            generateQuery();
        }

        private void JoinerMainView_ColumnSelection(object? sender, ColumnOL e)
        {
            joinerMainView.SelectedColumns?.Add(e);
            generateQuery();
        }

        private async void JoinerMainView_SelectedColumnChanged(object? sender, EventArgs e)
        {
            var result = await tableService.CheckColumnIfTable(joinerMainView.SelectedColumn ?? new ColumnOL());
            joinerMainView.ColumnFromForeignKeyColumn = (List<ColumnOL>?)await columnService.GetByTable(result.FirstOrDefault() ?? new TableOL());
        }

        private async void JoinerMainView_SelectedTableChanged(object? sender, EventArgs e)
        {
            joinerMainView.SelectedTables?.Remove(joinerMainView.SelectedTable ?? new TableOL());
            joinerMainView.SelectedTables?.Add(joinerMainView.SelectedTable ?? new TableOL());
            await loadColumnsByTable();
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
    }
}
