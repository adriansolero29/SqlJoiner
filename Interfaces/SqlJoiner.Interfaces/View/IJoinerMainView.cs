using SqlJoiner.Models;

namespace SqlJoiner.Interfaces.View
{
    public interface IJoinerMainView
    {
        List<CustomDatabaseEntityModelOL> FullDatabaseEntityModel { get; set; }
        List<SchemaOL>? SchemaList { get; set; }
        List<TableOL>? TableList { get; set; }

        SchemaOL? SelectedSchema { get; set; }
        TableOL? SelectedTable { get; set; }
        string? SqlJoinedResult { get; set; }

        event EventHandler? Init;
        event EventHandler? SelectedSchemaValueChanged;

        event EventHandler? GenerateSql;
        event EventHandler? LoadSchema;
        event EventHandler? LoadTable;
        event EventHandler? LoadColumns;
    }
}
