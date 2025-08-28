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
        SchemaOL? CurrentSelectedSchema { get; set; }
        TableOL? CurrentSelectedTable { get; set; }
        CustomRecurseColumnOL CurrentSelectedColumn { get; set; }
        string GeneratedQuery { get; set; }
        IEnumerable<object> GeneratedOutputFromQuery { get; set; }
        string JsonOutputFromQuery { get; set; }

        event EventHandler Init;
        event EventHandler SelectSchema;
        event EventHandler SelectTable;
        event EventHandler SelectColumn;
        event EventHandler UnSelectSchema;
        event EventHandler UnSelectTable;
        event EventHandler UnSelectColumn;
        event EventHandler GenerateQuery;
        event EventHandler LoadTableBySelectedSchema;
        event EventHandler LoadColumnFromSelectedTable;
        event EventHandler LoadForeignKeyColumnsBySelectedForeignKey;
        event EventHandler Reset;
        event EventHandler RunQuery;
    }
}
