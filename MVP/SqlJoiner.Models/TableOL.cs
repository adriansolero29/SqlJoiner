using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
	[Serializable]
    public class TableOL : IModel
    {
        public string? TempHeadId { get; set; }
        public string? TempId { get; set; }
        public SchemaOL? Schema { get; set; }
        public string? Name { get; set; }
	}
}
