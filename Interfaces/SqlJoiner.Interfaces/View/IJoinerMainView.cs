using SqlJoiner.Models;
using System.Diagnostics.Tracing;

namespace SqlJoiner.Interfaces.View
{
    public interface IJoinerMainView
    {
        List<SchemaOL>? SchemaList { get; set; }
        List<TableOL>? TableList { get; set; }
        List<ColumnOL>? ColumnList { get; set; }
        List<ColumnOL>? ForeignKeyColumnsList { get; set; }
        SchemaOL? SelectedSchema { get; set; }
        TableOL? SelectedTable { get; set; }
        ColumnOL? SelectedColumn { get; set; }

        event EventHandler Init;
        event EventHandler SelectedSchemaValueChanged;
        event EventHandler SelectedTableValueChanged;
        event EventHandler SelectedColumnValueChanged;
        event EventHandler GenerateQuery;
    }
}
