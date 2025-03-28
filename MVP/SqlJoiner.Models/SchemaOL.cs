using SqlJoiner.Models.Base;

namespace SqlJoiner.Models
{
	[Serializable]
    public class SchemaOL : IModel
    {
		private string? _schemaName;
		public string? SchemaName
		{
			get { return _schemaName; }
			set { _schemaName = value; }
		}
	}
}
