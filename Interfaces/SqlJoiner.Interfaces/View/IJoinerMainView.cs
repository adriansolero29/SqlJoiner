using SqlJoiner.Models;
using System.Diagnostics.Tracing;

namespace SqlJoiner.Interfaces.View
{
    public interface IJoinerMainView
    {
        List<CustomDatabaseEntityModelOL> FullDatabaseEntityModel { get; set; }
        List<SchemaOL>? SchemaList { get; set; }
        List<TableOL>? TableList { get; set; }
        List<ColumnOL>? ColumnList { get; set; }
        List<ColumnOL>? ColumnFromForeignKeyColumn { get; set; }
        List<TableOL>? SelectedTables { get; set; }
        List<ColumnOL>? SelectedColumns { get; set; }
        List<TableOL>? SelectedForeignKeyColumns { get; set; }

        SchemaOL? SelectedSchema { get; set; }
        TableOL? SelectedTable { get; set; }
        ColumnOL? SelectedColumn { get; set; }
        string? SqlJoinedResult { get; set; }

        event EventHandler? Init;
        event EventHandler? SelectedSchemaValueChanged;
        event EventHandler? SelectedTableChanged;
        event EventHandler? SelectedColumnChanged;
        event EventHandler<ColumnOL>? ColumnSelection;
        event EventHandler<ColumnOL>? ColumnRemoveSelection;
        event EventHandler<TableOL>? TableSelection;
        event EventHandler<TableOL>? TableRemoveSelection;

        event EventHandler? GenerateSql;
        event EventHandler? LoadSchema;
        event EventHandler? LoadTable;
        event EventHandler? LoadColumns;
    }
}
