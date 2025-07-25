using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
	[Serializable]
    public class SchemaOL : IModel
    {
        public string? TempId { get; set; }
        public string? SchemaName { get; set; }
    }
}
