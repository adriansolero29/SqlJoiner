using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
    public class CustomRecurseColumnOL
    {
        public string? TempHeadTableId { get; set; }
        public ColumnOL? ColumnInfo { get; set; } = new ColumnOL();
        public TableOL? ReferencedTable { get; set; } = new TableOL();
        public ColumnOL? ReferencedColumn { get; set; } = new();
        public string? FieldFromSourceTable { get; set; }
        public string? FieldFromReferencedTable { get; set; }
    }
}
