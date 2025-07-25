using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
	[Serializable]
    public class ColumnOL : IModel
    {
        public string? HeadTempId { get; set; }
        public string? TempId { get; set; }
        public TableOL? Table { get; set; }
        public string? ColumnName { get; set; }
        public int OrdinalPosition { get; set; }
        public string? DataType { get; set; }
        public bool IsForeignKey { get; set; }
        public string? FieldFromReferencedTable { get; set; }
    }
}
